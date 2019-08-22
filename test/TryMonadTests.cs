using NUnit.Framework;
using SpikeLanguageExt;

namespace SpikeLanguageExtTests
{
    [TestFixture]
    public class TryMonadTests
    {
        private TryMonad sut;

        [SetUp]
        public void SetUp()
        {
            sut = new TryMonad();
        }

        [Test]
        public void ShouldGetInteger()
        {
            sut
                .DoTry("10")
                .Match(_ => Assert.AreEqual(10, _),
                       __ => Assert.IsTrue(false) );
        }

        [Test]
        public void ShouldNotGetInteger()
        {
            sut
                .DoTry("AA")
                .Match(_ => Assert.IsTrue(false),
                       __ => Assert.IsTrue(true) );
        }   
    }

}