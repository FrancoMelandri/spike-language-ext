using Moq;
using NUnit.Framework;
using Snippets;
using System;

namespace SpikeLanguageExtTests
{
    [TestFixture]
    public class CatalogServiceTests
    {
        private CatalogService sut;
        private Mock<IApiClient> apiClient;
        private Catalog catalog;

        [SetUp]
        public void SetUp()
        {
            apiClient = new Mock<IApiClient>();

            sut = new CatalogService(apiClient.Object);                            
        
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
        public void Get_HappyPath_ReturnCatalog()
        { 
            apiClient
                .Setup(m => m.Get())
                .Returns(catalog);

            var result = sut.Get();

            Assert.IsTrue(result.IsRight);
            result.IfRight(catalog => Assert.AreEqual(2, catalog.Products.Length));
        }

        [Test]
        public void Get_WhenException_ReturnError()
        { 
            apiClient
                .Setup(m => m.Get())
                .Throws(new Exception("error"));

            var result = sut.Get();

            Assert.IsTrue(result.IsLeft);
            result.IfLeft(err => Assert.AreEqual("error", err));
        }
    }
}