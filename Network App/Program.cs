namespace Network_App;

using System.Text.RegularExpressions;

internal partial class Program {
    private static void Main() {
        Console.WriteLine("Welcome to Ian and David Chat");

        Console.WriteLine("Will you be the host or client? 1 for host, 2 for client: ");
        bool host = ParseHost();

        Console.WriteLine("Enter a port number between 49152 and 65535: ");
        int port = ParsePort();

        if (host) {
            Host.Host.Start(port);
        }
        else {
            Client.Client.Start(port);
        }
    }

    private static bool ParseHost() {
        string? host_string = Console.ReadLine();
        if (host_string is not null && HostRegex().IsMatch(host_string)) {
            int input = int.Parse(host_string);
            Console.WriteLine("You are the {0}", input == 1 ? "host" : "client");
            return input == 1;
        }
        else {
            Console.WriteLine("Invalid input. Please enter 1 for host, 2 for client.");
        }

        return ParseHost();
    }

    private static int ParsePort() {
        string? port_string = Console.ReadLine();
        if (port_string is not null && PortRegex().IsMatch(port_string)) {
            int port = int.Parse(port_string);
            if (port >= 49152 && port <= 65535) {
                return port;
            }
            else {
                Console.WriteLine("Port number out of bounds. Please enter: [49152 : 65535].");
            }
        }
        else {
            Console.WriteLine("Invalid port. Please enter a port number between 49152 and 65535.");
        }

        return ParsePort();
    }

    [GeneratedRegex(@"^\s*(1|2)\s*$")]
    private static partial Regex HostRegex();

    [GeneratedRegex(@"^\s*\d+\s*$")]
    private static partial Regex PortRegex();
}