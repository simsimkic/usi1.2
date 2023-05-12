using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Model;
using ZdravoCorp.Service;
using ZdravoCorp.ViewModel;
using ZdravoCorp.ViewModel.Form;
using ZdravoCorp.ViewModel.Table;

namespace ZdravoCorp.Commands
{
    internal class DelayAndCreateAppointmentCommand : CommandBase
    {
        private readonly DelayableAppointmentTableViewModel _delayableAppointmentTableViewModel;

        public DelayAndCreateAppointmentCommand(DelayableAppointmentTableViewModel delayableAppointmentTableViewModel)
        {
            _delayableAppointmentTableViewModel = delayableAppointmentTableViewModel;
            _delayableAppointmentTableViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            return _delayableAppointmentTableViewModel.SelectedAppointment is not null;
        }

        public override void Execute(object? parameter)
        {
            var doctorScheduleService = GetDoctorSchedule();
            var timeSlots = GetTimeSlots(doctorScheduleService);
            try
            {
                doctorScheduleService.RescheduleAppointment(GetDelayedAppointment(), timeSlots.Item2);
                var patient = GetUrgentPatient();
                var doctor = GetDoctor();
                SchedulingService.CreateUrgentAppointment(doctor, patient, timeSlots.Item1, IsUrgentOperation());
                NotificationService.CreatDelayedAppointmentNotification(doctor.Id, GetDelayedPatient().Id, timeSlots.Item1, timeSlots.Item2);
                NotificationService.CreateNewAppointmentNotification(doctor.Id, patient.Id, timeSlots.Item1);

                MessageBox.Show("Uspešno ste odlozili postojeći i zakazali urgentni pregled", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Greška prilikom odlaganja postojećeg i zakazivanja urgentnog pregleda.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_delayableAppointmentTableViewModel.SelectedAppointment))
            {
                OnCanExecutedChanged();
            };
        }

        private Tuple<TimeSlot, TimeSlot> GetTimeSlots(DoctorScheduleService doctorScheduleService)
        {
            var selectedTimeSlot = _delayableAppointmentTableViewModel.SelectedAppointment.TimeSlot;
            selectedTimeSlot.CutTo(_delayableAppointmentTableViewModel.UrgentAppointmentForm.GetDuration());

            var today = DateTime.Today;
            var delayedTimeSlot = doctorScheduleService.FindNextFree(new TimeSlot(today.AddHours(20)), new TimeSlot(today.AddHours(8), today.AddHours(20)), _delayableAppointmentTableViewModel.SelectedAppointment.GetDuration());

            return new Tuple<TimeSlot, TimeSlot>(selectedTimeSlot, delayedTimeSlot);
        }

        private Patient GetUrgentPatient()
        {
            return GetFromDAOService.GetPatientById(_delayableAppointmentTableViewModel.UrgentAppointmentForm.SelectedPatient.Id);
        }

        private Patient GetDelayedPatient()
        {
            return GetFromDAOService.GetPatientById(_delayableAppointmentTableViewModel.SelectedAppointment.PatientId);
        }

        private Appointment GetDelayedAppointment()
        {
            return _delayableAppointmentTableViewModel.SelectedAppointment.Appointment;
        }

        private Doctor GetDoctor()
        {
            return GetFromDAOService.GetDoctorById(_delayableAppointmentTableViewModel.SelectedAppointment.DoctorId);
        }

        private DoctorScheduleService GetDoctorSchedule()
        {
            return new DoctorScheduleService(GetFromDAOService.GetScheduleById(_delayableAppointmentTableViewModel.SelectedAppointment.DoctorId));
        }

        private bool IsUrgentOperation()
        {
            return _delayableAppointmentTableViewModel.UrgentAppointmentForm.IsOperation;
        }
    }
}
