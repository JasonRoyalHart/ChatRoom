using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net;

namespace ChatRoomClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();
            
            NetworkStream stream = client.Connect();

            client.GetName(stream);

            Task.Run(() => client.DisplayMessages(stream));
            Task.Run(() => client.SendInput(stream));
            while (true) { }
        }
    }
}
