using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;
using ET;
using Microsoft.AspNet.Identity;
using PagedList;
using PagedList.Mvc;

namespace MMOasisAlajuela.Controllers
{
    public class UsersController : Controller
    {
        private UsersBL UserBL = new UsersBL();

        // GET: Users
        public ActionResult Index(/*string searchString, int? page*/)
        {
            if (Request.IsAuthenticated)
            {
                var UsersList = from c in UserBL.UsersList()
                                select c;

                //if (!string.IsNullOrEmpty(searchString))
                //{
                //    UsersList = UsersList.Where(r => r.FullName.ToUpper().Contains(searchString.ToUpper())
                //                                     || r.UserName.ToUpper().Contains(searchString.ToUpper()));
                //}

                //ViewBag.Search = searchString;

                ////int role = AppUsersBL.Login(Request.ServerVariables["LOGON_USER"]);

                ////ViewBag.Role = role;
                //ViewBag.Search = searchString;

                //return View(UsersList.ToList().ToPagedList(page ?? 1, 15));
                return View(UsersList.ToList());
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }
    }
}