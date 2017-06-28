using ITAssetsDatabase.BusinessDomain;
using System;
using System.Collections.Generic;


namespace ITAssetsDatabase.DataAccess.Repositorys.Assets
{
    public interface IDeviceRepository : IDisposable
    {
        List<Device> GetDeviceList(string term);
        Device GetDeviceById(int DeviceId);
    }
}
