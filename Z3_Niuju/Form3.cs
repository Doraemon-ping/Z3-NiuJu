using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Z3_Niuju
{
    public partial class Form3 : Form
    {
        string serverPass = null;
        //验证
        string id;
        string userNmae;
        string passWord;


        string url = "http://10.25.11.7:7004/api/user";


        public Form3()
        {
            InitializeComponent();
            id = System.Configuration.ConfigurationManager.ConnectionStrings["ID"].ConnectionString;

            userNmae = SQLiteTool.getUserNameById(id);
            passWord = SQLiteTool.getAccountById(id);

            pass();

            MessageBox.Show(passWord);
            MessageBox.Show(serverPass);

            if (serverPass == passWord)
            {
               

            }
            else
            {
                MessageBox.Show("验证失败");
                Application.Exit();
            }
        }

        public async void pass() {

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

            }
            catch (Exception ex)
            {
                MessageBox.Show("ex:"+ex.Message);
            }

        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
