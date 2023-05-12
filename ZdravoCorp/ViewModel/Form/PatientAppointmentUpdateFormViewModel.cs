using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ZdravoCorp.ViewModel.Table;

namespace ZdravoCorp.ViewModel
{
    public class PatientAppointmentUpdateFormViewModel : ViewModelBase
    {
        private ObservableCollection<AppointmentViewModel> _appointments;
        public AppointmentViewModel SelectedAppointment;
        public string DateAndTime { get; set; }
        public ICommand UpdateAppointmentCommand { get; }
        public ICommand CloseCommand { get; set; }

        public PatientAppointmentUpdateFormViewModel(Window window, ObservableCollection<AppointmentViewModel> appointments, AppointmentViewModel selectedAppointment) 
        {
            SelectedAppointment = selectedAppointment;
            _appointments = appointments;
            DateAndTime = selectedAppointment.FromDate;
            CloseCommand = new CloseCommand(window);
            UpdateAppointmentCommand = new RelayCommand(UpdateTime, CanUpdateTime);
        }

        public bool CanUpdateTime()
        {
            return !(String.IsNullOrEmpty(DateAndTime))
                    && (PatientAppointmentCreateFormViewModel.ConvertStringToDateTime(this.DateAndTime).CompareTo(DateTime.Now) > 0)
                    && !(PatientAppointmentTableViewModel.IsPastOrWithin24Hours(PatientAppointmentCreateFormViewModel.ConvertStringToDateTime(this.DateAndTime)));
        }

        public void UpdateTime()
        {
            if (CanUpdateTime())
            {
                SelectedAppointment.FromDate = PatientAppointmentCreateFormViewModel.ConvertStringToDateTime(this.DateAndTime).ToString("dd/MM/yyyy HH:mm");

                var newTimeSlot = PatientAppointmentCreateFormViewModel.MakeTimeSlot(PatientAppointmentCreateFormViewModel.ConvertStringToDateTime(this.DateAndTime), 15);

                if(SchedulingService.IsAvailableForUpdate(SelectedAppointment.Appointment.Doctor, newTimeSlot, SelectedAppointment.Appointment)
                    && SchedulingService.IsAvailableForUpdate(SelectedAppointment.Appointment.Patient, newTimeSlot, SelectedAppointment.Appointment))
                {
                    SelectedAppointment.Appointment.TimeSlot = newTimeSlot;
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

    }
}
