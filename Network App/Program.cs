using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Host
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("Welcome to Ian and David Chat");
            Console.WriteLine("Input the port you have port forwarded with your IP address");

            string port_string = Console.ReadLine();
            int port = string.IsNullOrWhiteSpace(port_string) ? 50000 : int.Parse(port_string);
          
            TcpListener host = new TcpListener(IPAddress.Any, port);
            host.Start();
            Console.WriteLine("Server has been started with your IP and Port");

            TcpClient client = host.AcceptTcpClient();
            Console.WriteLine("Client has connected to the host");

            NetworkStream stream = client.GetStream();

            

            while (true)
            {
                string message = Console.ReadLine();

            }
        }
    }
}
