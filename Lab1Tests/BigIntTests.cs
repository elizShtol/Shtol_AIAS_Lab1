using System;
using FluentAssertions;
using Lab1;
using NUnit.Framework;

namespace Lab1Tests
{
    public class BigIntTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("0", "0", "0", TestName = "Zeros")]
        [TestCase("11111111111111111", "0", "11111111111111111", TestName = "PositiveAndZero")]
        [TestCase("-11111111111111111", "0", "-11111111111111111", TestName = "NegativeAndZero")]
        [TestCase("11111111111111111", "22222222222222222", "33333333333333333",
            TestName = "PositiveNum_SameLength_WithoutDischargeTransition")]
        [TestCase("11111111111111111", "222222222222222", "11333333333333333",
            TestName = "PositiveNum_DifferentLength_WithoutDischargeTransition")]
        [TestCase("111111111111", "222222222999", "333333334110",
            TestName = "PositiveNum_SameLength_WithDischargeTransition")]
        [TestCase("111111111111", "22222999", "111133334110",
            TestName = "PositiveNum_DifferentLength_WithDischargeTransition")]
        [TestCase("-11111111111111111", "-22222222222222222", "-33333333333333333",
            TestName = "NegativeNum_SameLength_WithoutDischargeTransition")]
        [TestCase("-11111111111111111", "-2222222222222222", "-13333333333333333",
            TestName = "NegativeNum_DifferentLength_WithoutDischargeTransition")]
        [TestCase("-111111111111", "-222222222999", "-333333334110",
            TestName = "NegativeNum_SameLength_WithDischargeTransition")]
        [TestCase("-111111111111", "-22222999", "-111133334110",
            TestName = "NegativeNum_DifferentLength_WithDischargeTransition")]
        [TestCase("-111111111111", "111111111111", "0", TestName = "PosAndNegNum_SameAbs")]
        [TestCase("222222222222", "-111111111111", "111111111111",
            TestName = "AbsPosMoreThenNeg_WithoutDischargeTransition")]
        [TestCase("-222222222222", "111111111111", "-111111111111",
            TestName = "AbsPosLessThenNeg_WithoutDischargeTransition")]
        [TestCase("222222222222", "-111111111444", "111111110778",
            TestName = "AbsPosMoreThenNeg_WithDischargeTransition")]
        [TestCase("-222222222222", "111111111444", "-111111110778",
            TestName = "AbsPosLessThenNeg_WithDischargeTransition")]
        public static void Sum(string n1, string n2, string expected)
        {
            var actualBigInt = new BigInt(n1) + new BigInt(n2);
            var expectedBigInt = new BigInt(expected);
            Assert.That(actualBigInt == expectedBigInt);
        }

        [TestCase("0", "0", "0", TestName = "Zeros")]
        [TestCase("11111111111111111", "0", "11111111111111111", TestName = "PositiveAndZero")]
        [TestCase("-11111111111111111", "0", "-11111111111111111", TestName = "NegativeAndZero")]
        [TestCase("22222222222222222", "11111111111111111", "11111111111111111",
            TestName = "PositiveNum_SameLength_WithoutDischargeTransition")]
        [TestCase("11111111111111111", "11111111", "11111111100000000",
            TestName = "PositiveNum_DifferentLength_WithoutDischargeTransition")]
        [TestCase("111111111111", "222222222999", "-111111111888",
            TestName = "PositiveNum_SameLength_WithDischargeTransition")]
        [TestCase("111111111111", "22222999", "111088888112",
            TestName = "PositiveNum_DifferentLength_WithDischargeTransition")]
        [TestCase("-22222222222222222", "-11111111111111111", "-11111111111111111",
            TestName = "NegativeNum_SameLength_WithoutDischargeTransition")]
        [TestCase("-22222222222222222", "-111111111", "-22222222111111111",
            TestName = "NegativeNum_DifferentLength_WithoutDischargeTransition")]
        [TestCase("-111111111111", "-222222222999", "111111111888",
            TestName = "NegativeNum_SameLength_WithDischargeTransition")]
        [TestCase("-111111111111", "-22222999", "-111088888112",
            TestName = "NegativeNum_DifferentLength_WithDischargeTransition")]
        [TestCase("111111111111", "-111111111111", "222222222222",
            TestName = "PosAndNegNum_WithoutDischargeTransition")]
        [TestCase("-222222222222", "111111111111", "-333333333333",
            TestName = "NegAndPosNum_WithoutDischargeTransition")]
        [TestCase("111111111111", "-111111119999", "222222231110", TestName = "PosAndNegNum_WithDischargeTransition")]
        [TestCase("-222222222222", "111111119999", "-333333342221", TestName = "NegAndPosNum_WithDischargeTransition")]
        public static void Difference(string n1, string n2, string expected)
        {
            var actualBigInt = new BigInt(n1) - new BigInt(n2);
            var expectedBigInt = new BigInt(expected);
            Assert.That(actualBigInt == expectedBigInt);
        }

        [TestCase("0", "0", true, TestName = "Zeros")]
        [TestCase("12345678990", "0", false, TestName = "PosAndZero")]
        [TestCase("-12345678990", "0", false, TestName = "NegAndZero")]
        [TestCase("1234567891234", "2234567891234", false, TestName = "NotEqualPositiveNum")]
        [TestCase("-1234567891234", "-2234567891234", false, TestName = "NotEqualNegativeNum")]
        [TestCase("-1234567891234", "2234567891234", false, TestName = "NotEqualPosAndNegNum")]
        [TestCase("-1234567891234", "-1234567891234", true, TestName = "EqualNegativeNum")]
        [TestCase("1234567891234", "1234567891234", true, TestName = "EqualPositiveNum")]
        [TestCase("1234567891234", "-1234567891234", false, TestName = "NotEqualPosAndNegNum_WithEqualAbs")]
        public static void OperatorEqual(string n1, string n2, bool expected)
        {
            var actual = new BigInt(n1) == new BigInt(n2);
            actual.Should().Be(expected);
        }

        [TestCase("123456789", "123456789", false, TestName = "EqualPositive")]
        [TestCase("-123456789", "-123456789", false, TestName = "EqualNegative")]
        [TestCase("-123456789", "123456789", false, TestName = "PosAndNeg_WithEqualAbs")]
        [TestCase("-123456789", "-123456790", true, TestName = "NegMoreNeg")]
        [TestCase("-123456789", "-123456788", false, TestName = "NegLessNeg")]
        [TestCase("123456789", "123456788", true, TestName = "PosMorePos")]
        [TestCase("123456788", "123456789", false, TestName = "PosLessPos")]
        [TestCase("123456789", "-1234", true, TestName = "PosAndNeg")]
        [TestCase("-123456789", "1234", false, TestName = "NegAndPos")]
        [TestCase("0", "-123456789", true, TestName = "ZeroAndNeg")]
        [TestCase("0", "123456789", false, TestName = "ZeroAndPos")]
        public static void OperatorMore(string n1, string n2, bool expected)
        {
            var actual = new BigInt(n1) > new BigInt(n2);
            actual.Should().Be(expected);
        }

        [TestCase("123456789", "123456789", true, TestName = "EqualPositive")]
        [TestCase("-123456789", "-123456789", true, TestName = "EqualNegative")]
        [TestCase("-123456789", "123456789", false, TestName = "PosAndNeg_WithEqualAbs")]
        [TestCase("-123456789", "-123456790", true, TestName = "NegMoreNeg")]
        [TestCase("-123456789", "-123456788", false, TestName = "NegLessNeg")]
        [TestCase("123456789", "123456788", true, TestName = "PosMorePos")]
        [TestCase("123456788", "123456789", false, TestName = "PosLessPos")]
        [TestCase("123456789", "-1234", true, TestName = "PosAndNeg")]
        [TestCase("-123456789", "1234", false, TestName = "NegAndPos")]
        [TestCase("0", "-123456789", true, TestName = "ZeroAndNeg")]
        [TestCase("0", "123456789", false, TestName = "ZeroAndPos")]
        public static void OperatorMoreOrEqual(string n1, string n2, bool expected)
        {
            var actual = new BigInt(n1) >= new BigInt(n2);
            actual.Should().Be(expected);
        }

        [TestCase("123456789", "123456789", false, TestName = "EqualPositive")]
        [TestCase("-123456789", "-123456789", false, TestName = "EqualNegative")]
        [TestCase("-123456789", "123456789", true, TestName = "PosAndNeg_WithEqualAbs")]
        [TestCase("-123456789", "-123456790", false, TestName = "NegMoreNeg")]
        [TestCase("-123456789", "-123456788", true, TestName = "NegLessNeg")]
        [TestCase("123456789", "123456788", false, TestName = "PosMorePos")]
        [TestCase("123456788", "123456789", true, TestName = "PosLessPos")]
        [TestCase("123456789", "-1234", false, TestName = "PosAndNeg")]
        [TestCase("-123456789", "1234", true, TestName = "NegAndPos")]
        [TestCase("0", "-123456789", false, TestName = "ZeroAndNeg")]
        [TestCase("0", "123456789", true, TestName = "ZeroAndPos")]
        public static void OperatorLess(string n1, string n2, bool expected)
        {
            var actual = new BigInt(n1) < new BigInt(n2);
            actual.Should().Be(expected);
        }

        [TestCase("123456789", "123456789", true, TestName = "EqualPositive")]
        [TestCase("-123456789", "-123456789", true, TestName = "EqualNegative")]
        [TestCase("-123456789", "123456789", true, TestName = "PosAndNeg_WithEqualAbs")]
        [TestCase("-123456789", "-123456790", false, TestName = "NegMoreNeg")]
        [TestCase("-123456789", "-123456788", true, TestName = "NegLessNeg")]
        [TestCase("123456789", "123456788", false, TestName = "PosMorePos")]
        [TestCase("123456788", "123456789", true, TestName = "PosLessPos")]
        [TestCase("123456789", "-1234", false, TestName = "PosAndNeg")]
        [TestCase("-123456789", "1234", true, TestName = "NegAndPos")]
        [TestCase("0", "-123456789", false, TestName = "ZeroAndNeg")]
        [TestCase("0", "123456789", true, TestName = "ZeroAndPos")]
        public static void OperatorLessOrEqual(string n1, string n2, bool expected)
        {
            var actual = new BigInt(n1) <= new BigInt(n2);
            actual.Should().Be(expected);
        }

        [TestCase("0", "0", "0", TestName = "Zeros")]
        [TestCase("12345678", "0", "0", TestName = "PosAndZero")]
        [TestCase("-12345678", "0", "0", TestName = "NegAndZero")]
        [TestCase("12345678", "1", "12345678", TestName = "PosAndOne")]
        [TestCase("-12345678", "1", "-12345678", TestName = "NegAndOne")]
        [TestCase("12345678", "12345678", "152415765279684", TestName = "Positive")]
        [TestCase("-12345678", "-12345678", "152415765279684", TestName = "Negative")]
        [TestCase("-12345678", "12345678", "-152415765279684", TestName = "PosAndNeg")]
        public static void Multiplication(string n1, string n2, string expected)
        {
            var actual = new BigInt(n1) * new BigInt(n2);
            var expectedBigInt = new BigInt(expected);
            Assert.That(actual == expectedBigInt);
        }

        [TestCase("0", "123456789123", "0", TestName = "ZeroAndPos")]
        [TestCase("0", "-123456789123", "0", TestName = "ZeroAndNeg")]
        [TestCase("123456789123", "1", "123456789123", TestName = "PosAndOne")]
        [TestCase("-123456789123", "1", "-123456789123", TestName = "NegAndOne")]
        [TestCase("123456789123", "123456789123", "1", TestName = "PositiveEqualAbs")]
        [TestCase("123456789123", "2", "61728394561", TestName = "Positive")]
        [TestCase("-123456789123", "-123456789123", "1", TestName = "NegativeEqualAbs")]
        [TestCase("-123456789123", "123456789123", "-1", TestName = "PositiveAndNegativeEqualAbs")]
        [TestCase("-123456789123", "2", "-61728394561", TestName = "PosAndNeg")]
        [TestCase("-123456789123", "-2", "61728394561", TestName = "NegAndNeg")]
        public static void Division(string n1, string n2, string expected)
        {
            var actual = new BigInt(n1) / new BigInt(n2);
            var expectedBigInt = new BigInt(expected);
            Assert.That(actual == expectedBigInt);
        }

        [TestCase("0", "0", TestName = "Zeros")]
        [TestCase("123456789", "0", TestName = "PosAndZero")]
        [TestCase("-123456789", "0", TestName = "NegAndZero")]
        public static void Division_ShouldThrowException(string n1, string n2)
        {
            Assert.Throws<DivideByZeroException>(() =>
            {
                var bigInt = new BigInt(n1) / new BigInt(n2);
            });
        }

        [TestCase("0", "123456789", "0", TestName = "ZeroAndPos")]
        [TestCase("0", "-123456789", "0", TestName = "ZeroAndNeg")]
        [TestCase("123456789", "123456789", "0", TestName = "PosWithEqualAbs")]
        [TestCase("-123456789", "-123456789", "0", TestName = "NegWithEqualAbs")]
        [TestCase("-123456789", "123456789", "0", TestName = "NegAndPosWithEqualAbs")]
        [TestCase("123456789", "1", "0", TestName = "PosWithOne")]
        [TestCase("-123456789", "1", "0", TestName = "NegWithOne")]
        [TestCase("123456789", "2", "1", TestName = "PosAndLittlePos")]
        [TestCase("123456789", "123456788", "1", TestName = "PosAndPos")]
        [TestCase("-123456789", "123456788", "-1", TestName = "NegAndPos")]
        [TestCase("-123456789", "-2", "1", TestName = "NegAndNeg")]
        public static void RemainderFromDivision(string n1, string n2, string expected)
        {
            var actual = new BigInt(n1) % new BigInt(n2);
            var expectedBigInt = new BigInt(expected);
            var a = -15 % 14;
            var b = new BigInt("61728394") * new BigInt("2");
            Assert.That(actual == expectedBigInt);
        }

        [TestCase("0", "0", TestName = "Zeros")]
        [TestCase("123456789", "0", TestName = "PosAndZero")]
        [TestCase("-123456789", "0", TestName = "NegAndZero")]
        public static void RemainderFromDivision_ShouldThrowException(string n1, string n2)
        {
            Assert.Throws<DivideByZeroException>(() =>
            {
                var bigInt = new BigInt(n1) / new BigInt(n2);
            });
        }

        [TestCase("123456789", "19")]
        [TestCase("123456789", "23")]
        [TestCase("1111111111111111111", "11")]
        [TestCase("1", "123456789")]
        public static void GetInverseElementModulo(string n1, string n2)
        {
            var n1BigInt = new BigInt(n1);
            var n2BigInt = new BigInt(n2);
            var actual = BigInt.GetInverseElementModulo(n1BigInt, n2BigInt);
            Assert.That(actual * n1BigInt % n2BigInt == new BigInt("1"));
        }

        [TestCase("0", "0", TestName = "Zeros")]
        [TestCase("123456789", "0", TestName = "PosAndZero")]
        [TestCase("0", "123456789", TestName = "ZeroAndPos")]
        [TestCase("123456789", "-123456789", TestName = "PosAndNeg")]
        [TestCase("123456789", "1", TestName = "PosAndOne")]
        [TestCase("-123456789", "-123456789", TestName = "NegAndNeg")]
        public static void GetInverseElementModulo_ShouldThrowException(string n1, string n2)
        {
            Assert.Throws<Exception>(() =>
            {
                var bigInt = BigInt.GetInverseElementModulo(new BigInt(n1), new BigInt(n2));
            });
        }
    }
}