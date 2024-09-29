using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Z3_Niuju
{
    internal static class Program
    {
        public static readonly ILogger Logger = LogManager.GetCurrentClassLogger();



        public static string logPath;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
         public static void  Main()
        {
            if (!new Class1().passServer().Result) {

                MessageBox.Show("验证失败！");
                Environment.Exit(0);
            }
           // MessageBox.Show(new Class1().passServer().Result.ToString());
            // 简单的配置日志记录到一个文件
            var config = new NLog.Config.LoggingConfiguration();
            DateTime dateTime = DateTime.Now;
            string riqi = dateTime.Year.ToString()+"_"+dateTime.Month.ToString(); 
            // 定义日志文件路径和日志格式
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "log"+"\\"+riqi+"\\"+riqi+dateTime.Day.ToString()+"_logfile.txt" };
            logPath = logfile.FileName.ToString();
            Console.WriteLine(logPath);
            // 将日志级别设置为 Debug 及以上的所有日志信息写入文件
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
            // 应用配置
            LogManager.Configuration = config;
            // 记录日志
            Logger.Info("Application started.");
            try
            {
              // 
              // 
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            catch (Exception e) {
                Logger.Error("数据查询异常:"+e.Message);
            }
        }
    }
}
