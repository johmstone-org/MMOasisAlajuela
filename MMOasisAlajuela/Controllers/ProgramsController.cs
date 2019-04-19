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
    public class ProgramsController : Controller
    {
        private ProgramsBL PBL = new ProgramsBL();
        private ProgramDetailsBL PDBL = new ProgramDetailsBL();

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

                ViewBag.PDate = Program.ProgramDate.ToString("dd/MM/yyyy");

                ViewBag.PSchedule = Program.ProgramSchedule;

                ViewBag.Status = Program.CompleteFlag;

                ViewBag.PID = id;

                return View(PDBL.Details(id).ToList());
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }
    }
}