using LanguageExt;
using static LanguageExt.Prelude;

namespace Snippets
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
            => Try(() => _cache
                            .Get()
                            .Match(
                                _ => _,
                                () => _catalog
                                        .Get()
                                        .Match(
                                            _ => 
                                            {
                                                _cache.Set(_);
                                                return _;
                                            },
                                            _ => (Either<string, Catalog>)_                                                                             
                                        )))
                .Match(
                    catalog => catalog,
                    ex => ex.Message
                );
    }
}