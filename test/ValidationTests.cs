using System;
using Moq;
using NUnit.Framework;
using static SpikeLanguageExt.UsingExtension;
using SpikeLanguageExt;

namespace SpikeLanguageExtTests
{
    [TestFixture]
    public class ValidationBasicTests
    {
        private ValidationBasic sut;

        [SetUp]
        public void SetUp()
        {
            sut = new ValidationBasic();
        }

        [Test]
        public void WhenAllDataIsValid_ReturEmptyString()
        {
            var result = sut.Validate("aa@aa.com", "aaaa", "1234");
            Assert.AreEqual("", result);
        }

        [Test]
        public void WhenEmailIsNotValid_ReturErrorString()
        {
            var result = sut.Validate("aa.com", "aaaa", "1234");
            Assert.AreEqual("invalid", result);
        }

        [Test]
        public void WhenPasswordIsNotValid_ReturErrorString()
        {
            var result = sut.Validate("aa@aa.com", "aaa", "1234");
            Assert.AreEqual("invalid", result);
        }

        [Test]
        public void WhenCodeIsNotValid_ReturErrorString()
        {
            var result = sut.Validate("aa@aa.com", "aaaa", "abcd");
            Assert.AreEqual("invalid", result);
        }
    }

    [TestFixture]
    public class ValidationBetterTests
    {
        private ValidationBetter sut;

        [SetUp]
        public void SetUp()
        {
            sut = new ValidationBetter();
        }

        [Test]
        public void WhenAllDataIsValid_ReturEmptyString()
        {
            var result = sut.Validate("aa@aa.com", "aaaa", "1234");
            Assert.AreEqual("", result);
        }

        [Test]
        public void WhenEmailIsNotValid_ReturErrorString()
        {
            var result = sut.Validate("aa.com", "aaaa", "1234");
            Assert.AreEqual("invalid mail", result);
        }

        [Test]
        public void WhenPasswordIsNotValid_ReturErrorString()
        {
            var result = sut.Validate("aa@aa.com", "aaa", "1234");
            Assert.AreEqual("invalid password", result);
        }

        [Test]
        public void WhenCodeIsNotValid_ReturErrorString()
        {
            var result = sut.Validate("aa@aa.com", "aaaa", "abcd");
            Assert.AreEqual("invalid code", result);
        }
    }

    [TestFixture]
    public class ValidationChainTests
    {
        private ValidationChain sut;

        [SetUp]
        public void SetUp()
        {
            sut = new ValidationChain();
        }

        [Test]
        public void WhenAllDataIsValid_ReturEmptyString()
        {
            var result = sut.Validate("aa@aa.com", "aaaa", "1234");
            Assert.AreEqual("", result);
        }

        [Test]
        public void WhenEmailIsNotValid_ReturErrorString()
        {
            var result = sut.Validate("aa.com", "aaaa", "1234");
            Assert.AreEqual("invalid mail", result);
        }

        [Test]
        public void WhenPasswordIsNotValid_ReturErrorString()
        {
            var result = sut.Validate("aa@aa.com", "aaa", "1234");
            Assert.AreEqual("invalid password", result);
        }

        [Test]
        public void WhenCodeIsNotValid_ReturErrorString()
        {
            var result = sut.Validate("aa@aa.com", "aaaa", "abcd");
            Assert.AreEqual("invalid code", result);
        }
    }
}