using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model.Enum;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model
{
    public class Appointment: ISerializable
    {
        private Doctor _doctor;
        private Patient _patient;
        private TimeSlot _timeSlot;
        private Anamnesis _anamnesis;
        private bool _isOperation;
        private int _id;
        public bool IsCanceled;
        private int _changeCount;
        private DateTime _createdAt;
        public Doctor Doctor { get => _doctor; set => _doctor = value; }
        public Patient Patient { get => _patient; set => _patient = value; }
        public TimeSlot TimeSlot { get => _timeSlot; set => _timeSlot = value; }
        public Anamnesis Anamnesis { get => _anamnesis; set => _anamnesis = value; }
        public bool IsOperation { get => _isOperation; set => _isOperation = value; }
        public int Id { get => _id; set => _id = value; }
        public int ChangeCount { get => _changeCount; set => _changeCount = value; }
        public DateTime CreatedAt { get => _createdAt; }
        public Appointment()
        {
        }

        public Appointment(Doctor doctor, Patient patient, TimeSlot timeSlot, bool isOperation, int id)
        {
            Doctor = doctor;
            Patient = patient;
            TimeSlot = timeSlot;
            IsOperation = isOperation;
            _changeCount = 0;
            _createdAt = DateTime.Now;
            _id = id;
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
