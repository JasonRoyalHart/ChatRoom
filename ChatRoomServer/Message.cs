using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoomServer
{
    class Message
    {
        public string message;
        public string user;

        public string privateUser;

        public Message()
        {
            privateUser = "";
        }

        public string GetMessage()
        {
            return user + ": " + message;
        }
        public string FindPrivateUser()
        {
            string user = "";
            foreach (char character in message)
            {
                if (character.ToString() == " ")
                {
                    break;
                }
                else if (character.ToString() != "@" && character.ToString() != ":")
                {
                    user += character.ToString();
                }
            }
            Console.WriteLine("PRivate user: " + user);
            return user;
        }
    }
}
