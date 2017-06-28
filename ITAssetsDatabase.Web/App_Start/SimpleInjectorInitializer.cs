[assembly: WebActivator.PostApplicationStartMethod(typeof(ITAssetsDatabase.Web.App_Start.SimpleInjectorInitializer), "Initialize")]

namespace ITAssetsDatabase.Web.App_Start
{
    using System.Reflection;
    using System.Web.Mvc;

    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using DataAccess.Repositorys.Assets;
    using DataAccess.Repositorys.KnowledgeBase;

    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            
            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            
            container.Verify();
            
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
     
        private static void InitializeContainer(Container container)
        {
            // Register your types, for instance:

            // Asset Database
            container.Register<IAssetRepository, AssetRepository>(Lifestyle.Scoped);
            container.Register<IDomainRepository, DomainRepository>(Lifestyle.Scoped);
            container.Register<IAssetActionsRepository, AssetActionsRepository>(Lifestyle.Scoped);
            container.Register<IAssetAssignedRepository, AssetAssignedRepository>(Lifestyle.Scoped);
            container.Register<IAssetAssigneeRepository, AssetAssigneeRepository>(Lifestyle.Scoped);
            container.Register<IAssetSignOffRepository, AssetSignOffRepository>(Lifestyle.Scoped);
            container.Register<IBuildsRepository, BuildsRepository>(Lifestyle.Scoped);
            container.Register<IDeviceRepository, DeviceRepository>(Lifestyle.Scoped);
            container.Register<IEmployeeRepository, EmployeeRepository>(Lifestyle.Scoped);
            container.Register<IHostnameRepository, HostnameRepository>(Lifestyle.Scoped);
            container.Register<ILocationRepository, LocationRepository>(Lifestyle.Scoped);
            // Knowledgebase
            container.Register<IApplicationRepository, ApplicationRepository>(Lifestyle.Scoped);
            container.Register<IArticleCategoryRepository, ArticleCategoryRepository>(Lifestyle.Scoped);
            container.Register<IArticleRepository, ArticleRepository>(Lifestyle.Scoped);
            container.Register<IArticleTypeRepository, ArticleTypeRepository>(Lifestyle.Scoped);
            container.Register<IUploadedFileKBRepository, UploadedFileKBRepository>(Lifestyle.Scoped);
        }
    }
}