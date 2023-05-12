using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Serializer;
using ISerializable = ZdravoCorp.Serializer.ISerializable;

namespace ZdravoCorp.Storage
{
    public class Storage<T> where T : ISerializable, new()
    {
        private string _storagePath;
        private Serializer<T> _serializer;

        public Storage(){}
        public Storage(string storagePath)
        {
            _serializer = new Serializer<T>();
            _storagePath = storagePath;
        }

        public Dictionary<int, T> Load()
        {
            return _serializer.FromJSON(_storagePath);
        }

        public void Save(Dictionary<int, T> objects)
        {
            _serializer.ToJSON(_storagePath, objects);
        }

    }
}
