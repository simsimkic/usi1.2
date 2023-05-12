using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model.Enum;

namespace ZdravoCorp.ViewModel.Structure
{
    public class StaticEquipmentRedistributionViewModel : EquipmentRedistributionViewModel
    {
        private EquipmentPurpose _purpose;
        public EquipmentPurpose Purpose
        {
            get
            {
                return _purpose;
            }
            set
            {
                _purpose = value;
                OnPropertyChanged(nameof(Purpose));
            }
        }
        public StaticEquipmentRedistributionViewModel(int quantity, string type, int roomID, string roomType, EquipmentPurpose purpose) : base(quantity, type, roomID, roomType)
        {
            _purpose=purpose;
        }


    }
}
