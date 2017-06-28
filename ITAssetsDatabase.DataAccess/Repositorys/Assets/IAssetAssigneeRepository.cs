using ITAssetsDatabase.BusinessDomain;
using System;


namespace ITAssetsDatabase.DataAccess.Repositorys.Assets
{
    public interface IAssetAssigneeRepository : IDisposable
    {
        int AddNewAssetAssigneedRecord(AssetAssignee newAssetAssignee);
        
    }
}
