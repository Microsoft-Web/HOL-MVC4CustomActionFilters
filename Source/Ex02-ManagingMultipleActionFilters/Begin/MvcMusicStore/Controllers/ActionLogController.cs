namespace MvcMusicStore.Controllers
{
    using MvcMusicStore.Models;
    using System.Linq;
    using System.Web.Mvc;

    public class ActionLogController : Controller
    {
        private MusicStoreEntities storeDB = new MusicStoreEntities();

        // GET: /ActionLog/
        public ActionResult Index()
        {
            var model = this.storeDB.ActionLogs.OrderByDescending(al => al.DateTime).ToList();

            return this.View(model);
        }
    }
}
