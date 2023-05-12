using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.View.Form;
using ZdravoCorp.ViewModel.Form;
using ZdravoCorp.ViewModel.Table;

namespace ZdravoCorp.Commands
{
    public class OpenUrgentAppointmentCommand : CommandBase
    {
        private readonly PatientTableViewModel _patientTableViewModel;

        public OpenUrgentAppointmentCommand(PatientTableViewModel patientTableViewModel)
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
            // Should add argument selectedPatient to the constructor
            var popup = new UrgentAppointmentFormView(_patientTableViewModel.SelectedPatient);
            popup.ShowDialog();
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_patientTableViewModel.SelectedPatient))
            {
                OnCanExecutedChanged();
            }
        }
    }
}
