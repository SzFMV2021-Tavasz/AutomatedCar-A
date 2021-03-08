namespace AutomatedCar.ViewModels
{
    public class RpmGaugeViewModel : GaugeViewModelBase
    {
        public RpmGaugeViewModel()
        {
        }

        public override void SetValue(int value)
        {
            //TODO validation?
            this.Value = value;
        }
    }
}
