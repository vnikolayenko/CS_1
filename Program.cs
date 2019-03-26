using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lab1_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            string path = Path();
            char[] text = FileReader(path);
            char[] text64 = Base64Encoding(File.ReadAllBytes(path));
            Console.WriteLine("\nТекстовий файл:");
            Probability(text);
            Console.WriteLine("\nТекстовий файл в base64:");
            Probability(text64);

            char[] commpressed = FileReader(path.Split('.')[0] + ".txt.bz2");
            char[] commpressed64 = Base64Encoding(File.ReadAllBytes(path.Split('.')[0] + ".txt.bz2"));
            Console.WriteLine("\nСтиснутий файл:");
            Probability(commpressed);
            Console.WriteLine("\nСтиснутий файл в base64:");
            Probability(commpressed64);

        }
        static string Path()
        {
            Console.WriteLine("Введіть номер [1-3]:");
            string path;
            try
            {
                switch (int.Parse(Console.ReadLine()))
                {
                    case 1:
                        path = @"D:\Labs\3 course 2 term\CompSystem\Lab1(Entropy_Base64)\example1.txt";
                        break;
                    case 2:
                        path = @"D:\Labs\3 course 2 term\CompSystem\Lab1(Entropy_Base64)\example2.txt";
                        break;
                    case 3:
                        path = @"D:\Labs\3 course 2 term\CompSystem\Lab1(Entropy_Base64)\example3.txt";
                        break;
                    default:
                        Console.WriteLine("Введено не правильне значення. За замовчування використовується 1");
                        path = @"D:\Labs\3 course 2 term\CompSystem\Lab1(Entropy_Base64)\example1.txt";
                        break;
                }
            }
            catch
            {
                Console.WriteLine("Введено не правильне значення. За замовчування використовується 1");
                path = @"D:\Labs\3 course 2 term\CompSystem\Lab1(Entropy_Base64)\example1.txt";
            }
            return path;
        }
        static void Probability(char[] text)
        {
            int lenght = text.Count(char.IsLetter);
            Dictionary<char, double> letter = new Dictionary<char, double>();
            // all symbols in text
            foreach (var symbol in text) 
            {
                if (letter.ContainsKey(symbol))
                    letter[symbol]++;
                else letter[symbol] = 1;
            }

            double Probability;
            double entropy = 0;
            // entrophy
            for (int i = 0; i < letter.Count; i++) 
            {
                Probability = letter.ElementAt(i).Value / lenght;
                entropy += Probability * Math.Log(1.0 / Probability, 2);
            }

            Console.WriteLine("Ентропія = {0:f3}", entropy);
            Console.WriteLine("Кількість інформації в тексті = {0:f0} байт", entropy * lenght / 8);
        }
        static char[] FileReader(string path)
        {
            StreamReader reader = new StreamReader(path, Encoding.GetEncoding(1251));
            string text = "";
            while (reader.Peek() != -1)
            {
                text += reader.ReadLine();
            }
            text = text.ToLower();
            // text string to char[]
            char[] arr = new char[text.Length];
            for (int i = 0; i < text.Length; i++) 
                arr[i] = text[i];

            return arr;
        }
        private static char Base64Table(byte b)
        {
            char[] indexTable = new char[64] {
        'A','B','C','D','E','F','G','H','I','J','K','L','M',
        'N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
        'a','b','c','d','e','f','g','h','i','j','k','l','m',
        'n','o','p','q','r','s','t','u','v','w','x','y','z',
        '0','1','2','3','4','5','6','7','8','9','+','/'};

            if ((b >= 0) && (b <= 63))
            {
                return indexTable[b];
            }
            else
            {
                return ' ';
            }
        }
        public static char[] Base64Encoding(byte[] data)
        {
            int length, length2;
            int blockCount;
            int paddingCount;
            length = data.Length;
            //checkinf for 3 bytes
            if ((length % 3) == 0)
            {
                paddingCount = 0;
                blockCount = length / 3;
            }
            else
            {
                paddingCount = 3 - (length % 3);
                blockCount = (length + paddingCount) / 3;
            }

            length2 = length + paddingCount;

            byte[] source2 = new byte[length2];

            for (int x = 0; x < length2; x++)
            {
                if (x < length)
                {
                    source2[x] = data[x];
                }
                else
                {
                    source2[x] = 0;
                }
            }
            //converting to base64
            byte b1, b2, b3;
            byte temp, temp1, temp2, temp3, temp4;
            byte[] buffer = new byte[blockCount * 4];
            char[] result = new char[blockCount * 4];

            for (int x = 0; x < blockCount; x++)
            {
                b1 = source2[x * 3];
                b2 = source2[x * 3 + 1];
                b3 = source2[x * 3 + 2];

                temp1 = (byte)((b1 & 252) >> 2);

                temp = (byte)((b1 & 3) << 4);
                temp2 = (byte)((b2 & 240) >> 4);
                temp2 += temp;

                temp = (byte)((b2 & 15) << 2);
                temp3 = (byte)((b3 & 192) >> 6);
                temp3 += temp;

                temp4 = (byte)(b3 & 63);

                buffer[x * 4] = temp1;
                buffer[x * 4 + 1] = temp2;
                buffer[x * 4 + 2] = temp3;
                buffer[x * 4 + 3] = temp4;

            }

            for (int x = 0; x < blockCount * 4; x++)
            {
                result[x] = Base64Table(buffer[x]);
            }

            switch (paddingCount)
            {
                case 0:
                    break;
                case 1:
                    result[blockCount * 4 - 1] = '=';
                    break;
                case 2:
                    result[blockCount * 4 - 1] = '=';
                    result[blockCount * 4 - 2] = '=';
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
