using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using WatchingSystemFiles;
using System.Xml;
using System.Threading;

namespace Zebra
{
    class SSSECls
    {
        IntPtr ReaderHandle = IntPtr.Zero;
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
       
        public  int init()
        {
            byte[] com = ASCIIEncoding.ASCII.GetBytes("USB1");
            ReaderHandle = ICC_Reader_Open(com);
            if(ReaderHandle==IntPtr.Zero)
            {
                return -1;

            }
            else
            {
                return 0;
            }
        }
        public int Read()
        {
            byte[] revdata = new byte[17];

            ICC_DispSound(ReaderHandle, 1, 1); //声音提示信息 type(1~10)
            /*
                1、请插卡 2、请刷卡 3、读卡错误 4、请输入密码  5、密码错误  6、操作成功 7、操作超时 8、操作失败 9、请取回卡
            */
            while (true)
            {
                Thread.Sleep(1000);
                int n = 0;
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

                }
            }
        }
    }
}
