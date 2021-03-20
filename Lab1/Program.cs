using System;
using System.Text;

namespace Lab1
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Введите строку для шифрования");
            var text = Console.ReadLine();
            Console.WriteLine("Введите параметры для RSA-шифрования (2 простых числа)");
            var p = new BigInt(Console.ReadLine());
            var q = new BigInt(Console.ReadLine());
            var rsa = new RSA(p,q);
            Console.WriteLine($"Открытый ключ ({rsa.E}, {rsa.N})");
            Console.WriteLine($"Закрытый ключ ({rsa.D}, {rsa.N})");
            var encoding = rsa.Encode(text);
            Console.WriteLine("Закодированное сообщение");
            foreach (var bigInt in encoding)
            {
                Console.Write($"{bigInt} ");
            }
            Console.WriteLine();
            var decode = rsa.Decode(encoding);
            Console.WriteLine(decode);
            if(text.GetHashCode() == decode.GetHashCode())
                Console.WriteLine("Шифрование и дешифрование прошло успешно");
            else
                Console.WriteLine("Что-то пошло не так...");
        }
    }
}