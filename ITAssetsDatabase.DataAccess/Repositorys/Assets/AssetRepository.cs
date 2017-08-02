using System.Collections.Generic;
using System.Linq;
using ITAssetsDatabase.BusinessDomain;
using System;


namespace ITAssetsDatabase.DataAccess.Repositorys.Assets

{

    public class AssetRepository : IAssetRepository, IDisposable
    {
        private ITAssetsDatabaseDBContext db = new ITAssetsDatabaseDBContext();


        // Get All Assets
        public List<Asset> GetAllAssets()
        { 
            return (db.Assets.ToList());
        }

        // Total Asset count
 

        public int GetTotalAssetCount()
        {
            return (db.Assets.Count());
        }


        // Get Assets SKipped
        public List<Asset> GetSetNumberOfAssets(int skipCount, int takeCount)  
        {

            return (db.Assets.OrderByDescending(x => x.CreateDate).Skip(skipCount).Take(takeCount).ToList());            
        }
        

        // Get Asset By ID
        public Asset GetAssetById(int? id)
        {
            return (db.Assets.Find(id));
        }


        // Add new asset
        public void AddNewAsset(Asset newAsset)
        {             
            db.Assets.Add(newAsset);
            db.SaveChanges();
        }

        // Delete asset by ID
        public void DeleteAssetById(int id)
        {
            var asset = db.Assets.Find(id);
            db.Assets.Remove(asset);
            db.SaveChanges();
        }

        // Update Asset 

        public void UpdateAsset(int Id, int HostnameId, int DomainNameId, int BuildId) {

            var assetquery = GetAssetById(Id);

            assetquery.HostnameId = HostnameId;
            assetquery.DomainNameId = DomainNameId;
            assetquery.BuildId = BuildId;

            db.SaveChanges();

        }
    

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

    
    }    
}
