using LanguageExt;

namespace Railway
{
    public interface IProductApiFunctional
    {
        Either<string, Product> Get(string id);
    }
}