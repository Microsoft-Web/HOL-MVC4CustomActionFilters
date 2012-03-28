using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MvcMusicStore.ViewModels;
using MvcMusicStore.Models;
using MvcMusicStore.Filters;

namespace MvcMusicStore.Controllers
{
    [MyNewCustomActionFilter (Order = 1)]
    [CustomActionFilter (Order = 2)]
    public class StoreController : Controller
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();

        //
        // GET: /Store/

        public ActionResult Index()
        {
            // Retrieve the list of genres
            var genres = from genre in storeDB.Genres
                         select genre.Name;

            // Create your view model
            var viewModel = new StoreIndexViewModel
            {
                Genres = genres.ToList(),
                NumberOfGenres = genres.Count()
            };

            return View(viewModel);
        }

        //
        // GET: /Store/Browse?genre=Disco

        public ActionResult Browse(string genre)
        {
            // Retrieve Genre and its Associated Albums from database

            var genreModel = storeDB.Genres.Include("Albums")
                .Single(g => g.Name == genre);

            var viewModel = new StoreBrowseViewModel()
            {
                Genre = genreModel,
                Albums = genreModel.Albums.ToList()
            };

            return View(viewModel);
        }

        //
        // GET: /Store/Details/5

        public ActionResult Details(int id)
        {
            var album = storeDB.Albums.Single(a => a.AlbumId == id);

            return View(album);
        }
    }
}
