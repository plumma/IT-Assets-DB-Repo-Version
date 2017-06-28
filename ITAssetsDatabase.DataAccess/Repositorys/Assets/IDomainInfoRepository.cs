using ITAssetsDatabase.BusinessDomain;
using System.Collections.Generic;

namespace ITAssetsDatabase.DataAccess.Repositorys.Assets
{
    public interface IDomainInfoRepository
    {
            IEnumerable<Domain> GetDomains();
            DomainInfo1 GetDomainInfo1();
            //DomainInfo2 GetDomainInfo2();
    
    }
}
