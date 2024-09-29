using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Z3_Niuju
{
    internal class Class1
    {

        //验证
        string id;
        string userNmae;
        string passWord;
        string url = "http://10.25.11.7:7004/api/user";
        public   async Task< bool> passServer()
        {
            try
            {
                id = System.Configuration.ConfigurationManager.ConnectionStrings["ID"].ConnectionString;
                userNmae = SQLiteTool.getUserNameById(id);
                passWord = SQLiteTool.getAccountById(id);
                string serverPass = await pass();
                return passWord.Equals(serverPass);
            }
            catch (Exception e) {

                return false;
            }
            
           
        }

        public async  Task<string> pass()
        {
            string serverPass = "1";
            Dictionary<String, Object> data = new Dictionary<String, Object>
                {
                    {"id",id },   //老码
                    {"userName",userNmae },   //工厂编码
            };
            string json = JsonConvert.SerializeObject(data);
            //MessageBox.Show(json);
            try
            {
                var response = await MyhHttp.myPost(url, json);
                MyResponse serverresponse = JsonConvert.DeserializeObject<MyResponse>(response);
                serverPass = serverresponse.password;
                // MessageBox.Show(pas);
                return serverPass;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ex:" + ex.Message);
                return serverPass;

            }
            
        }
    }
}
