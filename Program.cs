using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Data_Entropy
{
    class Program
    {
        static char[] alphabet = new char[33] {'а', 'б', 'в', 'г', 'ґ', 'д', 'е', 'є', 'ж', 'з', 'и',
                                            'і', 'ї', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с',
                                            'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ь', 'ю', 'я' };
        static readonly string example1 = @"D:\Labs\3 course 2 term\CompSystem\Lab1(Entropy_Base64)\example1.txt";
        static readonly string example2 = @"D:\Labs\3 course 2 term\CompSystem\Lab1(Entropy_Base64)\example2.txt";
        static readonly string example3 = @"D:\Labs\3 course 2 term\CompSystem\Lab1(Entropy_Base64)\example3.txt";
        static void Main(string[] args)
        {

            Console.OutputEncoding = Encoding.UTF8;
            //Присвоюємо змінним тексти файлів
            string reader1 = File.ReadAllText(example1, Encoding.GetEncoding(1251));
            string reader2 = File.ReadAllText(example2, Encoding.GetEncoding(1251));
            string reader3 = File.ReadAllText(example3, Encoding.GetEncoding(1251));
            //Єнтропія кожного тексту
            double entropy1 = CalculateEntropy(reader1);
            double entropy2 = CalculateEntropy(reader2);
            double entropy3 = CalculateEntropy(reader3);
            //Кількість символів в тексті
            int lenght1 = reader1.Count(Char.IsLetter);
            int lenght2 = reader2.Count(Char.IsLetter);
            int lenght3 = reader3.Count(Char.IsLetter);
            //Кількість інформації для кожного тексту
            double inf1 = CalculateEntropy(reader1) * lenght1 / 8;
            double inf2 = CalculateEntropy(reader2) * lenght2 / 8;
            double inf3 = CalculateEntropy(reader3) * lenght3 / 8;
            //Виведення на екран показників
            Console.WriteLine("\nEntropy1: {0}\n\nAmount of information1: {1}\n", CalculateEntropy(reader1), inf1);
            Console.WriteLine("\nEntropy2: {0}\n\nAmount of information2: {1}\n", CalculateEntropy(reader2), inf2);
            Console.WriteLine("\nEntropy3: {0}\n\nAmount of information3: {1}\n", CalculateEntropy(reader3), inf3);
        }
        //Рахує Єнтропію тексту
        public static double CalculateEntropy(string entropyString)
        {
            int len = entropyString.Count(Char.IsLetter);
            Dictionary<char, double> characterCounts = new Dictionary<char, double>();

            foreach (char c in entropyString.ToLower())
            {
                if (c == ' ') continue;
                double currentCount;
                characterCounts.TryGetValue(c, out currentCount);
                characterCounts[c] = currentCount + 1;
            }
            Console.WriteLine("/n" + entropyString + "/n");
            foreach (KeyValuePair<char, double> keyValue in characterCounts)
            {
                double prob = keyValue.Value / len;
                Console.WriteLine(keyValue.Key + " - " + prob);
            }
            /*
             *
             * for (int i = 0; i < alphabet.Length; i++)
            {
                characterCounts[alphabet[i]] = 0;
            }
            int count;
            for(int i = 0;i< characterCounts.Count;i++)
            {
                if (characterCounts.ElementAt(i).Value!=0)
                {
                  characterCounts.TryGetValue(alphabet[i],out count);
                    Console.WriteLine("{0}-{1}", alphabet[i], (count/entropyString.Count(Char.IsLetter)));
                }
            }*/
            IEnumerable<double> characterEntropies =
                from c in characterCounts.Keys
                let frequency = (double)characterCounts[c] / entropyString.Length
                select - 1 * frequency * Math.Log(frequency); 
            

            return characterEntropies.Sum();
        }
    }
    
}
