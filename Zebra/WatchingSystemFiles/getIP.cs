using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace WatchingSystemFiles
{
    public class getIP
    {
        public string GetLocalIp()  
        {  
            string hostname = Dns.GetHostName();//得到本机名   
            IPHostEntry localhost = Dns.GetHostByName(hostname);//方法已过期，只得到IPv4的地址   
            //PHostEntry localhost = Dns.GetHostEntry(hostname);  
            IPAddress localaddr = localhost.AddressList[0];  
            return localaddr.ToString();
        }
    }
}
