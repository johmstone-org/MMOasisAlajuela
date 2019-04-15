using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            return View();
        }

        public ActionResult MusicSheetsbySong (int SongID)
        {
            List<MusicSheets> Charts = MSBL.CaMusicSheetsbySong(SongID);

            var songname = from s in SongsBL.SongList()
                           where s.SongID == SongID
                           select s.SongName;

            ViewBag.SongID = SongID;
            ViewBag.SongName = songname.FirstOrDefault().ToString();

            return View(Charts.ToList());
        }

        public ActionResult CreatebySong(int SongID)
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

                ViewBag.FileStatus = "Invalid file format.";
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
    }
}