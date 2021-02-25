using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Chat_App_21T1
{
    class Server
    {
        List<Socket> clientSockets;
        Socket listeningSocket;
        int port;

        public Server(int port)
        {
            this.port = port;
            clientSockets = new List<Socket>();
        }

        public void Start()
        {
            listeningSocket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);

            listeningSocket.Blocking = false;
            listeningSocket.Bind(new IPEndPoint(IPAddress.Any, port));

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
    }
}