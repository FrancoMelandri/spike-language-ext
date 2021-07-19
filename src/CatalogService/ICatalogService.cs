using LanguageExt;

namespace Snippets
{
    public interface ICatalogService
    {
        Either<string, Catalog> Get();
    }
}