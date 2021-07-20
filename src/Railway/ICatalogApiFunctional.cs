using LanguageExt;

namespace Railway
{
    public interface ICatalogApiFunctional
    {
        Either<string, Catalog> Get();
    }
}