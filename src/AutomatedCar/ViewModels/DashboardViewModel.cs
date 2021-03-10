namespace AutomatedCar.ViewModels
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents;
    using AutomatedCar.SystemComponents.Packets;
    using ReactiveUI;
    using System;

    public class DashboardViewModel : ViewModelBase
    {
        private AutomatedCar _controlledCar;
        private GaugeViewModelBase _rpmGaugeViewModel;
        private GaugeViewModelBase _speedGaugeViewModel;
        private BreakPedalViewModel _breakPedalViewModel;
        private GasPedalViewModel _gasPedalViewModel;
        private TransmissionViewModel _transmissionViewModel;
        private TurnSignalViewModelBase _leftTurnSignalViewModel;
        private TurnSignalViewModelBase _rightTurnSignalViewModel;
        private ACCOptionsViewModel _accOptionsViewModel;
        private LaneKeepingAndParkingPilotViewModel _laneKeepingAndParkingPilotViewModel;
        private LastSignViewModel _lastSignViewModel;
        private CarInfoViewModel _carInfoViewModel;
        private SteeringWheelViewModel _steeringWheelViewModel;

        public DashboardViewModel(AutomatedCar controlledCar)
        {
            this.ControlledCar = controlledCar;
            this.RpmGaugeViewModel = new RpmGaugeViewModel();
            this.SpeedGaugeViewModel = new SpeedGaugeViewModel();
            this.TransmissionViewModel = new TransmissionViewModel();
            this.LeftTurnSignalViewModel = new LeftTurnSignalViewModel();
            this.RightTurnSignalViewModel = new RightTurnSignalViewModel();
            this.BreakPedalViewModel = new BreakPedalViewModel();
            this.GasPedalViewModel = new GasPedalViewModel();
            this.ACCOptionsViewModel = new ACCOptionsViewModel();
            this.LaneKeepingAndParkingPilotViewModel = new LaneKeepingAndParkingPilotViewModel();

            this.LeftTurnSignalViewModel.Toggle();
            this.RightTurnSignalViewModel.Toggle();
            this.SpeedGaugeViewModel.SetValue(50);

            this.RpmGaugeViewModel.SetValue(3000);                     
            this.BreakPedalViewModel.Value = 75;
            this.GasPedalViewModel.Value = 50;

            this.RpmGaugeViewModel.SetValue(3000);
                     
            this.BreakPedalViewModel = new BreakPedalViewModel();
            this.GasPedalViewModel = new GasPedalViewModel();

            // removing on request by @mrknowitall1
            //this.TransmissionViewModel.CurrentGear = Gear.P;
            //this.TransmissionViewModel.Caption = $"Gear: {this.TransmissionViewModel.CurrentGear}";

            this.ACCOptionsViewModel = new ACCOptionsViewModel();
            this.LaneKeepingAndParkingPilotViewModel = new LaneKeepingAndParkingPilotViewModel();
            this.LastSignViewModel = new LastSignViewModel();
            this.LastSignViewModel.ResourcePath = "/Assets/WorldObjects/roadsign_priority_stop.png";

            this.CarInfoViewModel = new CarInfoViewModel();
            this.CarInfoViewModel.SpeedLimit = 60;
            this.CarInfoViewModel.SteeringWheelAngle = 25;
            this.CarInfoViewModel.X = 350;
            this.CarInfoViewModel.Y = 500;

            this.SteeringWheelViewModel = new SteeringWheelViewModel();
        }

        public AutomatedCar ControlledCar
        {
            get => this._controlledCar;
            private set => this.RaiseAndSetIfChanged(ref this._controlledCar, value);
        }

        public GaugeViewModelBase RpmGaugeViewModel
        {
            get => this._rpmGaugeViewModel;
            set => this.RaiseAndSetIfChanged(ref this._rpmGaugeViewModel, value);
        }

        public GaugeViewModelBase SpeedGaugeViewModel
        {
            get => this._speedGaugeViewModel;
            set => this.RaiseAndSetIfChanged(ref this._speedGaugeViewModel, value);
        }

        public BreakPedalViewModel BreakPedalViewModel
        {
            get => this._breakPedalViewModel;
            set => this.RaiseAndSetIfChanged(ref this._breakPedalViewModel, value);
        }

        public GasPedalViewModel GasPedalViewModel
        {
            get => this._gasPedalViewModel;
            set => this.RaiseAndSetIfChanged(ref this._gasPedalViewModel, value);
        }

        public TransmissionViewModel TransmissionViewModel
        {
            get => this._transmissionViewModel;
            set => this.RaiseAndSetIfChanged(ref this._transmissionViewModel, value);
        }

        public TurnSignalViewModelBase LeftTurnSignalViewModel
        {
            get => this._leftTurnSignalViewModel;
            set => this.RaiseAndSetIfChanged(ref this._leftTurnSignalViewModel, value);
        }

        public TurnSignalViewModelBase RightTurnSignalViewModel
        {
            get => this._rightTurnSignalViewModel;
            set => this.RaiseAndSetIfChanged(ref this._rightTurnSignalViewModel, value);
        }

        public ACCOptionsViewModel ACCOptionsViewModel
        {
            get => this._accOptionsViewModel;
            set => this.RaiseAndSetIfChanged(ref this._accOptionsViewModel, value);
        }

        public LaneKeepingAndParkingPilotViewModel LaneKeepingAndParkingPilotViewModel
        {
            get => this._laneKeepingAndParkingPilotViewModel;
            set => this.RaiseAndSetIfChanged(ref this._laneKeepingAndParkingPilotViewModel, value);
        }

        public LastSignViewModel LastSignViewModel
        {
            get => this._lastSignViewModel;
            set => this.RaiseAndSetIfChanged(ref this._lastSignViewModel, value);
        }

        public CarInfoViewModel CarInfoViewModel
        {
            get => this._carInfoViewModel;
            set => this.RaiseAndSetIfChanged(ref this._carInfoViewModel, value);
        }

        public SteeringWheelViewModel SteeringWheelViewModel
        {
            get => this._steeringWheelViewModel;
            set => this.RaiseAndSetIfChanged(ref this._steeringWheelViewModel, value);
        }

        public void ToggleRightIndicator() => this.RightTurnSignalViewModel.Toggle();

        public void ToggleLeftIndicator() => this.LeftTurnSignalViewModel.Toggle();

        public void ToggleACC() => this.ACCOptionsViewModel.Toggle();

        public void IncreaseACCDesiredSpeed() => this.ACCOptionsViewModel.IncreaseDesiredSpeed();

        public void DecreaseACCDesiredSpeed() => this.ACCOptionsViewModel.DecreaseDesiredSpeed();

        public void SetToNextACCDesiredDistance() => this.ACCOptionsViewModel.SetToNextDesiredDistance();

        public void SetToPreviousACCDesiredDistance() => this.ACCOptionsViewModel.SetToPreviousDesiredDistance();

        public void ToggleParkingPilot() => this.LaneKeepingAndParkingPilotViewModel.ToggleParkingPilot();

        public void ToggleLaneKeeping() => this.LaneKeepingAndParkingPilotViewModel.ToggleLaneKeeping();

        public void DisplayLaneKeepingWarning() => this.LaneKeepingAndParkingPilotViewModel.DisplayLaneKeepingWarning();

        public void ShiftUp() => this.TransmissionViewModel.ShiftUp();

        public void ShiftDown() => this.TransmissionViewModel.ShiftDown();

        public void MoveGasPedalDown(double duration)
        {
            this.GasPedalViewModel.Value = (int)Math.Min(this.GasPedalViewModel.Value + (duration * 100), 100);
        }

        public void MoveGasPedalUp(double duration)
        {
            this.GasPedalViewModel.Value = (int)Math.Max(this.GasPedalViewModel.Value - (duration * 100), 0);
        }

        public void MoveBrakePedalDown(double duration)
        {
            this.BreakPedalViewModel.Value = (int)Math.Min(this.BreakPedalViewModel.Value + (duration * 200), 100);
        }

        public void MoveBrakePedalUp(double duration)
        {
            this.BreakPedalViewModel.Value = (int)Math.Max(this.BreakPedalViewModel.Value - (duration * 200), 0);
        }

        public void SteerLeft(double duration)
        {
            this.SteeringWheelViewModel.Value = (int)Math.Max(this.SteeringWheelViewModel.Value - (duration * 100), -100);
        }

        public void SteerRight(double duration)
        {
            this.SteeringWheelViewModel.Value = (int)Math.Min(this.SteeringWheelViewModel.Value + (duration * 100), 100);
        }

        public void SteerRightToIdle(double duration)
        {
            this.SteeringWheelViewModel.Value = (int)Math.Min(this.SteeringWheelViewModel.Value + (duration * 100), 0);
        }

        public void SteerLeftToIdle(double duration)
        {
            this.SteeringWheelViewModel.Value = (int)Math.Max(this.SteeringWheelViewModel.Value - (duration * 100), 0);
        }

        public void HandlePackets(object sender, EventArgs e)
        {
            if (this.ControlledCar != null)
            {
                VirtualFunctionBus bus = ControlledCar.VirtualFunctionBus;
                if (bus != null)
                {
                    bus.HMIPacket = new HMIPacket(this.GasPedalViewModel.Value, this.BreakPedalViewModel.Value, this.SteeringWheelViewModel.Value, this.TransmissionViewModel.CurrentGear);
                    if (bus.PowertrainPacket != null)
                    {
                        this.CarInfoViewModel.X = bus.PowertrainPacket.X;
                        this.CarInfoViewModel.Y = bus.PowertrainPacket.Y;
                        this.SpeedGaugeViewModel.SetValue(bus.PowertrainPacket.Speed);
                        this.RpmGaugeViewModel.SetValue(bus.PowertrainPacket.Rpm);
                        this.CarInfoViewModel.SteeringWheelAngle = (int)bus.PowertrainPacket.CarHeadingAngle;
                    }
                }
            }
        }
    }
}