using System.Collections.Generic;
using System.Linq;
using ITAssetsDatabase.BusinessDomain;
using System;

namespace ITAssetsDatabase.DataAccess.Repositorys.Assets
{
    public class BuildsRepository : IBuildsRepository, IDisposable
    {
        private ITAssetsDatabaseDBContext db = new ITAssetsDatabaseDBContext();

        public List<Build> GetBuildList(string term) {

            return (db.Builds.Where(l => l.BuildName.Contains(term) || l.Domain.Contains(term)).ToList());
        }


        public Build GetBuildByID(int BuildId) {

            return (db.Builds.Find(BuildId));
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
