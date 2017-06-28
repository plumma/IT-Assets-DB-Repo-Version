using ITAssetsDatabase.BusinessDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITAssetsDatabase.DataAccess.Repositorys.Assets
{
    public interface IDomainRepository : IDisposable
    {
        IEnumerable<Domain> GetDomains();
    }
}
