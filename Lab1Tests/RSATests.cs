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
            var encode = rsa.Encode(@"C:\Users\lizas\RiderProjects\AIASLab1\Lab1Tests\text.txt");
            var decode = rsa.Decode(encode);
            using (var sr = new StreamReader(@"C:\Users\lizas\RiderProjects\AIASLab1\Lab1Tests\text.txt"))
            {
                var text = sr.ReadToEnd();
                Assert.That(text.GetHashCode() == decode.GetHashCode());
            }
        }
    }
}