using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chat_App_21T1
{
    class Server
    {
        Dictionary<Socket, string> clientNicknameSocket;
        List<Socket> clientSockets;

        Socket listeningSocket;
        int port;

        public Server(int port)
        {
            this.port = port;
            clientNicknameSocket = new Dictionary<Socket, string>();
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
                    Socket clientSocket = listeningSocket.Accept();
                    Byte[] recieveBuffer = new byte[1024];
                    int bytesReceived = clientSocket.Receive(recieveBuffer);
                    string nickname = ASCIIEncoding.ASCII.GetString(recieveBuffer);
                    nickname = nickname.Substring(0, bytesReceived);
                    Console.WriteLine($"{nickname} joined the chat!");

                    clientNicknameSocket.Add(clientSocket, nickname);
                    clientSockets.Add(clientSocket);
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
                            Socket dc = clientSockets[i];
                            string nickName = clientNicknameSocket[dc];
                            Console.WriteLine($"{nickName} Disconnected!");

                            clientSockets[i].Close();
                            clientNicknameSocket.Remove(dc);
                            clientSockets.RemoveAt(i);

                            Packet disconnectionPacket = new Packet();
                            disconnectionPacket.nickname = "Server";
                            disconnectionPacket.message = $"{nickName} Disconnected!";
                            disconnectionPacket.textColor = ConsoleColor.Red;

                            for (int j = 0; j < clientSockets.Count; j++)
                            {
                                clientSockets[j].Send(Util.ObjectToByteArray(disconnectionPacket));
                            }
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