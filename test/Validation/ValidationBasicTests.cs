using NUnit.Framework;
using Validation;

namespace ValidationTests
{
    [TestFixture]
    public class ValidationBasicTests
    {
        private ValidationBasic _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new ValidationBasic();
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
            Assert.AreEqual("invalid", result);
        }

        [Test]
        public void WhenPasswordIsNotValid_ReturErrorString()
        {
            var result = _sut.Validate("aa@aa.com", "aaa", "1234");
            Assert.AreEqual("invalid", result);
        }

        [Test]
        public void WhenCodeIsNotValid_ReturErrorString()
        {
            var result = _sut.Validate("aa@aa.com", "aaaa", "abcd");
            Assert.AreEqual("invalid", result);
        }
    }
}