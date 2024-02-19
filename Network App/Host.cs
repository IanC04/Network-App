namespace Host;

using Client;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

internal class Host {

    private static Host? host = null;

    public static void Start(int port) {
        Console.WriteLine(Console.Title = "Host");

        try {
            host = new(port);

            Thread receiveThread = new(new ThreadStart(host.ReceiveMessage));
            receiveThread.Start();

            string message = "You are connected to " + host.name + ", say hi!";
            host.SendMessage(message);

            while (true) {
                Console.Write("You: ");
                message = Console.ReadLine();
                if (message is null) {
                    message = "";
                }
                if (message.Trim().ToLower() == "exit") {
                    host.SendMessage(message);
                    host.Disconnect();
                    break;
                }

                host.SendMessage(message);
            }
        }
        catch (InvalidOperationException e) {
            Console.Error.WriteLine(e.Message);
        }
        catch (Exception e) {
            Console.Error.WriteLine(e.Message);
        }
    }

    private readonly TcpListener listener;
    private readonly TcpClient tcpClient;
    private readonly NetworkStream stream;
    private readonly string name;
    private readonly int port;

    private Host(int port) {
        if (host is null) {
            Console.WriteLine("Enter a name: ");
            string? name_string = Console.ReadLine();
            this.name = string.IsNullOrWhiteSpace(name_string) ? System.Environment.MachineName : name_string.Trim();

            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            this.port = port;

            Console.WriteLine("Waiting for someone to connect...");
            tcpClient = listener.AcceptTcpClient();
            stream = tcpClient.GetStream();

            Console.WriteLine("Established connection with: {0}", tcpClient);
        }
        else {
            throw new InvalidOperationException("Host already exists");
        }
    }

    private void SendMessage(string message) {
        message = host.name + ": " + message;
        byte[] data = Encoding.Unicode.GetBytes(message);
        stream.Write(data, 0, data.Length);
        stream.Flush();
    }

    private void ReceiveMessage() {
        try {
            while (true) {
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
        }
        catch {
            Console.WriteLine("Connection Lost :(");
            Disconnect();
        }
    }

    private void Disconnect() {
        stream.Close();
        tcpClient.Close();
        listener.Stop();
    }

    public override string ToString() {
        return String.Format("Host: {0} with IP: {1} and Port: {2}", name, listener.Server, port);
    }
}
