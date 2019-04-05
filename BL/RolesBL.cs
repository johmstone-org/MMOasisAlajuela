using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using ET;

namespace BL
{
    public class RolesBL
    {
        private RolesDAL RolesDAL = new RolesDAL();

        public List<Roles> Roles()
        {
            return RolesDAL.Roles();
        }

        public bool Create(Roles role, string insertuser)
        {
            return RolesDAL.Create(role, insertuser);
        }

        public bool Update(string actiontype, Roles role, string insertuser)
        {
            return RolesDAL.Update(actiontype, role, insertuser);
        }
    }
}
