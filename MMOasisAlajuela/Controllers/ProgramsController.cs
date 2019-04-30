using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ET;
using BL;
using Microsoft.AspNet.Identity;
using System.Globalization;

namespace MMOasisAlajuela.Controllers
{
    public class ProgramsController : Controller
    {
        private ProgramsBL PBL = new ProgramsBL();
        private ProgramDetailsBL PDBL = new ProgramDetailsBL();
        private SongsBL SBL = new SongsBL();

        // GET: Programs
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return View(PBL.ProgramList().ToList());
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
            
        }

        public ActionResult Create()
        {
            if (Request.IsAuthenticated)
            {
                Programs Program = new Programs();
                Program.ProgramDate = DateTime.Today;

                return View(Program);
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
                       
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create (Programs Pgrm)
        {
            if (ModelState.IsValid)
            {
                string InsertUser = User.Identity.GetUserName();

                int r = PBL.AddNew(Pgrm, InsertUser);

                if (r>0)
                {
                    return this.RedirectToAction("Details", "Programs", new { id = r });
                }
                else
                {
                    
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
            }

            return View(Pgrm);

        }

        public ActionResult Details (int id)
        {
            if (Request.IsAuthenticated)
            {
                var details = from p in PBL.ProgramList()
                              where p.ProgramID == id
                              select p;
                Programs Program = details.FirstOrDefault();

                CultureInfo ci = new CultureInfo("Es-Es");

                string day = ci.DateTimeFormat.GetDayName(Program.ProgramDate.DayOfWeek).ToString();

                ViewBag.DayName = day.First().ToString().ToUpper() + day.Substring(1);

                ViewBag.PDate = Program.ProgramDate.ToString("dd/MM/yyyy");

                ViewBag.PSchedule = Program.ProgramSchedule;

                ViewBag.Status = Program.CompleteFlag;
                                
                ViewBag.PID = id;

                List<ProgramDetails> PDetails = new List<ProgramDetails>();

                PDetails = PDBL.Details(id).ToList();

                foreach (var r in PDetails)
                {
                    r.Status = Program.CompleteFlag;
                }

                return View(PDetails);
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }

        public ActionResult AddSong(int id = 0)
        {
            ProgramDetails PD = new ProgramDetails();

            var details = from p in PBL.ProgramList()
                          where p.ProgramID == id
                          select p;

            Programs Program = details.FirstOrDefault();

            CultureInfo ci = new CultureInfo("Es-Es");

            string day = ci.DateTimeFormat.GetDayName(Program.ProgramDate.DayOfWeek).ToString();

            ViewBag.DayName = day.First().ToString().ToUpper() + day.Substring(1);

            ViewBag.PDate = Program.ProgramDate.ToString("dd/MM/yyyy");

            ViewBag.PSchedule = Program.ProgramSchedule;

            ViewBag.PID = id;

            PD.SongsList = SBL.SongList().OrderBy(x => x.SongName).ToList();

            PD.ProgramID = id;

            return View(PD);       
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSong(ProgramDetails PD)
        {
            if (Request.IsAuthenticated)
            {
                PD.ActionType = "CREATE";

                string InsertUser = User.Identity.GetUserName();

                var r = PDBL.AddSong(PD, InsertUser);

                PD.SongsList = SBL.SongList().OrderBy(x => x.SongName).ToList();

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    return View(PD);
                }
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }

        public ActionResult Disable(int id)
        {
            string InsertUser = User.Identity.GetUserName();

            int r = PDBL.Disable(id, InsertUser);

            if (r == 0)
            {
                ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                //ViewBag.SongID = MS.SongID;
                return this.RedirectToAction("Details", "Programs", new { id = r });
                //return View();
            }
        }
    }
}