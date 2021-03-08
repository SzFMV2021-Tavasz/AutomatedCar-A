namespace AutomatedCar.ViewModels
{
    public class SpeedGaugeViewModel : GaugeViewModelBase
    {
        public SpeedGaugeViewModel()
        {
        }

        public override void SetValue(int value)
        {
            //TODO validation?
            this.Value = value;
        }
    }
}
