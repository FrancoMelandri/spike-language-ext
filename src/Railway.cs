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

    public interface IProductConverter
    {
        ProductView Convert(Product product);
    }

    public interface ICatalogApiFunctional
    {
        Either<string, Catalog> Get();
    }

    public interface IProductApiFunctional
    {
        Either<string, Product> Get(string id);
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
        ICatalogApi _catalogApi;
        IProductApi _productApi;
        IProductConverter _productConverter;

        public Railway(ICatalogApi catalogApi,
                       IProductApi productApi,
                       IProductConverter productConverter)
        {
            _catalogApi = catalogApi;
            _productApi = productApi;
            _productConverter = productConverter;
        }

        public ProductView[] GetProducts()
        {
            List<ProductView> products = new List<ProductView>();

            var catalog = _catalogApi.Get();
            if (catalog == null)
                return products.ToArray();

            foreach (string id in catalog.ProductList)
            {
                var product = _productApi.Get(id);
                if (product != null)
                    products.Add(_productConverter.Convert(product));
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
                .Map(GetProductDetails)
                .Match(FilterEmptyProduct,
                       EmptyProduct)
                .ToArray();


        private IEnumerable<Option<ProductView>> GetProductDetails(Catalog catalog)
            => catalog
                    .ProductList
                    .Map(GetProductAndConvert);        

        private Option<ProductView> GetProductAndConvert(string product)
            => _productApi
                    .Get(product)
                    .Map(_productConverter.Convert)
                    .ToOption();

        private IEnumerable<ProductView> FilterEmptyProduct(IEnumerable<Option<ProductView>> productView)
            => productView
                    .Filter(op => op.IsSome)
                    .Map(op => op.IfNone(() => default));

        private IEnumerable<ProductView> EmptyProduct(string err)
            => Enumerable.Empty<ProductView>();
    }
}