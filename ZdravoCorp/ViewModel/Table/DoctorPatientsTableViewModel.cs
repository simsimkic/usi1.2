using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Commands;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Model;
using ZdravoCorp.View;
using System.ComponentModel;

namespace ZdravoCorp.ViewModel.Table
{
    public class DoctorPatientsTableViewModel: ViewModelBase
    {
        private readonly DAO<Patient> PatientDAO = DAOFactory.GetInstance().PatientDAO;

        private readonly ObservableCollection<PatientViewModel> _patients;

        public ObservableCollection<PatientViewModel> Patients => _patients;
        
        private string _search;
        public string Search
        {
            get
            {
                return _search;
            }
            set
            {
                _search = value.ToLower();
                OnPropertyChanged(nameof(Search));
                SearchPatient();
                //nekafunckija
            }
        }
        private Doctor Doctor { get; set; }
        
        public ICommand OpenMedicalRecord { get; }
        private PatientViewModel _selectedPatient;
        //public event EventHandler<PropertyChangedEventArgs> PropertyChanged;
        public PatientViewModel SelectedPatient
        {
            get
            {
                return _selectedPatient;
            }
            set
            {
                _selectedPatient = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedPatient)));
                OnPropertyChanged(nameof(SelectedPatient));
            }
        }

        public DoctorPatientsTableViewModel(Doctor doctor)
        {
            Doctor = doctor;
            _patients = new ObservableCollection<PatientViewModel>();
            LoadPatientsViewModels();
            OpenMedicalRecord = new OpenMedicalRecordCommand(this, doctor);
        }

        private void LoadPatientsViewModels()
        {
            _patients.Clear();
            // LinQ can`t be used because ObservableCollection does not have a definition for AddRange(). 
            foreach (Patient patient in PatientDAO.GetAll().Values)
            {
                _patients.Add(new PatientViewModel(patient));
            }
        }

        private void SearchPatient()
        {
            if (!string.IsNullOrEmpty(Search))
            {
                var SearchedPatients = new ObservableCollection<PatientViewModel>(_patients.Where(obj => obj.FirstName.ToString().ToLower().Contains(Search) || obj.LastName.ToString().ToLower().Contains(Search)));
                _patients.Clear();
                foreach(PatientViewModel Patient in SearchedPatients)
                {
                    _patients.Add(Patient);
                }

            }
            else
            {
                LoadPatientsViewModels();
            }

        }
    }

}
