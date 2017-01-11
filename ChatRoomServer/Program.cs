using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net;

namespace ChatRoomServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TextLogger logger = new TextLogger();
            Server server = new Server(logger);
            TcpListener listener = server.CreateServer();
            Task.Run(() => server.RunServer(listener));
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

        }
    }
}
