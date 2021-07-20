using Moq;
using NUnit.Framework;
using Railway;

namespace RailwayTests
{
    [TestFixture]
    public class RailwayImperativeTests
    {        
        private RailwayImperative sut;
        private Mock<ICatalogApi> catalogApi;
        private Mock<IProductApi> productApi;

        [SetUp]
        public void SetUp()
        {
            catalogApi = new Mock<ICatalogApi>();
            productApi = new Mock<IProductApi>();
            sut = new RailwayImperative(catalogApi.Object,
                              productApi.Object,
                              new ProductConverter());
        }

        [Test]
        public void GetProducts_WhenNoCatalog_EmptyArray()
        {
            catalogApi
                .Setup(m => m.Get())
                .Returns((Catalog)null);
            
            var result =  sut.GetProducts();

            Assert.IsEmpty(result);
        }

        [Test]
        public void GetProducts_WhenNoProduct_EmptyArray()
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

            Assert.IsEmpty(result);
        }

        [Test]
        public void GetProducts_WhenProduct_ReturnConverted()
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
                .Returns(new Product{ Id = "1", Name = "Name1" });
            productApi
                .Setup(m => m.Get("2"))
                .Returns(new Product{ Id = "2", Name = "Name2" });
            
            var result =  sut.GetProducts();

            Assert.AreEqual(2, result.Length());
            Assert.AreEqual("1 - Name1", result[0].Identifier);
            Assert.AreEqual("2 - Name2", result[1].Identifier);
        }
    }
}