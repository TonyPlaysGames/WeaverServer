using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebweaverServer
{
    internal class Player
    {
        private User user;
        private BlockingCollection<string> mailbox;

        public int balance = 0;

        public Player(User user, BlockingCollection<string> mailbox)
        {
            this.user = user;
            this.mailbox = mailbox;
        }
    
        public void change_balance(int change) { balance += change; }


        public void AddMail(string message)
        {
            mailbox.Add(message);
        }

        private void SendMail(string message, User recipient)
        {
            // TODO
        }
    }
}
