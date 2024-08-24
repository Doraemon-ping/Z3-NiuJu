using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using NLog;
using System.Diagnostics;


namespace Z3_Niuju
{
    public partial class Form1 : Form
    {
        public SQLiteTool sQLiteTool;

        bool ScanPort = false;//扫码枪串口连接状态
        bool NiuJuPort = false;//扭矩抢连接状态
        bool MES = false;//MES连接状态
        bool ScanRead = false;//扫码枪串口读取
        bool NiuJuREad = false;//扭矩枪读取
        int post = 0;//报工状态
        private PortConfig Scan_Port = new PortConfig();//扭矩枪串口
        private List<string> ScanPortList;
        SerialPort serialPort;
        string ipAddress;
        string Api_AddEquipInfo;
        Thread readThread;//串口读取线程
        Thread jiankong;//监控串口读取是否正常
        Thread port;//监控串口是否打开
        Thread JkNes;//此线程用于监听mes

        bool readPortIsAlive = false;
        List<byte> buf = new List<byte>();//当数据不完整时用于存储
        int i = 0;
        int count = 0;//生产产品数量；

        //用于初始化串口
        string portName;
        string portportRate;
        string dataw;

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            Scan_Port.Path = "port2.xml";
            Scan_Port.Loda();
            //加载串口配置

            ScanPortList = Scan_Port.Port_List;
            portName = ScanPortList[0];
            portportRate = ScanPortList[1];
            dataw = ScanPortList[2];
            string party = ScanPortList[3];
            this.WindowState = FormWindowState.Normal;
            this.Size = new Size(800, 600); // 设置为合适的默认大小

            try//可能引发异常
            {
                ipAddress = System.Configuration.ConfigurationManager.ConnectionStrings["IPAddress"].ConnectionString;
                Api_AddEquipInfo = System.Configuration.ConfigurationManager.ConnectionStrings["Api_AddEquipInfo"].ConnectionString;
                InitializeComponent();
                InitializeSerialPort(portName, int.Parse(portportRate), int.Parse(dataw), StopBits.One, Parity.None);
            }
            catch (Exception ex)
            {
                Program.Logger.Error("发生异常: " + ex.Message);
            }

            MES = MyhHttp.isConnettionToIp("10.25.206.7");
            readThread = new Thread(ReadSerialPort);
            jiankong = new Thread(MonitorThread);
            port = new Thread(portIsAlive);
            JkNes = new Thread(IsMes);
            readThread.Start();
            jiankong.Start();
            port.Start();
            JkNes.Start();

            readPortIsAlive = readThread.IsAlive;


            if (readPortIsAlive)
            {
                button3.BackColor = Color.Green;
            }
            else
            {
                button3.BackColor = Color.Red;
            }

            chushihua();
            richTextBox7.Text = count.ToString();

            sQLiteTool = new SQLiteTool();
           // sQLiteTool.load();
           // sQLiteTool.insert("11","12","2024-08-24 01:12;12");
        }

        //用于监控串口是否打开
        public void portIsAlive()
        {

            //用于监控串口是否关闭
            Program.Logger.Info("监听串口已开启");
            while (true)
            {
                try
                {
                    // 假设 serialPort 是已经初始化好的串口对象
                    if (!serialPort.IsOpen)
                    {
                        Console.WriteLine("串口已关闭，尝试重新打开...");
                        // 尝试重新打开串口或进行其他处
                        NiuJuPort = false;
                        chushihua();
                        serialPort.Open();
                    }
                    else
                    {
                        NiuJuPort = true;
                        chushihua();
                    }

                    // 添加必要的监控逻辑，例如读取数据或其他操作
                }
                catch (Exception ex)
                {
                    // 记录异常，防止线程因未处理的异常而终止
                    Console.WriteLine($"串口打开监控线程异常: {ex.Message}");
                    NiuJuPort = false;
                    chushihua();

                    // 可以选择休眠一段时间以避免频繁重试，减轻系统压力
                    Thread.Sleep(1000);
                }
            }


        }

        //监控串口读取服务是否正常
        public void MonitorThread()
        {
            while (true)
            {
                // 如果线程为空或已经停止，创建并启动它
                if (readThread == null || !readThread.IsAlive)
                {
                    readPortIsAlive = false;
                    if (readPortIsAlive)
                    {
                        button3.BackColor = Color.Green;
                    }
                    else
                    {
                        button3.BackColor = Color.Red;
                    }
                    Console.WriteLine("Worker thread is being restarted.");
                    readThread = new Thread(ReadSerialPort);
                    readThread.Start();
                    readPortIsAlive = readThread.IsAlive;
                    if (readPortIsAlive)
                    {
                        button3.BackColor = Color.Green;
                    }
                    else
                    {
                        button3.BackColor = Color.Red;
                    }

                }

                // 每隔一段时间检查一次线程状态
                Thread.Sleep(5000);
            }
        }

