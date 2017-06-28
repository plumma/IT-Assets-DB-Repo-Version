
using System.DirectoryServices;


namespace ITAssetsDatabase.Web.Helpers
{

    public static class LDAPHelper
    {
        public static SearchResultCollection SearchUser(string searchstring)
        {            
            string path = "LDAP://ldap.forumsys.com/dc=example,dc=com";
                       
            //var username = "cn=read-only-admin,dc=example,dc=com";
            var username = "uid=boyle,dc=example,dc=com";
            var password = "password";
         
           DirectoryEntry rootEntry = new  DirectoryEntry(path, username, password, AuthenticationTypes.None);

            
            DirectorySearcher search = new DirectorySearcher(rootEntry)
            {
                Filter = "(&" + "(objectClass=inetOrgPerson)" + "(cn=*" + searchstring + "*" + "))"
                            };

            var searchlist = search.FindAll();
            
  

           return searchlist;
        

        }
    }
}


     