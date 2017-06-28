using System.Linq;
using ITAssetsDatabase.BusinessDomain;
using System;

namespace ITAssetsDatabase.DataAccess.Repositorys.Assets
{
   public class AssetSignOffRepository : IAssetSignOffRepository, IDisposable
    {
        private ITAssetsDatabaseDBContext db = new ITAssetsDatabaseDBContext();

        // Get Sign off sheet file name

        public string GetSignOffSheetFileName(int Id)
        {
            return (db.AssetSignoffs.Where(x => x.ID == Id).Select(x => x.Uploadpath).FirstOrDefault());

          }

        // Add sign off sheet record

        public void AddSignOffsheet(AssetSignoff model)
        {
            db.AssetSignoffs.Add(model);
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