        //监控MES链接是否正常
        public void IsMes()
        {

            //用于监控串口是否关闭
            Program.Logger.Info("MES网络监听已开启");
            while (true)
            {

                MES = MyhHttp.isConnettionToIp("10.25.206.7");
                chushihua();

                // 可以选择休眠一段时间以避免频繁重试，减轻系统压力
                Thread.Sleep(5000);
            }
        }

        //刷新ui控件
        public void chushihua()
        {
            if (ScanPort) { textBox2.BackColor = Color.Green; }
            else { textBox2.BackColor = Color.Red; }

            if (NiuJuPort) { textBox1.BackColor = Color.Green; }
            else { textBox1.BackColor = Color.Red; }

            if (MES) { textBox3.BackColor = Color.Green; }
            else { textBox3.BackColor = Color.Red; }

            if (ScanRead)
            {
                richTextBox5.BackColor = Color.Green;
                button1.BackColor = Color.Green;
                richTextBox5.Text = "扫码完成！";
            }
            else
            {
                richTextBox5.BackColor = Color.Red;
                button1.BackColor = Color.Red;
                richTextBox5.Text = "等待扫码！";
            }

            if (NiuJuREad)
            {
                richTextBox4.BackColor = Color.Green;
                button2.BackColor = Color.Green;
                richTextBox4.Text = "扭矩已经读取！";
            }
            else
            {
                richTextBox4.BackColor = Color.Red;
                button2.BackColor = Color.Red;
                richTextBox4.Text = "请测试扭矩！";
            }

            if (post == 0)
            {
                //不满足报工
                richTextBox6.Text = "报工条件不满足";
                richTextBox6.BackColor = Color.Yellow;
            }
            else if (post == 1)
            {
                richTextBox6.Text = "正在报工";
                richTextBox6.BackColor = Color.YellowGreen;
            }
            else if (post == 2)
            {
                richTextBox6.Text = "报工完成";
                richTextBox6.BackColor = Color.Green;
            }
            else if (post == 3)
            {
                richTextBox6.Text = "报工失败";
                richTextBox6.BackColor = Color.Red;
            }

        }

        //将读取到的值拼接成扭矩值
        public decimal ConstructDecimal(long num, long point)
        {
            // 将长整型数字转换为字符串
            string numStr = num.ToString();

            // 确保小数位数不超过实际数字长度
            if (point > numStr.Length)
            {
                throw new ArgumentException("Decimal places cannot be greater than the number of digits in the integer.");
            }

            // 分割整数和小数部分
            string integerPart = numStr.Substring(0, numStr.Length - (int)point);
            string fractionalPart = numStr.Substring(numStr.Length - (int)point);

            // 构造小数
            string decimalStr = $"{integerPart}.{fractionalPart}";

            // 转换为 decimal 类型
            decimal result = decimal.Parse(decimalStr);

            return result;
        }

