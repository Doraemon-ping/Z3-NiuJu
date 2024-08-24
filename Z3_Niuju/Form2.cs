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
    public partial class Form2 : Form
    {
        SQLiteTool sQLiteTool;
        DataTable dt;

        public Form2()
        {
            InitializeComponent();
            sQLiteTool = new SQLiteTool();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = null;
            //SQLiteTool sQLiteTool = new SQLiteTool;
            string time = this.dateTimePicker1.Value.Date.ToString("yyyy-MM-dd");
            string str = time + " 00:00:00";
            string end = time + " 23:59;59";

            dt = sQLiteTool.getBydate(str,end);
            Console.WriteLine(str+":"+end);
            this.dataGridView1.DataSource = dt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dt == null) { MessageBox.Show("请先查询！"); }
            else {
                ExcelTool.get_Excel_fromDateGridView(this.dataGridView1);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = null;
            dt = sQLiteTool.getTest();
            dataGridView1.DataSource = dt;
        }
    }
}
