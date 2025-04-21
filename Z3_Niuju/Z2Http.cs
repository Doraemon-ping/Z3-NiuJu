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

       public static Dictionary<string, object> DoSub(string code,List<Dictionary<string , object>> dt) {
            Dictionary<string, object> data = new Dictionary<string, object>{
            { "RequestGuid","ee6506fa-f737-4e30-990b-8766ea9a3f69_20230920100419719" },
            { "MachineId",129246 },
            { "ProductId",0 },
            { "StandardRouteId",827 },
            { "MachineType",0 },
            { "ActionTypeId",1 },
            { "Key",code },
            { "Data",new Dictionary<string,object>{
                { "ProductCode",code},
                { "ProductCodes",null},
                { "CustomerCode",""},
                { "BatchNumber",""},
                { "BoxCode",""},
                { "PartCode",""},
                { "RackCode",""},
                { "TrayCode",""},
                { "LoadingBox",""},
                { "HoldingFurnaceCode",""},
                { "InMaterialBatchBoxCode",""},
                { "Result",1},
                { "Quantity",0},
                { "Params",dt },
                { "OriginalDataList",null },
                { "WeldBadNess","" }}},

            { "PlainCode",null }
            };
            return data;
        }

        //参数数据
        public static List<Dictionary<string , object>> createDataTable (string v1 , string v2 , string v3) {

            int result1 = 1;
            int result2 = 1;
            int result3 = 1;

          



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


        // 封装函数：传入 productCode, machineId 和 actionTypeId，返回请求结果
       public  static async Task<string> CheckProductRouteAsync(string productCode, string machineId, string actionTypeId)
        {
            // 基础 URL
            string baseUrl = "http://10.3.15.132:8090/apis/Acc/ProductProcess/CheckProductRoute";

            // 拼接 URL
            string url = $"{baseUrl}?productCode={Uri.EscapeDataString(productCode)}&machineId={Uri.EscapeDataString(machineId)}&actionTypeId={Uri.EscapeDataString(actionTypeId)}";

            // 调用 HTTP 请求
            string result = await SendHttpRequestAsync(url);

            return result;
        }

        // 发送 HTTP 请求并获取响应
        static async Task<string> SendHttpRequestAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                // 发送 GET 请求
                HttpResponseMessage response = await client.GetAsync(url);

                // 确保请求成功
                if (response.IsSuccessStatusCode)
                {
                    // 读取响应内容
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    // 处理失败的请求
                    return $"请求失败: {response.StatusCode}";
                }
            }
        }
    }
}
