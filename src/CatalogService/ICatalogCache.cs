using LanguageExt;

namespace Snippets
{
    public interface ICatalogCache
    {
        Option<Catalog> Get();
        void Set(Catalog catalog);
    }
}