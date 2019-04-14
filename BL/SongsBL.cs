using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using ET;

namespace BL
{
    public class SongsBL
    {
        private SongsDAL SongsDAL = new SongsDAL();

        public List<Songs> Reportoires (int authorid)
        {
            return SongsDAL.Repertoires(authorid);
        }
    }
}
