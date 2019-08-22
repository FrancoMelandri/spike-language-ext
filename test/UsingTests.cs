using System;
using Moq;
using NUnit.Framework;
using static SpikeLanguageExt.UsingExtension;

namespace SpikeLanguageExtTests
{
    public interface ILog
    {
        void Log(string message);
    }

    public class CanBeDispose : IDisposable
    {
        private readonly ILog logger;

        public CanBeDispose(ILog logger)
        {
            this.logger = logger;
        }
        public void Dispose()
        {
            this.logger.Log("dispose");
        }
    }

    [TestFixture]
    public class UsingTests
    {
        [Test]
        public void ShouldCallDisposeInRawMode()
        {
            var called = false;
            Action action = () => called = true;
            var log = new Mock<ILog>();
            var disposable = new CanBeDispose(log.Object);

            Raw(disposable, action);

            Assert.IsTrue(called);
            log
                .Verify(m => m.Log("dispose"), Times.Once);                
        }

        [Test]
        public void ShouldCallDisposeInFunctionalWay()
        {
            var called = false;
            Func<CanBeDispose, bool> action = _ => called = true;
            var log = new Mock<ILog>();
            var disposable = new CanBeDispose(log.Object);

            Using(disposable, action);

            Assert.IsTrue(called);
            log
                .Verify(m => m.Log("dispose"), Times.Once);                
        }
    }
}