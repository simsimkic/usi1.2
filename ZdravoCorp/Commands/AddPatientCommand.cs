using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using ZdravoCorp.Model;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Model.Enum;
using ZdravoCorp.View;
using ZdravoCorp.ViewModel;
using ZdravoCorp.ViewModel.Form;

namespace ZdravoCorp.Commands
{
    public class AddPatientCommand : CommandBase
    {
        private readonly PatientFormViewModel _patientFormViewModel;
        private DAOFactory _daoFactory = DAOFactory.GetInstance();
        private string _nameRegex = @"^[A-ZČĆŠĐŽ][a-zčćšđž]+(\s[A-ZČĆŠĐŽ][a-zčćšđž]+)*$";

        public AddPatientCommand(PatientFormViewModel patientFormViewModel)
        {
            _patientFormViewModel = patientFormViewModel;
            _patientFormViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            return (_patientFormViewModel.Username is not null) &&  
                (_patientFormViewModel.FirstName is not null) && Regex.IsMatch(_patientFormViewModel.FirstName, _nameRegex) &&
                (_patientFormViewModel.LastName is not null) && Regex.IsMatch(_patientFormViewModel.LastName, _nameRegex) &&
                (_patientFormViewModel.Password is not null) &&
                (_patientFormViewModel.PasswordCheck is not null);
        }

        public override void Execute(object? parameter)
        {
            if (!_daoFactory.IsUnique(_patientFormViewModel.Username))
            {
                MessageBox.Show("Korisničko ime je zauzeto.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            } else if (_patientFormViewModel.Password != _patientFormViewModel.PasswordCheck)
            {
                MessageBox.Show("Šifra se ne poklapa", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            } else
            {
                var patient = new Patient(_daoFactory.PatientDAO.NextId(), _patientFormViewModel.Username, _patientFormViewModel.FirstName, _patientFormViewModel.LastName, _patientFormViewModel.Password, new MedicalRecord());
                _daoFactory.PatientDAO.Add(patient);
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
            