        //串口读取服务
        private void ReadSerialPort()
        {
            byte[] buffer = new byte[22]; // 缓冲区大小

            try
            {
                while (true)
                {
                    if (serialPort.BytesToRead > 0)
                    {
                        // 读取数据到缓冲区
                        int bytesRead = serialPort.Read(buffer, 0, buffer.Length);

                        if (bytesRead != 22 && i == 0)
                        {
                            MessageBox.Show("数据不正确！");
                            Program.Logger.Error("数据读取错误,数据位：" + bytesRead);
                            buf.AddRange(buffer);//第一次读取一部分
                            i++;
                            continue;//跳出循环
                        }
                        else
                        {
                            buf.AddRange(buffer);//第二次读取
                            Program.Logger.Info("拿到了完整数据，数位：" + buf.Count);

                            buffer = buf.ToArray();//处理成完整数组
                            buf.Clear();
                            i = 0;
                        }

                        // 处理读取的数据
                        long[] data = new long[2];

                        if (string.IsNullOrEmpty(richTextBox1.Text))//为空
                        {
                            ScanRead = false;
                        }
                        else { ScanRead = true; }
                        chushihua();

                        if ( ScanRead)
                        {

                            data = Parse(buffer);

                            decimal niuju = ConstructDecimal(data[0], data[1]);
                            richTextBox2.Text = niuju.ToString();
                            NiuJuREad = true;

                            sQLiteTool.insert(richTextBox1.Text,richTextBox2.Text,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                          //  DateTime d = new DateTime;
                            chushihua();
                        }
                        else if ( !ScanRead)
                        {
                            MessageBox.Show("请先扫二维码！");
                        }

                        if (!MES) {

                            MessageBox.Show("MES链接失败，请检查网络链接状态！");
                        }

                        bool postisTrue = false;

                        if (post == 1) { MessageBox.Show("请等待上一个产品报工完成！"); }
                        else { postisTrue = true; }

                        if (ScanRead && NiuJuREad&& MES && postisTrue )
                        {
                            post = 1;

                            upMes();

                        }


                    }

                    // 可以根据需要添加延时，防止CPU过度使用
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                Program.Logger.Info("读取串口时发生异常: " + ex.Message);
            }
        }

        //初始化串对象
        public void InitializeSerialPort(string portName, int baudRate, int dataBits, StopBits stopBits, Parity parity)
        {
            serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);

            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
            // Open the serial port
            //serialPort.Open();

            // Add event handler for data received
            //serialPort.DataReceived += SerialPort_DataReceived;

            // Open the serial port
            serialPort.Open();

            if (serialPort.IsOpen)
            {
                NiuJuPort = true;
            }
            else { }


        }

        public async void upMes()
        {
            post = 1;
            chushihua();
            string key = "扭矩值";
            string v = richTextBox2.Text;
            string barfromtext = richTextBox1.Text;

            string barcode = barfromtext.TrimEnd();
            //string barcode = richTextBox1.Text;
            string newbaercode = barcode;

            textBox4.Text = barcode;
            textBox5.Text = v;

            List<KeyValuePair> li = new List<KeyValuePair>();
            li.Add(MyhHttp.create_DataTable(key, v));
            Dictionary<string, object> dt = MyhHttp.create_json_test(barcode, newbaercode, "JY811101", "8111Niuju0001", "8111GWJN", false, li);
            string json = JsonConvert.SerializeObject(dt);
            Program.Logger.Info("准备报工：" + json);
            // richTextBox6.Text = json;
            string ip = ipAddress;
            string api = Api_AddEquipInfo;
            string url = "http://" + ip + api;
            richTextBox3.Text = json;

            richTextBox1.Text = string.Empty;
            richTextBox2.Text = string.Empty;
            if (string.IsNullOrEmpty(richTextBox1.Text)) { ScanRead = false; } else { ScanRead = true; }
            if (string.IsNullOrEmpty(richTextBox2.Text)) { NiuJuREad = false; } else { NiuJuREad = true; }

            NiuJuREad = false;
            try
            {
                var response = await MyhHttp.myPost(url, json);
                richTextBox6.Text = response;
                ServerResponse serverresponse = JsonConvert.DeserializeObject<ServerResponse>(response);
                int code = serverresponse.Code;
                if (code == 201)
                {
                    Program.Logger.Error("失败！错误信息：" + response);
                    post = 3;
                    // chushihua();
                }
                else if (code == 0)
                {

                    post = 2;
                    //chushihua();
                    Program.Logger.Info("保存成功！" + response);
                }
                chushihua();
                count++;
                richTextBox7.Text = count.ToString();
            }
            catch (Exception ex)
            {
                richTextBox2.Text = ($"An error occurred: {ex.Message}");
                post = 3;
                chushihua();
            }
        }

        //解析数据
        public long[] Parse(byte[] data)
        {
            // 确保数据长度正确
            if (data.Length < 21)
            {
                Program.Logger.Info("数据长度不足");
                return null;
            }
            byte[] head = new byte[4];
            Array.Copy(data, 0, head, 0, 4);
            // 将字节数组转换为32位无符号整数
            Array.Reverse(head);
            uint value = BitConverter.ToUInt32(head, 0);
            // 将整数转换为16进制字符串
            string hexString = value.ToString("X8"); // 使用 "X8" 确保结果为8位16进制表示
            // 输出结果
            Program.Logger.Info("16进制表示: " + hexString);

            // 解析控制码
            byte controlCode = data[4];
            Program.Logger.Info($"控制码: {controlCode:X2}");

            // 解析节点短地址
            byte[] shortadd = new byte[2];
            Array.Copy(data, 5, head, 0, 2);
            Array.Reverse(shortadd);
            uint value1 = BitConverter.ToUInt16(shortadd, 0);
            string shortaddString = value1.ToString("X4"); //
            Program.Logger.Info("短地址进制表示: " + hexString);

            byte[] niuData = new byte[4];
            Array.Copy(data, 16, niuData, 0, 4);
            Array.Reverse(niuData);
            // 将字节数组转换为32位无符号整数
            uint value2 = BitConverter.ToUInt32(niuData, 0);
            Program.Logger.Info("扭矩值: " + value2);

            byte shortCode = data[20];
            long value3 = shortCode;//小数点
            Program.Logger.Info("小数点: " + value3);

            long[] result = new long[2];

            result[0] = value2;
            result[1] = value3;

            return result;
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            serialPort.Close();
            Environment.Exit(0);

        }

        private void 打开日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string folderPath = Program.logPath;

            FileTool.OpenFolder(folderPath);
            
        }

        private void 下载扭矩数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2  = new Form2();
            form2.Show();
        }
    }
}
