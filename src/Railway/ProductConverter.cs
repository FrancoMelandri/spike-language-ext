namespace Railway
{
    public class ProductConverter : IProductConverter
    {
        public ProductView Convert(Product product)
            => new ProductView
            {
                Identifier = $"{product.Id} - {product.Name}"
            };        
    }
}