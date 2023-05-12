using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Commands;
using ZdravoCorp.Model;
using ZdravoCorp.View;
using ZdravoCorp.View.Table;
using ZdravoCorp.ViewModel.Table;

namespace ZdravoCorp.ViewModel
{
    public class DoctorMainViewModel: ViewModelBase
    {
        private Doctor Doctor { get; set; }
        public ICommand CreateWindowAppointmentsCommand { get; }
        public ICommand CreateWindowPatientsCommand { get; }
        public ICommand CreateWindowStartExaminationCommand { get; }
        public DoctorMainViewModel(Doctor doctor)
        {
            Doctor = doctor;
            CreateWindowAppointmentsCommand = new RelayCommand(CreateWindowAppointments, CanExecute);
            CreateWindowPatientsCommand = new RelayCommand(CreateWindowPatients, CanExecute);
            CreateWindowStartExaminationCommand = new RelayCommand(CreateWindowStartExamination, CanExecute);
        }

        public static bool CanExecute()
        {
            return true;
        }

        public void CreateWindowAppointments()
        {
            var createAppointmentView = new DoctorAppointmentView(Doctor);
            createAppointmentView.ShowDialog();
        }

        public void CreateWindowPatients()
        {
            var createWindowPatients = new DoctorsPatients(Doctor);
            createWindowPatients.ShowDialog();
        }

        public void CreateWindowStartExamination()
        {
            var createWindowStartExamination = new DoctorExaminationView(Doctor);
            createWindowStartExamination.ShowDialog();
        }
    }
}
