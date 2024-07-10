using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebweaverServer
{
    public static class GameDriver
    {
        private static List<Player> players = new List<Player>();
        private static BlockingCollection<string> command_queue = new BlockingCollection<string>();
        public static void Start()
        {
            
        }

        
        public static void AddCommand(string command)
        {
            command_queue.Add(command);
        }

        public static void AddPlayer(User user, BlockingCollection<string> mailbox)
        {
            var player = new Player(user, mailbox);
            players.Add(player);
        }
    }
}
 