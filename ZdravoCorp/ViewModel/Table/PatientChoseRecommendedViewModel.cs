using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Commands;
using ZdravoCorp.Model;

namespace ZdravoCorp.ViewModel.Table
{
    public class PatientChoseRecommendedViewModel : ViewModelBase
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
        private Patient Patient { get; set; }
        public ICommand ScheduleAppointmentCommand { get; }
        public ICommand CloseCommand { get; }
        public Window window;
        public PatientChoseRecommendedViewModel(Patient patient, Window window, List<TimeSlot> timeSlots, Doctor doctor)
        {
            _appointments = new ObservableCollection<AppointmentViewModel>();
            foreach (var slot in timeSlots)
            {
                MessageBox.Show(slot.From.ToString());
                MessageBox.Show(slot.To.ToString());
                Appointment appointment = new Appointment(doctor, patient, slot, false, 1);
                _appointments.Add(new AppointmentViewModel(appointment));
            }
            Patient = patient;
            CloseCommand = new CloseCommand(window);
            ScheduleAppointmentCommand = new ScheduleAppointmentCommand(this);
        }

    }
}
