
using System;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;

namespace WeaverClient
{
    internal class Message
    {
        public string User { get; private set; }
        public string Cmd { get; private set; }
        public string Content { get; private set; }

        public Message(string user, string cmd, string content)
        {
            User = user;
            Cmd = cmd;
            Content = content;
        }
    }

    public static class Client 
    {
        public static String Username = "UNINITIALIZED";
        private static NetworkStream Stream;
        private static StringBuilder messageBuffer = new StringBuilder();

        public static void Start(string localip, int port)
        {
            try
            {
                TcpClient client = new TcpClient(localip, port);
                Stream = client.GetStream();
                SendMessage(Username, false); // Send Username to server

                while (true)
                {
                    Thread.Sleep(100); // Dont eat up all cpu


                    // Process all received messages
                    ProcessIncomingMessages();

                    try
                    {
                        string userInput = Terminal.GetInput(10); // -0.01 seconds timeout
                        if (string.IsNullOrEmpty(userInput)) continue;
                        SendMessage(userInput, true);
                    }
                    catch (TimeoutException)
                    {
                        continue;
                    }
                }
                Stream.Close();
                client.Close();
            }
            catch (Exception ex) when (ex is SocketException || ex is ArgumentNullException)
            {
                Terminal.Println($"Error: {ex.Message}");
            }
        }

        private static void SendMessage(string message, bool format)
        {
            if (format)
                message = $";{Username},whisper;{message};END_MESSAGE;";

            Byte[] data = Encoding.ASCII.GetBytes(message);
            Stream.Write(data, 0, data.Length);
        }

        private static void ProcessIncomingMessages()
        {
            Byte[] data = new Byte[256];
            while (Stream.DataAvailable)
            {
                int bytes = Stream.Read(data, 0, data.Length);
                messageBuffer.Append(Encoding.ASCII.GetString(data, 0, bytes));
            }

            ExtractAndPrintMessages();
        }

        private static void ExtractAndPrintMessages()
        {
            string pattern = ";([A-Za-z0-9]+),([A-Za-z0-9]+);(.*?);END_MESSAGE;";
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(messageBuffer.ToString());

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    string user = match.Groups[1].Value;
                    string cmd = match.Groups[2].Value;
                    string content = match.Groups[3].Value;
                    Terminal.Println(content);
                }
            }

            // Remove processed messages from buffer
            int lastEndIndex = 0;
            if (matches.Count > 0)
                lastEndIndex = matches[matches.Count - 1].Index + matches[matches.Count - 1].Length;

            messageBuffer.Remove(0, lastEndIndex);
        }
    }
}