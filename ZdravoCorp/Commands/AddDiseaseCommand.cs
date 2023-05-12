using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model.Enum;
using ZdravoCorp.ViewModel;
using ZdravoCorp.ViewModel.Form;

namespace ZdravoCorp.Commands
{
    public class AddDiseaseCommand : CommandBase
    {
        private readonly MedicalRecordFormViewModel _medicalRecordFormViewModel;
        private readonly ExaminationMedicalRecordViewModel _examinationMedicalRecordViewModel;
        private bool _isExaminationMedicalRecord;
        public Disease? SelectedDisease;
        public AddDiseaseCommand(MedicalRecordFormViewModel medicalRecordFormViewModel)
        {
            _isExaminationMedicalRecord = false;

            _medicalRecordFormViewModel = medicalRecordFormViewModel;
            _medicalRecordFormViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public AddDiseaseCommand(ExaminationMedicalRecordViewModel medicalRecordFormViewModel)
        {
            _isExaminationMedicalRecord = true;

            _examinationMedicalRecordViewModel = medicalRecordFormViewModel;
            _examinationMedicalRecordViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }
        public override bool CanExecute(object? parameter)
        {
            return _isExaminationMedicalRecord ? _examinationMedicalRecordViewModel.SelectedDisease is not null :
                _medicalRecordFormViewModel.SelectedDisease is not null;
        }
        public override void Execute(object? parameter)
        {
            if (_isExaminationMedicalRecord)
            {
                _examinationMedicalRecordViewModel.Diseases.Add((Disease)_examinationMedicalRecordViewModel.SelectedDisease);
                _examinationMedicalRecordViewModel.AvaliableDiseases.Remove((Disease)_examinationMedicalRecordViewModel.SelectedDisease);

            }
            else
            {
                _medicalRecordFormViewModel.Diseases.Add((Disease)_medicalRecordFormViewModel.SelectedDisease);
                _medicalRecordFormViewModel.AvaliableDiseases.Remove((Disease)_medicalRecordFormViewModel.SelectedDisease);
            }
        }

        public void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SelectedDisease = GetDisease();
            if (e.PropertyName == nameof(SelectedDisease))
            {
                OnCanExecutedChanged();
            }
        }

        private Disease? GetDisease()
        {
            return _isExaminationMedicalRecord ? _examinationMedicalRecordViewModel.SelectedDisease : _medicalRecordFormViewModel.SelectedDisease;
        }
    }
}
