using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;
using ET;
using Microsoft.AspNet.Identity;

namespace MMOasisAlajuela.Controllers
{
    public class AuthorsController : Controller
    {
        private AuthorsBL AuthorsBL = new AuthorsBL();
        private SongsBL SongsBL = new SongsBL();

        // GET: Authors/Index
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return View(AuthorsBL.AuthorList().ToList());
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }

        //
        // GET: /Authors/Create
        public ActionResult Create()
        {
            if (Request.IsAuthenticated)
            {
                Authors Author = new Authors();

                return View(Author);
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }

        //
        // POST: /Authors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Authors Author)
        {
            if (Request.IsAuthenticated)
            {
                Author.ActionType = "CREATE";

                string InsertUser = User.Identity.GetUserName();

                var r = AuthorsBL.AddNew(Author, InsertUser);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    return View(Author);
                }
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }

        // GET: Authors/Edit/1
        public ActionResult Edit (int id = 0)
        {
            if (Request.IsAuthenticated)
            {
                Authors Author = AuthorsBL.Details(id);

                ViewBag.AuthorName = Author.AuthorName;

                if (Author == null)
                {
                    return HttpNotFound();
                }
                return View(Author);
            }            
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }

        // POST: /Authors/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit (Authors Author)
        {
            if (Request.IsAuthenticated)
            {
               string InsertUser = User.Identity.GetUserName();

                var r = AuthorsBL.Update(Author, InsertUser);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }

        // GET: /Authors/Details/1
        public ActionResult Details (int id = 0)
        {
            if (Request.IsAuthenticated)
            {
                List<Songs> Repertoire = SongsBL.Reportoires(id);

                Authors Author = AuthorsBL.Details(id);

                ViewBag.AuthorName = Author.AuthorName;
                ViewBag.AuthorID = id;

                if (Author == null)
                {
                    return HttpNotFound();
                }
                return View(Repertoire.ToList());
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }
    }
}