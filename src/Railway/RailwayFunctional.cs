using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace Railway
{
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