// See https://aka.ms/new-console-template for more information

using System.Collections;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Intrinsics.X86;
using System.Text;
using WebweaverServer;

namespace WebweaverServer
{
    class Program
    {
        
        private static Thread driver_thread = new Thread(new ThreadStart(GameDriver.Start));

        public static void Main(string[] args)
        {
            driver_thread.Start();


            var hostName = Dns.GetHostName();

            // This is the IP address of the local machine
            int port = 9001;
            //IPHostEntry localhost = Dns.GetHostEntry(hostName);
            //IPAddress localIpAddress = localhost.AddressList[0];
            IPEndPoint ipEndpoint = new IPEndPoint(System.Net.IPAddress.Any, port);

            // Could also get the IP of a webaddress
            //IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync("host.contoso.com");
            //IPAddress ipAddress = ipHostInfo.AddressList[0];
            

            Server.Start(ipEndpoint);
        }

    }
}