using System.Threading.Tasks;
using NUnit.Framework;
using SpikeLanguageExt;

namespace SpikeLanguageExtTests
{
    [TestFixture]
    public class ParallelTests
    {
        private ParallelServices sut;

        [SetUp]
        public void SetUp()
        {
            sut = new ParallelServices();
        }

        [Test]
        public async Task ShouldReturnErrorOne()
        {
            var result = await sut
                            .DoWork("1");
            Assert.AreEqual(1, result.Error);
        }

        [Test]
        public async Task ShouldReturnErrorTwo()
        {
            var result = await sut
                            .DoWork("2");
            Assert.AreEqual(2, result.Error);
        }

        [Test]
        public async Task ShouldReturnValue()
        {
            var result = await sut
                            .DoWork("test");
            Assert.AreEqual("ServiceOne-testServiceTwo-test", result.Content);
        }

        [Test]
        public async Task ShouldReturnValueAsync()
        {
            var result = await sut
                            .DoWorkAsync("test");
            Assert.AreEqual("ServiceOne-testServiceTwo-test", result.Content);
        }
    }
}
