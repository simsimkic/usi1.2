using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;




namespace ZdravoCorp.Model
{
    public class EquipmentBook
    {
        private readonly List<Equipment> _equipments;

        public List<Equipment> Equipment => _equipments;

        public EquipmentBook()
        {
            _equipments = new List<Equipment>();
        }

        public IEnumerable<Equipment> GetAllEquipment()
        {
            return _equipments;
        }

        public void Add(Equipment equipment)
        {
            _equipments.Add(equipment);
        }
    }
}
