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

    public interface ICatalogApiFunctional
    {
        Either<string, Catalog> Get();
    }

    public interface IProductApi
    {
        Product Get(string id);
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
        {
            return new ProductView
            {
                Identifier = $"{product.Id} - {product.Name}"
            };
        }
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
                    products.Add (productConverter.Convert(product));
            }
            return products.ToArray();
        }
    }

    public class RailwayFunctional
    {
        ICatalogApiFunctional catalogApi;
        IProductApiFunctional productApi;
        IProductConverter productConverter;

        public RailwayFunctional(ICatalogApiFunctional catalogApi,
                                 IProductApiFunctional productApi,
                                 IProductConverter productConverter)
        {
            this.catalogApi = catalogApi;
            this.productApi = productApi;
            this.productConverter = productConverter;
        }

        public ProductView[] GetProducts()
        {
            return 
                catalogApi
                .Get()
                .Match(
                     _ => _
                            .ProductList
                            .Map(p => productApi
                                         .Get(p)
                                         .Match(
                                            r => productConverter.Convert(r),
                                            r => new ProductView()
                                          ))
                            .Filter(p => !string.IsNullOrEmpty(p.Identifier))
                            .ToArray(),
                    _ => new ProductView[] { }
                );
        }
    }
}