using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;
using ZdravoCorp.Commands;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Model;
using ZdravoCorp.Service;
using ZdravoCorp.View;
using System.ComponentModel;

namespace ZdravoCorp.ViewModel.Table
{
    public class DoctorExaminationTableViewModel: ViewModelBase, INotifyPropertyChanged
    {
        private ObservableCollection<AppointmentViewModel> _appointments;
        public ObservableCollection<AppointmentViewModel> Appointments => _appointments;
        private AppointmentViewModel _selectedAppointment;

        public AppointmentViewModel SelectedAppointment
        {
            get { return _selectedAppointment; }
            set
            {
                _selectedAppointment = value;
                OnPropertyChanged(nameof(SelectedAppointment));
            }
        }

        private Doctor Doctor { get; set; }
        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                //GenerateDays(value);
                OnPropertyChanged(nameof(Date));
            }
        }

        private DAOFactory _dAOFactory;
        public ICommand StartExaminationCommand { get; }

        public DoctorExaminationTableViewModel(Doctor doctor)
        {
            Doctor = doctor;
            Date = DateTime.Now;
            _dAOFactory = DAOFactory.GetInstance();
            _appointments = new ObservableCollection<AppointmentViewModel>();
            List<Appointment> appointments = SchedulingService.GetAllAppointments(doctor);

            DateTime day = DateTime.Now;
            foreach (Appointment appointment in appointments)
            {
                if (day.Year == appointment.TimeSlot.From.Year && day.Month == appointment.TimeSlot.From.Month 
                    && day.Day == appointment.TimeSlot.From.Day && appointment.IsCanceled == false)
                {
                    _appointments.Add(new AppointmentViewModel(appointment));
                }
            }

            StartExaminationCommand = new RelayCommand(StartExamination, CanExecute);
        }


        public bool CanExecute()
        {
            return true;
        }


        public void StartExamination()
        {
            var createAppointmentView = new ExaminationMedicalRecordView(new PatientViewModel(SelectedAppointment.Appointment.Patient), SelectedAppointment);
            createAppointmentView.ShowDialog();
        }

        public bool IsDateTimeValid(DateTime currentTime, DateTime start, DateTime end)
        {
            TimeSpan threshold = TimeSpan.FromMinutes(15);
            TimeSpan difference = start.Subtract(currentTime);

            if (difference < threshold)
            {
                return true;
            }

            bool isInBetween = currentTime >= start && currentTime <= end;

            return isInBetween;
        }
    }
}
