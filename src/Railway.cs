using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace SecondSnippet
{
    public class Catalog
    {
        public string[] ProductList { get; set; }
    }

    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class ProductView
    {
        public string Identifier { get; set; }
    }

    public interface ICatalogApi
    {
        Catalog Get();
    }

    public interface IProductApi
    {
        Product Get(string id);
    }

    public interface ICatalogApiFunctional
    {
        Either<string, Catalog> Get();
    }

    public interface IProductApiFunctional
    {
        Either<string, Product> Get(string id);
    }

    public interface IProductConverter
    {
        ProductView Convert(Product product);
    }

    public class ProductConverter : IProductConverter
    {
        public ProductView Convert(Product product)
            => new ProductView
            {
                Identifier = $"{product.Id} - {product.Name}"
            };        
    }

    public class Railway
    {
        ICatalogApi catalogApi;
        IProductApi productApi;
        IProductConverter productConverter;

        public Railway(ICatalogApi catalogApi,
                       IProductApi productApi,
                       IProductConverter productConverter)
        {
            this.catalogApi = catalogApi;
            this.productApi = productApi;
            this.productConverter = productConverter;
        }

        public ProductView[] GetProducts()
        {
            List<ProductView> products = new List<ProductView>();

            var catalog = catalogApi.Get();
            if (catalog == null)
                return products.ToArray();

            foreach (string id in catalog.ProductList)
            {
                var product = productApi.Get(id);
                if (product != null)
                    products.Add(productConverter.Convert(product));
            }
            return products.ToArray();
        }
    }

    public class RailwayFunctional
    {
        ICatalogApiFunctional _catalogApi;
        IProductApiFunctional _productApi;
        IProductConverter _productConverter;

        public RailwayFunctional(ICatalogApiFunctional catalogApi,
                                 IProductApiFunctional productApi,
                                 IProductConverter productConverter)
        {
            _catalogApi = catalogApi;
            _productApi = productApi;
            _productConverter = productConverter;
        }

        public ProductView[] GetProducts()
            => _catalogApi
                .Get()
                .Map(catalog => GetProductDetails(catalog))
                .Match(pv => FilterEmptyProduct(pv),
                       err => Enumerable.Empty<ProductView>())
                .ToArray();

        private IEnumerable<Option<ProductView>> GetProductDetails(Catalog catalog)
            => catalog
                    .ProductList
                    .Map(product => GetProductAndConvert(product));        

        private Option<ProductView> GetProductAndConvert(string product)
            => _productApi
                    .Get(product)
                    .Map(p => _productConverter.Convert(p))
                    .Match(p => Option<ProductView>.Some(p),
                           e => Option<ProductView>.None);

        private IEnumerable<ProductView> FilterEmptyProduct(IEnumerable<Option<ProductView>> productView)
            => productView
                    .Filter(op => op.IsSome)
                    .Map(op => op.Some(p => p).None(() => default));
    }
}