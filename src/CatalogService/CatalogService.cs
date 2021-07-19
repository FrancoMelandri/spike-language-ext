using LanguageExt;
using static LanguageExt.Prelude;

namespace Snippets
{
    public class CatalogService : ICatalogService
    {
        private readonly IApiClient _apiClient;

        public CatalogService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public Either<string, Catalog> Get()
            => Try(() => _apiClient.Get())
                .Match(
                    catalog => catalog,
                    ex => (Either<string, Catalog>)ex.Message);   
    }
}