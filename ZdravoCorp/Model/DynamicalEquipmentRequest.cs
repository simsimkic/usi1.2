using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model.Enum;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model
{
    public class DynamicalEquipmentRequest : DynamicalEquipment,ISerializable
    {
        public DateTime RequestExecutionTimeframe;
        public bool RequestCompletionStatus;
        public int Id;
        public DynamicalEquipmentRequest(){}
        public DynamicalEquipmentRequest(DynamicalEquipmentType dynamicalEquipmentType, int quantity) : base(dynamicalEquipmentType, quantity)
        {
            RequestExecutionTimeframe = DateTime.UtcNow.AddDays(1);
            RequestCompletionStatus = false;
            Id = 1;
        }

        public bool IsRequestCompleted()
        {
            return RequestCompletionStatus;
        }
        public bool IsTimeLessThanNow()
        {
            return RequestExecutionTimeframe < DateTime.Now;
        }
        public void SetRequestComplete()
        {
            RequestCompletionStatus = true;
        }
        public int GetId()
        {
            return Id;
        }

        public void SetId(int id)
        {
            Id = id;
        }

    }
}
