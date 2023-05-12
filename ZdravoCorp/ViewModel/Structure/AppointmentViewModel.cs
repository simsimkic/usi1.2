using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Commands;
using ZdravoCorp.Model;
using ZdravoCorp.View;

namespace ZdravoCorp.ViewModel
{
    public class AppointmentViewModel : ViewModelBase
    {
        public  Appointment Appointment;
        
        public string Patient => Appointment.Patient.FirstName + " " + Appointment.Patient.LastName;
        public string Doctor => Appointment.Doctor.FirstName + " "  + Appointment.Doctor.LastName;

        public int PatientId => Appointment.Patient.Id;

        public int DoctorId => Appointment.Doctor.Id;

        public TimeSlot TimeSlot => Appointment.TimeSlot;
        
        public string IsOperation => Appointment.IsOperation ? "Operacija" : "Pregled";
        private string _fromDate;
        public string FromDate
        {
            get => Appointment.TimeSlot.From.ToString("dd/MM/yyyy hh:mm");
            set => _fromDate = value;
        }

        public string ToDate
        {
            get => Appointment.TimeSlot.To.ToString("dd/MM/yyyy hh:mm");
            set => _fromDate = value;
        }

        public string Duration => (Appointment.TimeSlot.To - Appointment.TimeSlot.From).TotalMinutes.ToString();
        public string IsCanceled  => Appointment.IsCanceled ? "Otkazana" : "Zakazana";
        public ICommand OpenMedicalRecordCommand { get; }
        public AppointmentViewModel(Appointment appointment) 
        {
            OpenMedicalRecordCommand = new RelayCommand(OpenMedicalRecord, CanCreateRow);
            Appointment = appointment;
        }

        public bool CanCreateRow()
        {
            return true;
        }

        public void OpenMedicalRecord()
        {
            var createAppointmentView = new MedicalRecordDoctorView(new PatientViewModel(Appointment.Patient));
            createAppointmentView.ShowDialog();
        }

        public int GetDuration()
        {
            return (int)(Appointment.TimeSlot.To - Appointment.TimeSlot.From).TotalMinutes;
        }
    }
}
