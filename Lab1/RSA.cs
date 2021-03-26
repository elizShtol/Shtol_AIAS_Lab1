using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lab1
{
    public class RSA
    {
        private readonly BigInt d;
        private readonly BigInt e;
        private readonly BigInt n;

        public RSA(BigInt p, BigInt q)
        {
            if (p.Sign < 0 || q.Sign < 0)
                throw new Exception("Numbers must be positive");
            if (!BigInt.IsItSimple(p) || !BigInt.IsItSimple(q))
                throw new Exception("Numbers must be simple");
            n = p * q;
            var fi = (p - new BigInt("1")) * (q - new BigInt("1"));
            e = fi - new BigInt("1");
            while (!(BigInt.IsItSimple(e) && BigInt.Gcd(e, fi, out var x, out var y) == new BigInt("1")))
                e -= new BigInt("2");
            d = BigInt.GetInverseElementModulo(e, fi);
        }
        

        public static BigInt[] EncodeFromFile(string pathToFile, BigInt e, BigInt n)
        {
            using (var sr = new StreamReader(pathToFile))
            {
                var text = sr.ReadToEnd().Trim();
                return Encode(text, e, n);
            }
        }

        public static BigInt[] Encode(string text, BigInt e, BigInt n)
        {
            var result = new List<BigInt>();

            foreach (var symbol in text)
            {
                var message = BigInt.ModPow(new BigInt(symbol), e, n);
                result.Add(message);
            }
            return result.ToArray();
        }
        
        public static void EncodeToFile(string text, BigInt e, BigInt n)
        {
            using (var file = new StreamWriter(@"C:\Users\lizas\RiderProjects\AIASLab1\Lab1\encoding.txt"))
            {
                file.Write("");
            }
            using (var file = new StreamWriter(@"C:\Users\lizas\RiderProjects\AIASLab1\Lab1\encoding.txt", true))
            {
                foreach (var message in Encode(text, e, n))
                {
                    file.Write($"{message} ");
                }
            }
        }
        
        public static void EncodeFromFileToFile(string pathToFile, BigInt e, BigInt n)
        {
            using (var file = new StreamReader(pathToFile))
            {
                var text = file.ReadToEnd().Trim();
                EncodeToFile(text, e, n);
            }
        }
        
        
        
        public static string Decode(BigInt[] message, BigInt d, BigInt n)
        {
            var text = new StringBuilder();
            foreach (var b in message)
            {
                var symbol = BigInt.TryToInt(BigInt.ModPow(b, d, n));
                text.Append((char) symbol);
            }

            return text.ToString();
        }

        public static string DecodeFromFile(string path, BigInt d, BigInt n)
        {
            using (var file = new StreamReader(path))
            {
                var message = file.ReadToEnd().Split().Where(x => x!="").Select(x => new BigInt(x)).ToArray();
                
                return Decode(message, d, n);
            }
        }
        
        

        public Tuple<BigInt, BigInt> GetOpenKeys()
        {
            return Tuple.Create(e, n);
        }
        
        public Tuple<BigInt, BigInt> GetClosedKeys()
        {
            return Tuple.Create(d, n);
        }
    }
}