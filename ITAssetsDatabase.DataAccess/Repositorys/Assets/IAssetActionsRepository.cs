using ITAssetsDatabase.BusinessDomain;
using System;
using System.Collections.Generic;


namespace ITAssetsDatabase.DataAccess.Repositorys.Assets
{
    public interface IAssetActionsRepository : IDisposable
    {
        IEnumerable<AssetAction> GetAssetActions(string action);

    }
}
