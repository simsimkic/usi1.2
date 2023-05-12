using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Model;
using ZdravoCorp.Model.DAO;

namespace ZdravoCorp.Service
{
    public static class ThreadService
    {
        public static void CallAllThread()
        {
            ProcessDynamicEquipmentRequestThread();
        }
        private static void ProcessDynamicEquipmentRequestThread()
        {
            List<DynamicalEquipmentRequest> _dynamicalEquipmentRequests = DynamicalEquipmentRequestDAO.GetUnfinishedDynamicalEquipmentRequests();
            _dynamicalEquipmentRequests.ForEach(x => DirectorDAO.UpdateDynamicalEquipmentBook(x));
            DynamicalEquipmentRequestDAO.UpdateCompletedEquipmentRequests(_dynamicalEquipmentRequests);
            DynamicalEquipmentRequestDAO.Save();
            DirectorDAO.Save();

        }
    }
}
