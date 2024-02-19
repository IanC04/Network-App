namespace Network_App;

using System.Text.RegularExpressions;

internal partial class Program {
    private static void Main() {
        Console.WriteLine("Welcome to Ian and David Chat");

        bool host = ParseHost();
        int port = ParsePort(49500); // Default: 49500

        Console.WriteLine("You are connecting on port: {0}.", port);

        if (host) {
            Host.Host.Start(port);
        }
        else {
            Client.Client.Start(port);
        }
    }

    private static bool ParseHost() {
        Console.WriteLine("Enter 1 for host, 2 for client: ");

        string host_string = Console.ReadLine() ?? "";
        if (HostRegex().IsMatch(host_string)) {
            int input = int.Parse(host_string);
            Console.WriteLine("You are the {0}", input == 1 ? "host" : "client");
            return input == 1;
        }
        else {
            Console.WriteLine("Invalid input.");
        }

        return ParseHost();
    }

    private static int ParsePort(int? defaultPort) {
        if (defaultPort is not null) {
            return (int) defaultPort;
        }

        Console.WriteLine("Please enter a port number between 49152 and 65535: ");
        string port_string = Console.ReadLine() ?? "";
        if (PortRegex().IsMatch(port_string)) {
            int port = int.Parse(port_string);
            if (port >= 49152 && port <= 65535) {
                return port;
            }
            else {
                Console.WriteLine("Port number out of bounds.");
            }
        }
        else {
            Console.WriteLine("Invalid port.");
        }

        return ParsePort(null);
    }

    [GeneratedRegex(@"^\s*(1|2)\s*$")]
    private static partial Regex HostRegex();

    [GeneratedRegex(@"^\s*\d+\s*$")]
    private static partial Regex PortRegex();
}