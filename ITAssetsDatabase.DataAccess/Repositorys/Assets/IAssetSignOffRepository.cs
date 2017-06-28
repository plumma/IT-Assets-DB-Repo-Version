using ITAssetsDatabase.BusinessDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITAssetsDatabase.DataAccess.Repositorys.Assets
{
    public interface IAssetSignOffRepository : IDisposable
    {

        string GetSignOffSheetFileName(int Id);
        void AddSignOffsheet(AssetSignoff model);

    }
}
