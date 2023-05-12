using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model
{
    public abstract class Person : ISerializable
    {
        public int Id;
        public string Username;
        public string FirstName;
        public string LastName;
        public string Password;
       
        public Person()
        {

        }
        public Person(int id, string username, string firstName, string lastName, string password)
        {
            Id = id;
            Username = username;
            LastName = lastName;
            FirstName = firstName;
            Password = password;
        }

        // may be unnecessary, delete later if possible 
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
