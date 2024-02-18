namespace Client;

using Host;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

internal class Client {

    private static Client? client = null;

    public static void Start(int port) {
        try {
            client = new(port);
        }
        catch (InvalidOperationException e) {
            Console.Error.WriteLine(e.Message);
        }
        catch (Exception e) {
            Console.Error.WriteLine(e.Message);
        }
    }

    private readonly TcpClient tcpClient;
    private readonly NetworkStream stream;
    private readonly string name;
    private readonly int port;

    private Client(int port) {
        if (client is null) {
            Console.WriteLine("Enter a name: ");
            string? name_string = Console.ReadLine();
            this.name = string.IsNullOrWhiteSpace(name) ? System.Environment.MachineName : name_string.Trim();

            tcpClient = new TcpClient();
            Console.WriteLine("Enter the host's IP Address: ");
            string? ipAddressString = Console.ReadLine();
            ipAddressString = string.IsNullOrWhiteSpace(ipAddressString) ? "192.168.0.1" : ipAddressString.Trim();

            Console.WriteLine("Connecting to {0} on port {1}", IPAddress.Parse(ipAddressString), port);
            tcpClient.Connect(IPAddress.Parse(ipAddressString), port);
            Console.WriteLine("Established connection with: {0} with IP: {1}", tcpClient, tcpClient.Client.RemoteEndPoint);

            stream = tcpClient.GetStream();

            Thread receiveThread = new(new ThreadStart(this.ReceiveMessage));
            receiveThread.Start();

            SendMessage();
        }
        else {
            throw new InvalidOperationException("Client already exists");
        }
    }


    private void SendMessage() {
        stream.BeginWrite(Encoding.Unicode.GetBytes("Hello, " + name), 0, 64, null, null);
    }

    private void ReceiveMessage() {
        try {
            byte[] msg = new byte[64];
            StringBuilder builder = new();
            int bytes = 0;
            do {
                bytes = stream.Read(msg, 0, msg.Length);
                builder.Append(Encoding.Unicode.GetString(msg, 0, bytes));
            }
            while (stream.DataAvailable);
            string message = builder.ToString();
            Console.WriteLine(message);
        }
        catch {
            Console.WriteLine("Connection Lost :(");
            Disconnect();
        }
    }

    private void Disconnect() {
        stream.Close();
        tcpClient.Close();
    }
}