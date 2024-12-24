using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z3_Niuju
{
    public class Z2res
    {
        public int Ret { get; set; }
        public int Msg { get; set; }
        public Data Data { get; set; }
    }

    public class Data
    {
        public int BomSerialNumber { get; set; }
        public string OtherData { get; set; }
    }
}
