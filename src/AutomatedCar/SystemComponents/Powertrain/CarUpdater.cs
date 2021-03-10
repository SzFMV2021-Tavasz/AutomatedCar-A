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

        private PowertrainComponentPacket powertrainComponentPacket;
        private VehicleTransform currentTransform;
        private Vector2 carPos;
        private float currentSteering;
        private Vector2 currentWheelDirection;
        private float currentDirection;
        private float deltaTime = 0.05f;
        public CarUpdater(IVirtualFunctionBus virtualFunctionBus, IVehicleForces vehicleForces, IIntegrator integrator, PowertrainComponentPacket powertrainPacket)
        {
            this.VirtualFunctionBus = virtualFunctionBus;
            this.VehicleForces = vehicleForces;
            this.Integrator = integrator;
            this.powertrainComponentPacket = powertrainPacket;

            currentSteering = 0;
            currentWheelDirection = new Vector2((float)Math.Cos(currentDirection), (float)Math.Sin(currentDirection));
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
            currentWheelDirection.X = (float)Math.Cos(currentDirection);
            currentWheelDirection.Y = (float)Math.Sin(currentDirection);
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
            World.Instance.ControlledCar.X = (int)currentTransform.Position.X;
            World.Instance.ControlledCar.Y = (int)currentTransform.Position.Y;
            World.Instance.ControlledCar.CarHeading = currentTransform.AngularDisplacement;
            World.Instance.ControlledCar.AngularVelocity = currentTransform.AngularVelocity;
            World.Instance.ControlledCar.CurrentSteering = currentSteering;
            World.Instance.ControlledCar.Velocity = currentTransform.Velocity;
            World.Instance.ControlledCar.Speed = (int)(currentTransform.Velocity.Length()*3.6);
        }
        public void UpdatePacket()
        {
            powertrainComponentPacket.X = (int)currentTransform.Position.X;
            powertrainComponentPacket.Y = (int)currentTransform.Position.Y;
            powertrainComponentPacket.Speed = (int)(currentTransform.Velocity.Length() * 3.6);
            powertrainComponentPacket.CarHeadingAngle = currentTransform.AngularDisplacement;
        }
        public void Calculate()
        {
            Integrator.Reset(currentTransform, deltaTime);
            CalculateSteeringAngle();
            SetCurrentDirection();
            SetCurrentWheelDirection();
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
    }
}
