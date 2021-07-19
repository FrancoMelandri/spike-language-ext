using Moq;
using NUnit.Framework;
using Snippets;
using LanguageExt;

namespace SpikeLanguageExtTests
{
    [TestFixture]
    public class LoggedCatalogServiceTest
    {
        private LoggedCatalogService sut;
        private Mock<ICatalogService> catalogService;
        private Mock<ILogger> logger;
        private Catalog catalog;

        [SetUp]
        public void SetUp()
        {
            logger = new Mock<ILogger>();
            catalogService = new Mock<ICatalogService>();

            sut = new LoggedCatalogService(catalogService.Object,
                                           logger.Object);                            
        
            catalog = new Catalog
            {
                Products = new [] 
                {
                    new Product 
                    {
                        Name = "Name1",
                        Description = "Description1"
                    },
                    new Product 
                    {
                        Name = "Name2",
                        Description = "Description2"
                    }
                }
            };
        }

        [Test]
        public void Get_WhenCatalog_NoLog()
        { 
            catalogService
                .Setup(m => m.Get())
                .Returns(catalog);

            var result = sut.Get();

            logger
                .Verify(m => m.Log(It.IsAny<string>()), Times.Never);
            Assert.IsTrue(result.IsRight);
        }

        [Test]
        public void ShouldLogError()
        { 
            Either<string, Catalog> error = "error";

            catalogService
                .Setup(m => m.Get())
                .Returns(error);

            var result = sut.Get();

            logger
                .Verify(m => m.Log("error"), Times.Once);
            Assert.IsTrue(result.IsLeft);
        }
    }
}