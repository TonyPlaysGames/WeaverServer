using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebweaverServer
{
    public class Message
    {
        public string Sender { get; private set; }
        public string Cmd { get; private set; }
        public string Content { get; private set; }

        public Message(string user, string cmd, string content)
        {
            Sender = user;
            Cmd = cmd;
            Content = content;
        }
    }


    public class Command
    {
        public string Target { get; private set; }
        public string Cmd { get; private set; }
        public Command(string target, string cmd)
        {
            Target = target;
            Cmd = cmd;
        }
    }
}
