using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Flurl;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.NetworkInformation;

namespace Z3_Niuju
{
    internal class Z2Http
    {
        //校验数据
       public static Dictionary<String, object> CheckProductRoute(string ProductCode, string ActionTypeId , string MachineId) {

            Dictionary<string, object> data = new Dictionary<string, object> {
                {"productCode",ProductCode },
                { "actionTypeId",ActionTypeId},
                { "machineId",MachineId},
                { "standardrouteid",""},
                { "rackCode",""},
                { "productid",""},
                { "loadingbox",""}};
            return data;
        }

       public static Dictionary<string, object> DoSub() {
            Dictionary<string, object> data = new Dictionary<string, object>{
            { "RequestGuid","ee6506fa-f737-4e30-990b-8766ea9a3f69_20230920100419719" },
            { "MachineId",129762 },
            { "ProductId","" },
            { "StandardRouteId","" },
            { "MachineType","" },
            { "ActionTypeId","" },
            { "Key","" },
            { "Data",new Dictionary<string,object>{
                { "ProductCode",""},
                { "ProductCodes",""},
                { "CustomerCode",""},
                { "BatchNumber",""},
                { "BoxCode",""},
                { "PartCode",""},
                { "RackCode",""},
                { "TrayCode",""},
                { "LoadingBox",""},
                { "HoldingFurnaceCode",""},
                { "InMaterialBatchBoxCode",""},
                { "Result",""},
                { "Quantity",""},
                { "Params","" },
                { "OriginalDataList","" },
                { "WeldBadNess","" }}},

            { "PlainCode","" }
            };
            return data;
        }

        //参数数据
        public static List<Dictionary<string , object>> createDataTable (string v1 , string v2 , string v3) {

            int result1 = 2;
            int result2 = 2;
            int result3 = 2;

            if (double.Parse(v1) > 10) { result1 = 1; }
            if (double.Parse(v2) > 10) { result1 = 1; }
            if (double.Parse(v3) > 10) { result1 = 1; }



            Dictionary<string, object> dic1 = new Dictionary<string, object> {
                { "ParamType","扭矩值1"},
                { "ParamValue",v1},
                { "Position",""},
                { "Result",result1},
            };

            Dictionary<string, object> dic2 = new Dictionary<string, object> {
                { "ParamType","扭矩值2"},
                { "ParamValue",v2},
                { "Position",""},
                { "Result",result2},
            };

            Dictionary<string, object> dic3 = new Dictionary<string, object> {
                { "ParamType","扭矩值3"},
                { "ParamValue",v3},
                { "Position",""},
                { "Result",result3},
            };

            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();

            data.Add(dic1);
            data.Add(dic2);
            data.Add(dic3);


            return data;
        }

    }
}
