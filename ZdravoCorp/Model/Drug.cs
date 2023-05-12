using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model.Enum;

namespace ZdravoCorp.Model
{
    public class Drug
    {
        private List<Alergy> _alergens;
        private Instruction _instruction;

        public Drug(List<Alergy> alergens, Instruction instruction)
        {
            Alergens = alergens;
            Instruction = instruction;
        }

        public List<Alergy> Alergens { get => _alergens; set => _alergens = value; }
        public Instruction Instruction { get => _instruction; set => _instruction = value; }
    }
}
