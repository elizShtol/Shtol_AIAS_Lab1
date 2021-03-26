using System.IO;
using Lab1;
using NUnit.Framework;

namespace Lab1Tests
{
    public class RSATests
    {
        [Test]
        public static void Encode_Decode()
        {
            var rsa = new RSA(new BigInt(111119), new BigInt(1717151));
            var openKeys = rsa.GetOpenKeys();
            var encode = RSA.EncodeFromFile(@"C:\Users\lizas\RiderProjects\AIASLab1\Lab1Tests\text.txt", openKeys.Item1, openKeys.Item2);
            var closedKeys = rsa.GetClosedKeys();
            var decode = RSA.Decode(encode, closedKeys.Item1, closedKeys.Item2);
            using (var sr = new StreamReader(@"C:\Users\lizas\RiderProjects\AIASLab1\Lab1Tests\text.txt"))
            {
                var text = sr.ReadToEnd();
                Assert.That(text.GetHashCode() == decode.GetHashCode());
            }
        }
    }
}