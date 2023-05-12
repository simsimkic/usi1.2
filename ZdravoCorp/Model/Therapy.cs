using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Model
{
    public class Therapy
    {
        private List<Drug> _drugs;

        public Therapy(List<Drug> drugs)
        {
            _drugs = drugs;
        }

        public List<Drug> Drugs { get => _drugs; set => _drugs = value; }
    }
}
