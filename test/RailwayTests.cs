using Moq;
using NUnit.Framework;
using System;
using LanguageExt;
using static LanguageExt.Prelude;
using SecondSnippet;

namespace test
{
    [TestFixture]
    public class RailwayTests
    {        
        private Railway sut;
        private Mock<ICatalogApi> catalogApi;
        private Mock<IProductApi> productApi;

        [SetUp]
        public void SetUp()
        {
            catalogApi = new Mock<ICatalogApi>();
            productApi = new Mock<IProductApi>();
            sut = new Railway(catalogApi.Object,
                              productApi.Object,
                              new ProductConverter());
        }

        [Test]
        public void ShouldReturnEmptyIfNoCatalog()
        {
            catalogApi
                .Setup(m => m.Get())
                .Returns((Catalog)null);
            

            var result =  sut.GetProducts();

            Assert.AreEqual (0, result.Length());
        }

        [Test]
        public void ShouldReturnEmptyIfNoProduct()
        {
            var catalog = new Catalog 
            {
                ProductList = new [] { "1", "2" }
            };
            catalogApi
                .Setup(m => m.Get())
                .Returns(catalog);
            productApi
                .Setup(m => m.Get(It.IsAny<string>()))
                .Returns((Product)null);
            

            var result =  sut.GetProducts();

            Assert.AreEqual (0, result.Length());
        }

        [Test]
        public void ShouldReturnProducts()
        {
            var catalog = new Catalog 
            {
                ProductList = new [] { "1", "2" }
            };
            catalogApi
                .Setup(m => m.Get())
                .Returns(catalog);
            productApi
                .Setup(m => m.Get("1"))
                .Returns(new Product{ Id = "1", Name = "Name" });
            productApi
                .Setup(m => m.Get("2"))
                .Returns(new Product{ Id = "3", Name = "Name" });
            

            var result =  sut.GetProducts();

            Assert.AreEqual (2, result.Length());
        }
    }
}