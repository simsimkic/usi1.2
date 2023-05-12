using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Commands;
using ZdravoCorp.Model;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Service;
using ZdravoCorp.View.Form;
using ZdravoCorp.ViewModel.Form;
using ZdravoCorp.ViewModel.Table;

namespace ZdravoCorp.ViewModel
{
	public class LoginViewModel : ViewModelBase
	{

		private string _userNamse;
		public string UserName
		{
			get
			{
				return _userNamse;
			}
			set
			{
				_userNamse = value;
				OnPropertyChanged(nameof(UserName));
			}
		}

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

		public ICommand SubmitCommand { get; }
		private List<Person> _people;
		private Person _person;
		private readonly MainViewModel _mainViewModel;
		public MainViewModel MainViewModel => _mainViewModel;
		public LoginViewModel(MainViewModel mainViewModel)
		{
			_mainViewModel = mainViewModel;
			_people = new List<Person>();
            _people.AddRange(DAOFactory.GetInstance().PatientDAO.GetAll().Values);
			_people.AddRange(DAOFactory.GetInstance().DoctorDAO.GetAll().Values);
            _people.AddRange(DAOFactory.GetInstance().DirectorDAO.GetAll().Values);
            _people.AddRange(DAOFactory.GetInstance().NurseDAO.GetAll().Values);
            SubmitCommand = new RelayCommand(LoginUser, CanUserLogin);

		}

		public bool CanUserLogin()
		{ 
			return !(String.IsNullOrEmpty(UserName))  && !(String.IsNullOrEmpty(Password)) ;
		}

		public void LoginUser()
		{
			var users = _people.Where(o => o.Username == UserName);
			if (users.Count() != 0)
			{
				var passwrod = users.Where(o => o.Password == Password).ToList();
				if (passwrod.Count() == 1)
				{
					_person = passwrod[0];
                    
                    if (_person.GetType() == typeof(Doctor))
					{
                        Doctor user = DAOFactory.GetInstance().DoctorDAO.GetAll().Values.FirstOrDefault(o => o.Username == UserName);
						NotificationService.NotifyDoctor(user.Id);
                        _mainViewModel.CurrentViewModel = new DoctorMainViewModel(user);
                    }
                    else if (_person.GetType() == typeof(Patient))
					{
						Patient user = DAOFactory.GetInstance().PatientDAO.GetAll().Values.FirstOrDefault(o => o.Username == UserName);
						NotificationService.NotifyPatient(user.Id);
                        _mainViewModel.CurrentViewModel = new PatientMainViewModel(user);
                    }
                    else if (_person.GetType() == typeof(Director))
					{
						_mainViewModel.CurrentViewModel = new DirectorViewModel(DAOFactory.GetInstance().DirectorDAO.GetAll()[1]);
			
					}
					else
					{
						_mainViewModel.CurrentViewModel = new PatientTableViewModel();
					}
				}
			}
			else
			{
				MessageBox.Show("Lose uneti podaci");
			}


        }
    }
}
