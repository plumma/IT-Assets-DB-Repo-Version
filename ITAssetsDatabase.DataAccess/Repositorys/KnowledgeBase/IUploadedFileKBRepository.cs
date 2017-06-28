using ITAssetsDatabase.BusinessDomain;
using System;


namespace ITAssetsDatabase.DataAccess.Repositorys.KnowledgeBase
{
    public interface IUploadedFileKBRepository : IDisposable
    {

        void UploadFile(UploadedFileKB uploadedKB);
        string GetUploadedFileNameByID(int Id);
    }
}
