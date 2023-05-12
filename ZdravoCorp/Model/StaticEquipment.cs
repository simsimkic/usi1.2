using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model.Enum;

namespace ZdravoCorp.Model
{
    public class StaticEquipment : Equipment
    {
        public StaticEquipmentType Type { get; set; }
        public EquipmentPurpose Purpose { get; set; }

        public StaticEquipment(StaticEquipmentType type, EquipmentPurpose purpose,int quantity) : base(quantity)
        {
            Type = type;
            Purpose = purpose;
        }
    }
}
