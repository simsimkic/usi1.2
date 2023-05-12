using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model;
using ZdravoCorp.Model.DAO;
using ZdravoCorp.Model.Enum;
using ZdravoCorp.ViewModel.Structure;

namespace ZdravoCorp.Service
{
    public static class EquipmentService
    {
        public static List<DynamicalEquipment> GetDepletingDynamicalEquipment()
        {
            return DirectorDAO.GetDepletingDynamicalEquipment();
            
        }

        public static void MakeDynamicalEquipmentRequest(DynamicalEquipmentType type, int requestedQuantity)
        {
            DynamicalEquipmentRequestDAO.AddRequest(new DynamicalEquipmentRequest(type,requestedQuantity));
        }

        public static IEnumerable<EquipmentRedistributionViewModel> GetEquipmentRedistributions()
        {

        return DirectorDAO.GetAllRooms().SelectMany(room => room.DynamicalEquipmentBook
            .Select(equipment => new EquipmentRedistributionViewModel(
                equipment.Quantity,
                equipment.Type.ToString(),
                room.RoomID,
                room.RoomType.ToString())))
            .ToList();
        }
        public static IEnumerable<StaticEquipmentRedistributionViewModel> GetStaticEquipmentRedistributions()
        {

            return DirectorDAO.GetAllRooms().SelectMany(room => room.StaticEquipmentBook
                .Select(equipment => new StaticEquipmentRedistributionViewModel(
                    equipment.Quantity,
                    equipment.Type.ToString(),
                    room.RoomID,
                    room.RoomType.ToString(),
                    equipment.Purpose)))
                .ToList();
        }
    }
}
