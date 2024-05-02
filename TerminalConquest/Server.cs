using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WebweaverServer
{
    public static class Server
    {

        private static List<User> users = new List<User>();

        // Starts server up, begins listening for activity
        public static void Start(IPEndPoint ipEndpoint)
        {
            TcpListener server = null;

            try
            {
                server = new TcpListener(ipEndpoint);

                // Start listening for client requests.
                server.Start();


                // Enter the listening loop.
                while (true)
                {
                    Console.WriteLine("SERVER: Waiting for a connection... ");
                    TcpClient client = server.AcceptTcpClient();

                    // On connection, generate new user and pass off handling to other thread
                    AddUser(client);
                    Console.WriteLine("SERVER: User connected!");
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SERVER: SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
            }

            Console.WriteLine("\nSERVER: Completed tasks, exiting server. Hit enter to continue...");
            Console.Read();
        }

        private static void AddUser(TcpClient client)
        {
            var mailbox = new BlockingCollection<string>(); 
            var user = new User(client, mailbox);

            users.Add(user);
            Thread user_thread = new Thread(new ThreadStart(user.Start));
            user_thread.Start();

            GameDriver.AddPlayer(user, mailbox);
        }
    }
}
