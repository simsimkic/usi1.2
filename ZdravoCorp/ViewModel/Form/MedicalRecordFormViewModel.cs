using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ZdravoCorp.Commands;
using ZdravoCorp.Model;
using ZdravoCorp.Model.Enum;


namespace ZdravoCorp.ViewModel.Form
{
    public class MedicalRecordFormViewModel : ViewModelBase
    {
        public PatientViewModel SelectedPatient { get; set; }
		public ObservableCollection<Alergy> AvaliableAlergies { get; set; }
		public ObservableCollection<Disease> AvaliableDiseases { get; set; }

		private Alergy? _selectedAlergy;
		public Alergy? SelectedAlergy
		{
			get
			{
				return _selectedAlergy;
			}
			set
			{
				_selectedAlergy = value;
				OnPropertyChanged(nameof(SelectedAlergy));
			}
		}

		private Disease? _selectedDisease;
		public Disease? SelectedDisease
		{
			get
			{
				return _selectedDisease;
			}
			set
			{
				_selectedDisease = value;
				OnPropertyChanged(nameof(SelectedDisease));
			}
		}

		private double _height;
		public double Height	
		{
			get
			{
                return _height;

            }
			set
			{
                _height = value;
				OnPropertyChanged(nameof(Height));
			}
		}

		private double _weight;
		public double Weight
		{
			get
			{
				return _weight;
            }
			set
			{
                _weight = value;
				OnPropertyChanged(nameof(Weight));
			}
		}

		private ObservableCollection<Alergy> _alergies;
		public ObservableCollection<Alergy> Alergies => _alergies;

        private ObservableCollection<Disease> _diseases;
		public ObservableCollection<Disease> Diseases => _diseases;

        public ICommand AddAlergy { get; }
        public ICommand AddDisease { get; }

        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }
		
        public MedicalRecordFormViewModel(PatientViewModel selectedPatient)
        {
            SelectedPatient = selectedPatient;

			_height = selectedPatient?.MedicalRecord?.Height ?? 0;
			_weight = selectedPatient?.MedicalRecord?.Weight ?? 0;

            _diseases = new ObservableCollection<Disease>(selectedPatient?.MedicalRecord?.Diseases ?? new List<Disease>());
            _alergies = new ObservableCollection<Alergy>(selectedPatient?.MedicalRecord?.Alergies ?? new List<Alergy>());

            AvaliableAlergies = GetAvaliableAlergies();
			AvaliableDiseases = GetAvaliableDisease();

			AddAlergy = new AddAlergyCommand(this);
			AddDisease = new AddDiseaseCommand(this);

			SubmitCommand = new SubmitMedicalRecordCommand(this);
		}

		private ObservableCollection<Alergy> GetAvaliableAlergies()
		{
			var allAlergies = Enum.GetValues(typeof(Alergy)).Cast<Alergy>().ToList();
            foreach (Alergy alergy in _alergies)
			{
				allAlergies.Remove(alergy);
			}
			return new ObservableCollection<Alergy> (allAlergies);

        }

        private ObservableCollection<Disease> GetAvaliableDisease()
        {
            var allDiseases = Enum.GetValues(typeof(Disease)).Cast<Disease>().ToList();
            foreach (Disease disease in _diseases)
            {
                allDiseases.Remove(disease);
            }
            return new ObservableCollection<Disease>(allDiseases);

        }
	}
}
