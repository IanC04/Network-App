namespace Host;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

internal class Host {
    private static Host host = null;

    public static void Start(int port) {
        Console.WriteLine(Console.Title = "Host");
        try {
            host = new Host(port);
            host.initiate();
        }
        catch (Exception e) {
            Console.WriteLine(e.Message);
        }
    }

    private TcpListener listener;
    private TcpClient client;
    private NetworkStream stream;
    private string name;
    private int port;
    private int ipv4;

    private Host(int port) {
        if (host == null) {
            Console.WriteLine("Enter a name: ");
            name = Console.ReadLine();
            if (string.IsNullOrEmpty(name)) {
                name = System.Environment.MachineName;
            }

            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            Console.WriteLine("Waiting for someone to connect...");
            client = listener.AcceptTcpClient();
            stream = client.GetStream();
        }
        else {
            throw new Exception("Host already exists");
        }
    }
    private void initiate() {
        Console.WriteLine("Established connection with: {0}", client);

        Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
        receiveThread.Start();

        SendMessage();
    }
    private static void SendMessage() {
        Console.WriteLine("Host: {0}");
    }

    private void ReceiveMessage() {
        try {
            /*while ((i = stream.Read(bytes, 0, bytes.Length)) != 0) {
                // Translate data bytes to a ASCII string.
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("Received: {0}", data);

                // Process the data sent by the client.
                data = data.ToUpper();

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                // Send back a response.
                stream.Write(msg, 0, msg.Length);
                Console.WriteLine("Sent: {0}", data);*/

            byte[] msg = new byte[64];
            StringBuilder builder = new StringBuilder();
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
        client.Close();
        listener.Stop();
    }
    override public string ToString() {
        return String.Format("Host: {0} with IP: {1}", name, listener.Server);
    }
}
