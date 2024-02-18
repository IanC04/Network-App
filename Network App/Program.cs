using System.Text.RegularExpressions;

namespace Network_App {
    internal class Program {
        private static void Main() {
            Console.WriteLine("Welcome to Ian and David Chat");
            Console.WriteLine("Will you be the host or client? 1 for host, 2 for client: ");

            int input = -1;
            do {
                string input_string = Console.ReadLine();
                if (Regex.IsMatch(input_string, @"(1|2)")) {
                    input = int.Parse(input_string);
                }
                else {
                    Console.WriteLine("Invalid input. Please enter 1 for host, 2 for client: ");
                }
            }
            while (input != 1 && input != 2);

            int port = -1;
            do {
                string port_string = Console.ReadLine();
                if (Regex.IsMatch(port_string, @"\d+")) {
                    port = int.Parse(port_string);
                }
                else {
                    Console.WriteLine("Invalid port. Please enter a 16-bit port between 49152 and 65535");
                }
            }
            while (port < 49152 || port > 65535);

            switch (input) {
                case 1: Host.Host.Start(port); break;
                case 2: Client.Client.Start(port); break;
                default: throw new Exception("Invalid input");
            }
        }
    }
}