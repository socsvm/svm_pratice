using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Diagnostics;

using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO.Ports;

namespace TCPclient
{
    public partial class Form1 : Form
    {

        byte[] message = new byte[4096];

        byte[] buffer = new byte[26];

        //IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("163.18.59.224"), 4001);
        //IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.56.1"), 8080);
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("163.18.49.38"), 9999);
        Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        

        Thread myThead = null;

        public Form1()
        {
            InitializeComponent();
            
        }

        Stopwatch stopWatch = new Stopwatch();
            IPEndPoint clientep1;
            long RX_AA_count = 0;
            long RX_55_count = 0;
            int TX_flag = 0;
            long TX_AA_count = 0;
            long TX_55_count = 0;
            int sec = 0;
            int minute = 0;
            int hour = 0;    
        private void BeginListen()
        {
            int bytes = 0;
            
            while (true)
            {
                try
                {
                    if (server != null && server.Connected == true)
                    {
                        bytes = server.Receive(message);
                        if (bytes == 5)        //  判斷封包長度
                        {
                            Console.WriteLine("bytes：" + bytes);
                            for (int x = 0; x < bytes; x++)
                            {
                                if (message[x] == 0xaa)
                                {
                                    RX_AA_count++;
                                }
                                if (message[x] == 0x55)
                                {
                                    RX_55_count++;
                                }
                            }
                        }
                    }                               
                }
                catch (SocketException se)
                {           

                }
            }
        }
        #region//声名委托
        delegate void SetTextCallback(string text, int num);
       
        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            findSerialPort();       //  搜尋可用com port

            server.Connect(ipep);
            NetworkStream stream = new NetworkStream(server);
            StreamReader sr = new StreamReader(stream);
            StreamWriter sw = new StreamWriter(stream);

            clientep1 = (IPEndPoint)server.RemoteEndPoint;
            textBox6.Text = "與"+clientep1 + "連線";
            myThead = new Thread(new ThreadStart(BeginListen));
            myThead.Start();
            timer1.Enabled = true;
            timer2.Enabled = true;
            stopWatch.Start();

            //*****************圖表測試**************
            //int[,] array = new int[,] {
            //{1,8,9,7,105,11,50,999,500,1},
            //{12,15,11,18,733,5,4,3,2,500} };

            int[,] array = new int[,] {
            {1,8,9,7,15,11,50,99,50,1},
            {12,15,11,18,73,5,4,3,2,50} };

            int[] svmLine = new int[100];
            int a, b;   // SVM出來的參數    ax + b = y
            int maxSize = 0;
            a = 5; b = 3;
            for (int i = 0; i < 100; i++)
            {
                if (a * i + b < 100)
                {
                    svmLine[i] = a * i + b;
                }
                else
                {
                    maxSize = i-1;
                    break;
                }
            }

            Console.WriteLine("maxSize：" + maxSize);
            //標題 最大數值
            Series series1 = new Series("A類", 1000);
            Series series2 = new Series("B類", 1000);
            Series series_svmLine = new Series("SVM", a * maxSize + b);

            //設定線條顏色
            series1.Color = Color.Blue;
            series2.Color = Color.Red;
            //series1.Color = Color.White;
            //series2.Color = Color.White;
            series_svmLine.Color = Color.Green;

            //設定字型
            series1.Font = new System.Drawing.Font("微軟正黑體", 12);
            series2.Font = new System.Drawing.Font("微軟正黑體", 12);
            series_svmLine.Font = new System.Drawing.Font("微軟正黑體", 12);

            //折線圖
            series1.ChartType = SeriesChartType.Point;
            series2.ChartType = SeriesChartType.Point;
            series_svmLine.ChartType = SeriesChartType.Line;

            //將數值顯示在線上
            series1.IsValueShownAsLabel = true;
            series2.IsValueShownAsLabel = true;
            series_svmLine.IsValueShownAsLabel = false;

            //將數值新增至序列
            for (int index = 0; index < 10; index++)
            {
                series1.Points.AddXY(index, array[0, index]);
                series2.Points.AddXY(index, array[1, index]);
            }

            //series1.Points.AddXY( 80);

            //  繪製SVM分割線
            for (int i = 0; i <= maxSize; i++)
            {
                series_svmLine.Points.AddXY(i, svmLine[i]);
            }

            //將序列新增到圖上
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Series.Add(series_svmLine);
            
            //*******************************************




        }

        private void findSerialPort()       //  搜尋可用com port
        {
            string[] ports = SerialPort.GetPortNames();// Get a list of serial port names.

            if (ports.Length == 0)
            {
                MessageBox.Show("找不到COM port", "");
                Environment.Exit(1); //結束程序
            }

            foreach (string s in ports)
                comboBox1.Items.Add(s);
            comboBox1.SelectedIndex = 0; //預設第一個子選項索引值

            //throw new NotImplementedException();
        }

        private void timer1_Tick_1(object sender, System.EventArgs e)
        {
            byte[] TX_aa = new byte[] { 0xaa };
            byte[] TX_55 = new byte[] { 0x55 };


            //if (server != null && server.Connected == true)
            //{
            //    if (TX_flag == 1)
            //    {
            //        TX_flag = 0;
            //        server.Send(TX_aa, 1, 0);
            //        TX_AA_count++;
            //    }
            //    else
            //    {
            //        TX_flag = 1;
            //        server.Send(TX_55, 1, 0);
            //        TX_55_count++;
            //    }
            //}


            
        }

        private void timer2_Tick_1(object sender, System.EventArgs e)
        {
            if (stopWatch.ElapsedMilliseconds >= 1000)
            {
                sec++;
                if (sec > 59)
                {
                    sec = 0;
                    minute++;
                    if (minute > 59)
                    {
                        minute = 0;
                        hour++;
                    }
                }
                textBox5.Text = hour.ToString("D2") + ":" + minute.ToString("D2") + ":" + sec.ToString("D2");

                stopWatch.Restart();
            }
            

            //for (int i = 0; i < message.Length; i++)
            //{
            //    label7.Text = "" + message[i].ToString();
            //}
            //label7.Text = "" + message[0].ToString();
            label7.Text = System.Text.Encoding.UTF8.GetString(message);
        }

        private void button1_Click(object sender, EventArgs e)      //  send測試資料
        {
            String stringData = "ABC\n";
            //byte[] Data = new byte[] { 0xaa };
            byte[] Data = System.Text.Encoding.Default.GetBytes(stringData);
            server.Send(Data);
        }

        private void button4_Click(object sender, EventArgs e)      //  com port連線按鈕
        {
            try
            {
                serialPort1.Close();
                this.serialPort1 = new SerialPort(comboBox1.Text, 9600, Parity.None, 8, StopBits.One);
                serialPort1.Open();
                timer1.Enabled = true;
            }
            catch
            {
            }
        }
    }

   


}
