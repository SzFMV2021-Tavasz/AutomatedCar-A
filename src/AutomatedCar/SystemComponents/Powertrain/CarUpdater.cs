using AutomatedCar.Models;
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
        private Vector2 carPos;
        private float currentSteering;
        private Vector2 currentWheelDirection;
        private float currentDirection;
        private float deltaTime = 0.05f;
        private const float unitsPerMeters = 48f;

        public CarUpdater(IVirtualFunctionBus virtualFunctionBus, IVehicleForces vehicleForces, IIntegrator integrator, PowertrainComponentPacket powertrainPacket, IVehicleConstants vehicleConstants)
        {
            this.VirtualFunctionBus = virtualFunctionBus;
            this.VehicleForces = vehicleForces;
            this.Integrator = integrator;
            this.powertrainComponentPacket = powertrainPacket;
            this.VehicleConstants = vehicleConstants;

            currentSteering = 0;
            currentWheelDirection = currentDirection.MakeUnitVectorFromRadians();
            currentDirection = 0;
            priorityChecker = new PriorityChecker();
            transmission = new Transmission() {Gear = Gear.D};
            priorityChecker.virtualFunctionBus = this.VirtualFunctionBus;
            CreateCurrentTransform();
        }

        private void CreateCurrentTransform()
        {
            this.carPos = Vector2.Zero;
            currentTransform = new VehicleTransform(carPos, 0, Vector2.Zero, 0);
        }
        private void SetCurrentWheelDirection()
        {
            currentDirection.MakeUnitVectorFromRadians(out currentWheelDirection.X, out currentWheelDirection.Y);
        }
        private void SetCurrentDirection()
        {
            currentDirection = currentTransform.AngularDisplacement + currentSteering;
        }
        private void CalculateSteeringAngle()
        {
            PacketEnum priority = priorityChecker.SteeringPriorityCheck();
            if (priority == PacketEnum.HMI)
            {
                currentSteering = (float)((VirtualFunctionBus.HMIPacket.SteeringWheelAngle * 0.6)*(Math.PI / 180));
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
        }

        public void UpdatePacket()
        {
            var worldTransform = ConvertTransformToWorldSpace(currentTransform);

            powertrainComponentPacket.X = (int)(worldTransform.Position.X);
            powertrainComponentPacket.Y = (int)(worldTransform.Position.Y);
            powertrainComponentPacket.Speed = (int)(worldTransform.Velocity.Length() * 3.6);
            powertrainComponentPacket.SteeringWheel = VirtualFunctionBus.HMIPacket.SteeringWheelAngle;
            powertrainComponentPacket.Rpm = (int)Math.Max(1000,VehicleConstants.GetCrankshaftSpeed(VirtualFunctionBus.HMIPacket.GasPedal / 100f));
        }

        public void Calculate()
        {
            CalculateSteeringAngle();
            SetCurrentDirection();
            SetCurrentWheelDirection();
            Integrator.Reset(currentTransform, deltaTime);
           // transmission.Gear = VirtualFunctionBus.HMIPacket.Gear;
            transmission.SetInsideGear((int)(currentTransform.Velocity.Length() * 3.6));
            PacketEnum priority = priorityChecker.AccelerationPriorityCheck();
            if (priority == PacketEnum.AEB)
            {
                Integrator.AccumulateForce(WheelKind.Front, VehicleForces.GetBrakingForce(1f, currentTransform.Velocity));
            }
            else if (priority == PacketEnum.HMI)
            {
                if (transmission.Gear != Gear.R)
                {
                    Integrator.AccumulateForce(WheelKind.Front, VehicleForces.GetBrakingForce(VirtualFunctionBus.HMIPacket.BrakePedal / 100f, currentTransform.Velocity));
                    if (transmission.Gear == Gear.D)
                    {
                        Integrator.AccumulateForce(WheelKind.Front, VehicleForces.GetTractiveForce(VirtualFunctionBus.HMIPacket.GasPedal / 100f, currentWheelDirection, transmission.InsideGear));
                    }
                }
                else
                {
                    Integrator.AccumulateForce(WheelKind.Front, VehicleForces.GetBrakingForce(VirtualFunctionBus.HMIPacket.BrakePedal / 100f, currentTransform.Velocity));
                    Integrator.AccumulateForce(WheelKind.Front, VehicleForces.GetTractiveForceInReverse(VirtualFunctionBus.HMIPacket.GasPedal / 100f, currentWheelDirection));
                }
                 
            }
            else if (priority == PacketEnum.ACC || priority == PacketEnum.PP)
            {
            }
            Integrator.AccumulateForce(WheelKind.Front, VehicleForces.GetDragForce(currentTransform.Velocity));
            Integrator.AccumulateForce(WheelKind.Back, VehicleForces.GetDragForce(currentTransform.Velocity));
            Integrator.AccumulateForce(WheelKind.Front, VehicleForces.GetWheelDirectionHackForce(currentWheelDirection, currentTransform.Velocity));
            Integrator.AccumulateForce(WheelKind.Back, VehicleForces.GetWheelDirectionHackForce(currentWheelDirection, currentTransform.Velocity));
        }
        public void SetCurrentTransform()
        {
            currentTransform = Integrator.NextVehicleTransform;
        }

        private VehicleTransform ConvertTransformToWorldSpace(VehicleTransform original)
        {
            var worldPosition = original.Position / 48f;
            return original with { Position = worldPosition };
        }
    }
}
