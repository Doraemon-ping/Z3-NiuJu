using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Z3_Niuju
{
    public class ExcelTool
    {

        public static void get_Excel_fromDateGridView(DataGridView dataGridView1)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                FileName = "扭矩导出数据.xlsx"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook workbook = excelApp.Workbooks.Add();
                Excel.Worksheet worksheet = workbook.Worksheets[1];

                // 添加列标题
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
                }

                // 添加行数据
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value;
                    }
                }

                workbook.SaveAs(saveFileDialog.FileName);
                workbook.Close();
                excelApp.Quit();

                MessageBox.Show("Export completed!");
            }
        }
    }
}
