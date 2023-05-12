using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model.Enum;
using ZdravoCorp.View;
using ZdravoCorp.ViewModel.Table;

namespace ZdravoCorp.Commands
{
    public class OpenPatientCommand : CommandBase
    {
        private PatientTableViewModel _patientTableViewModel;

        public OpenPatientCommand(PatientTableViewModel patientTableViewModel)
        {
            _patientTableViewModel = patientTableViewModel;
        }

        public override void Execute(object? parameter)
        {
            var popup = new PatientFormView(_patientTableViewModel);
            popup.ShowDialog();
        }
    }
}
