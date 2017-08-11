using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
namespace Zebra
{
    public class ZebraDLL
    {
        [DllImport("ZBRPrinter.dll", EntryPoint = "ZBRPRNReadMagByTrk", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern int ZBRPRNReadMagByTrk(IntPtr hPrinter, int PrinterType, int trkNum, byte[] trkBuf, out int respSize, out int err);

        [DllImport("ZBRPrinter.dll", EntryPoint = "ZBRPRNGetSDKVer", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall,
            SetLastError = true)]
        public static extern void ZBRPRNGetSDKVer(out int major, out int minor, out int engLevel);

        // Handle

        [DllImport("ZBRPrinter.dll", EntryPoint = "ZBRGetHandle", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall,
                   SetLastError = true)]
        public static extern int ZBRGetHandle(out IntPtr _handle, byte[] drvName, out int prn_type, out int err);

        //没用
        [DllImport("ZBRPrinter.dll", EntryPoint = "ZBRPRNGetPrinterStatus",CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int ZBRPRNGetPrinterStatus(out int statusCode);
        //没有
        [DllImport("ZBRPrinter.dll", EntryPoint = "ZBRPRNGetMsgSuppressionState",CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int ZBRPRNGetMsgSuppressionState(IntPtr hPrinter,int PrinterType,out int state,out int err);
        //...
        [DllImport("ZBRPrinter.dll", EntryPoint = "ZBRPRNClrErrStatusLn",CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int ZBRPRNClrErrStatusLn(IntPtr hPrinter,int printerType,out int err);

        [DllImport("ZBRPrinter.dll", EntryPoint = "ZBRPRNGetOpParam",CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int ZBRPRNGetOpParam(IntPtr hPrinter,int printerType,int paramIDx,byte[] opParam,out int respSize,out int err);

        /// <summary>
        /// 强行退卡carType:1=Contact,2=Contactless,3=UHF   moveType=1
        /// </summary>
        /// <param name="hPrinter"></param>
        /// <param name="printerType"></param>
        /// <param name="cardType">1=Contact,2=Contactless,3=UHF</param>
        /// <param name="moveType">0=move card to print ready position 1=eject card</param>
        /// <param name="err"></param>
        /// <returns></returns>
        [DllImport("ZBRPrinter.dll", EntryPoint = "ZBRPRNEndSmartCard", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int ZBRPRNEndSmartCard(IntPtr hPrinter, int printerType, int cardType, int moveType, out int err);
      

    }
}
