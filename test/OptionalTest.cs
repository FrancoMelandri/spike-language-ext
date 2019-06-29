using NUnit.Framework;
using Moq;

namespace SpikeLanguageExtTest {

    [TestFixture]
    public class OptionalTest {
        
        [SetUp]    
        public void Setup () {
        }

        [Test]
        public void ShouldBeGreen() {
            Assert.AreEqual (true, false);
        }
    }
}