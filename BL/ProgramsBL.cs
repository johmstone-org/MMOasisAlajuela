using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ET;
using DAL;

namespace BL
{
    public class ProgramsBL
    {
        private ProgramsDAL PDAL = new ProgramsDAL();

        public List<Programs> ProgramList()
        {
            return PDAL.ProgramList();
        }
    }
}
