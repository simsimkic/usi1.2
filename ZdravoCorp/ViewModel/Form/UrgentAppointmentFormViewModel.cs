using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Commands;
using ZdravoCorp.Model;
using ZdravoCorp.Model.Enum;

namespace ZdravoCorp.ViewModel.Form
{
    public class UrgentAppointmentFormViewModel : ViewModelBase
    {
        public PatientViewModel SelectedPatient { get; set; }

        private bool _isOperation;
		public bool IsOperation
		{
			get
			{
				return _isOperation;
			}
			set
			{
				_isOperation = value;
				OnPropertyChanged(nameof(IsOperation));
			}
		}

		private int _duration;
		public int Duration
		{
			get
			{
				return _duration;
			}
			set
			{
				_duration = value;
				OnPropertyChanged(nameof(Duration));
			}
		}

		private Specialization? _selectedSpecialization;
		public Specialization? SelectedSpecialization
		{
			get
			{
				return _selectedSpecialization;
			}
			set
			{
				_selectedSpecialization = value;
				OnPropertyChanged(nameof(SelectedSpecialization));
			}
		}

		private ObservableCollection<Specialization> _specializations;
		public ObservableCollection<Specialization> Specializations
		{
			get
			{
				return _specializations;
			}
			set
			{
				_specializations = value;
				OnPropertyChanged(nameof(Specializations));
			}
		}

		public ICommand CreateUrgentAppointment { get; }

        public UrgentAppointmentFormViewModel(PatientViewModel selectedPatient)
        {
			SelectedPatient = selectedPatient;
            _specializations = GetAllSpecializations();
            CreateUrgentAppointment = new TryToCreateUrgentAppointmentCommand(this); 
        }

        private ObservableCollection<Specialization> GetAllSpecializations()
        {
            var allSpecializations = Enum.GetValues(typeof(Specialization)).Cast<Specialization>().ToList();
            return new ObservableCollection<Specialization>(allSpecializations);

        }

		public int GetDuration()
		{
			return _isOperation ? _duration : 15;
        }
    }
}
