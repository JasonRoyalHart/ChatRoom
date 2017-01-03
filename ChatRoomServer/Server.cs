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
        public Queue<Message> messageQueue = new Queue<Message>();
        public List<string> log = new List<string>();

        public TcpListener CreateServer()
        {
            TcpListener server = new TcpListener(IPAddress.Parse("192.168.1.29"), 15000);
            server.Start();
            return server;
        }
        public void RunServer(TcpListener server)
        {
            while (true)
            {
                Console.WriteLine("Listening for connections...");
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connected!");
                NetworkStream stream = client.GetStream();
                string name = GetName(client);
                AddToClientDictionary(name, stream);
                Log(name + " has joined the chat room.");
                Message entryMessage = new Message();
                entryMessage.user = name;
                entryMessage.message = name + " has joined the chat room.";
                messageQueue.Enqueue(entryMessage);
                Broadcast();
                Task.Run(() => ListenForMessages(server, stream, name));


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
            return responseData;
        }
        public void ListenForMessages(TcpListener listener, NetworkStream stream, string name)
        {
            byte[] message = new Byte[256];
            string messageString = "";
            while (true)
            {
                Message messageWithUser = new Message();
                Array.Clear(message, 0, message.Length);
                stream.Read(message, 0, message.Length);
                messageString = System.Text.Encoding.ASCII.GetString(message).TrimEnd('\0');
                Console.WriteLine("Recieved {0}", messageString);
                messageWithUser.user = name;
                messageWithUser.message = messageString;
                if (messageWithUser.message[0].ToString() == "@")
                {
                    messageWithUser.privateUser = messageWithUser.FindPrivateUser();
                }
                AddMessageToQueue(messageWithUser);
                Broadcast();

            }
        }
        public string GetMessage(TcpListener server, TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] message = new Byte[256];
            stream.Read(message, 0, message.Length);
            string convertedMessage = System.Text.Encoding.ASCII.GetString(message).TrimEnd('\0');
            return convertedMessage;
        }
        public void AddToClientDictionary(string name, NetworkStream stream)
        {
            clients[name] = stream;
        }
        public void AddMessageToQueue(Message message)
        {
            messageQueue.Enqueue(message);
        }
        public void DisplayQueue()
        {
            foreach (Message message in messageQueue)
            {
                Console.WriteLine(message.GetMessage());
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
            while (messageQueue.Count() > 0)
            {
                Message message = GetMessageFromQueue();
                Log(message.GetMessage());
                PublishMessage(message);
            }
        }
        public Message GetMessageFromQueue()
        {
            Message message = new Message();
            if (messageQueue.Count != 0)
            {
                message = messageQueue.Dequeue();
            }
            return message;
        }
        public void PublishMessage(Message message)
        {
            byte[] byteMessage = System.Text.Encoding.ASCII.GetBytes(message.GetMessage());
            foreach (var entry in clients.ToList())
            {
                if (message.privateUser != "")
                {
                    if (entry.Key == message.privateUser || entry.Key == message.user)
                    {
                        entry.Value.Write(byteMessage, 0, byteMessage.Length);
                    }
                }
                else
                {
                    entry.Value.Write(byteMessage, 0, byteMessage.Length);
                }
            }
        }
        public void Log(string message)
        {
            log.Add(message);

        }        
    }
}

