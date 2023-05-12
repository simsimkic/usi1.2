using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Model;
using ZdravoCorp.Service;
using ZdravoCorp.ViewModel.Form;
using ZdravoCorp.ViewModel.Table;
using System.Windows;

namespace ZdravoCorp.Commands
{
    public class ScheduleAppointmentCommand : CommandBase
    {
        private PatientChoseRecommendedViewModel _patientChoseRecommendedVM;

        public ScheduleAppointmentCommand(PatientChoseRecommendedViewModel patientChoseRecommendedVM)
        {
            _patientChoseRecommendedVM = patientChoseRecommendedVM;
            _patientChoseRecommendedVM.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            return _patientChoseRecommendedVM.SelectedAppointment is not null ;
        }

        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                MessageBoxResult result = MessageBox.Show("Da li zelite da zakazete?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var doctor = _patientChoseRecommendedVM.SelectedAppointment.Appointment.Doctor;
                    var patient = _patientChoseRecommendedVM.SelectedAppointment.Appointment.Patient;
                    var timeSlot = _patientChoseRecommendedVM.SelectedAppointment.Appointment.TimeSlot;
                    var doctorSchedule = DAOFactory.GetInstance().DoctorScheduleDAO.GetAll()[doctor.Id];
                    var nextId = doctorSchedule.GetNextAppointmentId();
                    Appointment newAppointment = new Appointment(doctor, patient, timeSlot, false, nextId);

                    var doctorScheduleService = new DoctorScheduleService(doctorSchedule);
                    doctorScheduleService.AddAppointment(newAppointment);
                    MessageBox.Show("Uspesno ste zakazali pregled.");
                    _patientChoseRecommendedVM.CloseCommand.Execute(_patientChoseRecommendedVM.window);
                }
            }
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_patientChoseRecommendedVM.SelectedAppointment))
            {
                OnCanExecutedChanged();
            }
        }
    }
}
