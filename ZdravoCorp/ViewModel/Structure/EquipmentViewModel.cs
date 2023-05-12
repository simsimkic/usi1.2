using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model.Enum;
using ZdravoCorp.Model;

namespace ZdravoCorp.ViewModel.Structure
{
    public class EquipmentViewModel:ViewModelBase
    {
        private readonly Equipment _equipment;

        public int Quantity => _equipment.Quantity;
        public EquipmentViewModel(Equipment equipment)
        {
            _equipment = equipment;
        }
    }
}
