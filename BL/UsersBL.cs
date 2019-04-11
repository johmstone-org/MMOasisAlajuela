using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using ET;

namespace BL
{
    public class UsersBL
    {
        private UsersDAL UserDAL = new UsersDAL();

        public int Login(string username, string password)
        {
            return UserDAL.Login(username, password);
        }

        public AuthorizationCode AuthCode(string email)
        {
            return UserDAL.AuthCode(email);
        }

        public int ValidateGUID(string guid)
        {
            return UserDAL.ValidateGUID(guid);
        }

        public bool ResetPassword(string guid, string password)
        {
            return UserDAL.ResetPassword(guid, password);
        }

        public List<Users> CheckUserNameAvailability(string UserName)
        {
            return UserDAL.CheckUserNameAvailability(UserName);
        }

        public List<Users> CheckEmailAvailability(string Email)
        {
            return UserDAL.CheckEmailAvailability(Email);
        }

        public bool AddNewUser(Users user, string insertuser)
        {
            return UserDAL.AddNewUser(user,insertuser);
        }
    }
}
