using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Model;
using ZdravoCorp.ViewModel.Form;
using System.ComponentModel;
using ZdravoCorp.ViewModel.Table;

namespace ZdravoCorp.Commands
{
    public class RemovePatientCommand : CommandBase
    {
        private readonly PatientTableViewModel _patientTableViewModel;
        private DAO<Patient> PatientDAO = DAOFactory.GetInstance().PatientDAO;

        public RemovePatientCommand(PatientTableViewModel patientTableViewModel)
        {
            _patientTableViewModel = patientTableViewModel;
            _patientTableViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            return _patientTableViewModel.SelectedPatient is not null;
        }
        public override void Execute(object? parameter)
        {
            var patient = PatientDAO.GetById(_patientTableViewModel.SelectedPatient.Id);
            _patientTableViewModel.Patients.Remove(_patientTableViewModel.SelectedPatient);
            PatientDAO.Remove(patient.Id);
        }

        public void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_patientTableViewModel.SelectedPatient))
            {
                OnCanExecutedChanged();
            }
        }
    }
}
