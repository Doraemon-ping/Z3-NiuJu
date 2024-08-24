using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z3_Niuju
{
    internal class FileTool
    {
        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="folderPath">文件地址 </param>
        public static void OpenFolder(string folderPath)
        {
            try
            {
                Process.Start("explorer.exe", folderPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


    }
}
