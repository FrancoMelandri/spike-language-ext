using Moq;
using NUnit.Framework;
using Snippets;
using LanguageExt;
using System;

namespace SpikeLanguageExtTests
{
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
        public void Get_WhenHitCache_ReturnCatalog()
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
            result.IfRight(catalog => 
                    Assert.AreEqual(2, catalog.Products.Length));
        }

        [Test]
        public void Get_WhenDontHitCache_AndGetFromApi_SaveInCache_ReturnCatalog()
        { 
            catalogCache
                .Setup(m => m.Get())
                .Returns(Option<Catalog>.None);
            catalogService
                .Setup(m => m.Get())
                .Returns(catalog);

            var result = sut.Get();

            catalogCache
                .Verify(m => m.Set(catalog), Times.Once);
            catalogService
                .Verify(m => m.Get(), Times.Once);
            Assert.IsTrue(result.IsRight);
            result.IfRight(catalog =>
                    Assert.AreEqual(2, catalog.Products.Length));
        }

        [Test]
        public void Get_WhenDontHitCache_AndNotGetFromApi_ReturnError()
        { 
            catalogCache
                .Setup(m => m.Get())
                .Returns(Option<Catalog>.None);
            catalogService
                .Setup(m => m.Get())
                .Returns("error");

            var result = sut.Get();

            catalogCache
                .Verify(m => m.Set(It.IsAny<Catalog>()), Times.Never);
            catalogService
                .Verify(m => m.Get(), Times.Once);
            Assert.IsTrue(result.IsLeft);
            result.IfLeft(err => Assert.AreEqual("error", err));
        }

        [Test]
        public void Get_WhenException_ReturnError()
        { 
            catalogCache
                .Setup(m => m.Get())
                .Throws(new Exception("error"));

            var result = sut.Get();

            Assert.IsTrue(result.IsLeft);
            result.IfLeft(err => Assert.AreEqual("error", err));
        }
    }
}