using SocketServer;
using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        int port;

        Console.WriteLine("Please enter port" + "\r\n");

        while (true)
        {
            try
            {
                port = int.Parse(Console.ReadLine());
                break;
            }
            catch
            {
                Console.WriteLine("Port must be numeric");
            }
        }

        Thread t = new Thread(delegate ()
        {
            Server myserver = new Server("127.0.0.1", port);
        });
        t.Start();
    }
}