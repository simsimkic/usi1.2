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
using ZdravoCorp.Model.Enum;
using ZdravoCorp.View;

namespace ZdravoCorp.ViewModel.Structure
{
    public class PatientAnamnesisViewModel : ViewModelBase
    {
        public Appointment Appointment;
        public string Doctor => Appointment.Doctor.FirstName + " " + Appointment.Doctor.LastName;
        public string Specialization => Appointment.Doctor.Specialization.ToString();
        public string Anamnesis => (Appointment.Anamnesis?.Symptoms ?? " ") + "  " + (Appointment.Anamnesis?.Observations ?? " ") + " " + (Appointment.Anamnesis?.Conclusions ?? " ");
        public PatientAnamnesisViewModel(Appointment appointment)
        {
            Appointment = appointment;
        }

        
    }
}
