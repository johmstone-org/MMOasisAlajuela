using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.IO;
using ET;
using BL;
using Microsoft.AspNet.Identity;

namespace MMOasisAlajuela.Controllers
{
    public class MusicSheetsController : Controller
    {
        private MusicSheetsBL MSBL = new MusicSheetsBL();
        private SongsBL SongsBL = new SongsBL();
        private MSTypesBL MSTBL = new MSTypesBL();
        private InstrumentsBL IBL = new InstrumentsBL();

        // GET: MusicSheets
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                List<MusicSheets> Charts = MSBL.MSList();
                return View(Charts.ToList());
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
            
        }

        public ActionResult MusicSheetsbySong (int SongID)
        {
            if (Request.IsAuthenticated)
            {
                List<MusicSheets> Charts = MSBL.CaMusicSheetsbySong(SongID);

                var songname = from s in SongsBL.SongList()
                               where s.SongID == SongID
                               select s.SongName;

                ViewBag.SongID = SongID;
                ViewBag.SongName = songname.FirstOrDefault().ToString();

                return View(Charts.ToList());
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
            
        }

        public ActionResult CreatebySong(int SongID)
        {
            if (Request.IsAuthenticated)
            {
                MusicSheets Chart = new MusicSheets();

                var songlist = from s in SongsBL.SongList()
                               where s.SongID == SongID
                               select s;

                Chart.SongID = SongID;

                Chart.SongList = songlist.ToList();

                Chart.MSTypeList = MSTBL.MSTypeList().ToList();

                Chart.InstrumentList = IBL.InstrumentList().ToList();

                var SongName = from s in SongsBL.SongList()
                               where s.SongID == SongID
                               select s.SongName;

                ViewBag.SongName = SongName.FirstOrDefault().ToString();

                return View(Chart);
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
            
        }

        [HttpPost]
        public ActionResult CreatebySong(MusicSheets MS)
        {

            String FileExt = Path.GetExtension(MS.files.FileName).ToUpper();

            if (FileExt == ".PDF")
            {
                Stream str = MS.files.InputStream;
                BinaryReader Br = new BinaryReader(str);
                Byte[] FileDet = Br.ReadBytes((Int32)str.Length);

                MS.FileName = MS.files.FileName;
                MS.FileData = FileDet;

                string InsertUser = User.Identity.GetUserName();

                var r = MSBL.AddNew(MS, InsertUser);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    return RedirectToAction("MusicSheetsbySong", new { SongID = MS.SongID });
                }
            }
            else
            {

                ViewBag.FileStatus = "Archivo de formato Invalido.";
                return View();

            }

        }

        [HttpGet]
        public FileResult DownLoadFile(int id)
        {
            List<MusicSheets> ObjFiles = MSBL.MSList();

            var FileById = (from FC in ObjFiles
                            where FC.MSID.Equals(id)
                            select new { FC.FileName, FC.FileData }).ToList().FirstOrDefault();

            return File(FileById.FileData, "application/pdf", FileById.FileName);
            

        }

        public ActionResult Create()
        {
            if (Request.IsAuthenticated)
            {
                MusicSheets Chart = new MusicSheets();

                Chart.SongList = SongsBL.SongList().ToList();

                Chart.MSTypeList = MSTBL.MSTypeList().ToList();

                Chart.InstrumentList = IBL.InstrumentList().ToList();

                return View(Chart);
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
            
        }

        [HttpPost]
        public ActionResult Create(MusicSheets MS)
        {

            String FileExt = Path.GetExtension(MS.files.FileName).ToUpper();

            if (FileExt == ".PDF")
            {
                Stream str = MS.files.InputStream;
                BinaryReader Br = new BinaryReader(str);
                Byte[] FileDet = Br.ReadBytes((Int32)str.Length);

                MS.FileName = MS.files.FileName;
                MS.FileData = FileDet;

                string InsertUser = User.Identity.GetUserName();

                var r = MSBL.AddNew(MS, InsertUser);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    MS.ActionType = "CREATE";

                    MS.SongList = SongsBL.SongList().ToList();

                    MS.MSTypeList = MSTBL.MSTypeList().ToList();

                    MS.InstrumentList = IBL.InstrumentList().ToList();

                    return View(MS);
                }
            }
            else
            {

                ViewBag.FileStatus = "Invalid file format.";
                return View();

            }

        }

        public ActionResult Edit(int id = 0)
        {
            if ((Request.IsAuthenticated))
            {
                MusicSheets MS = MSBL.Details(id);

                MS.SongList = SongsBL.SongList().ToList();

                MS.MSTypeList = MSTBL.MSTypeList().ToList();

                MS.InstrumentList = IBL.InstrumentList().ToList();

                ViewBag.MSID = id;

                return View(MS);
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }

        //[HttpPost]
        public ActionResult Disable (int id)
        {
            MusicSheets MS = MSBL.Details(id);

            MS.ActiveFlag = false;

            string InsertUser = User.Identity.GetUserName();

            var r = MSBL.Update(MS, InsertUser);

            if (!r)
            {
                ViewBag.Mensaje = "Ha ocurrido un error inesperado.";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                //ViewBag.SongID = MS.SongID;
                return this.RedirectToAction("MusicSheetsbySong","MusicSheets", new { SongID = MS.SongID });
                //return View();
            }
        }
    }
}