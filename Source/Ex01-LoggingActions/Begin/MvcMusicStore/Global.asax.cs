namespace MvcMusicStore
{
    using MvcMusicStore.Models;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Linq;

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

            AppConfig.Configure();            
        }

        protected void Session_Start()
        {
            // Clean up Logs Table
            MusicStoreEntities storeDB = new MusicStoreEntities();
            foreach (var log in storeDB.ActionLogs.ToList())
            {
                storeDB.ActionLogs.Remove(log);
            }

            storeDB.SaveChanges();
        }
    }
}