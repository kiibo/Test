using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ch.Elca.Iiop;
using Ch.Elca.Iiop.Services;
using omg.org.CosNaming;
using hi;
using hi.modMZ;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Threading;

namespace Zebra
{
    public class SZYB
    {
        private readonly string strior;
        private readonly string yljgbm;
        private readonly string czybm;
        private short[] czy;
        private readonly bool isSaveExceptionLog;

        private const string SUCCESS = "SUC";
        private const string ERROR = "ERR";
        private const string EXCEPTION = "连接社保系统出现异常";
        public const string SELFBRLX = "9";
        public const string SUBSYSID_MZ = "mz";
        public const string SUBSYSID_MZGH = "mzgh";
        /// <summary>
        /// 自费项目编码
        /// </summary>
        public const string ZFXM_ZF = "01";

        /// <summary>
        /// 社保缴纳项目编码
        /// </summary>
        public const string ZFXM_YB = "02";

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="strior">医院ior串</param>
        /// <param name="yljgbm">医疗机构编号</param>
        /// <param name="czybm">操作员编号</param>
        /// <param name="czy">操作员名称</param>
        /// <param name="isSaveExceptionLog">是否保存异常日志</param>
        public SZYB(string strior, string yljgbm, string czybm, string czy, bool isSaveExceptionLog)
        {
            this.strior = strior;
            this.yljgbm = yljgbm;
            this.czybm = czybm;
            this.czy = TransStringToSA(czy);
            this.isSaveExceptionLog = isSaveExceptionLog;
        }

        /// <summary>
        /// string类型转为short[] 
        /// </summary>
        /// <param name="strArray"></param>
        /// <returns></returns>
        public static short[] TransStringToSA(string strArray)
        {
            short[] shortArray = new short[strArray.Length];
            char[] chars = strArray.ToCharArray();
            for (int i = 0; i < strArray.Length; i++)
            {
                shortArray[i] = transCharToShort(chars[i]);
            }
            return shortArray;
        }

        public static short transCharToShort(char c)
        {
            char[] t = new char[1];
            t[0] = c;
            string strTemp = new string(t);
            byte[] cc = System.Text.Encoding.Default.GetBytes(strTemp);
            if (cc.Length == 1)
            {
                return (short)(cc[0] < 0 ? cc[0] + 256 : cc[0]);
            }
            int v1 = cc[0] < 0 ? cc[0] + 256 : cc[0];
            int v2 = cc[1] < 0 ? cc[1] + 256 : cc[1];
            return (short)(v1 * 256 + v2);
        }

        //short[] 转为string类型 
        public static string TransSAToString(short[] sArray)
        {
            char[] charArray = new char[sArray.Length];
            for (int i = 0; i < sArray.Length; i++)
            {
                charArray[i] = TransShortToChar(sArray[i]);
            }
            return new String(charArray);
        }
        private static char TransShortToChar(short sTemp)
        {
            if (sTemp >= 0)
            {
                return (char)sTemp;
            }

            byte[] b = new byte[2];
            b[0] = (byte)(sTemp >> 8);
            b[1] = (byte)sTemp;
            string strTemp = System.Text.Encoding.Default.GetString(b);
            char[] charArrTemp = strTemp.ToCharArray();
            return charArrTemp[0];
        }

