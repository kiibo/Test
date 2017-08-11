using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebData
{
    public class Class1
    {
        public string[] getWebData()
        {
            WebData.cn.com.webxml.www.WeatherWebService ws = new cn.com.webxml.www.WeatherWebService();
            return ws.getWeatherbyCityName("上海");
        }

        public string getServiceData(string CityName, string CountryName)
        {
            string ss=string.Empty;
            ServiceReference.GlobalWeatherSoapClient ws = new ServiceReference.GlobalWeatherSoapClient();
            ss= ws.GetWeather(CityName, CountryName);

            return ss;
        }
    }
}
