using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Model
{
    public class RoomBook
    {
        private readonly List<Room> _rooms;

        public List<Room> Rooms => _rooms;

        public RoomBook()
        {
            _rooms = new List<Room>();
        }

        public IEnumerable<Room> GetAllRooms()
        {
            return _rooms;
        }

        public void Add(Room room)
        {
            _rooms.Add(room);
        }
    }
}
