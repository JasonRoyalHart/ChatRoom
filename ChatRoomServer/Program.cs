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
            Server server = new Server();
            TcpListener listener = server.CreateServer();
            Parallel.Invoke(() => server.ListenForConnections(listener), () => server.ListenForMessages(listener));
//            Task.Run(() => server.Broadcast());
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

        }
    }
}
