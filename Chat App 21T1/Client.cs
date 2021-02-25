using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chat_App_21T1
{
    class Client
    {
        IPAddress ipAddress;
        int port;

        public Client(IPAddress ipAddress, int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
        }

        public void Start()
        {
            Socket socket;
            Packet packetToSend = new Packet();

            socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);

            Console.WriteLine("Connecting to server...");
            socket.Connect(new IPEndPoint(ipAddress, port));
            socket.Blocking = false;
            Console.WriteLine("Connected to server!");

            Console.WriteLine("Please enter your nickname!");
            packetToSend.nickname = Console.ReadLine();

            packetToSend.textColor = Util.GetColorFromNumber(Util.GetColorNumberFromUser());

            Console.WriteLine("Please type your message now!");

            while (true)
            {
                try
                {
                    if (Console.KeyAvailable)
                    {
                        Console.ForegroundColor = packetToSend.textColor;
                        ConsoleKeyInfo key = Console.ReadKey();

                        if (key.Key == ConsoleKey.Enter)
                        {
                            packetToSend.message = packetToSend.nickname + ": " + packetToSend.message;
                            socket.Send(Util.ObjectToByteArray(packetToSend));

                            packetToSend.message = "";
                            Console.WriteLine();
                        }
                        else
                        {
                            packetToSend.message += key.KeyChar;
                        }
                    }

                    Byte[] recieveBuffer = new byte[4096];
                    int receivedBytes = socket.Receive(recieveBuffer);
                    Packet packetToRecieve = (Packet)Util.ByteArrayToObject(recieveBuffer);

                    Console.Write($"\r{new string(' ', (Console.WindowWidth - 1))}\r");
                    Console.ForegroundColor = packetToRecieve.textColor;
                    Console.WriteLine(packetToRecieve.message);

                    Console.ForegroundColor = packetToSend.textColor;
                    Console.Write(packetToSend.message);
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode != SocketError.WouldBlock)
                        Console.WriteLine(ex);
                }
            }
        }
    }
}