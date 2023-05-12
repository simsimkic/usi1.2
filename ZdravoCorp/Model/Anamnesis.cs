using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Model
{
    public class Anamnesis
    {
        private string _symptoms;
        private string _observations;
        private string _conclusions;
        private Therapy therapy;

        public Anamnesis(string symptoms, string observations, string conclusions, Therapy therapy)
        {
            Symptoms = symptoms;
            Observations = observations;
            Conclusions = conclusions;
            Therapy = therapy;
        }

        public string Symptoms { get => _symptoms; set => _symptoms = value; }
        public string Observations { get => _observations; set => _observations = value; }
        public string Conclusions { get => _conclusions; set => _conclusions = value; }
        public Therapy Therapy { get => therapy; set => therapy = value; }
    }
}
