using System;
using System.IO;

namespace Lab1
{
    internal class Program
    {
        private static void Main()
        {
            while (true)
            {
                Console.WriteLine("Для шифрования строки нажмите y, для дешифрования строки нажмите любую другую клавишу.");
                if(Console.ReadKey().KeyChar=='y')
                    Encode();
                else
                    Decode();
                Console.WriteLine("Для выхода из программы нажмите y, для продолжения работы нажмите любую другую клавишу.");
                if(Console.ReadKey().KeyChar=='y')
                    break;
            }
            
        }

        private static void Encode()
        {
            Console.WriteLine("Введите параметры для RSA-шифрования (2 простых числа)");
            var p = new BigInt(Console.ReadLine());
            var q = new BigInt(Console.ReadLine());
            var rsa = new RSA(p, q);
            var openKeys = rsa.GetOpenKeys();
            var closedKeys = rsa.GetClosedKeys();
            Console.WriteLine($"Открытый ключ ({openKeys.Item1}, {openKeys.Item2})");
            Console.WriteLine($"Закрытый ключ ({closedKeys.Item1}, {closedKeys.Item2})");
            Console.WriteLine("Для шифрования текста из файла нажмите y, для ввода файла с клавиатуры нажмите любую другую клавишу");
            var text = "";
            if (Console.ReadKey().KeyChar == 'y')
            {
                Console.WriteLine("Введите абсолютный путь до файла");
                var path = Console.ReadLine().Trim();
                RSA.EncodeFromFileToFile(path, openKeys.Item1, openKeys.Item2);
            }
            else
            {
                Console.WriteLine("Введите строку");
                text = Console.ReadLine();
                RSA.EncodeToFile(text, openKeys.Item1, openKeys.Item2);
            }
            Console.WriteLine("Закодированный файл находится в файле encoding.txt");
        }

        private static void Decode()
        {
            Console.WriteLine("Введите абсолютный путь до файла");
            var path = Console.ReadLine().Trim();
            Console.WriteLine("Введите закрытый ключ (2 числа через пробел)");
            var closedKey1 = new BigInt(Console.ReadLine().Trim());
            var closedKey2 = new BigInt(Console.ReadLine().Trim());
            var text = RSA.DecodeFromFile(path, closedKey1, closedKey2);
            Console.WriteLine(text);
            Console.WriteLine("Для сохранения текста в файл нажмите y, для продолжения работы любую другую");
            if(Console.ReadKey().KeyChar == 'y')
                using (var file = new StreamWriter(@"C:\Users\lizas\RiderProjects\AIASLab1\Lab1\decoding.txt"))
                {
                    file.Write(text);
                }
            Console.WriteLine(@"Декодированный файл находится в файле AIASLab1\Lab1\decoding.txt");
        }
        
    }
}