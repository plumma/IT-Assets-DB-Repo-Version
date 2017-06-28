using ITAssetsDatabase.BusinessDomain;
using System;
using System.Collections.Generic;

namespace ITAssetsDatabase.DataAccess.Repositorys.Assets
{
    public class HostnameRepository : IHostnameRepository, IDisposable
    {
        private ITAssetsDatabaseDBContext db = new ITAssetsDatabaseDBContext();

        
        // GetHostnames
        public IEnumerable<Hostname> GetHostnames() {

              return (db.Hostnames);
        }

        // Add Hostname Hostname Entity
        public void AddHostname(Hostname hostname)
        {
            db.Hostnames.Add(hostname);
            db.SaveChanges();
        }
    

         // Get Hostnames from Temp Entity
          public IEnumerable<Hostname_temp> GetHostnames_temp()
        {

            return (db.Hostname_temp);
        }

        
        // Add Hostnames Hostname_temp Entity
        public void AddHostname_temp(Hostname_temp hostname)
        {

            db.Hostname_temp.Add(hostname);
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
