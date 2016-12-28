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
    class Server
    {
        public Dictionary<string, NetworkStream> clients = new Dictionary<string, NetworkStream>();
        public Queue<string> messageQueue = new Queue<string>();

        public TcpListener CreateServer()
        {
            TcpListener server = new TcpListener(IPAddress.Parse("192.168.1.29"), 15000);
            server.Start();
            return server;
        }
        public void ListenForConnections(TcpListener server)
        {
            while (true)
            {
                Console.WriteLine("Listening for connections...");
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connected!");
                NetworkStream stream = client.GetStream();
                string name = GetName(client);
                AddToClientDictionary(name, stream);
                Console.WriteLine("{0} has joined the chat room.", name);
                messageQueue.Enqueue(name + " has joined the chat room.\n");

            }

        }

        public string GetName(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] message = System.Text.Encoding.ASCII.GetBytes("What is your name?");
            stream.Write(message, 0, message.Length);
            byte[] name = new Byte[256];
            stream.Read(name, 0, name.Length);
            string responseData = System.Text.Encoding.ASCII.GetString(name).TrimEnd('\0');
            stream.Write(message, 0, message.Length);
            return responseData;
        }
        public void ListenForMessages(TcpListener listener)
        {
            byte[] message = new Byte[256];
            string messageString = "";
            while (true)
            {
                if (clients.Count != 0)
                {
                    foreach (var entry in clients.ToList())
                    {
                        entry.Value.Read(message, 0, message.Length);
                        messageString = System.Text.Encoding.ASCII.GetString(message).TrimEnd('\0');
                        AddMessageToQueue(entry.Key, messageString);
                    }
                }
            }
        }
        public string GetMessage(TcpListener server, TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] message = new Byte[255];
            stream.Read(message, 0, message.Length);
            string responseData = System.Text.Encoding.ASCII.GetString(message).TrimEnd('\0');
            return responseData;
        }
        public void AddToClientDictionary(string name, NetworkStream stream)
        {
            clients[name] = stream;
        }
        public void AddMessageToQueue(string user, string message)
        {
            string queueMessage = user + ": " + message;
            messageQueue.Enqueue(queueMessage);
            Console.WriteLine(queueMessage + " added to queue.");
        }
        public void DisplayQueue()
        {
            foreach (string message in messageQueue)
            {
                Console.WriteLine(message);
            }
        }
        public void DisplayClients()
        {
            foreach (var client in clients)
            {
                Console.WriteLine(client.Key);
            }
        }
        public void Broadcast()
        {
            while (true)
            {
                string message = GetMessageFromQueue();
                PublishMessage(message);
            }
        }
        public string GetMessageFromQueue()
        {
            string message = "";
            if (messageQueue.Count != 0)
            {
                message = messageQueue.Dequeue();
            }
            return message;
        }
        public void PublishMessage(string message)
        {
            byte[] byteMessage = System.Text.Encoding.ASCII.GetBytes(message);
            //            Console.WriteLine("Size of clients: " + clients.Count());
            //DisplayClients();
                foreach (var entry in clients.Values.ToList())
                {
//                Console.WriteLine("writing a message");
                entry.Write(byteMessage, 0, byteMessage.Length);
            }
        }        
    }
}

