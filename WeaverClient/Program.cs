using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace WeaverClient
{

    class Program
    {
        internal static bool unity_ready = false;
        private static Thread client_program = new Thread(Program.Start);

        public static void Main(string[] args)
        {
            Program.Start();
        }

        private static void Start()
        {
            // Wait for unity to signal that it is ready.
            while (!unity_ready)
            {
                Thread.Sleep(100);
            }

            // Sleep anyway just in case...
            Thread.Sleep(500);

            // Then start the client
            var hostName = Dns.GetHostName();

            // This is the IP address of the local machine
            int port = 9001;
            string localip = "127.0.0.1";
            //IPHostEntry localhost = Dns.GetHostEntry(hostName);
            //IPAddress localIpAddress = localhost.AddressList[0];
            //IPEndPoint ipEndpoint = new IPEndPoint(localIpAddress, port);

            Client.Username = "TestUser1";
            Client.Start(localip, port);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnBeforeSceneLoadRuntimeMethod()
        {
            client_program.Start();
        }
    }
}