using ITAssetsDatabase.BusinessDomain;
using System.Collections.Generic;

namespace ITAssetsDatabase.DataAccess.Repositorys.Helpers
{
    public class ADLogonsRepository
    {
        private ITAssetsDatabaseDBContext db = new ITAssetsDatabaseDBContext();

        // Get AD Logons

        public ADLogon GetADLogons(int id) {

            return (db.ADLogons.Find(id));

        }


    }
}
