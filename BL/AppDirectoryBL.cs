using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using ET;

namespace BL
{
    public class AppDirectoryBL
    {
        private AppDirectoryDAL AppDirectoryDAL = new AppDirectoryDAL();

        public List<AppDirectory> AppProfilebyRole(int roleid)
        {
            return AppDirectoryDAL.AppProfilebyRole(roleid);
        }

        public List<AppDirectory> AppProfilebyUser(string username)
        {
            return AppDirectoryDAL.AppProfilebyUser(username);
        }

        public bool UpdateAppProfile(AppDirectory app, int roleid, string insertuser)
        {
            return AppDirectoryDAL.UpdateAppProfile(app, roleid, insertuser);
        }
    }
}
