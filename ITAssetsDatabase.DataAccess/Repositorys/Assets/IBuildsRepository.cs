using ITAssetsDatabase.BusinessDomain;
using System;
using System.Collections.Generic;

namespace ITAssetsDatabase.DataAccess.Repositorys.Assets
{
    public interface IBuildsRepository : IDisposable
    {
        List<Build> GetBuildList(string term);
        Build GetBuildByID(int BuildId);

    }
}
