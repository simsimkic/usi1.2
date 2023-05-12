using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model;

namespace ZdravoCorp.ViewModel.Structure
{
    public  class AnamnesisViewModel : ViewModelBase
    {
        public Appointment Appointment;
        public string Sympthoms => Appointment.Anamnesis.Symptoms;
        public string Observations => Appointment.Anamnesis.Observations;
        public string Conclusion => Appointment.Anamnesis.Conclusions;

        public AnamnesisViewModel(Appointment appointment)
        {
            Appointment = appointment;
        }
    }
}
