using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model
{
    public class Patient : Person, ISerializable
    {
        public MedicalRecord MedicalRecord;
        
        public Patient()
        {

        }
        public Patient(int id, string username, string firstName, string lastName, string password, MedicalRecord medicalRecord) : base(id, username, firstName, lastName, password)
        {
            MedicalRecord = medicalRecord;
        }
    }
}
