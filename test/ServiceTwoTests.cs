using System.Threading.Tasks;
using NUnit.Framework;
using SpikeLanguageExt;

namespace RailwayTests
{
    [TestFixture]
    public class ServiceTwoTests
    {
        private ServiceTwo serviceTwo;

        [SetUp]
        public void SetUp()
        {
            serviceTwo = new ServiceTwo();
        }

        [Test]
        public async Task ShouldReturnLeft()
        {
            var result = await serviceTwo
                            .DoWork("2");
            result
                .Match( _ => Assert.IsTrue(false),
                        __ => Assert.IsTrue(true));
        }

        [Test]
        public async Task ShouldReturnRight()
        {
            var result = await serviceTwo
                            .DoWork("test");
            result
                .Match( _ => Assert.IsTrue(true),
                        __ => Assert.IsTrue(false));
        }
    }    
}