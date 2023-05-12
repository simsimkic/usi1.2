using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Model.DAO
{
    public class DynamicalEquipmentRequestDAO : DAO<DynamicalEquipmentRequest>
    {

        public DynamicalEquipmentRequestDAO(string filePath) : base(filePath)
        {
        }
        
        public static List<DynamicalEquipmentRequest> GetUnfinishedDynamicalEquipmentRequests()
        {
            return DAOFactory.GetInstance().DynamicalRequestDAO.GetAll().Values.Where(r => !r.IsRequestCompleted() && r.IsTimeLessThanNow()).ToList();
        }

        public static void AddRequest(DynamicalEquipmentRequest dynamicalEquipmentRequest )
        {
            DAOFactory.GetInstance().DynamicalRequestDAO.Add(dynamicalEquipmentRequest);
        }

        public static void UpdateCompletedEquipmentRequests(List<DynamicalEquipmentRequest> dynamicalEquipmentRequests)
        {
            dynamicalEquipmentRequests.ForEach(x => DAOFactory.GetInstance().DynamicalRequestDAO.GetById(x.Id).SetRequestComplete());
        }
        public static void Save()
        {
            DAOFactory.GetInstance().DynamicalRequestDAO.SaveAll();
        }
    }
}
