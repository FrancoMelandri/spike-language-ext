using System.Threading.Tasks;
using NUnit.Framework;
using SpikeLanguageExt;

namespace SpikeLanguageExtTests
{
    [TestFixture]
    [Ignore("")]
    public class ServiceOneTests
    {
        private ServiceOne serviceOne;

        [SetUp]
        public void SetUp()
        {
            serviceOne = new ServiceOne();
        }

        [Test]
        public async Task ShouldReturnLeft()
        {
            var result = await serviceOne
                            .DoWork("1");
            result
                .Match( _ => Assert.IsTrue(false),
                        __ => Assert.IsTrue(true));
        }

        [Test]
        public async Task ShouldReturnRight()
        {
            var result = await serviceOne
                            .DoWork("test");
            result
                .Match( _ => Assert.IsTrue(true),
                        __ => Assert.IsTrue(false));
        }
    }
}
