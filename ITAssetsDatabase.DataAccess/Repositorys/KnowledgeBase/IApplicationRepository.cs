using ITAssetsDatabase.BusinessDomain;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ITAssetsDatabase.DataAccess.Repositorys.KnowledgeBase
{
    public interface IApplicationRepository : IDisposable
    {
        IQueryable<Application> SearchApplications(string term);
        IEnumerable<Application> GetApplications();
        void AddApp(Application model);
    }
}
