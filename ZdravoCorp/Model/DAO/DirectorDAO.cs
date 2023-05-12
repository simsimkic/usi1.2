using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Commands;
using ZdravoCorp.Model.Enum;
using ZdravoCorp.ViewModel.Structure;

namespace ZdravoCorp.Model.DAO
{
    public class DirectorDAO : DAO<Director>
    {
        private static Hospital _hospital;
        public DirectorDAO(string filePath) : base(filePath)
        {
            _hospital = new Hospital();
            _hospital = GetAll()[1].Hospital;
        }

        public static List<DynamicalEquipment> GetDepletingDynamicalEquipment()
        {
            var equipment = _hospital.GetAllRoomAndWarehouse().SelectMany(s => s.DynamicalEquipmentBook)
                      .GroupBy(o => new { o.Type })
                      .Select(g => new { g.Key.Type, Quantity = g.Sum(o => o.Quantity) })
                      .Where(e => e.Quantity <= 5)
                      .ToList();
            return equipment.Select(e => new DynamicalEquipment(e.Type, e.Quantity)).ToList();
        }
        public static void UpdateDynamicalEquipmentBook(DynamicalEquipmentRequest _dynamicalEquipmentRequest) {
            _hospital.Warehouse.DynamicalEquipmentBook.Find(x => x.Type == _dynamicalEquipmentRequest.Type ).Quantity+=_dynamicalEquipmentRequest.Quantity;
            
        }
        public static void Save()
        {
            DAOFactory.GetInstance().DirectorDAO.SaveAll();
        }

        public static IEnumerable<Room> GetAllRooms()
        {
            return _hospital.GetAllRoomAndWarehouse();
        }

        public static void TransferDynamicalEquipment(EquipmentRedistributionViewModel selectedElementFrom, EquipmentRedistributionViewModel selectedElementTo, int transferQuantity)
        {
            Room sourceRoom = _hospital.getRoomById(selectedElementFrom.RoomID);
            Room destinationRoom = _hospital.getRoomById(selectedElementTo.RoomID);

            var Type = (DynamicalEquipmentType)System.Enum.Parse(typeof(DynamicalEquipmentType), selectedElementTo.Type);
            
            sourceRoom.DecreaseDynamicalEquipmentQuantity(transferQuantity, Type);
            destinationRoom.IncreaseDynamicalEquipmentQuantity(transferQuantity, Type);
            Save();
        }
        public static void TransferStaticEquipment(StaticEquipmentRedistributionViewModel selectedElementFrom, StaticEquipmentRedistributionViewModel selectedElementTo, int transferQuantity)
        {
            Room sourceRoom = _hospital.getRoomById(selectedElementFrom.RoomID);
            Room destinationRoom = _hospital.getRoomById(selectedElementTo.RoomID);

            StaticEquipmentType type = (StaticEquipmentType)System.Enum.Parse<StaticEquipmentType>(selectedElementTo.Type);
            EquipmentPurpose purpose = selectedElementTo.Purpose;

            if (sourceRoom.GetStaticEquipmentQuantity(type, purpose) > transferQuantity)
            {
                sourceRoom.DecreaseStaticEquipmentQuantity(transferQuantity, type, purpose);
                destinationRoom.IncreaseStaticEquipmentQuantity(transferQuantity, type,purpose);
                Save();

            }
            else{
                MessageBox.Show("Prebacivanje " + type.ToString()+" "+ purpose.ToString() +" iz sobe "+sourceRoom.RoomID.ToString()+ " u sobu " +destinationRoom.RoomID.ToString() +" ne moze biti izvrseno.");
            }

        }



    }
}
