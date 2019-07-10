using System.Threading.Tasks;
using NUnit.Framework;
using SpikeLanguageExt;

namespace SpikeLanguageExtTests
{

    [TestFixture]
    public class ParallelTests
    {
        private ParallelServices parallel;

        [SetUp]
        public void SetUp()
        {
            parallel = new ParallelServices();
        }

        [Test]
            [Ignore("")]

        public async Task ShouldReturnErrorOne()
        {
            var result = await parallel
                            .DoWork("1");
            Assert.AreEqual(1, result.Error);
        }

        [Test]
            [Ignore("")]

        public async Task ShouldReturnErrorTwo()
        {
            var result = await parallel
                            .DoWork("2");
            Assert.AreEqual(2, result.Error);
        }

        [Test]
        public async Task ShouldReturnValue()
        {
            var result = await parallel
                            .DoWork("test");
            Assert.AreEqual("ServiceOne-testServiceTwo-test", result.Content);
        }

        [Test]
        public async Task ShouldReturnValue1()
        {
            var result = await parallel
                            .DoWorkAsync("test");
            Assert.AreEqual("ServiceOne-testServiceTwo-test", result.Content);
        }
    }
}
