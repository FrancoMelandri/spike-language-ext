using System;
using Moq;
using NUnit.Framework;
using static SpikeLanguageExt.UsingExtension;
using SpikeLanguageExt;

namespace SpikeLanguageExtTests
{
    [TestFixture]
    public class ValidationTests
    {
        private Validation sut;

        [SetUp]
        public void SetUp()
        {
            sut = new Validation();
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
}