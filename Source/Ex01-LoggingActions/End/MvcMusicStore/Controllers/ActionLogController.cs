namespace MvcMusicStore.Controllers
{
    using MvcMusicStore.Models;
    using System.Linq;
    using System.Web.Mvc;

    public class ActionLogController : Controller
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();

        //
        // GET: /ActionLog/

        public ActionResult Index()
        {
            var model = storeDB.ActionLogs.OrderByDescending(al => al.DateTime).ToList();

            return View(model);
        }

    }
}
