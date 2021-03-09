using AutomatedCar.Models;
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
        public IIntegrator Integrator { get;}

        private PowertrainComponentPacket powertrainComponentPacket;
        private IPriorityChecker priorityChecker;
        private VehicleTransform currentTransform;
        private Vector2 carPos;
        private float currentSteering;
        private Vector2 currentWheelDirection;
        private float currentDirection;
        private float deltaTime = 1 / 30;
        public CarUpdater(IVirtualFunctionBus virtualFunctionBus, IVehicleForces vehicleForces, IIntegrator integrator, PowertrainComponentPacket powertrainPacket)
        {
            this.VirtualFunctionBus = virtualFunctionBus;
            this.VehicleForces = vehicleForces;
            this.Integrator = integrator;
            this.powertrainComponentPacket = powertrainPacket;

            currentSteering = World.Instance.ControlledCar.CurrentSteering;
            currentWheelDirection = new Vector2((float)Math.Cos(currentDirection), (float)Math.Sin(currentDirection));
            currentDirection = World.Instance.ControlledCar.CarHeading;
            priorityChecker = new PriorityChecker();
            priorityChecker.virtualFunctionBus = this.VirtualFunctionBus;
            CreateCurrentTransform();
        }

        private void CreateCurrentTransform()
        {
            this.carPos = new Vector2(World.Instance.ControlledCar.X, World.Instance.ControlledCar.Y);
            currentTransform = new VehicleTransform(carPos, World.Instance.ControlledCar.CarHeading, World.Instance.ControlledCar.Velocity, World.Instance.ControlledCar.AngularVelocity);
        }
        public void UpdateWorldObject()
        {
            World.Instance.ControlledCar.X = (int)currentTransform.Position.X;
            World.Instance.ControlledCar.Y = (int)currentTransform.Position.Y;
            World.Instance.ControlledCar.CarHeading = currentTransform.AngularDisplacement;
            World.Instance.ControlledCar.AngularVelocity = currentTransform.AngularVelocity;
            World.Instance.ControlledCar.CurrentSteering = currentSteering;
            World.Instance.ControlledCar.Velocity = currentTransform.Velocity;
            World.Instance.ControlledCar.Speed = (int)currentTransform.Velocity.Length();
        }
        public void UpdatePacket()
        {
            
        }
        public void Calculate()
        {
           

        }
        public void SetCurrentTransform()
        {
            currentTransform = Integrator.NextVehicleTransform;
        }
    }
}
