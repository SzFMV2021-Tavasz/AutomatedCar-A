namespace AutomatedCar.SystemComponents
{
    public abstract class SystemComponent
    {
        protected IVirtualFunctionBus virtualFunctionBus;

        protected SystemComponent(IVirtualFunctionBus virtualFunctionBus)
        {
            this.virtualFunctionBus = virtualFunctionBus;
            virtualFunctionBus.RegisterComponent(this);
        }

        public abstract void Process();
    }
}