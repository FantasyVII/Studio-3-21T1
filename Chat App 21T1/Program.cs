using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat_App_21T1
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0] == "-server")
                {
                    Socket listeningSocket;

                    listeningSocket = new Socket(
                        AddressFamily.InterNetwork,
                        SocketType.Stream,
                        ProtocolType.Tcp);

                    listeningSocket.Bind(new IPEndPoint(IPAddress.Any, 420));
                    Console.WriteLine("Waiting for connection...");
                    listeningSocket.Listen(10);
                    Socket clientSocket = listeningSocket.Accept();

                    clientSocket.Send(ASCIIEncoding.ASCII.GetBytes("Hello Client!"));

                    Byte[] recieveBuffer = new byte[1024];
                    clientSocket.Receive(recieveBuffer);
                    Console.WriteLine(ASCIIEncoding.ASCII.GetString(recieveBuffer));
                }
                else if (args[0] == "-client")
                {
                    Socket socket;

                    socket = new Socket(
                        AddressFamily.InterNetwork,
                        SocketType.Stream,
                        ProtocolType.Tcp);

                    Console.WriteLine("Connecting to server...");
                    socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 420));
                    Console.WriteLine("Connected to server!");

                    Byte[] recieveBuffer = new byte[1024];
                    socket.Receive(recieveBuffer);
                    Console.WriteLine(ASCIIEncoding.ASCII.GetString(recieveBuffer));

                    socket.Send(ASCIIEncoding.ASCII.GetBytes("Hello Server!"));
                }
            }
            else
                Console.WriteLine("Please provide arguemnts to this application either type -server or -client");
        }
    }
}