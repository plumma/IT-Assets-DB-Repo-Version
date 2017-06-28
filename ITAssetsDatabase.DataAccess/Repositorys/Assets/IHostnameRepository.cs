using ITAssetsDatabase.BusinessDomain;
using System;
using System.Collections.Generic;


namespace ITAssetsDatabase.DataAccess.Repositorys.Assets
{
    public interface IHostnameRepository : IDisposable
    {
        IEnumerable<Hostname> GetHostnames();
        IEnumerable<Hostname_temp> GetHostnames_temp();
        void AddHostname_temp(Hostname_temp hostname);
        void AddHostname(Hostname hostname);
    }
}
