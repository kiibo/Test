using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using WatchingSystemFiles;
using System.Xml;
using System.Threading;
using System.Net.NetworkInformation;
using System.Collections;
using System.Text.RegularExpressions;


namespace Zebra
{

    public partial class Zebra : Form
    {
        ErrorCode ErrorCod = new ErrorCode();

        [DllImport("SSSE32.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr ICC_Reader_Open(byte[] trkBuf);
        [DllImport("SSSE32.dll", CharSet = CharSet.Ansi)]
        public static extern int PICC_Reader_Request(byte[] trkBuf);
        [DllImport("SSSE32.dll", CharSet = CharSet.Ansi)]
        public static extern int PICC_Reader_SetTypeA(IntPtr ReaderHandle);//设置读typeA
        [DllImport("SSSE32.dll", CharSet = CharSet.Ansi)]
        public static extern int PICC_Reader_SetTypeB(IntPtr ReaderHandle);//设置读typeB
        [DllImport("SSSE32.dll", CharSet = CharSet.Ansi)]
        public static extern int  PICC_Reader_Select(IntPtr ReaderHandle, byte cardtype);//选择卡片，41为typea,M1 42为typeb,TypeB卡片需先上电后选卡
        [DllImport("SSSE32.dll", CharSet = CharSet.Ansi)]
        public static extern int PICC_Reader_Request(IntPtr ReaderHandle);//typea M1请求卡片
        [DllImport("SSSE32.dll", CharSet = CharSet.Ansi)]
        public static extern int PICC_Reader_RFControl(long ReaderHandle);//关闭天线
        [DllImport("SSSE32.dll", CharSet = CharSet.Ansi)]
        public static extern int PICC_Reader_Authentication(IntPtr ReaderHandle, byte Mode, byte SecNr);//认证密钥 M1
        [DllImport("SSSE32.dll", CharSet = CharSet.Ansi)]
        public static extern int PICC_Reader_Authentication_Pass(IntPtr ReaderHandle, byte Mode, byte SecNr,byte[] password);//认证密钥 M1
        [DllImport("SSSE32.dll", CharSet = CharSet.Ansi)]
        public static extern int PICC_Reader_Read(IntPtr ReaderHandle, byte Addr, byte[] Data);	//读取卡片M1
        [DllImport("SSSE32.dll", CharSet = CharSet.Ansi)]
        public static extern int ICC_DispSound(IntPtr ReaderHandle,byte type,byte aaf); //声音提示信息 type(1~10)
        [DllImport("SSSE32.dll", CharSet = CharSet.Ansi)]
        public static extern int ICC_PosBeep(IntPtr ReaderHandle, byte time);//蜂鸣

        public static SZYB sz;
        public static Zebra formthis = null;
        public static ClsUMS Tran = new ClsUMS();
        Thread threadReadCard;  //读卡线程
        SEEE treadcard;
        public Zebra()
        {
            InitializeComponent();
            formthis = this;

            /*
            string Strior = GetFileString2(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "jacorb.ior"));
            string HospitalNo = "H6090";
            string OperatorID = "H609961";//H
            string OperatorName = "中信中航";
            bool SaveException = true;
            sz = new SZYB(Strior, HospitalNo, OperatorID, OperatorName, SaveException);
            string  YbCardPWD = "";
            hi.GRXX YbDetailInfo = new hi.GRXX();

            string YbCardOrialData = "%GAKAJFJXSAUKHKIWIADK?;07279391475383707923?";
            //YbCardOrialData = "%GALMMJSMSMUKZNNIDNDN?;07855945455346672626?";
            hi.GRXX grxx;
            hi.modMZ.BDGRXX bdGrxx;
            string errMsg = string.Empty;
            if (sz.GetGRXBXX(YbCardOrialData, YbCardPWD, out  grxx, out errMsg))
            {
                YbDetailInfo = grxx;
                string SBCardID = grxx.YLZH;
                //MainWindow.HisData.Cardno = MainWindow.HisData.SBCardID;
                //MainWindow.HisData.IDCardNo = grxx.SFZH;
                //MainWindow.HisData.SBCardComputer = grxx.DNH;
                //MainWindow.HisData.SBCardName = Citic_Data.TransSAToString(grxx.XM);
                //MainWindow.HisData.Hzxm = MainWindow.HisData.SBCardName;
                //MainWindow.HisData.Csrq = grxx.CSSJ;
                //MainWindow.HisData.SBCardAccount = grxx.ACCOUNT;
                //MainWindow.HisData.SBCBLX = grxx.CBLX;
            }
*/
            WatchingSystemFiles.WatchSystemFile watchfile = new WatchingSystemFiles.WatchSystemFile();
            WatchingSystemFiles.getIP iip = new WatchingSystemFiles.getIP();
            this.msg.Text = iip.GetLocalIp();
            watchfile.watchingFile();

        }

        

        public static string Flag = "";

        public class 医生
        {
            public string 医生姓名;
            public string 职称;
            public string 挂号代码;
            public string 护士站号;
            public string 挂号费;
            public string 诊疗费;
            public string 医生工号;
            public string 就诊位置;
            public string 候诊人数;
            public int MorningCount;//上午可用数量
            public int AfternoonCount;//下午可用挂号数量
            public int SumCount;//可用数量Sum
        }
        private void button1_Click(object sender, EventArgs e)
        {

            treadcard = new SEEE();
            treadcard.Init();
            threadReadCard = new Thread(new ThreadStart(treadcard.ReadM1));
            threadReadCard.IsBackground = true;
            threadReadCard.Start();

            /*
            int n = -1;
            IntPtr ReaderHandle = IntPtr.Zero;
            byte[] com = ASCIIEncoding.ASCII.GetBytes("USB1");
            ReaderHandle = ICC_Reader_Open(com);
            byte[] revdata = new byte[17];

            ICC_DispSound(ReaderHandle,1,1); //声音提示信息 type(1~10)

	        //1、请插卡 2、请刷卡 3、读卡错误 4、请输入密码  5、密码错误  6、操作成功 7、操作超时 8、操作失败 9、请取回卡

            while (true)
            {
                Thread.Sleep(1000);
                n = PICC_Reader_Request(ReaderHandle);
                if (n == 0)
                {
                    ICC_PosBeep(ReaderHandle, 2);



                    PICC_Reader_Select(ReaderHandle, 0x41);
                    n = PICC_Reader_Authentication(ReaderHandle, 0, 1);

                    n = PICC_Reader_Read(ReaderHandle, 4, revdata);

                    string returnStr = "";
                    for (int i = 0; i < revdata.Length; i++)
                    {
                        returnStr += revdata[i].ToString("X2");
                    }
                    this.msg.Text = "卡号：" + returnStr.Substring(0, 8);
                    this.OutXml.AppendText("卡号：" + returnStr.Substring(0, 8));
                }
            }
            */
/*
            IntPtr handle = IntPtr.Zero;
            int printerType = 0;
            int err = 0;
            string printerName = "Zebra ZXP Series 3 USB Card Printer";
            //if (Decimal.Parse("1.5") > 0)
            //    MessageBox.Show("Succ");

            //get handle to printer driver;
            byte[] prnDriver = ASCIIEncoding.ASCII.GetBytes(printerName);
            int result = ZebraDLL.ZBRGetHandle(out handle, prnDriver,out printerType,out err);
            //this.msg.Text = "getHandle msg=" + err.ToString()+",result="+result.ToString();

            byte[] trkBuf = null;
            int errValue = 0;
            int respSize = 0;
            string trackMsg = "";
            result = ZebraDLL.ZBRPRNEndSmartCard(handle,printerType,1,1,out err);
            //result = ZebraDLL.ZBRPRNGetMsgSuppressionState(handle, printerType,out errValue,out err);
            this.msg.Text = "--err:" + err.ToString() + ErrorCod.GetError(err) + "--respSize:" + respSize + ErrorCod.GetError(respSize); ;

            if (this.TrackNum.Text.Trim() == "" || this.TrackNum.Text == null)
            {
                MessageBox.Show("请输入磁道数");
            }
            else
            {
                int trkNum = int.Parse(this.TrackNum.Text);
                try
                {
                    trkBuf = new byte[50];
                    result = ZebraDLL.ZBRPRNReadMagByTrk(handle, printerType, trkNum, trkBuf, out respSize, out errValue);
                    if (result == 1 && errValue == 0)
                    {
                        trackMsg = ASCIIEncoding.ASCII.GetString(trkBuf, 0, respSize - 1);
                        this.msg.Text = "磁道" + this.TrackNum.Text + "信息：" + trackMsg;
                    }
                    else
                    {
                        this.msg.Text = "ReadMagByTrk errMessage=" + errValue.ToString() + ",result=" + result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    this.msg.Text = ex.Message;
                }
                finally
                {
                    trkBuf = null;
                }
            }
            */
        }

        private void start_Click(object sender, EventArgs e)
        {
            string resultzz = "10##";
            string iDcoard = "";
            if (iDcoard != "" || iDcoard != null)
            {
                resultzz = "";
            }
            string ResultCode = resultzz.Substring(0, 1);
            string ResultData = resultzz.Substring(1);
            string[] records = Regex.Split(ResultData, "@@", RegexOptions.IgnoreCase);
            if (records.Length > 0)
            {
                for (int i = 0; i < records.Length; i++)
                {
                    if (records[i] == "") continue;
                    string[] nodes = Regex.Split(records[i], "##", RegexOptions.IgnoreCase);
                    if (nodes.Length > 0)
                    {
                        string CardBalance = nodes[0];
                    }

                }
            }
            string sszsds = DateTime.Now.ToString("HHmmss");
            string strSecondTrace = "1245745112=123457851";
            int index = strSecondTrace.IndexOf('=') + 5;

            List<医生> ls = new List<医生> ();
            long ii = 0;

            string dd = "10:35";
            int dt=int.Parse(dd.Split(':')[0])-1;
            dd = dt.ToString() + ":" + dd.Split(':')[1];
            dd = dt.ToString("hh:mm");

            while (true)
            {
                ii++;
                if (ii > 10000000000000000)
                    break;
            }
            for (int i = 0; i < 10; i++)
            {
                Random ra = new Random(i);
                医生 ys = new 医生();
                ys.挂号代码 = i.ToString();
                ys.挂号费 = "3";
                ys.候诊人数 = "1";
                ys.护士站号 = "1";
                ys.MorningCount = (int)10*ra.Next(0,10);
                ys.AfternoonCount = (int)10 * ra.Next(0,10);
                ys.SumCount = ys.MorningCount + ys.AfternoonCount;
                ls.Add(ys);
            }
            ls=ls.OrderByDescending(a => a.MorningCount).ThenByDescending(a=>a.AfternoonCount)
                .ToList<医生>();

            Stack q = new Stack();
            q.Push("0");
            q.Push("14");

            q.Push("33");

            string aa=q.Pop().ToString();


            string outstr = "000000交易成功                                000118331566000001621773******2566   2512  15090460121470626876       3023401806200880000000001002016100815094501                                                                                  20160908150904                                                                                                                                                                                                                                                                 ";

            string bankTranInfo = outstr.Substring(0, 6).Trim() + "|" + outstr.Substring(6, 20).Trim() + "|" + outstr.Substring(42, 6).Trim() + "|" + outstr.Substring(48, 6).Trim() + "|" + outstr.Substring(54, 6).Trim() + "|" + outstr.Substring(60, 19).Trim() + "|" + outstr.Substring(79, 4).Trim() + "|" + outstr.Substring(83, 2).Trim() + "|" + outstr.Substring(85, 12).Trim() + "|" + outstr.Substring(97, 15).Trim() + "|" + outstr.Substring(112, 15).Trim() + "|" + outstr.Substring(127, 12).Trim() + "|" + outstr.Substring(139, 16).Trim() + "|" + outstr.Substring(155, 74).Trim() + "|" + outstr.Substring(229, 8).Trim() + "|" + outstr.Substring(237, 8).Trim() + "|" + outstr.Substring(245, 12).Trim();
            
            IntPtr handle = IntPtr.Zero;
            int printerType = 0;
            int err = 0;
            string printerName = "Zebra ZXP Series 3 USB Card Printer";
            //if (Decimal.Parse("1.5") > 0)
            //    MessageBox.Show("Succ");

            //get handle to printer driver;
            byte[] prnDriver = ASCIIEncoding.ASCII.GetBytes(printerName);
            int result = ZebraDLL.ZBRGetHandle(out handle, prnDriver, out printerType, out err);
            //this.msg.Text = "getHandle msg=" + err.ToString()+",result="+result.ToString();

            if (result == 1 && err == 0)
                this.msg.Text = "getHandle Success!!";
            else
            {
                this.msg.Text = "getHandle Failed!!" + ErrorCod.GetError(err);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string test = "622343423224888";
            test = test.Substring(0, 4) + "******" + test.Substring(test.Length - 4, 4);
            ////挂号
            //tmpP = new Citic_Drive2.FPPrint("6", string.Format(" 银行卡号：{0}", MainWindow.HisData.BankNo.Substring(0, 4) + "******" + MainWindow.HisData.BankNo.Substring(MainWindow.HisData.BankNo.Length - 4, 4)));
            ////预约取号
            //tmpP = new Citic_Drive2.FPPrint("6", string.Format(" 银行卡号：{0}", MainWindow.HisData.BankNo.Substring(0, 4) + "******" + MainWindow.HisData.BankNo.Substring(MainWindow.HisData.BankNo.Length - 4, 4)));
            this.msg.Text = test;

            /*

            if (this.TrackNum.Text.Trim() == "" || this.TrackNum.Text == null)
            {
                MessageBox.Show("请输入时间");
            }
            else
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(this.TrackNum.Text);
                    SynchronousTime(dt);
                    this.msg.Text = "系统同步成功";

                }
                catch
                {
                    this.msg.Text = "系统同步失败！";
                }
            }
             * */
        }

        #region 设置与服务器同步时间
        /// <summary>
        /// 设置与服务器同步时间 
        /// </summary>
        private static void SynchronousTime(DateTime time)
        {
            try
            {
                #region 更改计算机时间

                SystemTime sysTime = new SystemTime();

                DateTime ServerTime = time;

                sysTime.wYear = Convert.ToUInt16(ServerTime.Year);

                sysTime.wMonth = Convert.ToUInt16(ServerTime.Month);

                //处置北京时间 

                int nBeijingHour = ServerTime.Hour - 8;

                if (nBeijingHour <= 0)
                {
                    nBeijingHour += 24;

                    sysTime.wDay = Convert.ToUInt16(ServerTime.Day - 1);

                    sysTime.wDayOfWeek = Convert.ToUInt16(ServerTime.DayOfWeek - 1);
                }
                else
                {
                    sysTime.wDay = Convert.ToUInt16(ServerTime.Day);

                    sysTime.wDayOfWeek = Convert.ToUInt16(ServerTime.DayOfWeek);
                }

                sysTime.wHour = Convert.ToUInt16(nBeijingHour);

                sysTime.wMinute = Convert.ToUInt16(ServerTime.Minute);

                sysTime.wSecond = Convert.ToUInt16(ServerTime.Second);

                sysTime.wMiliseconds = Convert.ToUInt16(ServerTime.Millisecond);

                Win32.SetSystemTime(ref sysTime);

                #endregion
            }
            catch
            {
                //产生错误则不引发异常。
            }
        }
        #endregion



        /// <summary>
        /// 文字图片生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            Graphics g = Graphics.FromImage(new Bitmap(1, 1));
            Font font = new Font("微软雅黑",32);
            SizeF sizeF = g.MeasureString(this.TrackNum.Text, font); //测量出字体的高度和宽度  
            System.Drawing.Brush brush; //笔刷，颜色  
            brush = System.Drawing.Brushes.Black;
            PointF pf = new PointF(0, 0);
            Bitmap img = new Bitmap(Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height));
            g = Graphics.FromImage(img);
            g.DrawString(this.TrackNum.Text, font, brush, pf);
            //img.Save(System.Windows.Forms.Application.StartupPath + "\\" + "img_name" + "\\" + this.TrackNum.Text + ".bmp", System.Drawing.Imaging.ImageFormat.Png);


            //RototeImg(GetSourceImg(System.Windows.Forms.Application.StartupPath + "\\" + "img_name" + "\\" + this.TrackNum.Text + ".bmp"), 180);
            //Bitmap img2 = new Bitmap(Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height));
            img=RotateImage(img, 180);
            img.Save(System.Windows.Forms.Application.StartupPath + "\\" + "img_name" + "\\" + this.TrackNum.Text + "x.bmp", System.Drawing.Imaging.ImageFormat.Png);

        }


        #region 图片旋转
        public Image RototeImg(Image b, int angle)
        {
            angle = angle % 360;
            //弧度转换
            double radian = angle * Math.PI / 180.0;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);

            int w = b.Width;
            int h = b.Height;
            int W = (int)(Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));
            int H = (int)(Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));
            //目标位图
            Bitmap dsImage = new Bitmap(W, H);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(dsImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality; 

            //计算偏移量

            Point Offset = new Point((W - w) / 2, (H - h) / 2);
            Rectangle rect = new Rectangle(Offset.X, Offset.Y, w, h);
            Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform(360 - angle);

            g.TranslateTransform(-center.X, -center.Y);
            g.DrawImage(b, rect);

            g.ResetTransform();
            g.Save();
            g.Dispose();

            b.Dispose();
            dsImage.Save("FocusPoint1.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            return dsImage;
        }
        #endregion

        public  string GetFileString(string fileName)
        {
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                string retStr = System.Text.Encoding.Default.GetString(data);
                retStr = retStr.Substring(0, retStr.Length - 2);
                return retStr;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public  string GetFileString2(string fileName)
        {
            string iorString = "";
            try
            {
                StreamReader reader;
                if (File.Exists(fileName))
                {
                    reader = new StreamReader(fileName, System.Text.Encoding.UTF8);
                    if (reader.Peek() != -1)
                    {
                        iorString = reader.ReadLine();
                        reader.Close();
                    }
                }
                return iorString;
            }
            catch (Exception ex)
            {
                return iorString;
            }
        }

        public Image GetSourceImg(string filename)
        {
            Image img;
            img = Bitmap.FromFile(filename);
            return img;
        }

        public  Bitmap RotateImage(Image image, int angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            const double pi2 = Math.PI / 2.0;
            double oldWidth = (double)image.Width;
            double oldHeight = (double)image.Height;
            double theta = ((double)angle) * Math.PI / 180.0;
            double locked_theta = theta;
            while (locked_theta < 0.0)
                locked_theta += 2 * Math.PI;
            double newWidth, newHeight;
            int nWidth, nHeight;
            #region Explaination of the calculations
            #endregion
            double adjacentTop, oppositeTop;
            double adjacentBottom, oppositeBottom;
            if ((locked_theta >= 0.0 && locked_theta < pi2) ||
                (locked_theta >= Math.PI && locked_theta < (Math.PI + pi2)))
            {
                adjacentTop = Math.Abs(Math.Cos(locked_theta)) * oldWidth;
                oppositeTop = Math.Abs(Math.Sin(locked_theta)) * oldWidth;
                adjacentBottom = Math.Abs(Math.Cos(locked_theta)) * oldHeight;
                oppositeBottom = Math.Abs(Math.Sin(locked_theta)) * oldHeight;
            }
            else
            {
                adjacentTop = Math.Abs(Math.Sin(locked_theta)) * oldHeight;
                oppositeTop = Math.Abs(Math.Cos(locked_theta)) * oldHeight;
                adjacentBottom = Math.Abs(Math.Sin(locked_theta)) * oldWidth;
                oppositeBottom = Math.Abs(Math.Cos(locked_theta)) * oldWidth;
            }
            newWidth = adjacentTop + oppositeBottom;
            newHeight = adjacentBottom + oppositeTop;
            nWidth = (int)Math.Ceiling(newWidth);
            nHeight = (int)Math.Ceiling(newHeight);
            Bitmap rotatedBmp = new Bitmap(nWidth, nHeight);
            using (Graphics g = Graphics.FromImage(rotatedBmp))
            {
                Point[] points;
                if (locked_theta >= 0.0 && locked_theta < pi2)
                {
                    points = new Point[] { new Point( (int) oppositeBottom, 0 ), new Point( nWidth, (int) oppositeTop ),new Point( 0, (int) adjacentBottom )};
                }
                else if (locked_theta >= pi2 && locked_theta < Math.PI)
                {
                    points = new Point[] {  new Point( nWidth, (int) oppositeTop ), new Point( (int) adjacentTop, nHeight ),new Point( (int) oppositeBottom, 0 ) };
                }
                else if (locked_theta >= Math.PI && locked_theta < (Math.PI + pi2))
                {
                    points = new Point[] {  new Point( (int) adjacentTop, nHeight ),  new Point( 0, (int) adjacentBottom ), new Point( nWidth, (int) oppositeTop ) };
                }
                else
                {
                    points = new Point[] { new Point( 0, (int) adjacentBottom ), new Point( (int) oppositeBottom, 0 ),new Point( (int) adjacentTop, nHeight )};
                }
                g.DrawImage(image, points);
            }
            return rotatedBmp;
        }

        private void M1_Card_Click(object sender, EventArgs e)
        {
            SendCard();
            string test="";
            List<string> ll = new List<string>();
            if (this.TrackNum.Text == null || this.TrackNum.Text.Trim() == "")
            {
                //MessageBox.Show("请输入");
            }
            else
            {
                test = this.TrackNum.Text;
            }
            string safsa="00000440<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
            safsa += @"
<Response>
    <TransCode>2006</TransCode>
    <ResultCode>0</ResultCode>
    <ErrorMsg></ErrorMsg>
    <PatientID>330119</PatientID>
    <PatName>陈菊</PatName>
    <PatSex>女</PatSex>
    <Birthday>1985-10-06</Birthday>
    <Age>30岁</Age>
    <IDCard>500227198510064625</IDCard>
    <AccBalance>0.00</AccBalance>
    <ZyBalance>0.00</ZyBalance>
    <Tel>18616787823</Tel>
</Response>";
            
            //20块数据处理  年月日+标识+身份证
            string IDCardNo = "43112919881207261X";
            string te5st=IDCardNo.Substring(0, 10) + "****" + IDCardNo.Substring(IDCardNo.Length-4,4);
            string writestr = IDCardNo.Substring(6, 8);
            if (IDCardNo.Contains("X"))
            {
                writestr += "17"+IDCardNo.Substring(0,17)+"0";
            }
            else
            {
                writestr += "18"+IDCardNo;
            }
            //20块数据处理


            test = writestr;

            byte[] data = new byte[16];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = 0xFF;
            }
            if (test.Length % 2 == 1)
            {
                test = test + "F";
            }
            byte[] datat = new byte[test.Length / 2];
            for (int i = 0; i < datat.Length; i++)
            {
                datat[i] = Convert.ToByte(test.Substring(i * 2, 2), 16);
                ll.Add(test.Substring(i * 2, 2));
            }
            for (int i = 0; i < datat.Length; i++)
            {
                data[i] = datat[i];
            }
            //写卡
            this.msg.Text = "data.Length="+data.Length.ToString()+"/r/n data="+data.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string OutXml = this.OutXml.Text;
            string a = "012391431";
            string b = "我啊凤姐啊";
            string c = "1000";
            string Len=string.Format("{0}{1}{2}", a.PadRight(12), b.PadRight(20), c.PadLeft(10));

            try
            {
                string ResultCode = "";
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.LoadXml(OutXml);
                XmlNodeList topM = doc.DocumentElement.ChildNodes;
                foreach (XmlElement ell in topM)
                {
                    if (ell.Name == "Result")
                    {
                        ResultCode = ell.InnerText;
                        continue;
                    }
                    if (ell.Name == "Register")
                    {
                        XmlNodeList allList = ell.ChildNodes;

                        foreach (XmlElement ell2 in allList)
                        {
                            if (ell2.Name == "ClinicNO")
                            {
                                ResultCode = ell2.InnerText;
                                continue;
                            }
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                this.msg.Text=ex.ToString();
            }
        }

        private void WebService_Click(object sender, EventArgs e)
        {
            WebData.Class1 webdata = new WebData.Class1();
            string[] r = webdata.getWebData();

            string s = webdata.getServiceData("Guilin", "China");

        }


        public void SendCard()
        {
            while (true)
            {

                int n = 0;

                if (n != 0) continue;
                while (true)
                {

                    if (n == 0) continue;
                    string M1 = "";
                    if (M1 == "")
                    {
                        continue;
                    }
                    else
                    {
        
                        WriteCard();//写卡成功，Flag为0，失败其他
                        
                        if (Flag == "0")
                        {
                                return;
                 
                        }
                        else
                        {
                            break;
                        }

                    }
                    //}
                }
            }
        }

        public void WriteCard()
        {
            for (int k = 0; k < 9; k++)
            {
                int n = 0;
                if (n!= 0)
                {
                    if (k < 8)
                        continue;
                    else
                    {
                        Flag = "-1";
                    }
                }
        

            
                if (n != 0)
                {
                    if (k < 8)
                    {
                        continue;
                    }
                    else
                    {

                        Flag = "-1";
                    }
                }
                string M1 = "";
                if (M1 == "")
                {
                    if (k < 8)
                    {
                        continue;
                    }
                    else
                    {
                        Flag = "-1";
                    }
                }
                else
                {
 
                    if (false)
                    {
                        Flag = "0";
                        break;
                    }
                    else//写卡失败
                    {
                        if (k < 8)
                        {
                            continue;
                        }
                        else
                        {
                            Flag = "-1";
                        }

                    }
                }
            }
        }

        public class SEEE
        {
            [DllImport("SSSE32.dll", CharSet = CharSet.Ansi)]
            public static extern IntPtr ICC_Reader_Open(byte[] trkBuf);
            [DllImport("SSSE32.dll", CharSet = CharSet.Ansi)]
            public static extern int PICC_Reader_Request(byte[] trkBuf);
            [DllImport("SSSE32.dll", CharSet = CharSet.Ansi)]
            public static extern int PICC_Reader_SetTypeA(IntPtr ReaderHandle);//设置读typeA
            [DllImport("SSSE32.dll", CharSet = CharSet.Ansi)]
            public static extern int PICC_Reader_SetTypeB(IntPtr ReaderHandle);//设置读typeB
            [DllImport("SSSE32.dll", CharSet = CharSet.Ansi)]
            public static extern int PICC_Reader_Select(IntPtr ReaderHandle, byte cardtype);//选择卡片，41为typea,M1 42为typeb,TypeB卡片需先上电后选卡
            [DllImport("SSSE32.dll", CharSet = CharSet.Ansi)]
            public static extern int PICC_Reader_Request(IntPtr ReaderHandle);//typea M1请求卡片
            [DllImport("SSSE32.dll", CharSet = CharSet.Ansi)]
            public static extern int PICC_Reader_RFControl(long ReaderHandle);//关闭天线
            [DllImport("SSSE32.dll", CharSet = CharSet.Ansi)]
            public static extern int PICC_Reader_Authentication(IntPtr ReaderHandle, int Mode, int SecNr);//认证密钥 M1
            [DllImport("SSSE32.dll", CharSet = CharSet.Ansi)]
            public static extern int PICC_Reader_Authentication_Pass(IntPtr ReaderHandle, byte Mode, byte SecNr, byte[] password);//认证密钥 M1
            [DllImport("SSSE32.dll", CharSet = CharSet.Ansi)]
            public static extern int PICC_Reader_Read(IntPtr ReaderHandle, int Addr, byte[] Data);	//读取卡片M1
            [DllImport("SSSE32.dll", CharSet = CharSet.Ansi)]
            public static extern int ICC_DispSound(IntPtr ReaderHandle, byte type, byte aaf); //声音提示信息 type(1~10)
            [DllImport("SSSE32.dll", CharSet = CharSet.Ansi)]
            public static extern int ICC_PosBeep(IntPtr ReaderHandle, byte time);//蜂鸣

            int n = -1;
            IntPtr ReaderHandle = IntPtr.Zero;
            public int Init()
            {
                byte[] com = ASCIIEncoding.ASCII.GetBytes("USB1");
                ReaderHandle = ICC_Reader_Open(com);
                if (ReaderHandle == IntPtr.Zero)
                {
                    return -1;

                }
                else
                {
                    return 0;
                }
            }
            public void ReadM1()
            {
                byte[] revdata = new byte[19];

                ICC_DispSound(ReaderHandle, 1, 1); //声音提示信息 type(1~10)
                /*
                    1、请插卡 2、请刷卡 3、读卡错误 4、请输入密码  5、密码错误  6、操作成功 7、操作超时 8、操作失败 9、请取回卡
                */
                while (true)
                {
                    Thread.Sleep(1000);
                    n = PICC_Reader_Request(ReaderHandle);
                    if (n == 0)
                    {
                        ICC_PosBeep(ReaderHandle, 2);



                        //PICC_Reader_Select(ReaderHandle, 0x41);
                        n = PICC_Reader_Authentication(ReaderHandle, 0, 1);

                        n = PICC_Reader_Read(ReaderHandle, 4, revdata);

                        string returnStr = "";
                        for (int i = 0; i < revdata.Length; i++)
                        {
                            returnStr += revdata[i].ToString("X2");
                        }
                        //formthis.msg.Text = "卡号：" + returnStr.Substring(0, 8);
                        formthis.OutXml.AppendText("卡号：" + returnStr.Substring(0, 8)+"\n");
                    }
                }
            }

            public void Te()
            {
                SqliteLibrary.SqliteHelper db = new SqliteLibrary.SqliteHelper("Data Source=" + System.Environment.CurrentDirectory.ToString() + @"\record.db");
                while (true)
                {               
                    try
                    {
                        int iSeed;
                        iSeed = DateTime.Now.Hour * 3600 * 1000 + DateTime.Now.Minute * 60 * 1000 + DateTime.Now.Second * 1000 + DateTime.Now.Millisecond;
                        Random ro = new Random(iSeed);
                        string rs = "";
                        for (int l = 0; l < 6; l++)
                        {
                            int iResult = ro.Next(1, 33);
                            rs += iResult.ToString("00");

                        }
                        int iRe = ro.Next(1, 16);
                        rs += iRe.ToString("00");
                        string insertstr = "insert into GetN(NumN) values('" + rs + "')";
                        db.ExecuteNonQuery(insertstr, System.Data.CommandType.Text, null);
                        formthis.OutXml.AppendText(DateTime.Now.Hour+":"+DateTime.Now.Minute+":"+DateTime.Now.Second+"--" + rs + "\n");
                    }                
                    catch (Exception ex)
                    {
                        Thread.Sleep(1000);
                        //throw new Exception(ex.Source+"#异常#"+ex.Message);
                        formthis.TrackNum.AppendText("异常" + ex.Message + "\n");
                    }
                }

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //treadcard = new SEEE();
            ////treadcard.Init();
            //threadReadCard = new Thread(new ThreadStart(treadcard.Te));
            //threadReadCard.IsBackground = true;
            //threadReadCard.Start();
            while (true)
            {
                string ip = "192.168.6.206";
                string strv = "0292|SC0000|N|||||161118153006|6217731101497069||||9F2608C04598F94EB3AE449F2701809F101307010103A0A802010A0100000000008B0DED3C9F37042C0791239F3602015C9505000004E8009A031611189C01319F02060000000000005F2A02015682027C009F1A0201569F03060000000000009F3303E0F0C8|D41100000071153006||||6269.17|  ";
                string[] temp=strv.Split('|');

                Ping pingSender = new Ping();
                PingReply reply = pingSender.Send(ip, 10);//第一个参数为ip地址，第二个参数为ping的时间
                if (reply.Status == IPStatus.Success)
                {
                    //ping的通
                    pingSender.Dispose();
                }
                else
                {
                    pingSender.Dispose();//ping不通
                }
            }

            if ((DateTime.Now.Hour * 60 + DateTime.Now.Minute) > 360 && (DateTime.Now.Hour * 60 + DateTime.Now.Minute) < 1020)
            {
                button5.Visible = false; 
            }


            string info = "031 340119197708304166  F      |00000000        郝三零    |";
            if (info.Length > 2)
            {
                string[] strs = info.Split(' ');
                string ss = strs[1].Trim();
                ss = strs[1].Trim();
                ss = strs[3].Trim() == "F" ? "男" : "女";
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            int n=-1;
            n=Tran.CUMS_Init(1);
            formthis.OutXml.AppendText("CUMS_Init：" + n + "\n");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string setAmt = "1";
            #region 设置参数

            string request = string.Empty;
            char[] tranmemo = new char[1024];
            for (int i = 0; i < 1024; i++)
            {
                tranmemo[i] = ' ';
            }
            string money = decimal.Parse(setAmt).ToString("f2").Replace(".", "").PadLeft(12, '0');  //金额左补0，共12位
            request = "00000000000092110000000000010033333320170310444444444444555555666666                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                777";             //校验值
            #endregion

            Tran.CUMS_SetReq(request.ToCharArray());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int n = -1;
            n=Tran.CUMS_EnterCard();
            formthis.OutXml.AppendText("CUMS_EnterCard：" + n + "\n");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Tran.CUMS_EjectCard();
            Tran.CUMS_CardClose();
        }

        private void button10_Click(object sender, EventArgs e)
        {
             byte[] state = new byte[4];
             Tran.CUMS_CheckCard(ref state);
             formthis.OutXml.AppendText("CUMS_CheckCard：" + state[0].ToString() + "\n");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string dir = "";
            string conut=LogProcess.Process_log(dir);
            this.OutXml.AppendText(conut.ToString());
        }

        private void ImgToByte_Click(object sender, EventArgs e)
        {
            string imapath = @"D:\logo.png";
            Image imag = Image.FromFile(imapath);
            byte[] imagbyte= ImageHelper.ImageToBytes(imag);

        }


    }

    public class ClsUMS
    {

        #region DLL
        //初始化
        [DllImport("umsapi.dll", EntryPoint = "UMS_Init", CallingConvention = CallingConvention.Winapi)]
        public static extern int UMS_Init(int type);

        //设置入参
        [DllImport("umsapi.dll", EntryPoint = "UMS_SetReq")]
        public static extern int UMS_SetReq(char[] request);

        //进卡
        [DllImport("umsapi.dll", EntryPoint = "UMS_EnterCard")]
        public static extern int UMS_EnterCard();

        //检测卡
        [DllImport("umsapi.dll", EntryPoint = "UMS_CheckCard")]
        public static extern int UMS_CheckCard(byte[] state);

        //读卡
        [DllImport("umsapi.dll", EntryPoint = "UMS_ReadCard")]
        public static extern int UMS_ReadCard(byte[] cardno);

        // 弹卡
        [DllImport("umsapi.dll", EntryPoint = "UMS_EjectCard")]
        public static extern int UMS_EjectCard();

        //关闭读卡器
        [DllImport("umsapi.dll", EntryPoint = "UMS_CardClose")]
        public static extern int UMS_CardClose();

        //启动加密
        [DllImport("umsapi.dll", EntryPoint = "UMS_StartPin")]
        public static extern int UMS_StartPin();

        //启动明文密码
        [DllImport("umsapi.dll")]
        public static extern int UMS_PinStartTextMode(int type);

        //获取明文键值
        [DllImport("umsapi.dll")]
        public static extern int UMS_PinGetOneText(byte[] keyCode);

        //关闭明文输入模式
        [DllImport("umsapi.dll")]
        public static extern int UMS_PinCloseTextMode();

        //输入密码
        [DllImport("umsapi.dll", EntryPoint = "UMS_GetOnePass")]
        public static extern int UMS_GetOnePass(byte[] keyCode);


        //输入完成
        [DllImport("umsapi.dll", EntryPoint = "UMS_GetPin")]
        public static extern int UMS_GetPin();

        //开始交易
        [DllImport("umsapi.dll")]
        public static extern int UMS_TransCard(char[] request, byte[] response);
        #endregion

        #region 参数
        int returnStr = -1;
        #endregion

        #region 私有方法
        //初始化
        public int CUMS_Init(int type)
        {
            returnStr = UMS_Init(type);
            return returnStr;
        }

        /// <summary>
        /// 设置入参
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public int CUMS_SetReq(char[] request)
        {
            returnStr = UMS_SetReq(request);
            return returnStr;
        }

        //进卡
        public int CUMS_EnterCard()
        {
            returnStr = UMS_EnterCard();
            return returnStr;
        }

        //检测卡
        public int CUMS_CheckCard(ref byte[] sta)
        {
            returnStr = UMS_CheckCard(sta);
            return returnStr;
        }

        //读卡
        public int CUMS_ReadCard(ref byte[] card)
        {
            returnStr = UMS_ReadCard(card);
            return returnStr;
        }

        //弹卡
        /// <summary>
        /// 弹卡
        /// </summary>
        /// <returns></returns>
        public int CUMS_EjectCard()
        {
            returnStr = UMS_EjectCard();
            return returnStr;
        }

        //关闭卡机
        /// <summary>
        /// 关闭卡机
        /// </summary>
        /// <returns></returns>
        public int CUMS_CardClose()
        {
            returnStr = UMS_CardClose();
            return returnStr;
        }

        //启动密码
        public int CUMS_StartPin()
        {
            returnStr = UMS_StartPin();
            return returnStr;
        }

        /// <summary>
        /// 启动明文密码
        /// </summary>
        /// <param name="type">1:银行卡账户应用 2:社保账户应用</param>
        /// <returns>0：成功</returns>
        public int CUMS_StartPin_M(int type)
        {
            returnStr = UMS_PinStartTextMode(type);
            return returnStr;
        }

        /// <summary>
        ///  获取明文串
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        public int CUMS_GetOnePass_M(ref byte[] keyCode)
        {
            returnStr = UMS_PinGetOneText(keyCode);
            return returnStr;
        }

        /// <summary>
        ///  关闭卡机
        /// </summary>
        /// <returns></returns>
        public int CUMS_ClosePin_M()
        {
            returnStr = UMS_PinCloseTextMode();
            return returnStr;
        }

        //输入密码
        public int CUMS_GetOnePass(ref byte[] keyCode)
        {
            returnStr = UMS_GetOnePass(keyCode);
            return returnStr;
        }

        //获取密码
        public int CUMS_GetPin()
        {
            returnStr = UMS_GetPin();
            return returnStr;
        }

        //开始交易
        public int CUMS_TransCard(char[] request, byte[] response)
        {
            UMS_TransCard(request, response);
            return returnStr;
        }
        #endregion
    }

    #region 013.时间同步
    [StructLayout(LayoutKind.Sequential)]
    public struct SystemTime
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMiliseconds;
    }
    public class Win32
    {
        [DllImport("Kernel32.dll")]
        public static extern bool SetSystemTime(ref SystemTime SysTime);
        [DllImport("Kernel32.dll")]
        public static extern void GetSystemTime(ref SystemTime SysTime);
    }
    #endregion
}
