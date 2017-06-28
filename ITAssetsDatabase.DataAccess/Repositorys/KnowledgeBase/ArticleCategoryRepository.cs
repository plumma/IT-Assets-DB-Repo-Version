using System.Collections.Generic;
using ITAssetsDatabase.BusinessDomain;
using System;

namespace ITAssetsDatabase.DataAccess.Repositorys.KnowledgeBase

{
    public class ArticleCategoryRepository : IArticleCategoryRepository, IDisposable
    {
        private ITAssetsDatabaseDBContext db = new ITAssetsDatabaseDBContext();

 
        // Get Article Category List

        public IEnumerable<ArticleCategory> GetArticleCategorys()
        { 
            return (db.ArticleCategorys);
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
