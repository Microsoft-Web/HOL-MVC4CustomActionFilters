namespace MvcMusicStore.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using MvcMusicStore.Models;
    using MvcMusicStore.Filters;

    [CustomActionFilter]
    public class StoreController : Controller
    {
        private MusicStoreEntities storeDB = new MusicStoreEntities();

        // GET: /Store/
        public ActionResult Details(int id)
        {
            var album = this.storeDB.Albums.Find(id);
            if (album == null)
            {
                return this.HttpNotFound();
            }

            return this.View(album);
        }

        public ActionResult Browse(string genre)
        {
            // Retrieve Genre and its Associated Albums from database
            var genreModel = this.storeDB.Genres.Include("Albums")
                .Single(g => g.Name == genre);

            return this.View(genreModel);
        }

        public ActionResult Index()
        {
            var genres = this.storeDB.Genres;
            return this.View(genres);
        }

        // GET: /Store/GenreMenu
        [ChildActionOnly]
        public ActionResult GenreMenu()
        {
            var genres = this.storeDB.Genres.Take(9).ToList();

            return this.PartialView(genres);
        }
    }
}
