using LanguageExt;
using static LanguageExt.Prelude;

namespace CatalogService
{
    public class CachedCatalogService : ICatalogService
    {
        private readonly ICatalogCache _cache;
        private readonly ICatalogService _catalog;

        public CachedCatalogService(ICatalogService catalog,
                                    ICatalogCache cache)
        {
            _catalog = catalog;
            _cache = cache;
        }

        public Either<string, Catalog> Get()
            => Try(() => GetFromCacheOrApi())
                .Match(
                    catalog => catalog,
                    ex => ex.Message);

        private Either<string, Catalog> GetFromCacheOrApi()
            => _cache.Get()
                    .Match(
                        _ => _,
                        () => GetFromApi());

        private Either<string, Catalog> GetFromApi()
            => _catalog
                    .Get()
                    .Map(SaveCatalogInCache);

        private Catalog SaveCatalogInCache(Catalog catalog)
        {
            _cache.Set(catalog);
            return catalog;
        }
    }
}