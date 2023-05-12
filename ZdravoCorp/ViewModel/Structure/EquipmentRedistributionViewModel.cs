using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model;
using ZdravoCorp.Model.Enum;

namespace ZdravoCorp.ViewModel.Structure
{
    public class EquipmentRedistributionViewModel:ViewModelBase
    {
		private int _quantity;
		public int Quantity
		{
			get
			{
				return _quantity;
			}
			set
			{
				_quantity = value;
				OnPropertyChanged(nameof(Quantity));
			}
		}
		private string _type;
		public string Type
		{
			get
			{
				return _type;
			}
			set
			{
                _type = value;
				OnPropertyChanged(nameof(Type));
			}
		}
		private int _roomID;
		public int RoomID
		{
			get
			{
				return _roomID;
			}
			set
			{
                _roomID = value;
				OnPropertyChanged(nameof(RoomID));
			}
		}
		private string _roomType;

		public string RoomType
		{
			get
			{
				return _roomType;
			}
			set
			{
				_roomType = value;
				OnPropertyChanged(nameof(RoomType));
			}
		}
		public EquipmentRedistributionViewModel(int quantity, string type, int roomID, string roomType)
		{
			_quantity = quantity;
			_type = type;
			_roomID = roomID;
			_roomType = roomType;
		}

    }
}
