using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model.Enum;

namespace ZdravoCorp.Model
{
    public class UrgentAppointment
    {
        public bool IsOperation;
        public int Duration;
        public Specialization Specialization;

        public UrgentAppointment(bool isOperation, int duration, Specialization specialization)
        {
            IsOperation = isOperation;
            Duration = duration;
            Specialization = specialization;
        }
    }
}
