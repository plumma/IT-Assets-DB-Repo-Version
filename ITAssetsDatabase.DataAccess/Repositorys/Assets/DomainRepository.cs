using ITAssetsDatabase.BusinessDomain;
using System.Collections.Generic;
using System;


namespace ITAssetsDatabase.DataAccess.Repositorys.Assets
{
    public class DomainRepository : IDomainRepository, IDisposable
    {
        private ITAssetsDatabaseDBContext db = new ITAssetsDatabaseDBContext();

        // Returns to a SelectList (SelectList uses IEnumerable)

        public IEnumerable<Domain> GetDomains() {

            return (db.Domains);
        }


        #region MyRegion
        // Tidy up

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
