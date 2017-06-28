
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ITAssetsDatabase.Web.Web;
using ITAssetsDatabase.DataAccess;
using System.Data.Entity;
using SimpleInjector;
using ITAssetsDatabase.DataAccess.Repositorys.Assets;
using System.Reflection;
using SimpleInjector.Integration.Web.Mvc;

namespace ITAssetsDatabase.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Database.SetInitializer(new CreateDatabaseIfNotExists<ITAssetsDatabaseDBContext>());

            


        }
    }
}