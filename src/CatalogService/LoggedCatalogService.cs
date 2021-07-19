using LanguageExt;

namespace Snippets
{
    public class LoggedCatalogService : ICatalogService
    {
        private readonly ILogger _logger;
        private readonly ICatalogService _catalog;

        public LoggedCatalogService(ICatalogService catalog,
                                    ILogger logger)
        {
            _catalog = catalog;
            _logger = logger;
        }

        public Either<string, Catalog> Get()
        {
            var result = _catalog.Get();
            result.IfLeft(err => _logger.Log(err));
            return result;
        }
    }
}