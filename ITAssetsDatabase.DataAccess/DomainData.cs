using System.Collections.Generic;
using System.Linq;
using ITAssetsDatabase.BusinessDomain;

namespace ITAssetsDatabase.DataAccess
{
    public class DomainInfo1
    {
        public IEnumerable<Domain> Domains { get; set; }
        public int DomainId { get; set; }
        public string DomainName { get; set; }
        }

    //public class DomainInfo2
    //{
    //    public IEnumerable<Domain> Domains { get; private set; }
    //    public int DomainId { get; private set; }
    //    public string DomainName { get; private set; }

    //    public DomainInfo2(IEnumerable<Domain> domains, int domainId, string domainName)
    //    {
    //        Domains = domains;
    //        DomainId = domainId;
    //        DomainName = domainName;
    //    }
    //}
}
