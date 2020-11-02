using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 完整串口助手
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SearchAndAddSerialToComboBox(serialPort1, comboBox1);
            comboBox2.Text = "115200";//设置默认波特率
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);//中断事件
        }

        private void port_DataReceived(object sender,SerialDataReceivedEventArgs e)//串口数据接收事件
        {
            //radioButton3为数值模式接收，!radioButton3则为字符串模式接收
            if(!radioButton3.Checked)
            {
                string str = serialPort1.ReadExisting();//字符串方式去读
                textBox1.AppendText(str);//添加内容到已读的字符后面
            }
            else//数值模式接收
            {
                byte data;
                data = (byte)serialPort1.ReadByte();//此处需要强制类型转换，由于ReadByte为整型，需要将其转换为byte类型数据
                string str = Convert.ToString(data, 16).ToUpper();//将接收数据转换成十六进制大写的字符串
                textBox1.AppendText("0x" + (str.Length == 1 ? "0" + str : str) + " ");//补充0，0xA转换为0x0A
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] Data = new byte[1];//因为一字节是8位，存放数值为8位发送，故使用byte存放
            if(serialPort1.IsOpen)//判断是否开启
            {
                if(textBox2.Text!="")//发送空数据则无操作
                {
                    if(!radioButton1.Checked)//字符发送
                    {
                        try
                        {
                            serialPort1.WriteLine(textBox2.Text);//往串口传送字符数据
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show("串口数据发送错误，请检查串口连接","ERROR");
                            serialPort1.Close();
                            button2.Enabled = true;
                        }
                    }
                    else//数值发送，且只能发送数值，如0xAA,则只需要发送AA即可，且默认无换行符
                    {
                        for (int i = 0; i < (textBox2.Text.Length - textBox2.Text.Length % 2) / 2; i++)//因为ASCII为8字节，故而需要两位两位发送，只发送数字总数的偶数位
                        {
                            Data[0] = Convert.ToByte(textBox2.Text.Substring(i * 2, 2),16);
                            serialPort1.Write(Data, 0, 1);
                        }
                        if (textBox2.Text.Length % 2 != 0)//发送剩下的最后一位（如果数字总数为奇数的话）
                        {
                            Data[0] = Convert.ToByte(textBox2.Text.Substring(textBox2.Text.Length - 1, 1), 16);
                            serialPort1.Write(Data, 0, 1);
                        }
                    }
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if(serialPort1.IsOpen)                                                                      //判断是否开启，以进行文字切换
            {
                serialPort1.Close();
                button2.Text = "打开串口";
            }
            else
            {
                try
                {
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);
                    serialPort1.Open();
                    button2.Text = "关闭串口";                                                          //文字切换
                }
                catch
                {
                    MessageBox.Show("串口打开失败，请检查串口的连接","ERROR");
                }
            }
        }

        /*搜索并添加串口号到ComcoBox中*/
        private void SearchAndAddSerialToComboBox(SerialPort MyPort, ComboBox MyBox)
        {                                                               
            string Buffer;                                                                              //缓存
            MyBox.Items.Clear();                                                                        //清空ComboBox内容
            for (int i = 1; i < 20; i++)
            {
                try                                                                                     //遍历完成串口的添加
                {
                    Buffer = "COM" + i.ToString();
                    MyPort.PortName = Buffer;
                    MyPort.Open();                                                                      //如果失败，continue
                    MyBox.Items.Add(Buffer);                                                            //打开成功，添加至下拉列表
                    MyPort.Close();                                                                     //关闭
                }
                catch
                {
                }
            }
        }
    }
}
