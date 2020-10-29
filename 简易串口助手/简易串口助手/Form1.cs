using System;
using System.Windows.Forms;

namespace 简易串口助手
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)//窗口初始化函数
        {
            string str; //临时存储i大写的十六进制格式字符串
            for(int i=0;i<256;i++)
            {
                str = i.ToString("x").ToUpper();//"x"设置转换为十六进制，ToUpper转化为大写，ToLower转化为小写
                comboBox1.Items.Add("0x" + (str.Length == 1 ? "0" + str : str));//如果为0xA则可在其前加入0x0A
            }
            comboBox1.Text = "0x00";//设置初始为0x00
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string data = comboBox1.Text;//获取下拉框选中的数值
            string convertdata = data.Substring(2, 2);//把字符分开
            byte[] buffer = new byte[1];//数据一个字节就够了
            buffer[0] = Convert.ToByte(convertdata, 16);//将字符串转化为byte型变量（byte）==uchar
            try
            {
                serialPort1.Open();//打开串口
                serialPort1.Write(buffer, 0, 1);//发送一个字节
                serialPort1.Close();//关闭串口
            }
            catch//异常捕获
            {
                if (serialPort1.IsOpen)
                    serialPort1.Close();
                MessageBox.Show("请检查端口连接","error");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
