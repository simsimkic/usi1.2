using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;
using ZdravoCorp.Commands;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Model.Enum;
using ZdravoCorp.Model;
using ZdravoCorp.View;
using System.Collections.Specialized;
using System.IO.Packaging;
using System.Globalization;
using ZdravoCorp.Service;
using System.Numerics;

namespace ZdravoCorp.ViewModel.Table
{
    public class DoctorAppointmentTableViewModel: ViewModelBase, INotifyPropertyChanged
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
                GenerateDays(value);
                OnPropertyChanged(nameof(Date));
            }
        }

        private DAOFactory _dAOFactory;
        public ICommand CreateAppointmentCommand { get; }
        public ICommand UpdateAppointmentCommand { get; }
        public ICommand CancelAppointmentCommand { get; }

        public DoctorAppointmentTableViewModel(Doctor doctor)
        {
            Doctor = doctor;
            Date = DateTime.Now;
            _dAOFactory = DAOFactory.GetInstance();
            _appointments = new ObservableCollection<AppointmentViewModel>();
            List<Appointment> appointments = SchedulingService.GetAllAppointments(doctor);

            foreach (Appointment appointment in appointments)
            {
                _appointments.Add(new AppointmentViewModel(appointment));
            }

            CreateAppointmentCommand = new RelayCommand(CreateRow, CanCreateRow);
            CancelAppointmentCommand = new RelayCommand(CancelSelectedRow, CanCancelSelectedRow);
            UpdateAppointmentCommand = new RelayCommand(UpdateAppointment, CanCancelSelectedRow);
        }

        private bool CanCancelSelectedRow()
        {
            return (SelectedAppointment != null) && !(SelectedAppointment.Appointment.IsCanceled) && !(IsPastOrWithin24Hours(SelectedAppointment.Appointment.TimeSlot.From));
        }

        private void UpdateListOfAppointments(List<DateTime> nextThreeDays)
        {
            _appointments = new ObservableCollection<AppointmentViewModel>();
            _dAOFactory = DAOFactory.GetInstance();
            foreach (var day in nextThreeDays)
            {
                foreach (var appointment in SchedulingService.GetAllAppointments(Doctor))
                {
                        if (day.Year == appointment.TimeSlot.From.Year && day.Month == appointment.TimeSlot.From.Month && day.Day == appointment.TimeSlot.From.Day)
                        {
                            _appointments.Add(new AppointmentViewModel(appointment));
                        }
                }
            }
            OnPropertyChanged(nameof(Appointments));
            CollectionViewSource.GetDefaultView(Appointments).Refresh();
        }
        public static bool IsPastOrWithin24Hours(DateTime time)
        {
            return (time.CompareTo(DateTime.Now) < 0) || ((time - DateTime.Now).TotalHours < 24); ;
        }

        private void CancelSelectedRow()
        {
            if (CanCancelSelectedRow())
            {
                MessageBoxResult result = MessageBox.Show("Da li ste sigurni da želite otkazati pregled?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SelectedAppointment.Appointment.IsCanceled = true;
                    foreach (var appointment in SchedulingService.GetAllAppointments(SelectedAppointment.Appointment.Patient, Doctor))
                    {
                        if (appointment.Id == SelectedAppointment.Appointment.Id)
                        {
                            appointment.IsCanceled = true;
                        }
                    }
                    CollectionViewSource.GetDefaultView(Appointments).Refresh();
                }
            }
        }

        private DateTime ConvertString(string date)
        {
            DateTime result;
            if (!DateTime.TryParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                throw new ArgumentException("Input string is not in the correct format", nameof(date));
            }
            return result;
        }

        public bool CanCreateRow()
        {
            return true;
        }

        private void GenerateDays(DateTime currentDate)
        {
            List<DateTime> nextThreeDays = new List<DateTime>();
            nextThreeDays.Add(currentDate);
            for (int i = 1; i <= 3; i++)
            {
                DateTime nextDay = currentDate.AddDays(i);
                nextThreeDays.Add(nextDay);
            }

            UpdateListOfAppointments(nextThreeDays);
        }

        public void CreateRow()
        {
            var createAppointmentView = new AppointmentView(Doctor, _appointments);
            createAppointmentView.ShowDialog();
        }

        private void UpdateAppointment()
        {
            var updateAppointmentView = new UpdateAppointmentDoctor(_appointments, SelectedAppointment);
            updateAppointmentView.ShowDialog();
        }
    }
}
