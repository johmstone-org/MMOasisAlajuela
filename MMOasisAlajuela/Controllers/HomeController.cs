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
                var data = from p in AppBL.AppProfilebyUser(User.Identity.Name)
                           select p.MainClass;

                var FinalData = data.Distinct();

                List<AppDirectory> MainMenu = new List<AppDirectory>();

                foreach(var item in FinalData)
                {
                    AppDirectory r = new AppDirectory();
                    r.RARProfileID = Convert.ToInt32(MainMenu.Count()) + 2;
                    r.MainClass = item;
                    MainMenu.Add(r);
                }

                return View(MainMenu.ToList());
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }
                                                    
        public ActionResult _MainMenu(string MainClass, int ImagePathNumber)
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
                ViewBag.LabelMenu = AppBL.LabelMenu(MainClass).ToString();
                if (ImagePathNumber >= 10)
                {
                    ViewBag.ImagePath = "pic" + ImagePathNumber.ToString() + ".jpg";
                }
                else
                {
                    ViewBag.ImagePath = "pic0" + ImagePathNumber.ToString() + ".jpg";
                }
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