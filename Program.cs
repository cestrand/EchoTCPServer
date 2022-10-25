using System.Net.Sockets;

namespace EchoTCPServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting EchoTCPServer.");
            EchoTCPServer server = new();
            server.Run();
        }
    }

}