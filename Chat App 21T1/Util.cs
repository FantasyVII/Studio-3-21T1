using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Chat_App_21T1
{
    class Util
    {
        public static int GetColorNumberFromUser()
        {
            string[] colorNames =
            {
                 "Black", "DarkBlue",
                 "DarkGreen", "DarkCyan",
                 "DarkRed", "DarkMagenta",
                 "DarkYellow", "Gray",
                 "DarkGray", "Blue",
                 "Green", "Cyan",
                 "Red", "Magenta",
                 "Yellow", "White"
            };

            bool parsed = false;
            int colorNumber = 0;
            Console.WriteLine("Please select the color you want your text to be sent in");

            while (true)
            {
                for (int i = 0; i < colorNames.Length; i++)
                {
                    Console.ForegroundColor = GetColorFromNumber(i + 1);
                    Console.WriteLine($"{i + 1}: {colorNames[i]}");
                }

                parsed = int.TryParse(Console.ReadLine(), out colorNumber);

                if (!parsed)
                    Console.WriteLine("Your input was not a valid number, please try again");
                else
                {
                    if (colorNumber < colorNames.Length - 1)
                    {
                        return colorNumber;
                    }
                    else
                    {
                        Console.WriteLine("Your input exceded the maximum colors allowed!");
                    }
                }
            }
        }

        public static ConsoleColor GetColorFromNumber(int colorNumber)
        {
            switch (colorNumber)
            {
                case 1:
                    return ConsoleColor.Black;
                case 2:
                    return ConsoleColor.DarkBlue;
                case 3:
                    return ConsoleColor.DarkGreen;
                case 4:
                    return ConsoleColor.DarkCyan;
                case 5:
                    return ConsoleColor.DarkRed;
                case 6:
                    return ConsoleColor.DarkMagenta;
                case 7:
                    return ConsoleColor.DarkYellow;
                case 8:
                    return ConsoleColor.Gray;
                case 9:
                    return ConsoleColor.DarkGray;
                case 10:
                    return ConsoleColor.Blue;
                case 11:
                    return ConsoleColor.Green;
                case 12:
                    return ConsoleColor.Cyan;
                case 13:
                    return ConsoleColor.Red;
                case 14:
                    return ConsoleColor.Magenta;
                case 15:
                    return ConsoleColor.Yellow;
                case 16:
                    return ConsoleColor.White;
                default:
                    return ConsoleColor.White;
            }
        }

        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                Object obj = binForm.Deserialize(memStream);
                return obj;
            }
        }
    }
}