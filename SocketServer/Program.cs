using SocketServer;
using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Please enter port" + "\r\n");
        var port = int.Parse(Console.ReadLine());
        Thread t = new Thread(delegate ()
        { 
            Server myserver = new Server("127.0.0.1", port);
        });
        t.Start();
    }
}