using System;
using LanguageExt.Core;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Snippets
{
    public interface IApiClient
    {
        Catalog Get();
    }

    public interface ICache
    {
        Catalog Get();
        void Set(Catalog catalog);
    }
    
    public interface ICatalogCache
    {
        Option<Catalog> Get();
        void Set(Catalog catalog);
    }

    public interface ILogger
    {
        void Log(string message);
    }

    public class Catalog
    {
        public Product[] Products { get; set; }
    }

    public class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }        
    }

    public class CatalogService
    {
        private readonly IApiClient apiClient;
        private readonly ICache cache;
        private readonly ILogger logger;

        public CatalogService(IApiClient apiClient,
                              ICache cache,
                              ILogger logger)
        {
            this.apiClient = apiClient;
            this.cache = cache;
            this.logger = logger;
        }

        public Catalog Get()
        {
            try
            {
                var catalog = cache.Get();
                if (catalog == null)
                {
                    catalog = apiClient.Get();
                    cache.Set(catalog);
                }
                return catalog;
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
                return null;
            }
        }
    }

    public interface ICatalogService
    {
        Either<string, Catalog> Get();
    }

    public class LoggedCatalogService : ICatalogService
    {
        private readonly ILogger logger;
        private readonly ICatalogService catalog;

        public LoggedCatalogService(ICatalogService catalog,
                                    ILogger logger)
        {
            this.catalog = catalog;
            this.logger = logger;
        }

        public Either<string, Catalog> Get()
        {
            var result = catalog.Get();
            result.IfLeft(_ => logger.Log(_));
            return result;
        }
    }

    public class CachedCatalogService : ICatalogService
    {
        private readonly ICatalogCache cache;
        private readonly ICatalogService catalog;

        public CachedCatalogService(ICatalogService catalog,
                                    ICatalogCache cache)
        {
            this.catalog = catalog;
            this.cache = cache;
        }

        public Either<string, Catalog> Get()
        {
            return 
                Try(
                    () => cache.Get()
                            .Match(
                                _ => _,
                                () => catalog.Get()
                                        .Match(
                                            _ => 
                                            {
                                                cache.Set(_);
                                                return _;
                                            },
                                            _ => (Either<string, Catalog>)_                                                                             
                                        )
                            )
                )
                .Match(
                    _ => (Either<string, Catalog>)_,
                    ex => (Either<string, Catalog>)ex.Message
                );
        }
    }

    public class RealCatalogService : ICatalogService
    {
        private readonly IApiClient apiClient;

        public RealCatalogService(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public Either<string, Catalog> Get()
        {
            return 
                Try(
                    () => apiClient.Get()
                )
                .Match(
                    _ => (Either<string, Catalog>)_,
                    ex => (Either<string, Catalog>)ex.Message
                );
        }
    }
}