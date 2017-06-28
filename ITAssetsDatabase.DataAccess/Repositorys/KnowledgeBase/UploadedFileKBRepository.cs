using ITAssetsDatabase.BusinessDomain;
using System;

namespace ITAssetsDatabase.DataAccess.Repositorys.KnowledgeBase
{
    public class UploadedFileKBRepository : IUploadedFileKBRepository, IDisposable
    {
        private ITAssetsDatabaseDBContext db = new ITAssetsDatabaseDBContext();

        public void UploadFile(UploadedFileKB uploadedKB) {

            db.UploadedFilesKB.Add(uploadedKB);
            db.SaveChanges();
        }


        public string GetUploadedFileNameByID(int Id) {

            return (db.UploadedFilesKB.Find(Id).FileName);           

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
