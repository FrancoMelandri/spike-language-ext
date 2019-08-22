using Moq;
using NUnit.Framework;
using Snippets;
using System;
using LanguageExt;
using static LanguageExt.Prelude;

namespace SnippetsTests
{
    [TestFixture]
    public class SnippetsTests
    {
        private CatalogService sut;
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

            sut = new CatalogService(apiClient.Object,
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
        public void ShouldReturnCached()
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
        public void ShouldReturnFromApiAndCache()
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
        public void ShouldReturnNullIfException()
        { 
            cache
                .Setup(m => m.Get())
                .Throws(new Exception("error"));

            var result = sut.Get();
                
            logger
                .Verify(m => m.Log("error"), Times.Once);

            Assert.AreEqual(null, result);
        }
    }

    [TestFixture]
    public class CatalogSnippetsTests
    {
        private RealCatalogService sut;
        private Mock<IApiClient> apiClient;
        private Catalog catalog;

        [SetUp]
        public void SetUp()
        {
            apiClient = new Mock<IApiClient>();

            sut = new RealCatalogService(apiClient.Object);                            
        
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
        public void ShouldReturnCatalog()
        { 
            apiClient
                .Setup(m => m.Get())
                .Returns(catalog);

            var result = sut.Get();

            Assert.IsTrue(result.IsRight);
        }

        [Test]
        public void ShouldReturnError()
        { 
            apiClient
                .Setup(m => m.Get())
                .Throws(new Exception("error"));

            var result = sut.Get();

            Assert.IsTrue(result.IsLeft);
        }
    }

    [TestFixture]
    public class CachedCatalogServiceTest
    {
        private CachedCatalogService sut;
        private Mock<ICatalogCache> catalogCache;
        private Mock<ICatalogService> catalogService;
        private Catalog catalog;

        [SetUp]
        public void SetUp()
        {
            catalogCache = new Mock<ICatalogCache>();
            catalogService = new Mock<ICatalogService>();

            sut = new CachedCatalogService(catalogService.Object,
                                           catalogCache.Object);                            
        
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
        public void ShouldReturnCatalogFromCache()
        { 
            catalogCache
                .Setup(m => m.Get())
                .Returns(catalog);

            var result = sut.Get();

            catalogCache
                .Verify(m => m.Set(It.IsAny<Catalog>()), Times.Never);
            catalogService
                .Verify(m => m.Get(), Times.Never);
            Assert.IsTrue(result.IsRight);
        }

        [Test]
        public void ShouldReturnCatalogFromApi()
        { 
            Option<Catalog> empty = None;
            catalogCache
                .Setup(m => m.Get())
                .Returns(empty);
            catalogService
                .Setup(m => m.Get())
                .Returns(catalog);

            var result = sut.Get();

            catalogCache
                .Verify(m => m.Set(It.IsAny<Catalog>()), Times.Once);
            catalogService
                .Verify(m => m.Get(), Times.Once);
            Assert.IsTrue(result.IsRight);
        }
    }

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
        public void ShouldReturnCatalogWithoutLog()
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