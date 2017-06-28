using ITAssetsDatabase.BusinessDomain;
using System;
using System.Linq;


namespace ITAssetsDatabase.DataAccess.Repositorys.Assets
{
    public class AssetAssignedRepository : IAssetAssignedRepository, IDisposable
    {
        private ITAssetsDatabaseDBContext db = new ITAssetsDatabaseDBContext();

        // Add new AssetAssigned record

        public void AddAssetAssignedRecord(AssetAssigned newAssetAssigned)
        {
            db.AssetAssigned.Add(newAssetAssigned);
            db.SaveChanges();
        }


        // Add new AssetAssigned record

        public IQueryable<AssetAssigned> GetAssetAssignedRecordsByAssetId(int? id)
        { 
            return( db.AssetAssigned.Where(x => x.AssetId == id));
        }


        // Delete Asset Assigned records

        public void DeleteAssetAssignedRecordsById(int id)
        {
            var assetassignedtoremove = db.AssetAssigned.Where(a => a.AssetId == id).ToList();
            foreach (var record in assetassignedtoremove)
            db.AssetAssigned.Remove(record);
            db.SaveChanges();
        }

        // Latest user to be assigned to the asset.

        public AssetAssigned CurrentAssignee(int AssetId)
        { 
            return( db.AssetAssigned.Where(x => x.AssetId == AssetId && x.AssetStatusID == 2 || x.AssetStatusID == 1)
                         .OrderByDescending(s => s.CreateDate).FirstOrDefault());
        }


                // Get Asset Status
        public int GetAssetStatusId(int id)
        {
            return(db.AssetAssigned.Where(x => x.AssetId == id).OrderByDescending(x => x.CreateDate).Select(x => x.AssetStatus.Id).First());
            
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
