using LanguageExt;

namespace CatalogService
{
    public interface ICatalogCache
    {
        Option<Catalog> Get();
        void Set(Catalog catalog);
    }
}