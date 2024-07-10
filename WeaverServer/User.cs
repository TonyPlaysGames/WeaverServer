using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Sockets;
using System.Collections.Concurrent;

namespace WebweaverServer
{
    public class User
    {
        private NetworkStream stream;
        private TcpClient client;

        private int idle_time = 0;
        public string username = "NONAME";
        private BlockingCollection<string> mailbox; 
        // Should be used for reception of events to send back to the client

        public User(TcpClient client, BlockingCollection<string> mailbox) 
        {
            this.client = client;
            this.stream = client.GetStream();
            this.mailbox = mailbox; 

            // Get username from connection message
            this.username = GetMessage();
            SendMessage("(user) '" + username + "' recieved and added to system.");
        }


        private string GetMessage()
        {
            Byte[] bytes = new Byte[256];
            string message = null;
            int i = 0;

            // receive message sent by the client.
            i = stream.Read(bytes, 0, bytes.Length);
            message = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
            Console.WriteLine("(user) '" + username + "' Received message: {0}", message);

            return message;
        }

        public void SendMessage(string message)
        {
            SendMessage(message, username, "whisper");
        }

        // Overload for specific command
        public void SendMessage(string message, string cmd)
        {
            SendMessage(message, username, cmd);
        }

        // Overload for specific name and command
        public void SendMessage(string message, string name, string cmd)
        {
            string formattedMessage = $";{name},{cmd};{message};END_MESSAGE;";

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(formattedMessage);
            stream.Write(msg, 0, msg.Length);
            Console.WriteLine("(user) '" + username + "' Sent message: '{0}'", formattedMessage);
        }


        public void Start() 
        {
            // Once user starts, update till user disconnects
            while (client.Connected)
            {
                try
                {

                    if (stream.DataAvailable)
                    {
                        string message = GetMessage();

                        // TODO PARSE INPUT AND STUFF...
                        string command = CommandParser.Parse(message, this);

                        //add as command to the GameDriver
                        //GameDriver.AddCommand(command);
                        idle_time = 0;
                    }

                    // ... And send back any messages to the user
                    while (mailbox.TryTake(out string response))
                    {
                        SendMessage(response);
                        idle_time = 0;
                    }

                    // Method to ping the client and see if they are still there 
                    if (idle_time >= 200)
                    {
                        SendMessage("Pinged by server", "SERVER", "ping");
                        idle_time = 0;
                    }
                }
                catch (System.IO.IOException)
                {
                    Console.WriteLine("WARNING: IOException was thrown when reading from client '" + username + "'.");
                }
                
                idle_time += 1;
                Thread.Sleep(50); // 100 ms artifical delay to prevent thread from eating up CPU
            }

            // After client disconnects....
            client.Close();
            stream.Close();
            Console.WriteLine("SERVER: '" + username + "' disconnected from host");
        }


    }
}