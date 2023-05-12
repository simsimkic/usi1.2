using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Model;
using ZdravoCorp.Service;
using ZdravoCorp.View;
using ZdravoCorp.View.Form;
using ZdravoCorp.ViewModel;
using ZdravoCorp.ViewModel.Form;
using ZdravoCorp.ViewModel.Table;

namespace ZdravoCorp.Commands
{
    public class OpenMedicalRecordCommand : CommandBase
    {
        private readonly PatientTableViewModel _patientTableViewModel;
        private readonly DoctorPatientsTableViewModel _doctorPatientsTableViewModel;
        public PatientViewModel SelectedPatient;
        public Doctor Doctor;

        public OpenMedicalRecordCommand(PatientTableViewModel patientTableViewModel)
        {
            _patientTableViewModel = patientTableViewModel;
            SelectedPatient = patientTableViewModel.SelectedPatient;
            _patientTableViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }
        public OpenMedicalRecordCommand(DoctorPatientsTableViewModel doctorPatientsTableViewModel, Doctor doctor)
        {
            Doctor = doctor;
            _doctorPatientsTableViewModel = doctorPatientsTableViewModel;
            SelectedPatient = doctorPatientsTableViewModel.SelectedPatient;
            _doctorPatientsTableViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            return (GetSelectedPatient() is not null) && CheckPatient();
        }
        public override void Execute(object? parameter)
        {
            var popup = new MedicalRecordFormView(GetSelectedPatient());
            popup.ShowDialog();
        }

        public void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SelectedPatient = GetSelectedPatient();
            if (e.PropertyName == nameof(SelectedPatient))
            {
                OnCanExecutedChanged();
            }
        }

        private PatientViewModel GetSelectedPatient()
        {
            return (_doctorPatientsTableViewModel is null) ? _patientTableViewModel.SelectedPatient : _doctorPatientsTableViewModel.SelectedPatient;
        }

        public bool CheckPatient()
        {
            return (_doctorPatientsTableViewModel is null) ? true : FindDoctorsPatient();
        }

        public bool FindDoctorsPatient()
        {
            if (SchedulingService.GetAllPatients(Doctor).Any(patient => patient.Id == SelectedPatient.Patient.Id))
            {
                return true;
            }
            else
            {
                MessageBox.Show("Ne mozete izmeniti zdravstveni karton pacijenta koga niste do sad pregledali.");
                return false;
            }

        }
    }
}
