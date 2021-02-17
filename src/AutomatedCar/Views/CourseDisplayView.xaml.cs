namespace AutomatedCar.Views
{
    using System;
    using System.Windows.Controls;
    using AutomatedCar.Models;
    using AutomatedCar.ViewModels;

    public partial class CourseDisplayView : UserControl
    {
        public ViewModelBase ViewModel { get; private set; }
        public CourseDisplayView()
        {
            this.InitializeComponent();
            ViewModel = new CourseDisplayViewModel(World.Instance);
            this.Focusable = true;
            this.Focus();
        }
       
    }
}