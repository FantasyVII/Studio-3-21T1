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
                switch (args[0])
                {
                    case "-server":
                        if (args.Length >= 2)
                        {
                            int port = -1;
                            bool portParsed = int.TryParse(args[1], out port);

                            if (!portParsed)
                                throw new Exception("Arugment two should be a port number. The arguemnt provided was not a number");

                            Server server = new Server(port);
                            server.Start();
                        }
                        else
                            Console.WriteLine("Please provide one extra arguemnts to this application which is the port number. Example app.exe -server 420");
                        break;

                    case "-client":
                        if (args.Length >= 4)
                        {
                            IPAddress ipAddress;
                            int port = -1;

                            bool ipParsed = IPAddress.TryParse(args[1], out ipAddress);
                            bool portParsed = int.TryParse(args[2], out port);

                            if (!ipParsed)
                                throw new Exception("Arugment two should be an IP Address. The arguemnt provided was not a valid ip address");

                            if (!portParsed)
                                throw new Exception("Arugment three should be a port number. The arguemnt provided was not a number");

                            Client client = new Client(ipAddress, port, args[3]);
                            client.Start();
                        }
                        else
                            Console.WriteLine("Please provide two extra arguemnts to this application which are the ip address and port number. Example app.exe -client 127.0.0.1 420");
                        break;
                }
            }
            else
                Console.WriteLine("Please provide arguemnts to this application either type -server or -client");
        }
    }
}