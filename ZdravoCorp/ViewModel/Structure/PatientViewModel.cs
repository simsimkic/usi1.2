using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Commands;
using ZdravoCorp.Model;
using ZdravoCorp.Model.Enum;
using ZdravoCorp.View;
using ZdravoCorp.View.Form;
using ZdravoCorp.ViewModel.Form;

namespace ZdravoCorp.ViewModel
{
    public class PatientViewModel : ViewModelBase  
    {
        private Patient _patient;
        public Patient Patient
        {
            get
            {
                return _patient;
            }
            set
            {
                _patient = value;
                OnPropertyChanged(nameof(Patient));
            }
        }

        public int Id => _patient.Id;
        public string Name => _patient.FirstName + " " + _patient.LastName;
        public string Username => _patient.Username;
        public string FirstName => _patient.FirstName;
        public string LastName => _patient.LastName;
        public string Password => _patient.Password;
        public string HasMedicalRecord => (MedicalRecord.Height == 0) ? "Nema zdravstveni karton" : "";

        public MedicalRecordViewModel MedicalRecord { get; set; }
        public PatientViewModel(Patient patient)
        {
            _patient = patient;
            MedicalRecord = new MedicalRecordViewModel(_patient.MedicalRecord);
        }
    }
}
