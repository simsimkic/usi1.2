using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using ZdravoCorp.Model;
using ZdravoCorp.Model.Enum;
using ZdravoCorp.ViewModel;
using ZdravoCorp.ViewModel.Form;

namespace ZdravoCorp.Commands
{
    public class AddAlergyCommand : CommandBase
    {
        private readonly MedicalRecordFormViewModel _medicalRecordFormViewModel;
        private readonly ExaminationMedicalRecordViewModel _examinationMedicalRecordViewModel;
        private bool _isExaminationMedicalRecord;
        public Alergy? SelectedAlergy;
        public AddAlergyCommand(MedicalRecordFormViewModel medicalRecordFormViewModel)
        {
            _isExaminationMedicalRecord = false;
          
            _medicalRecordFormViewModel = medicalRecordFormViewModel;
            _medicalRecordFormViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }
        
        public AddAlergyCommand(ExaminationMedicalRecordViewModel medicalRecordFormViewModel)
        {
            _isExaminationMedicalRecord = true;
        
            _examinationMedicalRecordViewModel = medicalRecordFormViewModel;
            _examinationMedicalRecordViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }
        public override bool CanExecute(object? parameter)
        {
            return _isExaminationMedicalRecord ? _examinationMedicalRecordViewModel.SelectedAlergy is not null : 
                _medicalRecordFormViewModel.SelectedAlergy is not null;
        }
        public override void Execute(object? parameter)
        {

            if (_isExaminationMedicalRecord)
            {
                _examinationMedicalRecordViewModel.Alergies.Add((Alergy)_examinationMedicalRecordViewModel.SelectedAlergy);
                _examinationMedicalRecordViewModel.AvaliableAlergies.Remove((Alergy)_examinationMedicalRecordViewModel.SelectedAlergy);

            }
            else
            {
                _medicalRecordFormViewModel.Alergies.Add((Alergy)_medicalRecordFormViewModel.SelectedAlergy);
                _medicalRecordFormViewModel.AvaliableAlergies.Remove((Alergy)_medicalRecordFormViewModel.SelectedAlergy);
            }
            
        }

        public void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SelectedAlergy = GetAlergy();
            if (e.PropertyName == nameof(SelectedAlergy))
            {
                OnCanExecutedChanged();
            }
        }

        private Alergy? GetAlergy()
        {
            return _isExaminationMedicalRecord ? _examinationMedicalRecordViewModel.SelectedAlergy : _medicalRecordFormViewModel.SelectedAlergy;
        }
    }
}
