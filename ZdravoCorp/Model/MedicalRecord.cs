using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model.Enum;

namespace ZdravoCorp.Model
{
    public class MedicalRecord
    {
        public double Height; 
        public double Weight;
        public List<Disease> Diseases;
        public List<Alergy> Alergies;

        public MedicalRecord()
        {
        }

        public MedicalRecord(double height, double weight, List<Disease> diseases, List<Alergy> alergies)
        {
            Height = height;
            Weight = weight;
            Diseases = diseases;
            Alergies = alergies;
        }      
    }
}
