using Moq;
using NUnit.Framework;
using Snippets;
using System;

namespace SpikeLanguageExtTests
{
    [TestFixture]
    public class CatalogServiceImperativeTests
    {
        private CatalogServiceImperative sut;
        private Mock<IApiClient> apiClient;
        private Mock<ICache> cache;
        private Mock<ILogger> logger;
        private Catalog catalog;

        [SetUp]
        public void SetUp()
        {
            apiClient = new Mock<IApiClient>();
            cache = new Mock<ICache>();
            logger = new Mock<ILogger>();

            sut = new CatalogServiceImperative(apiClient.Object,
                                     cache.Object,
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
        public void Get_WhenHitCache_ReturnCatalog()
        { 
            cache
                .Setup(m => m.Get())
                .Returns(catalog);

            var result = sut.Get();

            apiClient
                .Verify (m => m.Get(), Times.Never);

            Assert.AreEqual(result.Products[0].Name, "Name1");
        }

        [Test]
        public void Get_WhenNoCahce_AndGetApi_CacheAdnReturnCatalog()
        { 
            cache
                .Setup(m => m.Get())
                .Returns((Catalog)null);
            apiClient
                .Setup(m => m.Get())
                .Returns(catalog);

            var result = sut.Get();

            cache
                .Verify (m => m.Set(catalog), Times.Once);
                
            Assert.AreEqual(result.Products[0].Name, "Name1");
        }

        [Test]
        public void Get_WhenNoCahce_AndNoGetApi_DontCacheAndReturnNull()
        { 
            cache
                .Setup(m => m.Get())
                .Returns((Catalog)null);
            apiClient
                .Setup(m => m.Get())
                .Returns((Catalog)null);

            var result = sut.Get();

            cache
                .Verify (m => m.Set(catalog), Times.Never);
                
            Assert.IsNull(result);
        }

        [Test]
        public void Get_WhenException_LogAndReturnNull()
        { 
            cache
                .Setup(m => m.Get())
                .Throws(new Exception("error"));

            var result = sut.Get();
                
            logger
                .Verify(m => m.Log("error"), Times.Once);

            Assert.IsNull(result);
        }
    }
}