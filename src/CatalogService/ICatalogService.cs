﻿using LanguageExt;

namespace CatalogService
{
    public interface ICatalogService
    {
        Either<string, Catalog> Get();
    }
}