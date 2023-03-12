using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PPLV
{
    class SystemInfo
    {
        public static string getDate()
        {
            DateTime date = new DateTime();
            date = DateTime.Now;
            return "Date: " + date.ToShortDateString();
        }

        public static string getTime()
        {
            DateTime date = new DateTime();
            date = DateTime.Now;
            return "Time: " + date.ToLongTimeString();
        }

        public static int s;
        public static int m;
        public static int h;

        public static string getStopwatch()
        {
            s++;
            if (s == 60)
            {
                m++;
                s = 0;
                if (m == 60)
                {
                    h++;
                    m = 0;
                }
            }
            return "Stopwatch: " + string.Format("{0:00}:{1:00}:{2:00}", h, m, h);
        }

        public static string getOS()
        {
            OperatingSystem os = Environment.OSVersion;
            return "ОС: " + Convert.ToString(os);
        }
        public static string getLANIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return "LAN IP: " + ip.ToString();
                }
            }
            return "No adapters";
        }
        public static string getWANIP()
        {
            WebClient client = new WebClient();
            if (NetworkInterface.GetIsNetworkAvailable() &&
                new Ping().Send(new IPAddress(new byte[] { 8, 8, 8, 8 }), 2000).Status == IPStatus.Success)
            {
                var ip = client.DownloadString("https://ipinfo.io/ip");

                return "WAN IP: " + ip.ToString().Replace("\n", "");
            }
            return "No internet";
        }
    }
}
