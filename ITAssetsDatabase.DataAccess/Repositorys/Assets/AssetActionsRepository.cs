using System.Collections.Generic;
using ITAssetsDatabase.BusinessDomain;
using System;

namespace ITAssetsDatabase.DataAccess.Repositorys.Assets
{
    public class AssetActionsRepository : IAssetActionsRepository, IDisposable
    {
        private ITAssetsDatabaseDBContext db = new ITAssetsDatabaseDBContext();

        public IEnumerable<AssetAction> GetAssetActions(string action) {

            if (action == "NEW")
            {
                List<AssetAction> NEWSelectList = new List<AssetAction>();

                var q = new AssetAction();
                q.Action = "ASSIGN";
                q.ID = 5;

                NEWSelectList.Add(q);

                return (NEWSelectList);
            }
            else
            {
                return (db.AssetActions);
            }

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
