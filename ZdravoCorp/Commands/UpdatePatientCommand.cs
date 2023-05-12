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
using ZdravoCorp.ViewModel;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace ZdravoCorp.Commands
{
    public class UpdatePatientCommand : CommandBase
    {
        private readonly PatientFormViewModel _patientFormViewModel;
        private DAOFactory _daoFactory = DAOFactory.GetInstance();
        private string _nameRegex = @"^[A-ZČĆŠĐŽ][a-zčćšđž]+(\s[A-ZČĆŠĐŽ][a-zčćšđž]+)*$";

        public UpdatePatientCommand(PatientFormViewModel patientFormViewModel)
        {
            _patientFormViewModel = patientFormViewModel;
            _patientFormViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            return (_patientFormViewModel.Username is not null) &&
                (_patientFormViewModel.FirstName is not null) && Regex.IsMatch(_patientFormViewModel.FirstName, _nameRegex) &&
                (_patientFormViewModel.LastName is not null) && Regex.IsMatch(_patientFormViewModel.FirstName, _nameRegex) &&
                (_patientFormViewModel.Password is not null) &&
                (_patientFormViewModel.PasswordCheck is not null);
        }

        public override void Execute(object? parameter)
        {
            var patient = _daoFactory.PatientDAO.GetById(_patientFormViewModel.SelectedPatient.Id);

            if (!_daoFactory.IsUnique(_patientFormViewModel.Username, _patientFormViewModel.SelectedPatient.Id))
            {
                MessageBox.Show("Korisničko ime je zauzeto.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (_patientFormViewModel.Password != _patientFormViewModel.PasswordCheck)
            {
                MessageBox.Show("Šifra se ne poklapa", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var medicalRecord = new MedicalRecord(_patientFormViewModel.SelectedPatient.MedicalRecord.Height,
                    _patientFormViewModel.SelectedPatient.MedicalRecord.Weight,
                    _patientFormViewModel.SelectedPatient.MedicalRecord.Diseases,
                    _patientFormViewModel.SelectedPatient.MedicalRecord.Alergies);
                
                patient = new Patient(_patientFormViewModel.SelectedPatient.Id,
                    _patientFormViewModel.Username,
                    _patientFormViewModel.FirstName,
                    _patientFormViewModel.LastName,
                    _patientFormViewModel.Password,
                    medicalRecord);

                _patientFormViewModel.PatientTableViewModel.Patients.Remove(_patientFormViewModel.SelectedPatient);

                _patientFormViewModel.PatientTableViewModel.Patients.Add(new PatientViewModel(patient));
                _patientFormViewModel.PatientFormView.Close();
                CollectionViewSource.GetDefaultView(_patientFormViewModel.PatientTableViewModel.Patients).Refresh();
            }
        }

        public void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((e.PropertyName == nameof(_patientFormViewModel.Username)) ||
                (e.PropertyName == nameof(_patientFormViewModel.FirstName)) ||
                (e.PropertyName == nameof(_patientFormViewModel.LastName)) ||
                (e.PropertyName == nameof(_patientFormViewModel.Password)) ||
                (e.PropertyName == nameof(_patientFormViewModel.PasswordCheck)))
            {
                OnCanExecutedChanged();
            }
        }
    }
}
