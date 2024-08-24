using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.IO.Ports;
//读取并保存配置文件

namespace Z3_Niuju
{
    public class PortConfig
    {
        private string configPath;
        private XmlDocument xmlDoc;

        private List<string> PortConfigList = new List<string>();

        public PortConfig()
        {
        }

        private void LoadConfig()
        {
            xmlDoc = new XmlDocument();

            try
            {
                if (File.Exists(configPath))
                {
                    xmlDoc.Load(configPath);

                    XmlNode rootNode = xmlDoc.SelectSingleNode("/Configuration/SerialPortSettings");
                    if (rootNode != null)
                    {
                        string portName = rootNode.SelectSingleNode("PortName").InnerText;
                        int baudRate = int.Parse(rootNode.SelectSingleNode("BaudRate").InnerText);
                        int dataBits = int.Parse(rootNode.SelectSingleNode("DataBits").InnerText);
                        string parity = rootNode.SelectSingleNode("Parity").InnerText;

                        Program.Logger.Info($"PortName: {portName}");
                        Program.Logger.Info($"BaudRate: {baudRate}");
                        Program.Logger.Info($"DataBits: {dataBits}");
                        Program.Logger.Info($"Parity: {parity}");

                        PortConfigList.Add(portName);
                        PortConfigList.Add(baudRate.ToString());
                        PortConfigList.Add(dataBits.ToString());
                        PortConfigList.Add(parity);

                    }
                    else
                    {
                        Program.Logger.Info("配置文件格式不正确！");
                    }
                }
                else
                {
                    Program.Logger.Info("配置文件不存在！");
                }
            }
            catch (Exception ex)
            {
                Program.Logger.Info($"发生错误：{ex.Message}");
            }
        }//加载

        public void Loda()
        {
            LoadConfig();
        }//加载

        public void SaveConfig(string portName, int baudRate, int dataBits, string parity)
        {
            try
            {
                XmlNode rootNode = xmlDoc.SelectSingleNode("/Configuration/SerialPortSettings");
                if (rootNode != null)
                {
                    rootNode.SelectSingleNode("PortName").InnerText = portName;
                    rootNode.SelectSingleNode("BaudRate").InnerText = baudRate.ToString();
                    rootNode.SelectSingleNode("DataBits").InnerText = dataBits.ToString();
                    rootNode.SelectSingleNode("Parity").InnerText = parity;

                    xmlDoc.Save(configPath);
                    MessageBox.Show("配置文件已更新。");
                }
                else
                {
                    MessageBox.Show("配置文件格式不正确！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发生错误：{ex.Message}");
            }
        } //修改保存

        public override bool Equals(object obj)
        {
            return obj is PortConfig manager &&
                   configPath == manager.configPath &&
                   EqualityComparer<XmlDocument>.Default.Equals(xmlDoc, manager.xmlDoc);
        }

        public override int GetHashCode()
        {
            int hashCode = -1107521736;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(configPath);
            hashCode = hashCode * -1521134295 + EqualityComparer<XmlDocument>.Default.GetHashCode(xmlDoc);
            return hashCode;
        }


        public string Path
        {
            set
            {
                configPath = value;
            }
        }

        public List<string> Port_List
        {
            get
            {
                return PortConfigList;

            }
        }
    }




}
