using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using ET;

namespace BL
{
    public class AuthorsBL
    {
        AuthorsDAL AuthDAL = new AuthorsDAL();

        public List<Authors> AuthorList()
        {
            return AuthDAL.AuthorList();
        }

        public bool AddNew (Authors author, string insertuser)
        {
            return AuthDAL.AddNew(author, insertuser);
        }

        public Authors Details(int authorid)
        {
            return AuthDAL.Details(authorid);
        }

        public bool Update(Authors author, string insertuser)
        {
            return AuthDAL.Update(author, insertuser);
        }
    }
}
