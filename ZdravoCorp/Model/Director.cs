using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model
{
    public class Director : Person, ISerializable
    {
        public Hospital Hospital { get; set; }

        public Director() { }
        public Director(Hospital hospital,int id, string username, string firstName, string lastName, string password) : base(id, username, firstName, lastName, password)
        {
            Hospital = hospital;
        }
    }
}
