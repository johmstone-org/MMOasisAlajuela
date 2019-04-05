using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;
using ET;

namespace MMOasisAlajuela.Controllers
{
    public class HomeController : Controller
    {
        private AppDirectoryBL AppBL = new AppDirectoryBL();

        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }
                                                    
        public ActionResult _MainMenu(string MainClass)
        {
            var data = from p in AppBL.AppProfilebyUser(User.Identity.Name)
                       where p.MainClass == MainClass
                       select p;    
            if (data.FirstOrDefault() == null)
            {
                ViewBag.Class = null;
            }
            else
            {
                ViewBag.Class = MainClass;
            }
            return View(data.ToList());

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}