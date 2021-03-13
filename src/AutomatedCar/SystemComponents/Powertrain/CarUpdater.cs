﻿using AutomatedCar.Models;
using AutomatedCar.Models.Enums;
using AutomatedCar.SystemComponents.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedCar.SystemComponents.Powertrain
{
    public class CarUpdater : ICarUpdater
    {
        public IVirtualFunctionBus VirtualFunctionBus { get; }
        public IVehicleForces VehicleForces { get; }
        public IIntegrator Integrator { get; }
        public ITransmission transmission { get; set; }
        public IPriorityChecker priorityChecker { get; set; }
        public IVehicleConstants VehicleConstants { get; }
        public float UnitsPerMeters { get => unitsPerMeters; }

        private PowertrainComponentPacket powertrainComponentPacket;
        private VehicleTransform currentTransform;
        private float currentSteering;
        private Vector2 wheelDirection;
        private DateTime then;
        private float deltaTime;
        private const float unitsPerMeters = 48f;
        private readonly float forceMultiplier;

        private float wheelRotationVelocity = (float)Math.PI / 64;

        public CarUpdater(IVirtualFunctionBus virtualFunctionBus, IVehicleForces vehicleForces, IIntegrator integrator, PowertrainComponentPacket powertrainPacket, IVehicleConstants vehicleConstants, float forceMultiplier)
        {
            this.VirtualFunctionBus = virtualFunctionBus;
            this.VehicleForces = vehicleForces;
            this.Integrator = integrator;
            this.powertrainComponentPacket = powertrainPacket;
            this.VehicleConstants = vehicleConstants;
            this.forceMultiplier = forceMultiplier;

            currentSteering = 0;
            wheelDirection = Vector2.UnitX;
            priorityChecker = new PriorityChecker();
            transmission = new Transmission() {Gear = Gear.D};
            priorityChecker.virtualFunctionBus = this.VirtualFunctionBus;
            CreateCurrentTransform();

            then = DateTime.Now;
            deltaTime = 0.005f;
        }

        private void CreateCurrentTransform()
        {
            currentTransform = new VehicleTransform(Vector2.Zero, 0, Vector2.Zero, 0);
        }
        private void CalculateSteeringAngle()
        {
            PacketEnum priority = priorityChecker.SteeringPriorityCheck();
            if (priority == PacketEnum.HMI)
            {
                currentSteering = (float)(VirtualFunctionBus.HMIPacket.SteeringWheelAngle * 0.6).DegreesToRadians();
            }
        }
        public void UpdateWorldObject()
        {
            var worldTransform = ConvertTransformToWorldSpace(currentTransform);

            World.Instance.ControlledCar.X = (int)(worldTransform.Position.X);
            World.Instance.ControlledCar.Y = (int)(worldTransform.Position.Y);
            World.Instance.ControlledCar.CarHeading = worldTransform.AngularDisplacement;
            World.Instance.ControlledCar.AngularVelocity = worldTransform.AngularVelocity;
            World.Instance.ControlledCar.CurrentSteering = currentSteering;
            World.Instance.ControlledCar.Velocity = worldTransform.Velocity;
            World.Instance.ControlledCar.Speed = (int)(worldTransform.Velocity.Length()*3.6);

            FillTransformationMatrix(World.Instance.ControlledCar, worldTransform.AngularDisplacement);
        }

        public void UpdatePacket()
        {
            var worldTransform = ConvertTransformToWorldSpace(currentTransform);

            powertrainComponentPacket.X = (int)(worldTransform.Position.X);
            powertrainComponentPacket.Y = (int)(worldTransform.Position.Y);
            powertrainComponentPacket.Speed = (int)(worldTransform.Velocity.Length() * 3.6);
            powertrainComponentPacket.SteeringWheelAngleDegrees = currentSteering.RadiansToDegrees();
            powertrainComponentPacket.Rpm = (int)Math.Max(1000,VehicleConstants.GetCrankshaftSpeed(VirtualFunctionBus.HMIPacket.GasPedal / 100f));
        }

        public void Calculate()
        {
            CalculateSteeringAngle();

            var currentDirection = currentTransform.AngularDisplacement + currentSteering;
            wheelDirection = currentDirection.MakeUnitVectorFromRadians();
            var now = DateTime.Now;
            var deltaTime = (now - then).TotalSeconds;
            then = now;

            var gasPedal = VirtualFunctionBus.HMIPacket.GasPedal/ 100f;
            var brakePedal = VirtualFunctionBus.HMIPacket.BrakePedal / 100f;

            transmission.Gear = VirtualFunctionBus.HMIPacket.Gear;
            transmission.SetInsideGear((int)(currentTransform.Velocity.Length() * 3.6));

            Integrator.Reset(currentTransform, (float)deltaTime, transmission.Gear);

            PacketEnum priority = priorityChecker.AccelerationPriorityCheck();
            if (priority == PacketEnum.AEB)
            {
                Integrator.AccumulateForce(WheelKind.Front, forceMultiplier * VehicleForces.GetBrakingForce(1f, currentTransform.Velocity));
            }
            else if (priority == PacketEnum.HMI)
            {
                if (transmission.Gear != Gear.R)
                {
                    Integrator.AccumulateForce(WheelKind.Front, forceMultiplier * VehicleForces.GetBrakingForce(brakePedal, currentTransform.Velocity));
                    if (transmission.Gear == Gear.D)
                    {
                        Integrator.AccumulateForce(WheelKind.Front, forceMultiplier * VehicleForces.GetTractiveForce(gasPedal, wheelDirection, transmission.InsideGear));
                    }
                }
                else
                {
                    Integrator.AccumulateForce(WheelKind.Front, 8 * VehicleForces.GetBrakingForce(brakePedal, currentTransform.Velocity));
                    Integrator.AccumulateForce(WheelKind.Front, 8 * VehicleForces.GetTractiveForceInReverse(gasPedal, wheelDirection));
                }
            }
            else if (priority == PacketEnum.ACC || priority == PacketEnum.PP)
            {
            }
            Integrator.AccumulateForce(WheelKind.Front, forceMultiplier * VehicleForces.GetDragForce(currentTransform.Velocity));
            Integrator.AccumulateForce(WheelKind.Back, forceMultiplier * VehicleForces.GetDragForce(currentTransform.Velocity));
        }

        public void SetCurrentTransform()
        {
            currentTransform = Integrator.NextVehicleTransform;

            // HACK: cap vehicle speed at 100m/s to prevent blow-up
            if(currentTransform.Velocity.Length() > 100)
            {
                var fixedVelocity = 100 * Vector2.Normalize(currentTransform.Velocity);
                currentTransform = currentTransform with { Velocity = fixedVelocity };
            }

            currentTransform = MakeCarRotateTowardsWheelDirection(currentTransform);
        }

        private VehicleTransform MakeCarRotateTowardsWheelDirection(VehicleTransform original)
        {
            var w = Vector2.Normalize(wheelDirection);
            if(transmission.Gear == Gear.R)
            {
                w = -w;
            }

            var v0_len = original.Velocity.Length();

            if(v0_len < 0.01f)
            {
                return original;
            }

            var v0_normalized = original.Velocity / v0_len;
            var d = Vector2.Dot(v0_normalized, w);
            d = Math.Clamp(d, -1, 1);
            var theta = (float)Math.Acos(d);
            if(theta < 0.01f)
            {
                return original;
            }

            var alpha = Math.Min(theta, deltaTime * wheelRotationVelocity);
            var t = alpha / theta;
            var v1_normalized = t * w + (1f - t) * v0_normalized;
            var v1 = v0_len * v1_normalized;
            return original with { Velocity = v1 };
        }

        private VehicleTransform ConvertTransformToWorldSpace(VehicleTransform original)
        {
            var worldPosition =  unitsPerMeters * original.Position;
            var worldHeading = (float)(original.AngularDisplacement + Math.PI / 2).NormalizeRadians();
            return original with { Position = worldPosition, AngularDisplacement = worldHeading };
        }

        private void FillTransformationMatrix(RenderableWorldObject obj, float angle)
        {
            var sin = (float)Math.Sin(angle);
            var cos = (float)Math.Cos(angle);

            obj.M11 = cos;
            obj.M12 = sin;
            obj.M21 = -sin;
            obj.M22 = -cos;
        }
    }
}
