using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Commands;
using ZdravoCorp.Model;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Service;
using ZdravoCorp.View;
using ZdravoCorp.View.Table;
using ZdravoCorp.ViewModel.Form;
using ZdravoCorp.ViewModel.Structure;

namespace ZdravoCorp.ViewModel.Table
{
    public class PatientMedicalRecordTableViewModel : ViewModelBase
    {
        private readonly ObservableCollection<PatientAnamnesisViewModel> _appointments;
        public ObservableCollection<PatientAnamnesisViewModel> Appointments => _appointments;
        private PatientAnamnesisViewModel _selectedAppointment;
        public PatientAnamnesisViewModel SelectedAppointment
        {
            get { return _selectedAppointment; }
            set
            {
                _selectedAppointment = value;
                OnPropertyChanged(nameof(SelectedAppointment));
            }
        }
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
            }
        }
        public ICommand OpenMedicalRecordCommand { get; }
        public ICommand OpenAnamensisCommand { get ; }
        public ICommand CloseCommand { get; }
        private Patient _patient;
        public PatientMedicalRecordTableViewModel(Window window, Patient patient)
        {
            _patient = patient;
            OpenMedicalRecordCommand = new RelayCommand(OpenMedicalRecord, CanOpen);
            OpenAnamensisCommand = new RelayCommand(OpenAnamnesis, CanOpenAnamnesis);
            CloseCommand = new CloseCommand(window);
            _appointments = new ObservableCollection<PatientAnamnesisViewModel>(SchedulingService.GetAllAppointments(patient).Select(o => new PatientAnamnesisViewModel(o)));
            this.PropertyChanged += OnPropertyChanged;
        }


        public bool CanOpen()
        {
            return true;
        }

        public void OpenMedicalRecord()
        {
            var medicalRecordView = new MedicalRecordDoctorView(new PatientViewModel(_patient));
            medicalRecordView.DataContext = new MedicalRecordFormViewModel(new PatientViewModel(_patient));
            medicalRecordView.ShowDialog();
        }
        public bool CanOpenAnamnesis()
        {
            return SelectedAppointment is not null;
        }
        public void OpenAnamnesis()
        {
            var anamnesisView = new ShowAnamnesisView();
            anamnesisView.DataContext = new ShowAnamnesisViewModel(SelectedAppointment.Appointment);
            anamnesisView.ShowDialog();
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Search))
            {
                if(!string.IsNullOrEmpty(Search))
                {
                    var filteredAppointments = _appointments.Where(appointment => appointment.Doctor.ToLower().Contains(Search.ToLower()) 
                    || appointment.Specialization.ToLower().Contains(Search.ToLower())
                    || appointment.Anamnesis.ToLower().Contains(Search.ToLower())).ToList();
                    _appointments.Clear();
                    foreach(var appointment in filteredAppointments)
                    {
                        _appointments.Add(appointment);
                    }
                }
                else
                {
                    foreach (var appointment in SchedulingService.GetAllAppointments(_patient))
                    {
                        _appointments.Add(new PatientAnamnesisViewModel(appointment));
                    }
                }
            }
        }
    }
}
