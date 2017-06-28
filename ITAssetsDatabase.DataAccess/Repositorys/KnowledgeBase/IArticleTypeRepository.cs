using ITAssetsDatabase.BusinessDomain;
using System;
using System.Collections.Generic;

namespace ITAssetsDatabase.DataAccess.Repositorys.KnowledgeBase
{
    public interface IArticleTypeRepository : IDisposable
    {

        IEnumerable<ArticleType> GetArticleTypes();
    }
}