        #region 门诊处理方法
        /// <summary>
        /// 取个人基本信息
        /// </summary>
        /// <param name="ylzh">医疗证号（解析前）</param>
        /// <param name="mm">医疗证号密码</param>
        /// <param name="theGRXX">个人基本信息</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns></returns>
        public bool GetGRXBXX(string ylzh, string mm, out GRXX theGRXX, out string errMsg)
        {

            System.Collections.IDictionary dic = new System.Collections.Hashtable();
            //dic.Add(IiopClientChannel.ALLOW_REQUEST_MULTIPLEX_KEY,"9");
            //dic.Add(IiopClientChannel.CLIENT_CONNECTION_LIMIT_KEY,30000);
            //dic.Add(IiopClientChannel.CLIENT_RECEIVE_TIMEOUT_KEY, 30000);
            //dic.Add(IiopClientChannel.CLIENT_REQUEST_TIMEOUT_KEY, 30000);
            //dic.Add(IiopClientChannel.CLIENT_SEND_TIMEOUT_KEY, 30000);
            //dic.Add(IiopClientChannel.CLIENT_UNUSED_CONNECTION_KEEPALIVE_KEY, "3");
            //dic.Add(IiopClientChannel.MAX_NUMBER_OF_MULTIPLEXED_REQUESTS_KEY, "3");
            //dic.Add(IiopClientChannel.MAX_NUMBER_OF_RETRIES_KEY, "3");
            //dic.Add(IiopClientChannel.RETRY_DELAY_KEY,30000);

            dic.Add(IiopClientChannel.CLIENT_RECEIVE_TIMEOUT_KEY, 30000);//30s
            dic.Add(IiopClientChannel.CLIENT_REQUEST_TIMEOUT_KEY, 30000); //30s
            dic.Add(IiopClientChannel.CLIENT_SEND_TIMEOUT_KEY, 30000); //30s

            IiopClientChannel channel = new IiopClientChannel(dic);
            try
            {
                if (string.IsNullOrEmpty(mm)) mm = "";
                ChannelServices.RegisterChannel(channel, false);
                NamingContext nameService = (NamingContext)RemotingServices.Connect(typeof(NamingContext), strior);
                NameComponent[] name = new NameComponent[] { new NameComponent("MZYL", "Service") };
                intMZ mz = (intMZ)nameService.resolve(name);
                short[] res = mz.GetGRJBXX(yljgbm, ylzh, mm, czybm, czy, out theGRXX);
                ChannelServices.UnregisterChannel(channel);
                string resStr = TransSAToString(res);
                if (resStr.StartsWith(SUCCESS))
                {
                    ////DataLog.SaveLog("获取患者医保个人信息成功");
                    errMsg = string.Empty;
                    return true;
                }
                else
                {
                    if (isSaveExceptionLog)
                    {
                        ////DataLog.SaveLog(String.Format("GetGRXBXX:{0}\n", resStr));
                    }
                    errMsg = resStr.Substring(resStr.IndexOf(")") + 1); 
                    return false;
                }
            }
            catch (Exception ex)
            {
                ChannelServices.UnregisterChannel(channel);
                if (isSaveExceptionLog)
                {
                    ////DataLog.SaveLog(String.Format("GetGRXBXX:{0}\n", ex.Message));
                }
                errMsg = EXCEPTION;
                theGRXX = new GRXX();
                return false;
            }
        }


