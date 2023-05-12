using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Commands;
using ZdravoCorp.Model;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Model.Enum;
using ZdravoCorp.Service;

namespace ZdravoCorp.ViewModel.Table
{
    public class PatientTableViewModel : ViewModelBase
    {
        private readonly ObservableCollection<PatientViewModel> _patients;

        public ObservableCollection<PatientViewModel> Patients => _patients;

        private PatientViewModel _selectedPatient;
        public PatientViewModel SelectedPatient
        {
            get
            {
                return _selectedPatient;
            }
            set
            {
                _selectedPatient = value;
                OnPropertyChanged(nameof(SelectedPatient));
            }
        }

        public ICommand OpenMedicalRecord { get; }
        public ICommand CreateUrgentAppointment { get; }
        public ICommand AddPatient { get; }
        public ICommand UpdatePatient { get; }
        public ICommand RemovePatient { get; }

        // Navigation argument should be added.
        public PatientTableViewModel()
        {
            _patients = new ObservableCollection<PatientViewModel>();
            LoadPatientViewModels();
            OpenMedicalRecord = new OpenMedicalRecordCommand(this);
            AddPatient = new OpenPatientCommand(this);
            UpdatePatient = new OpenPatientCommand(this);
            RemovePatient = new RemovePatientCommand(this);
            CreateUrgentAppointment = new OpenUrgentAppointmentCommand(this);
        }

        private void LoadPatientViewModels()
        {
            _patients.Clear();
            
            // LinQ can`t be used because ObservableCollection does not have a definition for AddRange(). 
            foreach (Patient patient in GetFromDAOService.GetAllPatients())
            {
                _patients.Add(new PatientViewModel(patient));
            }
        }
    }
}
