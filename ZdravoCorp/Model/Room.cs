using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model.Enum;

namespace ZdravoCorp.Model
{
    public class Room
    {

        public int RoomID { get; set; }
        public RoomType RoomType { get; set; }
        public List<StaticEquipment> StaticEquipmentBook { get; set; }
        public List<DynamicalEquipment> DynamicalEquipmentBook { get; set; }
        public Room()
        {
            StaticEquipmentBook = new List<StaticEquipment>();
            DynamicalEquipmentBook = new List<DynamicalEquipment>();
        }
        public Room(int roomID, RoomType roomType, List<StaticEquipment> staticEquipmentBook, List<DynamicalEquipment> dynamicalEquipmentBook)
        {
            RoomID = roomID;
            RoomType = roomType;
            StaticEquipmentBook = staticEquipmentBook;
            DynamicalEquipmentBook = dynamicalEquipmentBook;
        }

        public void IncreaseDynamicalEquipmentQuantity(int quantity,DynamicalEquipmentType type)
        {
            DynamicalEquipmentBook.FirstOrDefault(r => r.Type == type).Quantity+=quantity;

        }
        public void DecreaseDynamicalEquipmentQuantity(int quantity, DynamicalEquipmentType type)
        {
            DynamicalEquipmentBook.FirstOrDefault(r => r.Type == type).Quantity-=quantity;

        }
        public void IncreaseStaticEquipmentQuantity(int quantity, StaticEquipmentType type,EquipmentPurpose purpose)
        {
            StaticEquipmentBook.FirstOrDefault(r => r.Type == type && r.Purpose==purpose).Quantity += quantity;

        }
        public void DecreaseStaticEquipmentQuantity(int quantity, StaticEquipmentType type, EquipmentPurpose purpose)
        {
            StaticEquipmentBook.FirstOrDefault(r => r.Type == type && r.Purpose == purpose).Quantity -= quantity;

        }
        public int GetStaticEquipmentQuantity(StaticEquipmentType type, EquipmentPurpose purpose)
        {
            return StaticEquipmentBook.FirstOrDefault(r => r.Type == type && r.Purpose == purpose).Quantity;
        }
        public override bool Equals(object obj)
        {
            return obj is Room room &&
                room.RoomID == RoomID;
        }

        public static bool operator ==(Room room1, Room room2)
        {
            if (room1 is null && room2 is null)
            {
                return true;
            }

            return !(room1 is null && room1.Equals(room2));

        }
        public static bool operator !=(Room room1, Room room2)
        {
            return !(room1 == room2);

        }

    }
}
