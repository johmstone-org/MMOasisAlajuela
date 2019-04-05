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
    public class RolesController : Controller
    {
        private RolesBL RolesBL = new RolesBL();

        // GET: Roles
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return View(RolesBL.Roles().ToList());
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }  
        }

        //
        // GET: /Roles/Create
        public ActionResult Create()
        {
            if(Request.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }

        //
        // POST: /Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Roles BS)
        {
            if (ModelState.IsValid)
            {
                string InsertUser = User.Identity.GetUserName();

                var r = RolesBL.Create(BS, InsertUser);

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

            return View(BS);
        }


        //
        // Post: /Roles/ActiveFlag
        [HttpPost]
        public ActionResult ActiveFlag(int id, bool status)
        {
            if ((Request.IsAuthenticated))
            {
                Roles role = new Roles();

                role.RoleID = id;

                if (status == true)
                {
                    role.ActiveFlag = false;
                }
                else
                {
                    role.ActiveFlag = true;
                }

                string InsertUser = User.Identity.GetUserName();

                var r = RolesBL.Update("MS",role, InsertUser);

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
    }
}