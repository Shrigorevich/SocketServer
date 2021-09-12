using SocketServer;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SocketServer
{
    public class Server
    {
        readonly Socket listener = null;
        readonly Storage Storage;

        public Server(string ip, int port)
        {
            Storage = new Storage();
            IPAddress address = IPAddress.Parse(ip);
            IPEndPoint endPoint = new IPEndPoint(address, port);
            listener = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(endPoint);
            listener.Listen(10);
            StartListener();
        }

        public void StartListener()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    Socket client = listener.Accept();
                    Console.WriteLine(((IPEndPoint)client.RemoteEndPoint).Address.ToString() + " Connected!");

                    Thread t = new Thread(new ParameterizedThreadStart(HandleDevice));
                    t.Start(client);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                listener.Close();
            }
        }

        public void HandleDevice(object obj)
        {
            Socket client = (Socket)obj;
            var clientIp = ((IPEndPoint)client.RemoteEndPoint).Address.ToString();
            Storage.AddClient(clientIp);

            var welcomeMessage = "Hello: " + clientIp + "\r\n";
            client.Send(Encoding.ASCII.GetBytes(welcomeMessage));

            var receivedMessage = string.Empty;

            try
            {
                while (true)
                {
                    if(client.Connected)
                    {
                        byte[] bytes = new byte[256];
                        int bytesRec = client.Receive(bytes);
                        receivedMessage += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                        try
                        {
                            if (receivedMessage.IndexOf("\n") > -1)
                            {
                                MessageProcessing(client, receivedMessage);
                                receivedMessage = string.Empty;
                            }

                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine("Exception: {0}", e.ToString());
                            client.Send(Encoding.ASCII.GetBytes("Invalid command. Please enter 'list', 'disconnect' or any integer value" + "\r\n"));
                        }
                    } else
                    {
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
                Storage.RemoveClient(clientIp);
                client.Close();
            }
        }

        public void MessageProcessing(Socket client, string message)
        {
            var clientIp = ((IPEndPoint)client.RemoteEndPoint).Address.ToString();
            message = message.Replace("\r\n", "");

            if (message.Equals("list"))
            {
                var storageData = Storage.QueryAll();
                var storageDataView = Helpers.ClientDataToView(storageData);

                client.Send(Encoding.ASCII.GetBytes(storageDataView));
            }
            else if (message.Equals("disconnect"))
            {
                Storage.RemoveClient(clientIp);
                client.Close();
            }
            else
            {
                var number = int.Parse(message);
                Storage.UpdateClientData(clientIp, number);
            }
        }
    }
}
