using ITAssetsDatabase.BusinessDomain;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ITAssetsDatabase.DataAccess.Repositorys.KnowledgeBase
{
    public interface IArticleRepository : IDisposable
    {
        IEnumerable<Article> GetArticleById_select(int articleId);
        Article GetArticleById(int articleId);
        IOrderedQueryable<Article> SearchKnowledgeBase(string SearchString);
        IOrderedQueryable<Article> SearchSummaryWithFilter(string term, int? ArticleTypeId);
        IOrderedQueryable<Article> SearchSummary(string term);
        int AddArticle(Article thisArticle);
    }
}
