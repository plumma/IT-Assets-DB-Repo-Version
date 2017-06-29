using ITAssetsDatabase.BusinessDomain;
using System;
using System.Collections.Generic;


namespace ITAssetsDatabase.DataAccess.Repositorys.Assets
{
    public class DomainInfoRepository
    {
        private ITAssetsDatabaseDBContext db = new ITAssetsDatabaseDBContext();

        public DomainInfo1 GetDomainInfo1()
        {
            var Domains = new DomainInfo1();
                     return Domains;

        }

        public IEnumerable<Domain> GetDomains()
        {
            throw new NotImplementedException();
        }



        //    public IEnumerable<Domain> GetDomains()
        //    {

        //        return (db.Domains);
        //    }
        //}

        //public class StupidRepository : IDomainInfoRepository
        //{
        //    private ITAssetsDatabaseDBContext db = new ITAssetsDatabaseDBContext();

        //public DomainInfo1 GetDomainInfo1()
        //{
        //    var info = new DomainInfo1();
        //    info.DomainName = "something";
        //    //return info;
        //    return new DomainInfo1() { DomainName = "something" };

        //}

        //public DomainInfo2 GetDomainInfo2()
        //{
        //    //var info = new DomainInfo2();
        //    //info.DomainName = "something";
        //    ////return info;
        //    //return new DomainInfo2() { DomainName = "something" };
        //    return new DomainInfo2(null, 0, "something");
        //}

        //public IEnumerable<Domain> GetDomains()
        //{
        //    return new List<Domain> { new Domain { Assets = null, DomainName = "Domname" } };
        //}



    }
}
