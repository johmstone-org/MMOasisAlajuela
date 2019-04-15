using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ET;
using DAL;

namespace BL
{
    public class MusicSheetsBL
    {
        private MusicSheetsDAL MSDAL = new MusicSheetsDAL();

        public List<MusicSheets> CaMusicSheetsbySong (int songid)
        {
            return MSDAL.MusicSheetsbySong(songid);
        }

        public bool AddNew (MusicSheets musicsheet, string insertuser)
        {
            return MSDAL.AddNew(musicsheet, insertuser);
        }

        public List<MusicSheets> MSList ()
        {
            return MSDAL.MSList();
        }
    }
}
