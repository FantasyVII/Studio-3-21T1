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
                    List<Socket> clientSockets = new List<Socket>();

                    Socket listeningSocket;

                    listeningSocket = new Socket(
                        AddressFamily.InterNetwork,
                        SocketType.Stream,
                        ProtocolType.Tcp);

                    listeningSocket.Blocking = false;
                    listeningSocket.Bind(new IPEndPoint(IPAddress.Any, 420));
                    Console.WriteLine("Waiting for connection...");
                    listeningSocket.Listen(10);

                    while (true)
                    {
                        try
                        {
                            clientSockets.Add(listeningSocket.Accept());
                        }
                        catch (SocketException ex)
                        {
                            if (ex.SocketErrorCode != SocketError.WouldBlock)
                                Console.WriteLine(ex);
                        }

                        for (int i = 0; i < clientSockets.Count; i++)
                        {
                            try
                            {
                                Byte[] recieveBuffer = new byte[4096];
                                int bytesReceived = clientSockets[i].Receive(recieveBuffer);

                                for (int j = 0; j < clientSockets.Count; j++)
                                {
                                    if (i != j)
                                    {
                                        clientSockets[j].Send(recieveBuffer, bytesReceived, SocketFlags.None);
                                    }
                                }
                            }
                            catch (SocketException ex)
                            {
                                if (ex.SocketErrorCode == SocketError.ConnectionAborted ||
                                    ex.SocketErrorCode == SocketError.ConnectionReset)
                                {
                                    clientSockets[i].Close();
                                    clientSockets.RemoveAt(i);
                                }

                                if (ex.SocketErrorCode != SocketError.WouldBlock)
                                {
                                    if (ex.SocketErrorCode != SocketError.ConnectionAborted || ex.SocketErrorCode != SocketError.ConnectionReset)
                                    {
                                        Console.WriteLine(ex);
                                    }
                                }
                            }
                        }
                    }
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
                    socket.Blocking = false;
                    Console.WriteLine("Connected to server!");

                    Console.WriteLine("Please enter your nickname!");
                    string nickName = Console.ReadLine();
                    nickName += ": ";

                    Console.WriteLine("Please type your message now!");
                    string stringToSend = "";

                    while (true)
                    {
                        try
                        {
                            if (Console.KeyAvailable)
                            {
                                ConsoleKeyInfo key = Console.ReadKey();

                                if (key.Key == ConsoleKey.Enter)
                                {
                                    string message = nickName + stringToSend;
                                    socket.Send(ASCIIEncoding.ASCII.GetBytes(message));
                                    message = "";
                                    Console.WriteLine();
                                }
                                else
                                {
                                    stringToSend += key.KeyChar;
                                }
                            }

                            Byte[] recieveBuffer = new byte[4096];
                            int receivedBytes = socket.Receive(recieveBuffer);
                            string strinToPrint = ASCIIEncoding.ASCII.GetString(recieveBuffer);
                            strinToPrint = strinToPrint.Substring(0, receivedBytes);

                            Console.WriteLine(strinToPrint);
                        }
                        catch (SocketException ex)
                        {
                            if (ex.SocketErrorCode != SocketError.WouldBlock)
                                Console.WriteLine(ex);
                        }
                    }
                }
            }
            else
                Console.WriteLine("Please provide arguemnts to this application either type -server or -client");
        }
    }
}