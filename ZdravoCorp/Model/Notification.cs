using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model
{
    public class Notification: ISerializable
    {
        public int Id { get; set; }
        public int DoctorID { get; set; }
        public int PatientID { get; set; }
        public TimeSlot Initial { get; set; }
        public TimeSlot Delayed { get; set; }
        public Notification()
        {

        }
        public Notification(int doctorID, int patientID, TimeSlot initial, TimeSlot delayed)
        {
            DoctorID = doctorID;
            PatientID = patientID;
            Initial = initial;
            Delayed = delayed;
        }
        public int GetId()
        {
            return Id;
        }

        public void SetId(int id)
        {
            Id = id;
        }
    }
}
