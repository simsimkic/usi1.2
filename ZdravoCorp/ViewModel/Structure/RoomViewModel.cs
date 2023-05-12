using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model;
using ZdravoCorp.Model.Enum;

namespace ZdravoCorp.ViewModel
{
    public class RoomViewModel : ViewModelBase
    {
        private readonly Room _room;
        private readonly ObservableCollection<StaticEquipmentViewModel> _staticEquipment;

        public ObservableCollection<StaticEquipmentViewModel> StaticEquipment => _staticEquipment;
        public int RoomID => _room.RoomID;
        public string RoomType => _room.RoomType.ToString();

        
        public RoomViewModel(Room room)
        {
            _room = room;
            _staticEquipment = new ObservableCollection<StaticEquipmentViewModel>();
            foreach (StaticEquipment equipment in _room.StaticEquipmentBook)
            {
                _staticEquipment.Add(new StaticEquipmentViewModel(equipment));
            }
        }
    }
}
