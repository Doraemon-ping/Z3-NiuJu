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
    internal class MyhHttp
    {

        public static Dictionary<string, object> create_json_test(string bar, string newbar, string cx, string eq, string gw, bool isfan, List<KeyValuePair> dt)
        {

            Dictionary<String, Object> data = new Dictionary<String, Object>
        {
            {"BarCode",bar },   //老码
            {"FactoryCode","8111" },   //工厂编码
            {"ProductLineNo",cx },  //产线编码
            {"EquipNo",eq },  //设备编码
            {"StationCodeNo",gw },  //工位编码
            {"Operator", "tp2022062112"},  //操作人编码
            {"ProductCode","" },  //成品物料编码,可为空
            {"MainPartCode",newbar},  //新码
            {"ChildPartCode",bar},  //老码
            {"ChildPartUUID","" },  //分零件二维码，多个用|分隔，可为空
            {"StartTime", ""},  //生产开始时间
            {"EndTime", "" },  //生产结束时间
            {"Result", "OK"},  //结果 OK or NG or NA
            {"Status", 1},  //生产状态 1正常 2警告 3异常 4停机
            {"IsByPass",false }, //是否bypass,默认否
            {"IsCheckRepeat",true}, //是否判断重复码,默认是
            {"IsReject",isfan }, //是否返工件,默认否
            { "DetailTable",dt  }  //工艺参数
    };





            return data;
        }


        public static KeyValuePair create_DataTable(string key, string value)
        {

            return
                    new KeyValuePair { Key = key, Value = value };
            // return list;
        }


        public static async Task<string> myPost(string url, string jsonData)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Request failed with status code: {response.StatusCode}");
                }

                return await response.Content.ReadAsStringAsync();
            }

            //return response;


        }

        public static bool isConnettionToIp(string ip) {
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send(ip);
                    return reply.Status == IPStatus.Success;
                }
            }


            catch (Exception ex)
            {
                // 处理可能的异常，例如 Ping 请求超时、地址无效等
                Console.WriteLine($"Error: {ex.Message}");
                Program.Logger.Error(ex.Message);
                return false;
            }
        }
    }
}
    


