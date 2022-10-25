using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EchoTCPServer
{
    public class EchoTCPServer
    {
        private readonly IPAddress bindAddr;
        private readonly UInt16 bindPort;
        private readonly ushort bufferSize;
        private readonly ushort maxConnections;

        // TcpListener works in blocking synchronous mode.
        TcpListener server = null;

        private bool isRunning = false;

        public bool IsRunning { get => isRunning; }

        public EchoTCPServer(string bindAddr="127.0.0.1", UInt16 bindPort=8000, ushort bufferSize=256, ushort maxConnections=1024)
        {
            this.bindAddr = IPAddress.Parse(bindAddr);
            this.bindPort = bindPort;
            this.bufferSize = bufferSize;
            this.maxConnections = maxConnections;
            
            server = new TcpListener(this.bindAddr, bindPort);
        }


        public void Run()
        {
            server.Start(maxConnections);
            isRunning = true;
            while(isRunning)
            {
                TcpClient client = server.AcceptTcpClient();
                Task.Run(() => ClientConnectedCallback(client));
            }
            server.Stop();
        }

        public Task ClientConnectedCallback(TcpClient client)
        {
            string remote = client.Client.RemoteEndPoint.ToString();
            Console.WriteLine($"{remote}: Connected.");
            byte[] buffer = new byte[bufferSize];
            NetworkStream stream = client.GetStream();
            while(client.Connected)
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    Console.WriteLine($"{remote}: {bytesRead} bytes received");
                    stream.Write(buffer, 0, bytesRead);
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"{remote}: {ex.Message}");
                }              
            }
            Console.WriteLine($"{remote}: Disconnected.");
            return Task.CompletedTask;
        }
    }
}
