using ITAssetsDatabase.BusinessDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ITAssetsDatabase.Web.Helpers
{
    public class DomainInfo1
    {
        public IEnumerable<Domain> Domains { get; set; }
        public int DomainId { get; set; }
        public String DomainName { get; set; }
        DomainInfo1() { }
    }

    public class DomainInfo2
    {
        public IEnumerable<Domain> Domains { get; private set; }
        public int DomainId { get; private set; }
        public String DomainName { get; private set; }
        public DomainInfo2(IEnumerable<Domain> domains, int domainId, String domainName)
        {
            Domains = domains;
            DomainId = domainId;
            DomainName = domainName;
        }
    }
}