using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lab1
{
    public class RSA
    {
        public readonly BigInt D;
        public readonly BigInt E;
        public readonly BigInt N;

        public RSA(BigInt p, BigInt q)
        {
            if (p.Sign < 0 || q.Sign < 0)
                throw new Exception("Numbers must be positive");
            if (!BigInt.IsItSimple(p) || !BigInt.IsItSimple(q))
                throw new Exception("Numbers must be simple");
            N = p * q;
            var fi = (p - new BigInt("1")) * (q - new BigInt("1"));
            E = fi - new BigInt("1");
            while (!(BigInt.IsItSimple(E) && BigInt.Gcd(E, fi, out var x, out var y) == new BigInt("1")))
                E -= new BigInt("2");
            D = BigInt.GetInverseElementModulo(E, fi);
        }

        public BigInt[] EncodeFromFile(string pathToFile)
        {
            var result = new List<BigInt>();
            using (var sr = new StreamReader(pathToFile))
            {
                var text = sr.ReadToEnd();
                foreach (var symbol in text)
                {
                    var message = BigInt.ModPow(new BigInt(symbol), E, N);
                    result.Add(message);
                }
            }

            return result.ToArray();
        }
        
        public BigInt[] Encode(string text)
        {
            var result = new List<BigInt>();
           
                foreach (var symbol in text)
                {
                    var message = BigInt.ModPow(new BigInt(symbol), E, N);
                    result.Add(message);
                }
            
            return result.ToArray();
        }

        public string Decode(BigInt[] message)
        {
            var text = new StringBuilder();
            foreach (var b in message)
            {
                var symbol = BigInt.TryToInt(BigInt.ModPow(b, D, N));
                text.Append((char) symbol);
            }

            return text.ToString();
        }
        
    }
}