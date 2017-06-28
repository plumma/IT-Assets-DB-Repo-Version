using System.Collections.Generic;
using System.Linq;
using ITAssetsDatabase.BusinessDomain;
using System;

namespace ITAssetsDatabase.DataAccess.Repositorys.Assets
{
    public class DeviceRepository : IDeviceRepository, IDisposable
    {
        private ITAssetsDatabaseDBContext db = new ITAssetsDatabaseDBContext();


        public List<Device> GetDeviceList(string term) {

            return (db.Devices.Where(l => l.Make.Contains(term) || l.Model.Contains(term) || l.Type.Contains(term)).ToList());

        }

        public Device GetDeviceById(int DeviceId) {

            return (db.Devices.Find(DeviceId));

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
