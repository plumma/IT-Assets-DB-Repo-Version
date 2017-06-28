using System.Collections.Generic;
using ITAssetsDatabase.BusinessDomain;
using System.Linq;
using System;



namespace ITAssetsDatabase.DataAccess.Repositorys.KnowledgeBase

{

    public class ArticleRepository : IArticleRepository, IDisposable
    {
        private ITAssetsDatabaseDBContext db = new ITAssetsDatabaseDBContext();


        // Get Article by Id

        public IEnumerable<Article> GetArticleById_select(int articleId)
        { 
            // Find not used as you cannot use Select to specify the fields as per controller actions
            return (db.Articles.Where( x => x.id == articleId));
        }

        public Article GetArticleById(int articleId)
        {
            // Find not used as you cannot use Select to specify the fields as per controller actions
            return (db.Articles.Find(articleId));
        }

        // Search for KnowledgeBase

        public IOrderedQueryable<Article> SearchKnowledgeBase (string SearchString)
        { 

            var q = db.Articles.Where(b => b.Details.Contains(SearchString) || 
                                           b.Summary.Contains(SearchString) ||
                                           b.Application.AppName.Contains(SearchString)).OrderByDescending(d => d.CreatedDate);
            return q;
        }


        // Search Summary field of KnowledgeBase with typre filter

        public IOrderedQueryable<Article> SearchSummaryWithFilter(string term, int? ArticleTypeId)
        { 

            var q = db.Articles.Where(s => s.Summary.Contains(term) && s.ArticleTypeId == ArticleTypeId).OrderByDescending(d => d.CreatedDate);

            return q;
        }


        // Search Summary field of KnowledgeBase

        public IOrderedQueryable<Article> SearchSummary(string term)
        {
             
            var q = db.Articles.Where(s => s.Summary.Contains(term)).OrderByDescending(d => d.CreatedDate);

            return q;
        }

        // Add Article

        public int AddArticle(Article thisArticle) { 

            db.Articles.Add(thisArticle);
            db.SaveChanges();

            return thisArticle.id;
        }

        #region Tidy Up


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


    }
}

