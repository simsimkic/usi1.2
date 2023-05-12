using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model.Enum;

namespace ZdravoCorp.Model
{
    public class Instruction
    {
        private int _timesPerDay;
        private int _amountPerDay;
        private MedicationIntakeTime _intakeTime;
        private string _additionalComments;

        public Instruction(int timesPerDay, int amountPerDay, string additionalComments, MedicationIntakeTime intakeTime)
        {
            TimesPerDay = timesPerDay;
            AmountPerDay = amountPerDay;
            AdditionalComments = additionalComments;
            IntakeTime = intakeTime;
        }

        public int TimesPerDay { get => _timesPerDay; set => _timesPerDay = value; }
        public int AmountPerDay { get => _amountPerDay; set => _amountPerDay = value; }
        public string AdditionalComments { get => _additionalComments; set => _additionalComments = value; }
        public MedicationIntakeTime IntakeTime { get => _intakeTime; set => _intakeTime = value; }
       
    }
}
