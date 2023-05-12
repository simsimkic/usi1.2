using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Model
{
    public class Hospital
    {
        private readonly Room _warehouse;
        private readonly RoomBook _rooms;
        public Room Warehouse => _warehouse;
        public RoomBook Rooms => _rooms;

        public Hospital()
        {
            _warehouse = new Room();
            _rooms = new RoomBook();

        }
        public Hospital(Room warehouse, RoomBook rooms)
        {
            _warehouse = warehouse;
            _rooms = rooms;
        }

        public Room getRoomById(int id)
        {
            return GetAllRoomAndWarehouse().FirstOrDefault(r => r.RoomID == id); ;
        }
        public IEnumerable<Room> GetAllRooms()
        {
            return _rooms.GetAllRooms();
        }

        public Room GetWarehouse()
        {
            return _warehouse;
        }
        public IEnumerable<Room> GetAllRoomAndWarehouse()
        {
            return _rooms.GetAllRooms().Append(GetWarehouse());
        }
    }
}
