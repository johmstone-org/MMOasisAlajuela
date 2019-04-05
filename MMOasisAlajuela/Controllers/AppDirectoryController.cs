using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;
using ET;

namespace MMOasisAlajuela.Controllers
{
    public class AppDirectoryController : Controller
    {
        private AppDirectoryBL AppDirBL = new AppDirectoryBL();
        private RolesBL RoleBL = new RolesBL();

        // GET: AppDirectory
        public ActionResult Index(int RoleID)
        {
            if(Request.IsAuthenticated)
            {
                var Profile = AppDirBL.AppProfilebyRole(RoleID);

                var roleName = from r in RoleBL.Roles()
                               where r.RoleID == RoleID
                               select r.RoleName;

                ViewBag.RoleName = roleName.FirstOrDefault();
                ViewBag.RoleID = RoleID;

                return View(Profile);
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }


        // Post: /Profiles/ActiveFlag
        [HttpPost]
        public ActionResult ActiveFlag(int profileid, int roleid, int appid, bool status)
        {
            if ((Request.IsAuthenticated))
            {
                AppDirectory App = new AppDirectory();

                App.RARProfileID = profileid;
                App.ApplicationID = appid;

                if (status == true)
                {
                    App.Status = false;
                }
                else
                {
                    App.Status = true;
                }

                string InsertUser = User.Identity.Name;

                var r = AppDirBL.UpdateAppProfile(App, roleid, InsertUser);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    return RedirectToAction("Index", new { RoleID = roleid });
                }

            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }
    }
}