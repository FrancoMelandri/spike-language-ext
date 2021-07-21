﻿using NUnit.Framework;
using Validation;

namespace ValidationTests
{
    [TestFixture]
    public class ValidationBetterTests
    {
        private ValidationBetter _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new ValidationBetter();
        }

        [Test]
        public void WhenAllDataIsValid_ReturEmptyString()
        {
            var result = _sut.Validate("aa@aa.com", "aaaa", "1234");
            Assert.AreEqual("", result);
        }

        [Test]
        public void WhenEmailIsNotValid_ReturErrorString()
        {
            var result = _sut.Validate("aa.com", "aaaa", "1234");
            Assert.AreEqual("invalid mail", result);
        }

        [Test]
        public void WhenPasswordIsNotValid_ReturErrorString()
        {
            var result = _sut.Validate("aa@aa.com", "aaa", "1234");
            Assert.AreEqual("invalid password", result);
        }

        [Test]
        public void WhenCodeIsNotValid_ReturErrorString()
        {
            var result = _sut.Validate("aa@aa.com", "aaaa", "abcd");
            Assert.AreEqual("invalid code", result);
        }
    }
}