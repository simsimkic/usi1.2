using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Model;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Model.Enum;
using ZdravoCorp.Service;
using ZdravoCorp.View;
using ZdravoCorp.View.Table;
using ZdravoCorp.ViewModel;
using ZdravoCorp.ViewModel.Form;
using ZdravoCorp.ViewModel.Table;

namespace ZdravoCorp.Commands
{
    public class FindAppointmentCommand : CommandBase
    {
        private ViewModel.Form.PatientMedicalRecordTableViewModel _patientAdvancedAppointmentSchedulingViewModel;
        private TimeOnly _from;
        private TimeOnly _to;
        public FindAppointmentCommand(ViewModel.Form.PatientMedicalRecordTableViewModel patientAdvancedAppointmentSchedulingViewModel) 
        {
            _patientAdvancedAppointmentSchedulingViewModel = patientAdvancedAppointmentSchedulingViewModel;
            _patientAdvancedAppointmentSchedulingViewModel.PropertyChanged += OnViewModelPropertyChanged;
            
        }


        public override bool CanExecute(object? parameter)
        {
            //MessageBox.Show((_patientAdvancedAppointmentSchedulingViewModel.SelectedDoctor is not null).ToString());
            return ((_patientAdvancedAppointmentSchedulingViewModel.SelectedDoctor is not null) 
                && !(string.IsNullOrEmpty(_patientAdvancedAppointmentSchedulingViewModel.From)) 
                && !(string.IsNullOrEmpty(_patientAdvancedAppointmentSchedulingViewModel.To)) 
                && ((_patientAdvancedAppointmentSchedulingViewModel.IsDoctorSelected)) || (_patientAdvancedAppointmentSchedulingViewModel.IsTimeSelected))
                && !(_patientAdvancedAppointmentSchedulingViewModel.Date == null);
        }   
        public override void Execute(object? parameter)
        {
            if(IsTimeOk() && IsDateOk()) 
            {
                List<TimeSlot> closestTimeSlots;
                var selectedDoctor = _patientAdvancedAppointmentSchedulingViewModel.SelectedDoctor;
                var doctorId = selectedDoctor.Doctor.Id;
                var latestTime = _patientAdvancedAppointmentSchedulingViewModel.Date;
                var span = CreateSpan(Convert(_patientAdvancedAppointmentSchedulingViewModel.From), Convert(_patientAdvancedAppointmentSchedulingViewModel.To));
                DoctorSchedule doctorSchedule = DAOFactory.GetInstance().DoctorScheduleDAO.GetAll()[doctorId];
                var doctorScheduleService = new DoctorScheduleService(doctorSchedule);

                if (_patientAdvancedAppointmentSchedulingViewModel.IsDoctorSelected)
                {
                    closestTimeSlots = doctorScheduleService.GetClosestTimeSlots(span, latestTime);
                    ScheduleAppointmentInOutOfSpan(closestTimeSlots, selectedDoctor, doctorSchedule);
                }
                else
                {
                    closestTimeSlots = SchedulingService.GetClosestTimeSlot(selectedDoctor.Doctor, span, latestTime);
                    ScheduleAppointmentInOutOfSpan(closestTimeSlots, selectedDoctor, doctorSchedule);
                }
            }
            else
            {
                MessageBox.Show("Pocetno vreme ne moze biti jednako ili vece od krajnjeg. Datum mora biti u buducnosti.");
            }
        }
        public void ScheduleAppointment(TimeSlot timeSlot, Doctor doctor, DoctorSchedule doctorSchedule)
        {
            MessageBoxResult result = MessageBox.Show("Ovo je prvi slobodan termin : " + timeSlot.From + ". Da li zelite da zakazete?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var nextId = DAOFactory.GetInstance().DoctorScheduleDAO.GetAll()[doctor.Id].GetNextAppointmentId();
                var patient = _patientAdvancedAppointmentSchedulingViewModel.Patient;
                Appointment newAppointment = new Appointment(doctor, patient, timeSlot, false, nextId);

                var doctorScheduleService = new DoctorScheduleService(doctorSchedule);
                doctorScheduleService.AddAppointment(newAppointment);

                MessageBox.Show("Uspesno ste zakazali pregled.");
            }

        }
        public void ScheduleAppointmentInOutOfSpan(List<TimeSlot> closestTimeSlots, DoctorViewModel selectedDoctor, DoctorSchedule doctorSchedule)
        {
            if (closestTimeSlots.Count == 1)
            {
                ScheduleAppointment(closestTimeSlots[0], selectedDoctor.Doctor, doctorSchedule);
            }
            else
            {
                var scheduleAppointments = new PatientChoseRecommendedView();
                scheduleAppointments.DataContext = new PatientChoseRecommendedViewModel(_patientAdvancedAppointmentSchedulingViewModel.Patient, scheduleAppointments, closestTimeSlots, selectedDoctor.Doctor);
                scheduleAppointments.ShowDialog();
            }
        }
        
        public TimeSlot CreateSpan(TimeOnly from, TimeOnly to)
        {
            TimeSlot span = new TimeSlot();
            DateTime now = DateTime.Now;
            if (to.Hour < now.Hour || from.Hour < now.Hour)
            {
                now = now.AddDays(1);
            }
            span.From = new DateTime(now.Year, now.Month, now.Day, from.Hour, from.Minute, from.Second);
            span.To = new DateTime(now.Year, now.Month, now.Day, to.Hour, to.Minute, to.Second);

            return span;
        }
        public bool IsTimeOk()
        {
            var from = Convert(_patientAdvancedAppointmentSchedulingViewModel.From);
            var to = Convert(_patientAdvancedAppointmentSchedulingViewModel.To);
            if (from >= to)
            {
                return false;
            }
            return true;
        }
        public bool IsDateOk()
        {
            return _patientAdvancedAppointmentSchedulingViewModel.Date.Date > DateTime.Now.Date;
        }
        public TimeOnly Convert(string time)
        {
            return  TimeOnly.Parse(time);
        }
        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_patientAdvancedAppointmentSchedulingViewModel.SelectedDoctor))
            {
                OnCanExecutedChanged();
            }
        }
    }
}
