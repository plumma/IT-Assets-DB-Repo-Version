using System.Collections.Generic;
using System;
using ITAssetsDatabase.BusinessDomain;


namespace ITAssetsDatabase.DataAccess.Repositorys.KnowledgeBase

{

    public class ArticleTypeRepository : IArticleTypeRepository, IDisposable
    {
        private ITAssetsDatabaseDBContext db = new ITAssetsDatabaseDBContext();


        // Get Article Type List

        public IEnumerable<ArticleType> GetArticleTypes()
        { 
            return (db.ArticleTypes);
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
