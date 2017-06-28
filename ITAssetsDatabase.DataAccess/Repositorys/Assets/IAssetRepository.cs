using ITAssetsDatabase.BusinessDomain;
using System.Collections.Generic;
using System;

namespace ITAssetsDatabase.DataAccess.Repositorys.Assets
{
    public interface IAssetRepository : IDisposable
    {

        List<Asset> GetAllAssets();
        Asset GetAssetById(int? id);
        void AddNewAsset(Asset newAsset);
        void DeleteAssetById(int id);
        void UpdateAsset(int Id, int HostnameId, int DomainNameId, int BuildId);
    }

}
