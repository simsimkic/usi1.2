using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model;

namespace ZdravoCorp.ViewModel.Table
{
    public class ShowAnamnesisViewModel : ViewModelBase
    {
        public string Sympthoms { get; set; }
        public string Observation { get; set; }
        public string Conclusion { get; set; }
        public ShowAnamnesisViewModel(Appointment appointment)
        {
            Sympthoms = appointment.Anamnesis?.Symptoms ?? "";
            Observation = appointment.Anamnesis?.Observations ?? "";
            Conclusion = appointment.Anamnesis?.Conclusions ?? "";

        }
    }
}
