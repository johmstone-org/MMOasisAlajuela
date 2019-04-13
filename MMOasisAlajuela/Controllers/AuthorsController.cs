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

        // GET: Authors
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
        // GET: /Author/Create
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
        // POST: /Author/Create
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
    }
}