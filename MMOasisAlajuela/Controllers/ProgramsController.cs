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

        // GET: Programs
        public ActionResult Index()
        {
            return View(PBL.ProgramList().ToList());
        }
    }
}