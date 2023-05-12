using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Commands;
using ZdravoCorp.Model;
using ZdravoCorp.Model.Enum;
using ZdravoCorp.Service;
using ZdravoCorp.ViewModel.Form;

namespace ZdravoCorp.ViewModel.Table
{
    public class DelayableAppointmentTableViewModel : ViewModelBase
    {

		public UrgentAppointmentFormViewModel UrgentAppointmentForm;

        private readonly ObservableCollection<AppointmentViewModel> _appointments;

        public ObservableCollection<AppointmentViewModel> Appointments => _appointments;

        private AppointmentViewModel _selectedAppointment;
		public AppointmentViewModel SelectedAppointment
		{
			get
			{
				return _selectedAppointment;
			}
			set
			{
				_selectedAppointment = value;
				OnPropertyChanged(nameof(SelectedAppointment));
			}
		}

		public ICommand DelayAndCreateAppointment { get; }

        public DelayableAppointmentTableViewModel(UrgentAppointmentFormViewModel urgentAppointmentForm)
        {
			UrgentAppointmentForm = urgentAppointmentForm;
            _appointments = new ObservableCollection<AppointmentViewModel>();
            LoadAppointmentViewModels();
            DelayAndCreateAppointment = new DelayAndCreateAppointmentCommand(this);
        }

		private void LoadAppointmentViewModels()
		{
            var delayableAppointments = SchedulingService.GetDelayableAppointments((Specialization)UrgentAppointmentForm.SelectedSpecialization, new TimeSlot(2), new TimeSlot(DateTime.Today.AddHours(20)), UrgentAppointmentForm.Duration, 5);

			foreach (KeyValuePair<int, Appointment> pair in delayableAppointments)
			{
				_appointments.Add(new AppointmentViewModel(pair.Value));
			}
		}
    }
}
