using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.Commands;
using ZdravoCorp.Model;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Model.Enum;
using ZdravoCorp.Service;
using ZdravoCorp.View;

namespace ZdravoCorp.ViewModel.Table
{
    public class PatientAppointmentTableViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly ObservableCollection<AppointmentViewModel> _appointments;
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
        private DAOFactory _dAOFactory;
        private Patient Patient { get; set; }
        public ICommand CreateAppointmentCommand { get; }
        public ICommand UpdateAppointmentCommand { get; }
        public ICommand CancelAppointmentCommand { get; }

        public PatientAppointmentTableViewModel(Patient patient)
        {
            Patient = patient;
            _dAOFactory = DAOFactory.GetInstance();
            _appointments = new ObservableCollection<AppointmentViewModel>();
            foreach (var appointment in SchedulingService.GetAllAppointments(patient))
            {
                   _appointments.Add(new AppointmentViewModel(appointment));
            }
            CancelAppointmentCommand = new RelayCommand(CancelSelectedRow, CanCancelSelectedRow);
            CreateAppointmentCommand = new RelayCommand(CreateRow, CanCreateRow);
            UpdateAppointmentCommand = new RelayCommand(UpdateAppointment, CanUpdateAppointment);
        }

        private bool CanUpdateAppointment()
        {
            return CanCancelSelectedRow();
        }
        private void UpdateAppointment()
        {
            var updateAppointmentView = new UpdateAppointmentTimeView(_appointments, SelectedAppointment);
            updateAppointmentView.ShowDialog();
        }
        private bool CanCancelSelectedRow()
        {
            return (SelectedAppointment != null) && !(SelectedAppointment.Appointment.IsCanceled) && !(IsPastOrWithin24Hours(SelectedAppointment.Appointment.TimeSlot.From));
        }

        public static bool IsPastOrWithin24Hours(DateTime time)
        {
            return (time.CompareTo(DateTime.Now) < 0) || ((time - DateTime.Now).TotalHours < 24);
        }

        private void CancelSelectedRow()
        {
            if (CanCancelSelectedRow())
            {
                MessageBoxResult result = MessageBox.Show("Da li ste sigurni da želite otkazati pregled?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SelectedAppointment.Appointment.IsCanceled = true;
                    foreach (var appointment in SchedulingService.GetAllAppointments(Patient, SelectedAppointment.Appointment.Doctor))
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

        public bool CanCreateRow()
        {
            return true;
        }

        public void CreateRow()
        {
            var createAppointmentView = new CreateAppointmentView(Patient, _appointments);
            createAppointmentView.ShowDialog();
        }
    }
}

