using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using ZdravoCorp.Commands;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Model;
using System.Text.RegularExpressions;
using System.Numerics;
using ZdravoCorp.Service;

namespace ZdravoCorp.ViewModel.Form
{
    public class DoctorAppointmentCreateFormViewModel: ViewModelBase
    {
        private readonly ObservableCollection<PatientViewModel> _patients;
        public ObservableCollection<PatientViewModel> Patients => _patients;
        private ObservableCollection<AppointmentViewModel> _appointments;
        private PatientViewModel _selectedPatient;
        public Doctor Doctor;
        public PatientViewModel SelectedPatient
        {
            get { return _selectedPatient; }
            set
            {
                _selectedPatient = value;
                OnPropertyChanged(nameof(SelectedPatient));
            }
        }

        public DAO<Patient> PatientDAO;
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

        public string DateAndTime { get; set; }
        public ICommand EnableTextBoxCommand { get; private set; }
        public ICommand DisableTextBoxCommand { get; private set; }
        public ICommand CreateAppointmentCommand { get; }
        public ICommand CloseCommand { get; set; }
        public DoctorAppointmentCreateFormViewModel(Window window, Doctor doctor, ObservableCollection<AppointmentViewModel> appointments)
        {
            Doctor = doctor;
            _appointments = appointments;
            PatientDAO = DAOFactory.GetInstance().PatientDAO;
            _patients = new ObservableCollection<PatientViewModel>();
            PatientDAO.GetAll().Select(d => new PatientViewModel(d.Value)).ToList().ForEach(vm => Patients.Add(vm));

            CloseCommand = new CloseCommand(window);
            EnableTextBoxCommand = new RelayCommand(EnableTextBox, CanExecute);
            DisableTextBoxCommand = new RelayCommand(DisableTextBox, CanExecute);
            CreateAppointmentCommand = new RelayCommand(CreateAppointment, CanCreateAppointment);
        }
        private bool CanCreateAppointment()
        {
            return (SelectedPatient != null) && !(String.IsNullOrEmpty(DateAndTime))
                && (ConvertStringToDateTime(this.DateAndTime).CompareTo(DateTime.Now) > 0);
        }
        private int GetDuration()
        {
            if (IsAppointmentSelected)
            {
                return 15;
            }
            return int.Parse(Duration);
        }
       
        public static DateTime ConvertStringToDateTime(string date)
        {
            if (date.Contains("/"))
            {
                return DateTime.ParseExact(date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            }
            return DateTime.ParseExact(date, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture); 
        }
        public static TimeSlot CreateTimeSlot(DateTime date, int minutes)
        {
            return new TimeSlot(date, date.AddMinutes(minutes));
        }

        private void CreateAppointment()
        {
            TimeSlot timeSlot = CreateTimeSlot(ConvertStringToDateTime(this.DateAndTime), GetDuration());
            if (CanCreateAppointment()
                && (SchedulingService.IsAvailable(Doctor, timeSlot))
                && (SchedulingService.IsAvailable(SelectedPatient.Patient, timeSlot)))
            {
                // Can maybe make function out of this?!
                var nextId = DAOFactory.GetInstance().DoctorScheduleDAO.GetAll()[Doctor.Id].GetNextAppointmentId();
                Appointment newAppointment = new Appointment(Doctor, SelectedPatient.Patient, timeSlot, IsOperationSelected, nextId);

                var doctorScheduleService = new DoctorScheduleService(DAOFactory.GetInstance().DoctorScheduleDAO.GetAll()[newAppointment.Doctor.GetId()]);
                doctorScheduleService.AddAppointment(newAppointment);

                _appointments.Add(new AppointmentViewModel(newAppointment));
                CloseCommand.Execute(null);
            }
            else
            {
                MessageBox.Show("Pacijent ili vi niste slobodni u tom trenutku.");
            }
        }
        public bool CanExecute()
        {
            return true;
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
