using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.Commands;
using ZdravoCorp.Model;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Service;

namespace ZdravoCorp.ViewModel
{
    public class PatientAppointmentCreateFormViewModel : ViewModelBase
    {
        private readonly ObservableCollection<DoctorViewModel> _doctors;
        public ObservableCollection<DoctorViewModel> Doctors => _doctors;
        private DoctorViewModel _selectedDoctor;
        public DoctorViewModel SelectedDoctor
        {
            get { return _selectedDoctor; }
            set
            {
                _selectedDoctor = value;
                OnPropertyChanged(nameof(SelectedDoctor));
            }
        }

        public DAO<Doctor> DoctorDAO;
        public Patient Patient;
        private ObservableCollection<AppointmentViewModel> _appointments;
        public string DateAndTime { get; set; }
        public ICommand CreateAppointmentCommand { get; }
        public ICommand CloseCommand { get; set; }
        public PatientAppointmentCreateFormViewModel(Window window, Patient patient, ObservableCollection<AppointmentViewModel> appointments)
        {
            Patient = patient;
            _appointments = appointments;
            DoctorDAO = DAOFactory.GetInstance().DoctorDAO;
            _doctors = new ObservableCollection<DoctorViewModel>();
            DoctorDAO.GetAll().Select(d => new DoctorViewModel(d.Value)).ToList().ForEach(vm => Doctors.Add(vm));

            CloseCommand = new CloseCommand(window);
            CreateAppointmentCommand = new RelayCommand(CreateAppointment, CanCreateAppointment);
        }
        private bool CanCreateAppointment()
        {
            return (SelectedDoctor != null) && !(String.IsNullOrEmpty(DateAndTime))
                && (ConvertStringToDateTime(this.DateAndTime).CompareTo(DateTime.Now) > 0);
        }

        public static DateTime ConvertStringToDateTime(string date)
        {
            return DateTime.ParseExact(date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
        }
        public static TimeSlot MakeTimeSlot(DateTime date, int minutes)
        {    
            return new TimeSlot(date, date.AddMinutes(minutes));
        }
        private void CreateAppointment()
        {
            TimeSlot timeSlot = MakeTimeSlot(ConvertStringToDateTime(this.DateAndTime), 15);
            if (CanCreateAppointment() 
                && (SchedulingService.IsAvailable(SelectedDoctor.Doctor, timeSlot))
                && (SchedulingService.IsAvailable(Patient, timeSlot))) 
            {
                var nextId = DAOFactory.GetInstance().DoctorScheduleDAO.GetAll()[SelectedDoctor.Doctor.Id].GetNextAppointmentId();
                Appointment newAppointment = new Appointment(SelectedDoctor.Doctor, Patient, timeSlot, false, nextId);

                var doctorScheduleService = new DoctorScheduleService(DAOFactory.GetInstance().DoctorScheduleDAO.GetAll()[newAppointment.Doctor.GetId()]);
                doctorScheduleService.AddAppointment(newAppointment);
                
                _appointments.Add(new AppointmentViewModel(newAppointment));
                CloseCommand.Execute(null);
            }
            else
            {
                MessageBox.Show("Doktor ili vi niste slobodni u tom trenutku.");
            }
        }


    }
}
