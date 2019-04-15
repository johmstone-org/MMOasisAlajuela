using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ET;
using BL;
using Microsoft.AspNet.Identity;

namespace MMOasisAlajuela.Controllers
{
    public class SongsController : Controller
    {
        private SongsBL SongsBL = new SongsBL();
        private AuthorsBL AuthorsBL = new AuthorsBL();

        // GET: Songs
        public ActionResult Index()
        {
            return View(SongsBL.SongList().ToList());
        }

        public ActionResult Create()
        {
            Songs Song = new Songs();

            Song.AuthorList = AuthorsBL.AuthorList();

            return View(Song);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Songs Song)
        {
            if (Request.IsAuthenticated)
            {

                string InsertUser = User.Identity.GetUserName();

                var r = SongsBL.AddNew(Song, InsertUser);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    Song.ActionType = "CREATE";

                    Song.AuthorList = AuthorsBL.AuthorList();

                    return View(Song);
                }
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }
        public ActionResult CreatebyAuthor(int id)
        {
            Songs Song = new Songs();

            var aut = from a in AuthorsBL.AuthorList()
                      where a.AuthorID == id
                      select a;

            Song.AuthorList = aut.ToList();              

            return View(Song);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatebyAuthor(Songs Song)
        {
            if (Request.IsAuthenticated)
            {

                string InsertUser = User.Identity.GetUserName();

                var r = SongsBL.AddNew(Song, InsertUser);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    Song.ActionType = "CREATE";

                    Song.AuthorList = AuthorsBL.AuthorList();

                    return View(Song);
                }
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }
    }
}