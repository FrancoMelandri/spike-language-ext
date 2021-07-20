namespace CatalogService
{
    public interface ICache
    {
        Catalog Get();
        void Set(Catalog catalog);
    }
}