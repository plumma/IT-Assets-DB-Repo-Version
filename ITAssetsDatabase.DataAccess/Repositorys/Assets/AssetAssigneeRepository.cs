using ITAssetsDatabase.BusinessDomain;
using System;


namespace ITAssetsDatabase.DataAccess.Repositorys.Assets
{
    public class AssetAssigneeRepository : IAssetAssigneeRepository, IDisposable
    {
        private ITAssetsDatabaseDBContext db = new ITAssetsDatabaseDBContext();

        // Add new record to the AssetAssignee entity

        public int AddNewAssetAssigneedRecord(AssetAssignee newAssetAssignee)

        {
            db.AssetAssignees.Add(newAssetAssignee);
            db.SaveChanges();

            return newAssetAssignee.Id;

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
