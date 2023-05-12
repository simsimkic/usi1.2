using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace ZdravoCorp.Serializer
{
    public class Serializer<T> where T: ISerializable, new() 
    {
        public Serializer()
        {
        }

        public void ToJSON(string fileName, Dictionary<int, T> objects)
        {
            using (StreamWriter file = File.CreateText(fileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, objects);
            }
        }

        public Dictionary<int, T> FromJSON(string fileName)
        {
            string json;
            using (StreamReader file = File.OpenText(fileName))
            {
                json = file.ReadToEnd();
            }

            Dictionary<int, T> objects = JsonConvert.DeserializeObject<Dictionary<int, T>>(json);

            return objects;
        }
    }
}
