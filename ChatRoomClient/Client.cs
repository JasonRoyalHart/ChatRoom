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
    class Client
    {
        public NetworkStream Connect()
        {
            Int32 port = 15000;
            TcpClient client = new TcpClient("192.168.1.29", port);
            NetworkStream stream = client.GetStream();
            return stream;
        }
        public void GetName(NetworkStream stream)
        {
            string message = GetMessage(stream);
            Console.WriteLine(message);
            string name = Console.ReadLine();
            byte[] toBytes = Encoding.ASCII.GetBytes(name);
            stream.Write(toBytes, 0, toBytes.Length);
        }
        public void DisplayMessages(NetworkStream stream)
        {
            while (true)
            {
                string message = GetMessage(stream);
                Console.WriteLine(message);
            }
        }
        public string GetMessage(NetworkStream stream)
        {
            Byte[] message = new Byte[256];
            Int32 bytes = stream.Read(message, 0, message.Length);
            string responseData = System.Text.Encoding.ASCII.GetString(message, 0, message.Length).TrimEnd('\0');
            return responseData;
        }
        public void SendInput(NetworkStream stream)
        {
            while (true)
            {
                string message = GetInput();
                byte[] toBytes = Encoding.ASCII.GetBytes(message);
                stream.Write(toBytes, 0, toBytes.Length);
            }
        }
        public string GetInput()
        {
            return Console.ReadLine();
        }

    }
}
