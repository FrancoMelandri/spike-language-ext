using Moq;
using NUnit.Framework;
using LanguageExt;
using SecondSnippet;

namespace SpikeLanguageExtTests
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

    [TestFixture]
    public class RailwayFunctionalTests
    {        
        private RailwayFunctional sut;
        private Mock<ICatalogApiFunctional> catalogApi;
        private Mock<IProductApiFunctional> productApi;

        [SetUp]
        public void SetUp()
        {
            catalogApi = new Mock<ICatalogApiFunctional>();
            productApi = new Mock<IProductApiFunctional>();
            sut = new RailwayFunctional(catalogApi.Object,
                                        productApi.Object,
                                        new ProductConverter());
        }

        [Test]
        public void ShouldReturnEmptyIfNoCatalog()
        {
            Either<string, Catalog> catalog = "error";
            catalogApi
                .Setup(m => m.Get())
                .Returns(catalog);
            
            var result =  sut.GetProducts();

            Assert.AreEqual (0, result.Length());
        }

        [Test]
        public void ShouldReturnEmptyIfNoProduct()
        {
            Either<string, Catalog> catalog = new Catalog
            {
                ProductList = new[] { "1", "2" }
            };
            Either<string, Product> product = "error";
            catalogApi
                .Setup(m => m.Get())
                .Returns(catalog);
            productApi
                .Setup(m => m.Get(It.IsAny<string>()))
                .Returns(product);

            var result = sut.GetProducts();

            Assert.AreEqual(0, result.Length());
        }

        [Test]
        public void ShouldReturnProducts()
        {
            Either<string, Catalog> catalog = new Catalog
            {
                ProductList = new[] { "1", "2" }
            };
            Either<string, Product> product = new Product { Id = "3", Name = "Name" };

            catalogApi
                .Setup(m => m.Get())
                .Returns(catalog);

            productApi
                .Setup(m => m.Get(It.IsAny<string>()))
                .Returns(product);

            var result = sut.GetProducts();

            Assert.AreEqual(2, result.Length());
        }
    }

}