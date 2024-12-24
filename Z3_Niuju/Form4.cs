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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Dictionary<string , object> data = Z3_Niuju.Z2Http.CheckProductRoute("124735023810017R0919133506M1", "1", "129762");
            string json = JsonConvert.SerializeObject(data);
            richTextBox2.Text = json;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<Dictionary<string , object>> dt = Z3_Niuju.Z2Http.createDataTable("12.2","11.6","14.3");
            Dictionary<string, object> data = Z3_Niuju.Z2Http.DoSub("124735023810017R0919133506M1", dt);
            string json = JsonConvert.SerializeObject(data);
            richTextBox1.Text = json;
        }


        private async void button5_Click(object sender, EventArgs e)
        {
            List<Dictionary<string, object>> dt = Z3_Niuju.Z2Http.createDataTable("12.2", "11.6", "14.3");
            Dictionary<string, object> data = Z3_Niuju.Z2Http.DoSub("124735023810017R0919133506M1", dt);
            string json = JsonConvert.SerializeObject(data);
            var response = await MyhHttp.myPost("http://10.3.15.132:8090/apis/Acc/ProductProcess/DoSub", this.richTextBox1.Text);
            MessageBox.Show(response);

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var response = await Z3_Niuju.Z2Http.CheckProductRouteAsync("124735023810033R0919133506M1", "129762", "1");
            MessageBox.Show(response);


        }
    }
}
