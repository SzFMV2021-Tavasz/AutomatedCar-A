namespace AutomatedCar.Views
{
    using System;
    using System.Windows.Controls;
    using AutomatedCar.Models;
    using AutomatedCar.ViewModels;

    public partial class DashboardView : UserControl
    {
        public ViewModelBase ViewModel { get; private set; }
        public DashboardView()
        {
            ViewModel = new DashboardViewModel(World.Instance.ControlledCar);
            this.InitializeComponent();
        }

    }
}