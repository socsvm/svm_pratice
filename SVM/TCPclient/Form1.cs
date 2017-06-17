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
using System.Collections.Generic;
using System.Linq;

namespace TCPclient
{

    public partial class Form1 : Form
    {
        byte[] message = new byte[100];

        byte[] buffer = new byte[26];

        int bytes = 0;
        bool status = false;

        List<string> trainingPointPositive = new List<string>();
        List<string> trainingPointNegative = new List<string>();

        bool isDrawSvmLine = false;
        Series series_svmLine;

        int drawMaxSize = 0, drawOffset = 0;
        int[] drawIntSurportVector, drawSvmLine;

        int[] intRecallPoint = new int[2];

        Series series3 = new Series("TrainingPoint", 1000);

        //IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.56.1"), 4001);      //  test IP
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("163.18.49.38"), 9999);      //  spark IP
        Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        Thread myThead = null;

        public Form1()
        {
            InitializeComponent();
            
        }

        Stopwatch stopWatch = new Stopwatch();
            IPEndPoint clientep1;
            int sec = 0;
            int minute = 0;
            int hour = 0;    
        private void BeginListen()      //socket接收監聽
        {
            
            while (true)
            {
                try
                {
                    if (server != null && server.Connected == true)
                    {
                        bytes = server.Receive(message);
                        if (bytes > 0)        //  判斷封包長度
                        {
                            status = true;
                            Console.WriteLine("bytes：" + bytes);

                            UartSendSurportVector();
                            bytes = 0;
                            Console.WriteLine("bytes2：" + bytes);
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
            timer2.Enabled = true;
            timer1.Enabled = true;
            stopWatch.Start();

            ////*****************圖表測試**************
            ////int[,] array = new int[,] {
            ////{1,8,9,7,105,11,50,999,500,1},
            ////{12,15,11,18,733,5,4,3,2,500} };

            //int[,] array = new int[,] {
            //{1,8,9,7,15,11,50,99,50,1},
            //{12,15,11,18,73,5,4,3,2,50} };

            //int[] svmLine = new int[100];
            //int a, b;   // SVM出來的參數    ax + b = y
            //int maxSize = 0;
            //a = 5; b = 3;       //暫定測試用
            //for (int i = 0; i < 100; i++)
            //{
            //    if (a * i + b < 100)
            //    {
            //        svmLine[i] = a * i + b;
            //    }
            //    else
            //    {
            //        maxSize = i - 1;
            //        break;
            //    }
            //}

            //Console.WriteLine("maxSize：" + maxSize);
            ////標題 最大數值
            //Series series1 = new Series("A類", 1000);
            //Series series2 = new Series("B類", 1000);
            //Series series_svmLine = new Series("SVM", a * maxSize + b);

            ////設定線條顏色
            //series1.Color = Color.Blue;
            //series2.Color = Color.Red;
            ////series1.Color = Color.White;
            ////series2.Color = Color.White;
            //series_svmLine.Color = Color.Green;

            ////設定字型
            //series1.Font = new System.Drawing.Font("微軟正黑體", 12);
            //series2.Font = new System.Drawing.Font("微軟正黑體", 12);
            //series_svmLine.Font = new System.Drawing.Font("微軟正黑體", 12);

            ////折線圖
            //series1.ChartType = SeriesChartType.Point;
            //series2.ChartType = SeriesChartType.Point;
            //series_svmLine.ChartType = SeriesChartType.Line;

            ////將數值顯示在線上
            //series1.IsValueShownAsLabel = true;
            //series2.IsValueShownAsLabel = true;
            //series_svmLine.IsValueShownAsLabel = false;

            ////將數值新增至序列
            //for (int index = 0; index < 10; index++)
            //{
            //    series1.Points.AddXY(index, array[0, index]);
            //    series2.Points.AddXY(index, array[1, index]);
            //}

            ////series1.Points.AddXY( 80);

            ////  繪製SVM分割線
            //for (int i = 0; i <= maxSize; i++)
            //{
            //    series_svmLine.Points.AddXY(i, svmLine[i]);
            //}

            ////將序列新增到圖上
            //this.chart1.Series.Add(series1);
            //this.chart1.Series.Add(series2);
            //this.chart1.Series.Add(series_svmLine);
            
            ////*******************************************
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
        }

        private void button1_Click(object sender, EventArgs e)      //  send測試資料
        {
            this.chart1.Series.Add(series3);

            String stringData = "End\n";
            byte[] Data = System.Text.Encoding.Default.GetBytes(stringData);
            server.Send(Data);

            socketRelink();
        }

        private void button4_Click(object sender, EventArgs e)      //  com port連線按鈕
        {
            try
            {
                serialPort1.Close();
                this.serialPort1 = new SerialPort(comboBox1.Text, 9600, Parity.None, 8, StopBits.One);
                serialPort1.Open();
                button4.Text = "ok";
                timer3.Enabled = true;
            }
            catch
            {

            }
        }

        private void socketRelink()     //  解決對spark傳送資料後會斷線的問題
        {
            Thread.Sleep(1000);     //待1秒後重新連線
            //Thread.Sleep(3000);     //待3秒後重新連線 - test用
           
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Connect(ipep);
            NetworkStream stream = new NetworkStream(server);

            Thread.Sleep(100);
        }

        private void btn_sendTrainingPoint_Click(object sender, EventArgs e)      //  設定學習特徵
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                if (textBox1.Text.IndexOf(".", 0) > 0 || textBox2.Text.IndexOf(".", 0) > 0)
                {
                    MessageBox.Show("請輸入0~99數值", "警告", MessageBoxButtons.OK);
                }
                else
                {
                    int[] trainingPoint = new int[2];
                    trainingPoint[0] = int.Parse(textBox1.Text);
                    trainingPoint[1] = int.Parse(textBox2.Text);

                    showTrainingPoint(trainingPoint);
                    socketSend(textBox7, textBox1, textBox2);
                }
            }
            else
            {
                MessageBox.Show("請填入正確數值", "警告", MessageBoxButtons.OK);
            }
        }

        private void socketSend(TextBox tb7, TextBox tb1, TextBox tb2)       //  透過socket傳送資料到spark
        {
            rememberTrainingPoint(tb7, tb1, tb2);

            byte[] Data = System.Text.Encoding.Default.GetBytes(tb7.Text + "," + tb1.Text + "," + tb2.Text + "\n");
            server.Send(Data);
            socketRelink();
        }

        private string socketRead()       //  透過socket讀取spark傳送來的資料
        {
            string surportVectorRead = System.Text.Encoding.UTF8.GetString(message, 0, bytes);      //  只傳送bytes[]中有資料的部分
            return surportVectorRead;
        }

        private void UartSendSurportVector()     //  將spark讀入的結果傳至zynq
        {
            string[] surportVector;
            surportVector = socketRead().Split(',');
            Console.WriteLine("surportVector[0]：" + surportVector[0]);
            Console.WriteLine("surportVector[1]：" + surportVector[1]);
            
            int[] intSurportVector = new int[2];
            intSurportVector[0] = int.Parse(surportVector[0]);
            intSurportVector[1] = int.Parse(surportVector[1]);
            int offset = findSurportvetorOffset(intSurportVector);

            timer4.Enabled = true;
            showSurportVector(offset, intSurportVector);

            string surportVectorSend = socketRead() + "," + offset;
            serialPort1.Write(surportVectorSend);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            issurportVectorGet();
        }

        private void issurportVectorGet()       //  確認GUI是否收到surportVector，若收到則enable button3
        {
            if (status == true)
            {
                Console.WriteLine("timer1_Tick");
                button3.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                timer1.Enabled = false;
            }
        }

        private void UartSendRecallPoint()      //  設定Recall特徵
        {
            clearTrainingPoint();
            int[] sendRecallPoint = new int[2];

            if (textBox4.Text != "" && textBox3.Text != "")
            {
                if (textBox4.Text.IndexOf(".", 0) > 0 || textBox3.Text.IndexOf(".", 0) > 0)
                {
                    MessageBox.Show("請輸入0~99數值", "警告", MessageBoxButtons.OK);
                }
                else
                {
                    string recallPoint = textBox4.Text + "," + textBox3.Text;
                    serialPort1.Write(recallPoint);

                    intRecallPoint[0] = int.Parse(textBox4.Text);
                    intRecallPoint[1] = int.Parse(textBox3.Text);
                }
            }
            else
            {
                MessageBox.Show("請填入正確數值", "警告", MessageBoxButtons.OK);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UartSendRecallPoint();
        }

        private void UartRead()     //  透過Uart讀取Zynq的辨識結果
        {
            if (serialPort1.BytesToRead > 0)      //  一定要判斷有沒有資料要收，要不然serialPort1.Read會一直處於等待狀態，以至於程式卡住
            {
                byte[] buffer = new byte[1];
                serialPort1.Read(buffer, 0, 1);

                int reCall = int.Parse(System.Text.Encoding.UTF8.GetString(buffer, 0, 1));
                showRecall(reCall);
            }

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            UartRead();
        }

        private void rememberTrainingPoint(TextBox tb7, TextBox tb1, TextBox tb2)        //  將輸入的TrainingPoint儲存下來供findSurportvetorOffset使用
        {
            switch (tb7.Text)
            {
                case "0":
                    Console.WriteLine("tb7.Text 0!");
                    trainingPointNegative.Add(tb1.Text + "," + tb2.Text);
                    break;
                case "1":
                    Console.WriteLine("tb7.Text 1!");
                    trainingPointPositive.Add(tb1.Text + "," + tb2.Text);
                    break;
                default:
                    Console.WriteLine("tb7.Text ERROR!");
                    break;
            }
        }

        private int findSurportvetorOffset(int[] intSurportVector)       //  由Surportveto找出offset值
        {
            Console.WriteLine("findSurportvetorOffset() ok");
            string[] pointNegative = trainingPointNegative.ToArray();
            string[] pointPositive = trainingPointPositive.ToArray();

            int[] findOffsetNegative = new int[pointNegative.Length];
            int[] findOffsetPositive = new int[pointPositive.Length];

            //**** 計算pointNegative & pointPositive的findOffset - START 
            for (int i = 0; i < pointNegative.Length; i++)
            {
                Console.WriteLine("pointNegative[" + i + "]：" + pointNegative[i]);
                string[] strPointNegative = pointNegative[i].Split(',');
                int[] intPointNegativ = new int[2];
                intPointNegativ[0] = int.Parse(strPointNegative[0]);
                intPointNegativ[1] = int.Parse(strPointNegative[1]);

                findOffsetNegative[i] = -(intSurportVector[0] * intPointNegativ[0] + intSurportVector[1] * intPointNegativ[1]);
                Console.WriteLine("findOffset[" + i + "]：" + findOffsetNegative[i]);
            }
            bubbleSort(findOffsetNegative);

            for (int i = 0; i < pointPositive.Length; i++)
            {
                Console.WriteLine("pointPositive[" + i + "]：" + pointPositive[i]);
                string[] strPointPositive = pointPositive[i].Split(',');
                int[] intPointPositive = new int[2];
                intPointPositive[0] = int.Parse(strPointPositive[0]);
                intPointPositive[1] = int.Parse(strPointPositive[1]);

                findOffsetPositive[i] = -(intSurportVector[0] * intPointPositive[0] + intSurportVector[1] * intPointPositive[1]);
                Console.WriteLine("findOffset[" + i + "]：" + findOffsetPositive[i]);
            }
            bubbleSort(findOffsetPositive);
            //**** 計算pointNegative & pointPositive的findOffset - END

            int offset = 0;

            if (findOffsetPositive[pointPositive.Length - 1] < findOffsetNegative[0])
            {
                offset = (findOffsetPositive[pointPositive.Length - 1] + findOffsetNegative[0]) / 2;
                Console.WriteLine("offset_A：" + offset);
            }
            else if (findOffsetPositive[0] > findOffsetNegative[pointNegative.Length - 1])
            {
                offset = (findOffsetPositive[0] + findOffsetNegative[pointNegative.Length - 1]) / 2;
                Console.WriteLine("offset_B：" + offset);
            }
            else
            {
                Console.WriteLine("offset ERROR!");
            }
            return offset;
        }

        private static void bubbleSort(int[] list)      //  氣泡排序
        {
            int n = list.Length;
            int temp;
            int Flag = 1; //旗標
            int i;
            for (i = 1; i <= n - 1 && Flag == 1; i++)
            {    // 外層迴圈控制比較回數
                Flag = 0;
                for (int j = 1; j <= n - i; j++)
                {  // 內層迴圈控制每回比較次數            
                    if (list[j] < list[j - 1])
                    {  // 比較鄰近兩個物件，右邊比左邊小時就互換。	       
                        temp = list[j];
                        list[j] = list[j - 1];
                        list[j - 1] = temp;
                        Flag = 1;
                    }
                }
            }
        }

        private void showSurportVector(int offset, int[] intSurportVector)        //  將surportVector顯示於圖表
        {
            //*****************圖表**************
            int[] svmLine = new int[100];
            int maxSize = 0;
            for (int i = 0; i < 100; i++)
            {
                if ((-(intSurportVector[0] * i + offset) / intSurportVector[1]) < 100)
                {
                    svmLine[i] = -(intSurportVector[0] * i + offset) / intSurportVector[1];
                }
                else
                {
                    maxSize = i - 1;
                    break;
                }
            }
            Console.WriteLine("maxSize：" + maxSize);

            drawMaxSize = maxSize;
            drawOffset = offset;
            drawIntSurportVector = intSurportVector;
            drawSvmLine = svmLine;

            ////標題 最大數值
            //Series series_svmLine = new Series("SVM", (-(intSurportVector[0] * maxSize + offset) / intSurportVector[1]));

            ////設定線條顏色
            //series_svmLine.Color = Color.Green;

            ////設定字型
            //series_svmLine.Font = new System.Drawing.Font("微軟正黑體", 12);

            ////折線圖
            //series_svmLine.ChartType = SeriesChartType.Line;

            ////將數值顯示在線上
            //series_svmLine.IsValueShownAsLabel = false;

            ////  繪製SVM分割線
            //for (int i = 0; i <= maxSize; i++)
            //{
            //    series_svmLine.Points.AddXY(i, svmLine[i]);
            //}

            //將序列新增到圖上
            //this.chart1.series.add(series1);
            //this.chart1.series.add(series2);
            isDrawSvmLine = true;
            //this.chart1.series.add(series_svmLine);

            //*******************************************
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            while (isDrawSvmLine)
            {
                Console.WriteLine("isDrawSvmLine");
                //標題 最大數值
                Series series_svmLine = new Series("SVM", (-(drawIntSurportVector[0] * drawMaxSize + drawOffset) / drawIntSurportVector[1]));

                //設定線條顏色
                series_svmLine.Color = Color.Green;

                //設定字型
                series_svmLine.Font = new System.Drawing.Font("微軟正黑體", 12);

                //折線圖
                series_svmLine.ChartType = SeriesChartType.Line;

                //將數值顯示在線上
                series_svmLine.IsValueShownAsLabel = false;

                //  繪製SVM分割線
                for (int i = 0; i <= drawMaxSize; i++)
                {
                    series_svmLine.Points.AddXY(i, drawSvmLine[i]);
                }

                //將序列新增到圖上
                this.chart1.Series.Add(series_svmLine);
                Console.WriteLine("this.chart1.Series.Add(series_svmLine);");
                if (drawOffset > 0)
                {
                    label1.Text = drawIntSurportVector[0] + "X + " + drawIntSurportVector[1] + "Y + " + drawOffset + " = 0";
                }
                else
                {
                    label1.Text = drawIntSurportVector[0] + "X + " + drawIntSurportVector[1] + "Y - " + -drawOffset + " = 0";
                }


                isDrawSvmLine = false;
            }
            //this.chart1.Series.Add(series_svmLine);

            //while (isDrawSvmLine)
            //{
            //    //*****************圖表測試**************
            //    //int[,] array = new int[,] {
            //    //{1,8,9,7,105,11,50,999,500,1},
            //    //{12,15,11,18,733,5,4,3,2,500} };

            //    int[,] array = new int[,] {
            //        {1,8,9,7,15,11,50,99,50,1},
            //        {12,15,11,18,73,5,4,3,2,50} };

            //    int[] svmLine = new int[100];
            //    int a, b;   // SVM出來的參數    ax + b = y
            //    int maxSize = 0;
            //    a = 5; b = 3;       //暫定測試用
            //    for (int i = 0; i < 100; i++)
            //    {
            //        if (a * i + b < 100)
            //        {
            //            svmLine[i] = a * i + b;
            //        }
            //        else
            //        {
            //            maxSize = i - 1;
            //            break;
            //        }
            //    }

            //    Console.WriteLine("maxSize：" + maxSize);
            //    //標題 最大數值
            //    int offset = 3;
            //    //Series series_svmLine = new Series("SVM", a * maxSize + b);
            //    //    Series series_svmLine = new Series("SVM", (-(drawIntSurportVector[0] * drawMaxSize + drawOffset) / drawIntSurportVector[1]));
            //    Series series_svmLine = new Series("SVM", (-(a*maxSize+offset)/b));

            //    //設定線條顏色
            //    series_svmLine.Color = Color.Green;

            //    //設定字型
            //    series_svmLine.Font = new System.Drawing.Font("微軟正黑體", 12);

            //    //折線圖
            //    series_svmLine.ChartType = SeriesChartType.Line;

            //    //將數值顯示在線上
            //    series_svmLine.IsValueShownAsLabel = false;

            //    //  繪製SVM分割線
            //    for (int i = 0; i <= maxSize; i++)
            //    {
            //        Console.WriteLine("i：" + i + ", svmLine：" + svmLine[i]);
            //        series_svmLine.Points.AddXY(i, svmLine[i]);
            //    }

            //    //將序列新增到圖上
            //    this.chart1.Series.Add(series_svmLine);

            //    //*******************************************
            //    isDrawSvmLine = false;
            //}
        }

        private void showRecall(int reCall)       //  顯示Recall結果
        {
            label7.Text = "(" + intRecallPoint[0] + ", " + intRecallPoint[1] + ") = " + reCall;

            if (reCall == 1)
            {
                Series series1 = new Series("A類", 1000);
                series1.Color = Color.Blue;
                series1.Font = new System.Drawing.Font("微軟正黑體", 12);
                series1.ChartType = SeriesChartType.Point;
                series1.IsValueShownAsLabel = true;
                series1.Points.AddXY(intRecallPoint[0], intRecallPoint[1]);
                this.chart1.Series.Add(series1);
            }
            else
            {
                Series series2 = new Series("B類", 1000);
                series2.Color = Color.Red;
                series2.Font = new System.Drawing.Font("微軟正黑體", 12);
                series2.ChartType = SeriesChartType.Point;
                series2.IsValueShownAsLabel = true;
                series2.Points.AddXY(intRecallPoint[0], intRecallPoint[1]);
                this.chart1.Series.Add(series2);
            }
        }

        private void showTrainingPoint(int[] trainingPoint)        //  將設定的TrainingPoint顯示於圖表
        {
            series3.Color = Color.YellowGreen;
            series3.Font = new System.Drawing.Font("微軟正黑體", 12);
            series3.ChartType = SeriesChartType.Point;
            series3.IsValueShownAsLabel = true;
            series3.Points.AddXY(trainingPoint[0], trainingPoint[1]);
        }

        private void clearTrainingPoint()       //  將TrainingPoint清除
        {
            chart1.Series.Clear();
        }
    }
}
