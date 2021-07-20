using Moq;
using NUnit.Framework;
using LanguageExt;
using Railway;

namespace RailwayTests
{
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