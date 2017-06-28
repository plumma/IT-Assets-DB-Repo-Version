using System.Collections.Generic;
using ITAssetsDatabase.BusinessDomain;
using System.Linq;
using System;

namespace ITAssetsDatabase.DataAccess.Repositorys.KnowledgeBase

{

    public class ApplicationRepository : IApplicationRepository, IDisposable
    {
        private ITAssetsDatabaseDBContext db = new ITAssetsDatabaseDBContext();

         
        // Get Article by Id

        public IQueryable<Application> SearchApplications(string term)
        { 
            return (db.Applications.Where(s => s.AppName.Contains(term)));
        }

        //Get Applications

        public IEnumerable<Application> GetApplications()
        {
            return (db.Applications);

        }

        // Add new Application 

        public void AddApp(Application model) { 

            db.Applications.Add(model);
            db.SaveChanges();
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

