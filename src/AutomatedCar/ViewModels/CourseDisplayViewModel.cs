namespace AutomatedCar.ViewModels
{
    using AutomatedCar.Models;
    using AutomatedCar.Visualization;

    public class CourseDisplayViewModel : ViewModelBase
    {
        public CourseDisplayViewModel(World world)
        {
            this.World = world;
        }

        public World World { get; private set; }
    }
}