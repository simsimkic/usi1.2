using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Commands;
using ZdravoCorp.Model;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Model.Enum;
using ZdravoCorp.View;
using ZdravoCorp.ViewModel.Table;

namespace ZdravoCorp.ViewModel.Form
{
    public class PatientFormViewModel : ViewModelBase
    {
		private string _username;
		public string Username
		{
			get
			{
				return _username;
			}
			set
			{
				_username = value;
				OnPropertyChanged(nameof(Username));
			}
		}

		private string _firstName;
		public string FirstName
		{
			get
			{
				return _firstName;
			}
			set
			{
				_firstName = value;
				OnPropertyChanged(nameof(FirstName));
			}
		}

		private string _lastName;
		public string LastName
		{
			get
			{
				return _lastName;
			}
			set
			{
				_lastName = value;
				OnPropertyChanged(nameof(LastName));
			}
		}

		// Change password type to SecureString.

		private string _password;
		public string Password
		{
			get
			{
				return _password;
			}
			set
			{
				_password = value;
				OnPropertyChanged(nameof(Password));
			}
		}

		private string _passwordCheck;
		public string PasswordCheck
		{
			get
			{
				return _passwordCheck;
			}
			set
			{
				_passwordCheck = value;
				OnPropertyChanged(nameof(PasswordCheck));
			}
		}

		public ICommand SubmitCommand { get; }

		public PatientViewModel SelectedPatient { get; }
		public PatientFormView PatientFormView { get; }
		public PatientTableViewModel PatientTableViewModel { get; }
        public PatientFormViewModel(PatientFormView patientFormView, PatientTableViewModel patientTableViewModel)
        {
			SelectedPatient = patientTableViewModel.SelectedPatient;
			PatientFormView = patientFormView;
			PatientTableViewModel = patientTableViewModel;
			SubmitCommand = GetCommand();
			SetPropertiesIfSelectedPatient();
        }

		private void SetPropertiesIfSelectedPatient()
		{
			if (SelectedPatient != null) 
			{
				_username = SelectedPatient.Username;
				_firstName = SelectedPatient.FirstName;
				_lastName = SelectedPatient.LastName;
				_password = SelectedPatient.Password;
				_passwordCheck = SelectedPatient.Password;
            }
		}

		private CommandBase GetCommand()
		{
			if (SelectedPatient is null)
			{
				return new AddPatientCommand(this);
			}
			return new UpdatePatientCommand(this);
		}
	}
}
