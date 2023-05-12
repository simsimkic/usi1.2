using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Model;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Model.Enum;
using ZdravoCorp.Service;
using ZdravoCorp.View.Table;
using ZdravoCorp.ViewModel;
using ZdravoCorp.ViewModel.Form;

namespace ZdravoCorp.Commands
{
    public class TryToCreateUrgentAppointmentCommand : CommandBase
    {
        private UrgentAppointmentFormViewModel _urgentAppointmentFormViewModel;
        public TryToCreateUrgentAppointmentCommand(UrgentAppointmentFormViewModel urgentAppointmentFormViewModel)
        {
            _urgentAppointmentFormViewModel = urgentAppointmentFormViewModel;
            _urgentAppointmentFormViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            return (_urgentAppointmentFormViewModel.IsOperation && _urgentAppointmentFormViewModel.Duration >= 15 && _urgentAppointmentFormViewModel.SelectedSpecialization is not null) 
                || (_urgentAppointmentFormViewModel.SelectedSpecialization is not null);
        }

        public override void Execute(object? parameter)
        {
            var span = new TimeSlot(2);
            var freePair = SchedulingService.GetFirstFreeTimeSlotAndDoctor((Specialization)_urgentAppointmentFormViewModel.SelectedSpecialization, span , _urgentAppointmentFormViewModel.GetDuration());
            var patient = GetFromDAOService.GetPatientById(_urgentAppointmentFormViewModel.SelectedPatient.Id);

            if (freePair is not null && SchedulingService.IsAvailable(patient, freePair.Item1))
            {
                try
                {
                    SchedulingService.CreateUrgentAppointment(freePair.Item2, patient, freePair.Item1, _urgentAppointmentFormViewModel.IsOperation);

                    MessageBox.Show("Uspešno ste zakazali urgentni pregled", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Greška prilikom zakazivanja urgentnog pregleda.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                var popup = new DelayableAppointmentTableView(_urgentAppointmentFormViewModel);
                popup.ShowDialog();
            }
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if ((e.PropertyName == nameof(_urgentAppointmentFormViewModel.IsOperation)) ||
                (e.PropertyName == nameof(_urgentAppointmentFormViewModel.Duration)) ||
                (e.PropertyName == nameof(_urgentAppointmentFormViewModel.SelectedSpecialization)))
            {
                OnCanExecutedChanged();
            }
        }
    }
}
