using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Model
{
    public class Equipment
    {
        public int Quantity { get; set; }

        public Equipment(){}
        protected Equipment(int quantity)
        {
            Quantity = quantity;
        }

 
    }
}