        /// <summary>
        /// 门诊挂号登记
        /// </summary>
        /// <param name="ylzh">医疗证号</param>
        /// <param name="mm">医疗证密码</param>
        /// <param name="theMZGHDJ">门诊挂号信息</param>
        /// <param name="theMZGHJS">门诊挂号结算</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns></returns>
        public bool SaveMZGH(string ylzh, string mm, MZGHDJ theMZGHDJ, out MZGHJSDetail[] theMZGHJS, out string errMsg)
        {

            IiopClientChannel channel = new IiopClientChannel();
            try
            {
                ChannelServices.RegisterChannel(channel, false);
                NamingContext nameService = (NamingContext)RemotingServices.Connect(typeof(NamingContext), strior);
                NameComponent[] name = new NameComponent[] { new NameComponent("MZYL", "Service") };
                intMZ mz = (intMZ)nameService.resolve(name);
                short[] res = mz.SaveMZGH(yljgbm, ylzh, mm, theMZGHDJ, out theMZGHJS, czybm, czy);
                ChannelServices.UnregisterChannel(channel);
                string resStr = TransSAToString(res);
                if (resStr.StartsWith(SUCCESS))
                {
                    errMsg = string.Empty;
                    return true;
                }
                else
                {
                    if (isSaveExceptionLog)
                    {
                        ////DataLog.SaveLog(String.Format("SaveMZGH:{0}\n", resStr));
                    }
                    errMsg = resStr.Substring(resStr.IndexOf(")") + 1);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ChannelServices.UnregisterChannel(channel);
                if (isSaveExceptionLog)
                {
                    ////DataLog.SaveLog(String.Format("SaveMZGH:{0}\n", ex.Message));
                }
                errMsg = EXCEPTION;
                theMZGHJS = null;
                return false;
            }
        }

        /// <summary>
        /// 查询已登记的门诊挂号信息
        /// </summary>
        /// <param name="mzlsh">门诊流水号</param>
        /// <param name="theMZGHXX">门诊挂号信息</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns></returns>
        public bool SearchMZGH(string mzlsh, out MZGHXX theMZGHXX, out string errMsg)
        {

            IiopClientChannel channel = new IiopClientChannel();
            try
            {
                ChannelServices.RegisterChannel(channel, false);
                NamingContext nameService = (NamingContext)RemotingServices.Connect(typeof(NamingContext), strior);
                NameComponent[] name = new NameComponent[] { new NameComponent("MZYL", "Service") };
                intMZ mz = (intMZ)nameService.resolve(name);
                short[] res = mz.SearchMZGH(yljgbm, mzlsh, czybm, czy, out theMZGHXX);
                ChannelServices.UnregisterChannel(channel);
                string resStr = TransSAToString(res);
                if (resStr.StartsWith(SUCCESS))
                {
                    errMsg = string.Empty;
                    return true;
                }
                else
                {
                    errMsg = resStr.Substring(resStr.IndexOf(")") + 1);
                    return false;
                }

            }
            catch (Exception ex)
            {
                ChannelServices.UnregisterChannel(channel);
                if (isSaveExceptionLog)
                {
                    ////DataLog.SaveLog(String.Format("SearchMZGH:{0}\n", ex.Message));
                }
                errMsg = EXCEPTION;
                theMZGHXX = new MZGHXX();
                return false;
            }
        }

        /// <summary>
        /// 保存门诊登记信息到社保局数据库
        /// </summary>
        /// <param name="theMZDJ">门诊登记信息</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool SaveMZDJ(MZDJ theMZDJ, out string errMsg)
        {
            IiopClientChannel channel = new IiopClientChannel();
            try
            {
                ChannelServices.RegisterChannel(channel, false);
                NamingContext nameService = (NamingContext)RemotingServices.Connect(typeof(NamingContext), strior);
                NameComponent[] name = new NameComponent[] { new NameComponent("MZYL", "Service") };
                intMZ mz = (intMZ)nameService.resolve(name);
                short[] res = mz.SaveMZDJ(yljgbm, theMZDJ, czybm, czy);
                ChannelServices.UnregisterChannel(channel);
                string resStr = TransSAToString(res);
                if (resStr.StartsWith(SUCCESS))
                {
                    errMsg = string.Empty;
                    return true;
                }
                else
                {
                    if (isSaveExceptionLog)
                    {
                        ////DataLog.SaveLog(String.Format("SaveMZDJ:{0}\n", resStr));
                    }
                    errMsg = resStr.Substring(resStr.IndexOf(")") + 1);
                    return false;
                }

            }
            catch (Exception ex)
            {
                ChannelServices.UnregisterChannel(channel);
                if (isSaveExceptionLog)
                {
                    ////DataLog.SaveLog(String.Format("SaveMZDJ:{0}\n", ex.Message));
                }
                errMsg = EXCEPTION;
                return false;
            }
        }

        /// <summary>
        /// 保存门诊处方费用明细，提交时按照单据逐单进行提交，同时返回社保结算结果信息
        /// </summary>
        /// <param name="mzlsh">门诊流水号</param>
        /// <param name="djh">单据号</param>
        /// <param name="theMZFY">费用列表</param>
        /// <param name="theMZJSJG">结算信息</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool SaveMZFY(string mzlsh, string djh, MZFYDetail[] theMZFY, out MZJSJG theMZJSJG, out string errMsg)
        {
            IiopClientChannel channel = new IiopClientChannel();
            try
            {
                ChannelServices.RegisterChannel(channel, false);
                NamingContext nameService = (NamingContext)RemotingServices.Connect(typeof(NamingContext), strior);
                NameComponent[] name = new NameComponent[] { new NameComponent("MZYL", "Service") };
                intMZ mz = (intMZ)nameService.resolve(name);
                short[] res = mz.SaveMZFY(yljgbm, mzlsh, djh, theMZFY, czybm, czy, out theMZJSJG);
                ChannelServices.UnregisterChannel(channel);
                string resStr = TransSAToString(res);
                if (resStr.StartsWith(SUCCESS))
                {
                    errMsg = string.Empty;
                    return true;
                }
                else
                {
                    if (isSaveExceptionLog)
                    {
                        ////DataLog.SaveLog(String.Format("SaveMZDJ:{0}\n", resStr));
                    }
                    errMsg = resStr.Substring(resStr.IndexOf(")") + 1);
                    return false;
                }

            }
            catch (Exception ex)
            {
                ChannelServices.UnregisterChannel(channel);
                if (isSaveExceptionLog)
                {
                    //DataLog.SaveLog(String.Format("SaveMZFY:{0}\n", ex.Message));
                }
                theMZJSJG = new MZJSJG();
                errMsg = EXCEPTION;
                return false;
            }
        }

        /// <summary>
        /// 门诊退费
        /// </summary>
        /// <param name="mzlsh">门诊流水号</param>
        /// <param name="djh">单据号</param>
        /// <param name="theMZFY">费用列表（数量，金额为负）</param>
        /// <param name="djh2">新单据号</param>
        /// <param name="theMZJSJG">结算结果（金额为负）</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool MZTF(string mzlsh, string djh, MZFYDetail[] theMZFY, string djh2, out MZJSJG theMZJSJG, out string errMsg)
        {
            IiopClientChannel channel = new IiopClientChannel();
            try
            {
                ChannelServices.RegisterChannel(channel, false);
                NamingContext nameService = (NamingContext)RemotingServices.Connect(typeof(NamingContext), strior);
                NameComponent[] name = new NameComponent[] { new NameComponent("MZYL", "Service") };
                intMZ mz = (intMZ)nameService.resolve(name);
                short[] res = mz.MZTF(yljgbm, mzlsh, djh, theMZFY, djh2, czybm, czy, out theMZJSJG);
                ChannelServices.UnregisterChannel(channel);
                string resStr = TransSAToString(res);
                if (resStr.StartsWith(SUCCESS))
                {
                    errMsg = string.Empty;
                    return true;
                }
                else
                {
                    if (isSaveExceptionLog)
                    {
                        //DataLog.SaveLog(String.Format("MZTF:{0}\n", resStr));
                    }
                    errMsg = resStr.Substring(resStr.IndexOf(")") + 1);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ChannelServices.UnregisterChannel(channel);
                if (isSaveExceptionLog)
                {
                    //DataLog.SaveLog(String.Format("MZTF:{0}\n", ex.Message));
                }
                theMZJSJG = new MZJSJG();
                errMsg = EXCEPTION;
                return false;
            }
        }


        /// <summary>
        /// 实现门诊支付确认，通过参数输入得实收金、找赎金，社保系统内部进行真正得社保支付
        /// </summary>
        /// <param name="mzlsh">门诊流水号</param>
        /// <param name="djh">单据号</param>
        /// <param name="ssj">实收金</param>
        /// <param name="zsj">找赎金</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool MZZFQR(string mzlsh, string djh, double ssj, double zsj, out string errMsg)
        {
            IiopClientChannel channel = new IiopClientChannel();
            try
            {
                ChannelServices.RegisterChannel(channel, false);
                NamingContext nameService = (NamingContext)RemotingServices.Connect(typeof(NamingContext), strior);
                NameComponent[] name = new NameComponent[] { new NameComponent("MZYL", "Service") };
                intMZ mz = (intMZ)nameService.resolve(name);
                short[] res = mz.MZZFQR(yljgbm, mzlsh, djh, ssj, zsj, czybm, czy);
                ChannelServices.UnregisterChannel(channel);
                string resStr = TransSAToString(res);
                if (resStr.StartsWith(SUCCESS))
                {
                    errMsg = string.Empty;
                    return true;
                }
                else
                {
                    if (isSaveExceptionLog)
                    {
                        //DataLog.SaveLog(String.Format("MZZFQR:{0}\n", resStr));
                    }
                    errMsg = resStr.Substring(resStr.IndexOf(")") + 1);
                    return false;
                }

            }
            catch (Exception ex)
            {
                ChannelServices.UnregisterChannel(channel);
                if (isSaveExceptionLog)
                {
                    //DataLog.SaveLog(String.Format("MZZFQR:{0}\n", ex.Message));
                }
                errMsg = EXCEPTION;
                return false;
            }
        }

        /// <summary>
        /// 取门诊单据费用明细(退费前用)
        /// </summary>
        /// <param name="mzlsh">门诊流水号</param>
        /// <param name="djh"></param>
        /// <param name="theMZFY"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool GetMZFYMX(string mzlsh, string djh, out MZFYDetail[] theMZFY, out string errMsg)
        {
            IiopClientChannel channel = new IiopClientChannel();
            try
            {
                NamingContext nameService = (NamingContext)RemotingServices.Connect(typeof(NamingContext), strior);
                NameComponent[] name = new NameComponent[] { new NameComponent("MZYL", "Service") };
                intMZ mz = (intMZ)nameService.resolve(name);
                short[] res = mz.GetMZFYMX(yljgbm, mzlsh, djh, czybm, czy, out theMZFY);
                ChannelServices.UnregisterChannel(channel);
                string resStr = TransSAToString(res);
                if (resStr.StartsWith(SUCCESS))
                {
                    errMsg = string.Empty;
                    return true;
                }
                else
                {
                    errMsg = resStr.Substring(resStr.IndexOf(")") + 1);
                    return false;
                }

            }
            catch (Exception ex)
            {
                ChannelServices.UnregisterChannel(channel);
                if (isSaveExceptionLog)
                {
                    //DataLog.SaveLog(String.Format("GetMZFYMX:{0}\n", ex.Message));
                }
                errMsg = EXCEPTION;
                theMZFY = null;
                return false;
            }
        }


        /// <summary>
        /// 取门诊结算结果(事后查询用)
        /// </summary>
        /// <param name="mzlsh">门诊流水号</param>
        /// <param name="djh">单据号</param>
        /// <param name="theMZJSJG">结算信息</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool GetMZJSJG(string mzlsh, string djh, out MZJSJG theMZJSJG, out string errMsg)
        {

            IiopClientChannel channel = new IiopClientChannel();
            try
            {
                ChannelServices.RegisterChannel(channel, false);
                NamingContext nameService = (NamingContext)RemotingServices.Connect(typeof(NamingContext), strior);
                NameComponent[] name = new NameComponent[] { new NameComponent("MZYL", "Service") };
                intMZ mz = (intMZ)nameService.resolve(name);
                short[] res = mz.GetMZJSJG(yljgbm, mzlsh, djh, czybm, czy, out theMZJSJG);
                ChannelServices.UnregisterChannel(channel);
                string resStr = TransSAToString(res);
                if (resStr.StartsWith(SUCCESS))
                {
                    errMsg = string.Empty;
                    return true;
                }
                else
                {
                    errMsg = resStr.Substring(resStr.IndexOf(")") + 1);
                    return false;
                }

            }
            catch (Exception ex)
            {
                ChannelServices.UnregisterChannel(channel);
                if (isSaveExceptionLog)
                {
                    //DataLog.SaveLog(String.Format("GetMZJSJG:{0}\n", ex.Message));
                }
                errMsg = EXCEPTION;
                theMZJSJG = new MZJSJG();
                return false;
            }
        }

        /// <summary>
        /// 取个人社康绑定信息和社区门诊报销限额
        /// </summary>
        /// <param name="ylzh">医疗证号（解析前）</param>
        /// <param name="mm">医疗证号密码</param>
        /// <param name="theGRXX">个人基本信息</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns></returns>
        public bool GetBDGRJBXX(string ylzh, string mm, out BDGRXX theBDGRXX, out string errMsg)
        {
            //System.Collections.IDictionary dic = new System.Collections.Hashtable();
            //dic.Add(IiopClientChannel.CLIENT_RECEIVE_TIMEOUT_KEY, 30000);//30s
            //dic.Add(IiopClientChannel.CLIENT_REQUEST_TIMEOUT_KEY, 30000); //30s
            //dic.Add(IiopClientChannel.CLIENT_SEND_TIMEOUT_KEY, 30000); //30s

            IiopClientChannel channel = new IiopClientChannel();
            try
            {
                if (string.IsNullOrEmpty(mm)) mm = "";
                ChannelServices.RegisterChannel(channel, false);
                NamingContext nameService = (NamingContext)RemotingServices.Connect(typeof(NamingContext), strior);
                NameComponent[] name = new NameComponent[] { new NameComponent("MZYL", "Service") };
                intMZ mz = (intMZ)nameService.resolve(name);
                // short[] res = mz.GetGRJBXX(yljgbm, ylzh, mm, czybm, czy, out theGRXX);
                short[] res = mz.GetBDGRJBXX(yljgbm, ylzh, mm, czybm, czy, out theBDGRXX);
                ChannelServices.UnregisterChannel(channel);
                string resStr = TransSAToString(res);
                if (resStr.StartsWith(SUCCESS))
                {
                    //DataLog.SaveLog("获取患者绑定本社康成功");
                    errMsg = string.Empty;
                    return true;

                }
                else
                {
                    if (isSaveExceptionLog)
                    {
                        //DataLog.SaveLog(String.Format("GetBDGRJBXX:{0}\n", resStr));
                    }
                    errMsg = resStr.Substring(resStr.IndexOf(")") + 1);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ChannelServices.UnregisterChannel(channel);
                if (isSaveExceptionLog)
                {
                    //DataLog.SaveLog(String.Format("GetGRXBXX:{0}\n", ex.Message));
                }
                errMsg = EXCEPTION;
                theBDGRXX = new BDGRXX();
                return false;
            }
        }

        #endregion

        /// <summary>
        /// 通过社保卡的参保类型来获取病人类型
        /// </summary>
        /// <param name="cblx"></param>
        /// <returns></returns>
        public static void GetMZGH_BRLX(ref string brlx)
        {
            if (Convert.ToInt16(brlx) == 1 || Convert.ToInt16(brlx) == 2 || Convert.ToInt16(brlx) == 3)
                return;
            else if (Convert.ToInt16(brlx) == 4)
                brlx = "6";
            else if (Convert.ToInt16(brlx) == 5)
                brlx = "7";
            else if (Convert.ToInt16(brlx) == 6)
                brlx = "4";
            else
                brlx = "9";
        }
    }
}
