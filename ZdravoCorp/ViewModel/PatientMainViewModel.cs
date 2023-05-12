using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Commands;
using ZdravoCorp.Model;
using ZdravoCorp.View;
using ZdravoCorp.View.Form;
using ZdravoCorp.ViewModel.Table;
using ZdravoCorp.ViewModel.Form;
using ZdravoCorp.View.Table;

namespace ZdravoCorp.ViewModel
{
    public class PatientMainViewModel : ViewModelBase
    {
        public ICommand PatientAppointmetsCommand { get;  }
        public ICommand SchedulingRecommendationCommand { get; }
        public ICommand OpenPatientMedicalRecordCommand { get; }
        private Patient _patient;
        public PatientMainViewModel(Patient patient)
        {
            _patient = patient;
            PatientAppointmetsCommand = new RelayCommand(OpenAppointments, CanClick);
            SchedulingRecommendationCommand = new RelayCommand(OpenSchedulingRecommendation, CanClick);
            OpenPatientMedicalRecordCommand = new RelayCommand(OpenPatientMedicalRecord, CanClick);
        }

        public bool CanClick()
        {
            return true;
        }

        public void OpenAppointments()
        {
            var patientAppointments = new PatientAppoinmentsView();
            patientAppointments.DataContext = new PatientAppointmentTableViewModel(_patient);
            patientAppointments.ShowDialog();
        }
        public void OpenSchedulingRecommendation()
        {
            var schedulingRecommendation = new PatientAdvancedAppointmentScheduilingView();
            schedulingRecommendation.DataContext = new Form.PatientMedicalRecordTableViewModel(_patient, schedulingRecommendation);
            schedulingRecommendation.ShowDialog();
        }

        public void OpenPatientMedicalRecord()
        {
            var patiendMedicalRecord = new PatientMedicalRecordTableView();
            patiendMedicalRecord.DataContext = new Table.PatientMedicalRecordTableViewModel(patiendMedicalRecord, _patient);
            patiendMedicalRecord.ShowDialog();
        }

    }
}
