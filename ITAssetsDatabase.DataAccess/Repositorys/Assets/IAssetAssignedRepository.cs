using ITAssetsDatabase.BusinessDomain;
using System;
using System.Linq;

namespace ITAssetsDatabase.DataAccess.Repositorys.Assets
{
    public interface IAssetAssignedRepository : IDisposable
    {
        void AddAssetAssignedRecord(AssetAssigned newAssetAssigned);
        IQueryable<AssetAssigned> GetAssetAssignedRecordsByAssetId(int? id);
        void DeleteAssetAssignedRecordsById(int id);
        AssetAssigned CurrentAssignee(int AssetId);
        int GetAssetStatusId(int id);

    }
}
