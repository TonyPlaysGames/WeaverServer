using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebweaverServer
{
    public static class CommandParser 
    {
        public static string Parse(string raw_msg, User user) {


            Regex regex = new Regex(";([A-Za-z0-9]+),([A-Za-z0-9]+);(.*);END_MESSAGE;");
            Match match = regex.Match(raw_msg);
            Message message = null;

            if (match.Success)
            {
                string sender = match.Groups[1].Value;
                string cmd = match.Groups[2].Value;
                string content = (match.Groups[3].Value).ToLower().Trim();
                message = new Message(sender, cmd, content);
            }
            else
            {
                throw new InvalidOperationException("Failed to parse message.");
            }


            switch (message.Content)
            { 
                case "help":
                case "h":
                    user.SendMessage("Available commands: help, echo, balance");
                    break;
                case "echo":
                    user.SendMessage(string.Join(" ", message.Content));  // Echoes back the input
                    break;
                case "balance":
                case "bal":
                    user.SendMessage("Balance: 100");  // Placeholder response
                    break;
                default:
                    user.SendMessage("Unknown command. Type 'help' for assistance.");
                    break;
            }

            // Temp until better processing exists
            return message.Content;

    //println("");

    // How should I go about parsing the commands here? Should I use an AST, with a set of commands that each allow for different things after their input?
    // Commnads could be something like... (Where caps is ignored) 'help'/'h' , 'echo', 'balance'/'bal', '
        }
    }

}
