using System.Collections.Generic;

namespace Railway
{
    public class RailwayImperative
    {
        ICatalogApi _catalogApi;
        IProductApi _productApi;
        IProductConverter _productConverter;

        public RailwayImperative(ICatalogApi catalogApi,
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
}