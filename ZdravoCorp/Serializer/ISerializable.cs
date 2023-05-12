using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Serializer
{
    public interface ISerializable
    {
        public int GetId();
        public void SetId(int id);
    }
}
