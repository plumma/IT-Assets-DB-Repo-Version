using System.Collections.Generic;
using ITAssetsDatabase.BusinessDomain;
using System.Linq;
using System;

namespace ITAssetsDatabase.DataAccess.Repositorys.Assets
{
    public class LocationRepository : ILocationRepository, IDisposable
    {
        private ITAssetsDatabaseDBContext db = new ITAssetsDatabaseDBContext();

        public List<Location> GetLocations(string term) { 

        return (db.Locations.Where(l => l.BuildingName.Contains(term) || l.Town.Contains(term)).ToList());

        }

        // Get Location by Location Id
        public Location GetLocationById(int id)        {

            return (db.Locations.Find(id));

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
