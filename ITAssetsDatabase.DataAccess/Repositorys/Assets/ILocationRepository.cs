using ITAssetsDatabase.BusinessDomain;
using System;
using System.Collections.Generic;


namespace ITAssetsDatabase.DataAccess.Repositorys.Assets
{
    public interface ILocationRepository : IDisposable
    {
        List<Location> GetLocations(string term);
        Location GetLocationById(int id);

    }
}
