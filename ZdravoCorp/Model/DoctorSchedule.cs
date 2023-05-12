using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model
{
    public class DoctorSchedule : ISerializable
    {
        public int Id;
        public int MaxAppointmentId;
        public Dictionary<DateOnly, List<TimeSlot>> FreeTimeSlots;
        public Dictionary<DateOnly, List<Appointment>> Appointments;

        public DoctorSchedule()
        {
        }

        public DoctorSchedule(int id, Dictionary<DateOnly, List<TimeSlot>> freeTimeSlots, Dictionary<DateOnly, List<Appointment>> appointments, int maxAppointmentId)
        {
            Id = id;
            FreeTimeSlots = freeTimeSlots;
            Appointments = appointments;
            MaxAppointmentId = maxAppointmentId;
        }

        public int GetId()
        {
            return Id;
        }

        public void SetId(int id)
        {
            Id = id;
        }
        public int GetNextAppointmentId()
        {
            return MaxAppointmentId++;
        }
    }
}
