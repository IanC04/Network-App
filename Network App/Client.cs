namespace Client;

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

            while (client.tcpClient.Connected) {
                Console.Write("You: ");
                message = Console.ReadLine() ?? "";
                if (message.Trim().Equals("exit", StringComparison.CurrentCultureIgnoreCase)) {
                    client.SendMessage(message);
                    client.Disconnect();
                    break;
                }

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

    private static IPAddress ParseIPAddress() {
        string ipString = Console.ReadLine() ?? "";
        if (IPAddress.TryParse(ipString, out IPAddress? ipAddress)) {
            return ipAddress;
        }
        else {
            Console.WriteLine("Invalid IP Address. Please enter a valid IP.");
        }

        return ParseIPAddress();
    }

    private readonly TcpClient tcpClient;
    private readonly NetworkStream stream;
    private readonly string name;
    private readonly int port;

    private Client(int port) {
        if (client is null) {
            Console.WriteLine("Enter a name: ");
            string name_string = Console.ReadLine() ?? "";
            this.name = string.IsNullOrWhiteSpace(name_string) ? System.Environment.MachineName : name_string.Trim();

            tcpClient = new TcpClient();
            Console.WriteLine("Enter the host's IP Address: ");
            IPAddress ipAddress = ParseIPAddress();

            Console.WriteLine("Connecting to {0} on port {1}", ipAddress, port);
            tcpClient.Connect(ipAddress, port);
            Console.WriteLine("Established connection with: {0} with IP: {1}", tcpClient, tcpClient.Client.RemoteEndPoint);

            stream = tcpClient.GetStream();
        }
        else {
            throw new InvalidOperationException("Client already exists");
        }
    }

    private void SendMessage(string message) {
        message = this.name + ": " + message;
        byte[] data = Encoding.Unicode.GetBytes(message);
        stream.Write(data, 0, data.Length);
        stream.Flush();
    }

    private void ReceiveMessage() {
        try {
            while (tcpClient.Connected) {
                byte[] msg = new byte[64];
                StringBuilder builder = new();
                int bytes = 0;
                do {
                    bytes = stream.Read(msg, 0, msg.Length);
                    builder.Append(Encoding.Unicode.GetString(msg, 0, bytes));
                }
                while (stream.DataAvailable);
                string message = builder.ToString();
                if (message.EndsWith("exit")) {
                    Disconnect();
                    break;
                }

                Console.WriteLine(message);
            }
        }
        catch {
            Console.WriteLine("Connection Lost :'(");
            Disconnect();
        }
        finally {
            Console.WriteLine("Disconnected");
        }
    }

    private void Disconnect() {
        SendMessage("exit");
        stream.Close();
        tcpClient.Close();
    }

    public override string ToString() {
        return String.Format("Client: {0} with TCP: {1} and Port: {2}", name, tcpClient, port);
    }
}