using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace Zebra
{
    public class LogProcess
    {
        public static SqliteLibrary.SqliteHelper db = new SqliteLibrary.SqliteHelper("Data Source=" + System.Environment.CurrentDirectory.ToString() + @"\record.db");
        public static MysqlHelper Mysqldb = new MysqlHelper();
        public static int WebSerCount1 = 0, WebSerCount2 = 0, WebSerCount3 = 0, WebSerCount4 = 0, WebSerCount5 = 0, WebSerCount6 = 0,BankMoney=0;
        public static string  Process_log(string dir)
        {
            string direct = @"F:\jinh-SVN同步最新版本\安徽儿童医院\Documents\新文件夹\";
            DirectoryInfo di = new DirectoryInfo(direct);
            for (int i = 0; i < di.GetDirectories().Count(); i++)
            {
                string userId = di.GetDirectories()[i].ToString();
                string userDirect = direct + userId ;
                DirectoryInfo Userdi = new DirectoryInfo(userDirect);
                for (int ii = 0; ii < Userdi.GetFiles().Count(); ii++)
                {
                    object filename = Userdi + @"\" + Userdi.GetFiles()[ii].ToString();
                    string dateTTT = Userdi.GetFiles()[ii].ToString().Substring(0);
                    if (!dateTTT.Contains(".log"))
                        continue;
                    double dateMoney = 0,Count=0,successCount=0,failCount=0;
                    FileStream fs = null;
                    bool insertDb = false;
                    fs = new FileStream(Path.Combine(filename.ToString()), FileMode.Open);
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        while (!reader.EndOfStream)
                        {
                            string sline = reader.ReadLine();
                            if (sline.Length < 23)
                                continue;
                            string tim = sline.Substring(1, 19);
                            string contex = sline.Substring(22);
                            //*/移除开机时网络不通的情况
                            //if (insertDb == false && sline.Contains("交易成功"))
                            //{
                            //    insertDb = true;
                            //}
                            //if (insertDb == false)
                            //    continue;
                            //*/移除开机时网络不通的情况
                            if (sline.Contains("传入参数"))
                            {
                                Count++;
                            }
                            if (sline.Contains("------返回参数"))
                            {
                                successCount++;
                            }
                            if (sline.Contains("异常--返回参数"))
                            {
                                failCount++;
                            }

                            if (sline.Contains("压钞："))
                            {
                                string sahzhf=sline;
                                dateMoney += double.Parse(sline.Substring(25));
                            }

                            if (sline.Contains("Unable to connect to the remote server")&&false)//连接不到服务 
                            {
                                string insertstr = "insert into LogProcess(user,time,trade,context) values('" + userId + "','" + tim + "','异常','" + contex + "')";
                                //db.ExecuteNonQuery(insertstr, System.Data.CommandType.Text, null);
                                //Mysqldb.Runsql(insertstr);
                            }
                            if(sline.Contains("就诊卡充值写入输出参数"))
                            {

                            }
                            if (sline.Contains("就诊卡充值写入入参数"))
                            {
                                string nextLine=reader.ReadLine();
                                string money = contex.Substring(10);
                                Process_XML(money, tim);
                                contex += nextLine;
                                if (nextLine.Contains("交易成功"))
                                {
                                    string insertstr = "insert into LogProcess(user,time,trade,context) values('" + userId + "','" + tim + "','充值成功','" + contex + "')";
                                    //db.ExecuteNonQuery(insertstr, System.Data.CommandType.Text, null);
                                    //Mysqldb.Runsql(insertstr);
                                }
                                else
                                {
                                    string insertstr = "insert into LogProcess(user,time,trade,context) values('" + userId + "','" + tim + "','充值异常','" + contex + "')";
                                    //db.ExecuteNonQuery(insertstr, System.Data.CommandType.Text, null);
                                    //Mysqldb.Runsql(insertstr);
                                }

                            }
                            //银联交易的
                            if (sline.Contains("InStr:")&&false)
                            {
                                string nextLine = reader.ReadLine();
                                //string money = contex.Substring(10);
                               // Process_XML(money, tim);
                                contex += nextLine;
                                if (nextLine.Contains("outstr:"))
                                {
                                    //卡号01769339 InStr:01000000168000 
                                    int pos=contex.IndexOf("InStr:");
                                    string moneyy = contex.Substring(pos + 8, 12);
                                    BankMoney += (int.Parse(moneyy) / 100);
                                    string insertstr = "insert into LogProcess(user,time,trade,context) values('" + userId + "','" + tim + "','银联交互成功','" + contex + "')";
                                    //db.ExecuteNonQuery(insertstr, System.Data.CommandType.Text, null);
                                    //Mysqldb.Runsql(insertstr);
                                }
                                else
                                {
                                    string insertstr = "insert into LogProcess(user,time,trade,context) values('" + userId + "','" + tim + "','银联交互异常','" + contex + "')";
                                    //db.ExecuteNonQuery(insertstr, System.Data.CommandType.Text, null);
                                    //Mysqldb.Runsql(insertstr);
                                }

                            }
                            if (sline.Contains("outstr:")&&false)
                            {
                                string insertstr = "insert into LogProcess(user,time,trade,context) values('" + userId + "','" + tim + "','银联异常','" + sline + "')";
                                //db.ExecuteNonQuery(insertstr, System.Data.CommandType.Text, null);
                                //int pos = sline.IndexOf("InStr:");
                                //string moneyy = sline.Substring(pos + 8, 12);
                                //BankMoney += (int.Parse(moneyy) / 100);
                            }

                            if (sline.Contains("获取病人信息GetPatInfo输入参数") )
                            {
                                WebSerCount1++;
                            }
                            if ( sline.Contains("挂号坐诊科室查询输入参数") )
                            {
                                WebSerCount2++;
                            }
                            if (  sline.Contains("挂号排班查询输入参数") )
                            {
                                WebSerCount3++;
                            }
                            if (  sline.Contains("就诊卡充值写入入参数") )
                            {
                                WebSerCount4++;
                            }
                            if (  sline.Contains("挂号请求写入/撤销处理输入参数") )
                            {
                                WebSerCount5++;
                            }
                            if (sline.Contains("建卡建档（自助发卡）输入参数"))
                            {
                                WebSerCount6++;
                            }
                        }

                    }

                    //
                    //string zzzrtstr = "insert into MoneyLog(UserID,Dat,Mony) values('" + userId + "','" + dateTTT + "','" + dateMoney.ToString() + "')";
                    //db.ExecuteNonQuery(zzzrtstr, System.Data.CommandType.Text, null);

                }
            }
            return "获取病人信息:" + WebSerCount1 + ",挂号坐诊科室查询:" + WebSerCount2 + ",挂号排班查询:" + WebSerCount3 + ",就诊卡充值:" + WebSerCount4 + ",挂号请求写入:" + WebSerCount5 + ",建卡建档:"+WebSerCount6+",银联成功："+BankMoney;
        }

        public static void Process_XML(string xml,string tim)
        {
            try
            {
                string ResultCode = "", card_no="", PATIENT_NAME="", OUT_TRADE_NO="", AMOUNT="", USER_ID="", PAYMENT_TYPE="";
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.LoadXml(xml);
                XmlNodeList topM = doc.DocumentElement.ChildNodes;
                foreach (XmlElement ell in topM)
                {
                    if (ell.Name == "CARD_NO")
                    {
                        card_no = ell.InnerText;
                        continue;
                    }
                    if (ell.Name == "PATIENT_NAME")
                    {
                        PATIENT_NAME = ell.InnerText;
                        continue;
                    }
                    if (ell.Name == "OUT_TRADE_NO")
                    {
                        OUT_TRADE_NO = ell.InnerText;
                        continue;
                    }
                    if (ell.Name == "AMOUNT")
                    {
                        AMOUNT = ell.InnerText;
                        continue;
                    }
                    if (ell.Name == "USER_ID")
                    {
                        USER_ID = ell.InnerText;
                        continue;
                    }
                    if (ell.Name == "PAYMENT_TYPE")
                    {
                        PAYMENT_TYPE = ell.InnerText;
                        continue;
                    }
                }
                //                                                                                                               card_no, PATIENT_NAME, OUT_TRADE_NO, AMOUNT, USER_ID, PAYMENT_TYPE
                string insertstr = "insert into TradeLog(card_no,PATIENT_NAME,OUT_TRADE_NO,AMOUNT,USER_ID,PAYMENT_TYPE,TradeTime) values('" + card_no + "','" + PATIENT_NAME + "','" + OUT_TRADE_NO + "'," + AMOUNT + ",'"+USER_ID+"','"+PAYMENT_TYPE+"','"+tim+"')";
                //db.ExecuteNonQuery(insertstr, System.Data.CommandType.Text, null);
                Mysqldb.Runsql(insertstr);


            }
            catch (Exception ex) { }

        }
    }
}
