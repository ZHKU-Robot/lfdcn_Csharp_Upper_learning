using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        int count;//用于定时器计数
        int time;//存储设定的定时值
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = comboBox1.Text;//将下拉框内容添加到一个变量str
            time = Convert.ToInt16(str.Substring(0,2));//得到设定定时值（从0位开始，获取两位），转换为16为整型传给time。
            progressBar1.Maximum = time;//设置进度条的最大值
            timer1.Start();//启动定时器
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*生成1-99的数*/
            for(int i =1;i<100;i++)
            {
                comboBox1.Items.Add(i.ToString()+" 秒");
            }
            comboBox1.Text = "1 秒";//设定初始值
            label3.Text = "";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            count++;//记录秒数
            label3.Text = (time - count).ToString() + "S";//获得剩余时间并显示出来
            progressBar1.Value = count;//实时显示进度条进度
            if(count==time)
            {
                timer1.Stop();//结束定时器
                System.Media.SystemSounds.Exclamation.Play();//提示音
                MessageBox.Show("时间到了！", "Tips");
            }
        }
    }
}
