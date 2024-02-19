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

            Thread receiveThread = new(new ThreadStart(client.ReceiveMessage));
            receiveThread.Start();

            string message = "You are connected to " + client.name + ", say hi!";
            client.SendMessage(message);

            while (true) {
                message = Console.ReadLine();
                if (message.Trim().ToLower() == "exit") {
                    client.SendMessage(message);
                    client.Disconnect();
                    break;
                }

                message = client.name+ ": " + message;
                client.SendMessage(message);
            }
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
        }
        else {
            throw new InvalidOperationException("Client already exists");
        }
    }

    private void SendMessage(string? message) {
        byte[] data = Encoding.Unicode.GetBytes(message);
        stream.Write(data, 0, data.Length);
        stream.Flush();
    }

    private void ReceiveMessage() {
        while (true) {
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
    }

    private void Disconnect() {
        stream.Close();
        tcpClient.Close();
    }

    public override string ToString() {
        return String.Format("Client: {0} with TCP: {1} and Port: {2}", name, tcpClient, port);
    }
}