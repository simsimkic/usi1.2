using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;
using ZdravoCorp.Commands;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Model;
using System.Text.RegularExpressions;
using static Xceed.Wpf.Toolkit.Calculator;
using ZdravoCorp.ViewModel.Table;
using ZdravoCorp.Service;
using System.Globalization;

namespace ZdravoCorp.ViewModel.Form
{
    public class DoctorAppointmentUpdateFormViewModel : ViewModelBase
    {
        private ObservableCollection<AppointmentViewModel> _appointments;
        public AppointmentViewModel SelectedAppointment;
        public string DateAndTime { get; set; }
        private string _duration;
        public string Duration
        {
            get
            {
                return _duration;

            }
            set
            {
                if (Regex.IsMatch(value, "^[0-9]+$"))
                {
                    _duration = value;
                    OnPropertyChanged(nameof(Duration));
                }
            }
        }

        private bool _isOperationSelected;
        public bool IsOperationSelected
        {
            get { return _isOperationSelected; }
            set
            {
                if (_isOperationSelected != value)
                {
                    _isOperationSelected = value;
                    OnPropertyChanged(nameof(IsOperationSelected));
                }
            }
        }

        private bool _isAppointmentSelected;
        public bool IsAppointmentSelected
        {
            get { return _isAppointmentSelected; }
            set
            {
                if (_isAppointmentSelected != value)
                {
                    _isAppointmentSelected = value;
                    OnPropertyChanged(nameof(IsAppointmentSelected));
                }
            }
        }
        private bool _isTextBoxEnabled;
        public bool IsTextBoxEnabled
        {
            get { return _isTextBoxEnabled; }
            set
            {
                _isTextBoxEnabled = value;
                OnPropertyChanged(nameof(IsTextBoxEnabled));
            }
        }
        public ICommand UpdateAppointmentCommand { get; }
        public ICommand CloseCommand { get; set; }
        public ICommand EnableTextBoxCommand { get; private set; }
        public ICommand DisableTextBoxCommand { get; private set; }

        public DoctorAppointmentUpdateFormViewModel(Window window, ObservableCollection<AppointmentViewModel> appointments, AppointmentViewModel selectedAppointment)
        {
            SelectedAppointment = selectedAppointment;
            _appointments = appointments;
            DateAndTime = selectedAppointment.FromDate;
            Duration = selectedAppointment.Duration;

            IsOperationSelected = IsOperation(selectedAppointment.IsOperation);
            IsAppointmentSelected = !IsOperationSelected;

            CloseCommand = new CloseCommand(window);
            EnableTextBoxCommand = new RelayCommand(EnableTextBox, CanExecute);
            DisableTextBoxCommand = new RelayCommand(DisableTextBox, CanExecute);
            UpdateAppointmentCommand = new RelayCommand(UpdateAppointment, CanUpdateAppointment);

        }

        private bool IsOperation(string  operation)
        {
            return (operation == "Operacija");
        }

        public bool CanUpdateAppointment()
        {
            return !(String.IsNullOrEmpty(DateAndTime))
                    && (DoctorAppointmentCreateFormViewModel.ConvertStringToDateTime(this.DateAndTime).CompareTo(DateTime.Now) > 0)
                    && (!DoctorAppointmentTableViewModel.IsPastOrWithin24Hours(DoctorAppointmentCreateFormViewModel.ConvertStringToDateTime(this.DateAndTime)));
        }

        public void UpdateAppointment()
        {
            if (CanUpdateAppointment())
            {
                SelectedAppointment.FromDate = DoctorAppointmentCreateFormViewModel.ConvertStringToDateTime(this.DateAndTime).ToString("dd.MM.yyyy hh:mm");
                TimeSlot newTimeSlot = DoctorAppointmentCreateFormViewModel.CreateTimeSlot(DoctorAppointmentCreateFormViewModel.ConvertStringToDateTime(this.DateAndTime), GetDuration());

                if ((SchedulingService.IsAvailableForUpdate(SelectedAppointment.Appointment.Doctor, newTimeSlot, SelectedAppointment.Appointment))
                    && (SchedulingService.IsAvailableForUpdate(SelectedAppointment.Appointment.Patient, newTimeSlot, SelectedAppointment.Appointment)))
                {
                    SelectedAppointment.Appointment.TimeSlot = newTimeSlot;
                    SelectedAppointment.Appointment.IsOperation = IsOperationSelected;
                    var doctorScheduleService = new DoctorScheduleService(DAOFactory.GetInstance().DoctorScheduleDAO.GetAll()[SelectedAppointment.Appointment.Doctor.GetId()]);
                    doctorScheduleService.UpdateAppointment(SelectedAppointment.Appointment);
                    CollectionViewSource.GetDefaultView(_appointments).Refresh();
                    CloseCommand.Execute(null);
                }
                else
                {
                    MessageBox.Show("Ne mozete izmeniti ovo vreme");
                }
            }
        }

        public bool CanExecute()
        {
            return true;
        }
        private int GetDuration()
        {
            if (IsAppointmentSelected)
            {
                return 15;
            }
            return int.Parse(Duration);
        }
        private void EnableTextBox()
        {
            IsTextBoxEnabled = true;
        }

        private void DisableTextBox()
        {
            IsTextBoxEnabled = false;
        }
    }
}
