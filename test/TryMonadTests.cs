using NUnit.Framework;
using SpikeLanguageExt;

namespace RailwayTests
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
                       _ => Assert.IsTrue(false) );
        }

        [Test]
        public void ShouldNotGetInteger()
        {
            sut
                .DoTry("AA")
                .Match(_ => Assert.IsTrue(false),
                       _ => Assert.IsTrue(true) );
        }   
    }

}