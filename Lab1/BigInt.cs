using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab1
{
    public class BigInt
    {
        public readonly sbyte Sign;
        public BigInt(int number)
        {
            var digits = new List<byte>();
            if (number < 0)
                Sign = -1;
            else
                Sign = 1;
            number = Math.Abs(number);
            if (number == 0)
            {
                Digits = new[] {(byte) 0};
                return;
            }

            while (number > 0)
            {
                digits.Insert(0, (byte) (number % 10));
                number /= 10;
            }

            Digits = digits.ToArray();
        }

        public BigInt(string number)
        {
            if (number[0] == '-')
                Sign = -1;
            else
                Sign = 1;

            Digits = number.Where(char.IsDigit).Select(i => byte.Parse(i.ToString())).ToArray();
        }

        public BigInt(byte[] digits, sbyte sign)
        {
            var digitsWithoutLeadingZeros = DeleteLeadingZeros(digits.ToList()).ToArray();
            Digits = new byte[digitsWithoutLeadingZeros.Length];
            digitsWithoutLeadingZeros.CopyTo(Digits, 0);
            Sign = sign;
        }

        public byte[] Digits { get; }

        public static BigInt operator +(BigInt n1, BigInt n2)
        {
            if (n1.Sign * n2.Sign > 0)
            {
                var p = 0;
                var sumDigits = new List<byte>();
                ConvertToSameLength(n1, n2, out var n1Digits, out var n2Digits);
                for (var i = n1Digits.Count - 1; i >= 0; i--)
                {
                    var sumDigit = (n1Digits[i] + n2Digits[i] + p) % 10;
                    p = (n1Digits[i] + n2Digits[i] + p) / 10;
                    sumDigits.Insert(0, (byte) Math.Abs(sumDigit));
                }

                sumDigits.Insert(0, (byte) p);
                var sumSign = n1.Sign;
                return new BigInt(DeleteLeadingZeros(sumDigits).ToArray(), sumSign);
            }

            if (n1.Sign < 0)
                return n2 - -n1;
            return n1 - -n2;
        }

        public static BigInt operator -(BigInt n1, BigInt n2)
        {
            if (n1.Sign * n2.Sign > 0)
            {
                var p = 0;
                var difDigits = new List<byte>();
                ConvertToSameLength(n1, n2, out var n1Digits, out var n2Digits);
                var biggestNumber = n1Digits;
                var smallestNumber = n2Digits;
                if (n2 > n1 && n2.Sign > 0 || n2 < n1 && n2.Sign < 0)
                {
                    biggestNumber = n2Digits;
                    smallestNumber = n1Digits;
                }

                for (var i = n1Digits.Count - 1; i >= 0; i--)
                {
                    var difDigit = (10 + biggestNumber[i] - smallestNumber[i] - p) % 10;
                    p = biggestNumber[i] - p < smallestNumber[i] ? 1 : 0;
                    difDigits.Insert(0, (byte) Math.Abs(difDigit));
                }

                var difSign = n1 >= n2 ? 1 : -1;
                return new BigInt(DeleteLeadingZeros(difDigits).ToArray(), (sbyte) difSign);
            }

            return n1 + -n2;
        }

        public static BigInt operator *(BigInt n1, BigInt n2)
        {
            if (n1 == new BigInt(0) || n2 == new BigInt(0))
                return new BigInt(0);
            var n1Abs = n1;
            if (n1.Sign < 0)
                n1Abs = -n1;
            var mult = new BigInt(0);
            var power = 0;
            for (var i = n2.Digits.Length - 1; i >= 0; i--)
            {
                mult += MultOn10inPower(MultOnDigit(n1Abs, n2.Digits[i]), power);
                power++;
            }

            if (n1.Sign * n2.Sign < 0)
                return -mult;
            return mult;
        }


        public static BigInt operator /(BigInt n1, BigInt n2)
        {
            if (n2 == new BigInt(0))
                throw new DivideByZeroException();
            if (n1 == new BigInt(0))
                return n1;
            if (n2 == new BigInt(1))
                return n1;
            if (n2 == new BigInt(-1))
                return -n1;
            if (n1 == n2)
                return new BigInt(1);
            if (n1 == -n2)
                return new BigInt(-1);
            var n1Abs = Abs(n1);
            var n2Abs = Abs(n2);
            var remainder = new List<byte>();
            var result = new List<byte>();
            foreach (var digit in n1Abs.Digits)
            {
                remainder.Add(digit);
                var smallDivResult = SmallDivision(new BigInt(remainder.ToArray(), 1), n2Abs);
                result.Add(smallDivResult.Item1);
                remainder = smallDivResult.Item2.Digits.ToList();
            }

            result = DeleteLeadingZeros(result);
            if (n1.Sign * n2.Sign < 0)
                return new BigInt(result.ToArray(), -1);
            return new BigInt(result.ToArray(), 1);
        }


        public static BigInt operator %(BigInt n1, BigInt n2)
        {
            if (n2 == new BigInt(0))
                throw new DivideByZeroException();
            var n1Abs = Abs(n1);
            var n2Abs = Abs(n2);
            var remainderAbs = n1Abs - n1Abs / n2Abs * n2Abs;
            if (n1.Sign * n2.Sign < 0 && remainderAbs != new BigInt(0))
                return -remainderAbs;
            return remainderAbs;
        }

        public static BigInt operator -(BigInt n1)
        {
            return new(n1.Digits, (sbyte) -n1.Sign);
        }

        public static BigInt GetInverseElementModulo(BigInt n1, BigInt n2)
        {
            if (n1 < new BigInt(1) || n2 <= new BigInt(1))
                throw new Exception("n1 must be >=1, n2 must be >1");
            var gcd = Gcd(n1, n2, out var x, out var y);
            if (Abs(gcd) != new BigInt(1))
                throw new Exception("Numbers must be simple");
            return (x % n2 + n2) % n2;
        }

        public static BigInt Gcd(BigInt n1, BigInt n2, out BigInt x, out BigInt y)
        {
            if (n1.Sign < 0 || n2.Sign < 0)
                throw new Exception("Numbers must be positive");
            if (n1 == new BigInt(0))
            {
                x = new BigInt(0);
                y = new BigInt(1);
                return n2;
            }

            var d = Gcd(n2 % n1, n1, out var x1, out var y1);
            x = y1 - n2 / n1 * x1;
            y = x1;
            return d;
        }

        public static bool operator >(BigInt n1, BigInt n2)
        {
            if (n1.Sign == n2.Sign)
            {
                ConvertToSameLength(n1, n2, out var n1Digits, out var n2Digits);
                for (var i = 0; i < n1Digits.Count; i++)
                {
                    if (n1Digits[i] * n1.Sign > n2Digits[i] * n2.Sign)
                        return true;
                    if (n1Digits[i] * n1.Sign < n2Digits[i] * n2.Sign)
                        return false;
                }

                return false;
            }

            return n1.Sign > n2.Sign;
        }

        public static bool operator <(BigInt n1, BigInt n2)
        {
            return n1 != n2 && !(n1 > n2);
        }

        public static bool operator ==(BigInt n1, BigInt n2)
        {
            return n1.Sign == n2.Sign && n1.Digits.SequenceEqual(n2.Digits);
        }

        public static bool operator !=(BigInt n1, BigInt n2)
        {
            return !(n1 == n2);
        }

        public static bool operator <=(BigInt n1, BigInt n2)
        {
            return n1 < n2 || n1 == n2;
        }

        public static bool operator >=(BigInt n1, BigInt n2)
        {
            return n1 > n2 || n1 == n2;
        }


        public static BigInt Pow(BigInt n1, BigInt power)
        {
            if (power == new BigInt(0))
                return new BigInt(1);
            if (power % new BigInt(2) == new BigInt(1))
                return Pow(n1, power-new BigInt(1)) * n1;
            var b = Pow(n1, power/new BigInt(2));
            return b * b;
        }

        public static bool IsItSimple(BigInt n1)
        {
            var i = new BigInt(2);
            while (i * i <= n1)
            {
                if (n1 % i == new BigInt(0))
                    return false;
                i += new BigInt(1);
            }

            return true;
        }

        public static int TryToInt(BigInt n1)
        {
            var power = 1;
            var result = 0;
            for (var i = n1.Digits.Length - 1; i >= 0; i--)
            {
                result += n1.Digits[i] * power;
                power *= 10;
            }

            return result;
        }
        
        public static BigInt ModPow(BigInt n1, BigInt power, BigInt mod) {
           if (power == new BigInt(0)) return new BigInt(1); 
           if (power % new BigInt(2) == new BigInt(1)) return (n1 * ModPow(n1, power-new BigInt(1), mod)) % mod; 
           var n2 = ModPow(n1, power/new BigInt(2), mod);
           return (n2 * n2) % mod;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var digit in Digits)
            {
                sb.Append(digit.ToString());
            }

            if (Sign < 0)
                sb.Insert(0, "-");
            return sb.ToString();
        }

        private static List<byte> DeleteLeadingZeros(List<byte> number)
        {
            while (number[0] == 0 && number.Count > 1)
                number.RemoveAt(0);
            return number;
        }

        private static void ConvertToSameLength(BigInt n1, BigInt n2, out List<byte> n1Digits, out List<byte> n2Digits)
        {
            n1Digits = n1.Digits.ToList();
            n2Digits = n2.Digits.ToList();
            for (var i = 0; i < Math.Abs(n1.Digits.Length - n2.Digits.Length); i++)
                if (n1Digits.Count < n2Digits.Count)
                    n1Digits.Insert(0, 0);
                else
                    n2Digits.Insert(0, 0);
        }

        private static BigInt MultOn10inPower(BigInt n1, int power)
        {
            var result = n1.Digits.ToList();
            for (var i = 0; i < power; i++) result.Add(0);

            return new BigInt(result.ToArray(), n1.Sign);
        }

        private static BigInt MultOnDigit(BigInt n1, byte digit)
        {
            var mult = new BigInt(0);
            for (var i = 0; i < digit; i++) mult += n1;

            return mult;
        }

        private static Tuple<byte, BigInt> SmallDivision(BigInt divisible, BigInt divisor)
        {
            var result = 0;
            var remainder = new BigInt(divisible.Digits, 1);
            while (remainder >= divisor)
            {
                remainder = remainder - divisor;
                result++;
            }

            return Tuple.Create((byte) result, remainder);
        }

        private static BigInt Abs(BigInt n1)
        {
            if (n1.Sign < 0)
                return -n1;
            return n1;
        }
    }
}