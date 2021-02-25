using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_App_21T1
{
    [Serializable]
    struct Packet
    {
        public string nickname;
        public string message;
        public ConsoleColor textColor;
    }
}