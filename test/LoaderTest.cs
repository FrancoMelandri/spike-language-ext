using NUnit.Framework;
using SpikeLanguageExt;

namespace SpikeLanguageExtTests
{

    [TestFixture]
    public class LoaderTest {
        
        Loader sut;

        [SetUp]    
        public void Setup () {
            sut = new Loader();
        }

        [Test]
        public void ShouldBeNone() {
            sut.Load()
                .Match( _ => Assert.IsTrue(false),
                        () => Assert.IsTrue(true) );
        }
    
        [Test]
        public void ShouldBeNoneExtended() {
            sut.Load()
                .Some( _ => Assert.IsTrue(false))
                .None(() => Assert.IsTrue(true));
        }
    }
}