using System;
using System.Collections.Generic;
using System.Windows.Documents;
using ZdravoCorp.Model;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Model.Enum;
using ZdravoCorp.ViewModel;
using ZdravoCorp.ViewModel.Form;

namespace ZdravoCorp
{
    public class MainViewModel : ViewModelBase
    {
		private ViewModelBase _currentViewModel;
		public ViewModelBase CurrentViewModel
		{
			get
			{
				return _currentViewModel;
			}
			set
			{
                _currentViewModel = value;
				OnPropertyChanged(nameof(CurrentViewModel));
			}
		}
		public MainViewModel()
        {
            CurrentViewModel = new LoginViewModel(this);
        }
    }
}