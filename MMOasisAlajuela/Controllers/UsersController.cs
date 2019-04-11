using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;
using ET;
using Microsoft.AspNet.Identity;
using PagedList;
using PagedList.Mvc;

namespace MMOasisAlajuela.Controllers
{
    public class UsersController : Controller
    {
        private UsersBL UserBL = new UsersBL();
        private RolesBL RolesBL = new RolesBL();

        // GET: Users
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return View(UserBL.UsersList().ToList());
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            if (Request.IsAuthenticated)
            {
                Users User = new Users();

                var Roles = from r in RolesBL.Roles()
                            where r.ActiveFlag == true
                            select r;

                User.RolesList = Roles.ToList();

                return View(User);
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Users NewUser)
        {
            
                var ValidationUserName = UserBL.CheckUserNameAvailability(NewUser.UserName);
                var ValidationEmail = UserBL.CheckEmailAvailability(NewUser.Email);

                if (ValidationUserName.Count == 0 && ValidationEmail.Count == 0)
                {
                    NewUser.Password = "Wxyz1234";

                    string InsertUser = User.Identity.GetUserName();

                    var r = UserBL.AddNewUser(NewUser, InsertUser);

                    if (!r)
                    {
                        ViewBag.Mensaje = "Ha ocurrido un error inesperado";
                        return View("~/Views/Shared/Error.cshtml");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    if (ValidationUserName.Count > 0 && ValidationEmail.Count > 0)
                    {
                        this.ModelState.AddModelError(String.Empty, "Este nombre de usuario y este email ya se encuentran registrados, por favor intente con otro correo y otro nombre de usuario.");
                    }
                    else
                    {
                        if (ValidationUserName.Count > 0)
                        {
                            this.ModelState.AddModelError(String.Empty, "Este nombre de usuario ya se encuentra registrado, por favor intente con otro nombre de usuario.");
                        }
                        else
                        {
                            this.ModelState.AddModelError(String.Empty, "Este correo electrónico ya se encuentra registrado, por favor intente con otro email.");
                        }
                    }
                }
            return View(NewUser);
        }

        // GET: Users/Edit/1
        public ActionResult Edit(int id = 0)
        {
            if ((Request.IsAuthenticated))
            {
                Users User = UserBL.Details(id);

                var Roles = from r in RolesBL.Roles()
                            where r.ActiveFlag == true
                            select r;

                User.RolesList = Roles.ToList();

                ViewBag.UserName = User.UserName.ToString();

                return View(User);
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }

        // POST: User/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit (Users UserEdit)
        {
           UserEdit.ActionType = "GUDP";

           string InsertUser = User.Identity.GetUserName();

           var r = UserBL.UpdateUser(UserEdit, InsertUser);

            if (!r)
            {
                ViewBag.Mensaje = "Ha ocurrido un error inesperado";
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: User/ActiveFlag
        public ActionResult ActiveFlag(int id, bool status)
        {
            if ((Request.IsAuthenticated))
            {
                Users UserEdit = UserBL.Details(id);

                UserEdit.ActionType = "MS";

                if (status == true)
                {
                    UserEdit.ActiveFlag = false;
                }
                else
                {
                    UserEdit.ActiveFlag = true;
                }

                string InsertUser = User.Identity.GetUserName();

                var r = UserBL.UpdateUser(UserEdit, InsertUser);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    return this.RedirectToAction("Index", "Users");
                }
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }

        // GET: User/Authorize/1
        public ActionResult Authorize(int userid)
        {
            if ((Request.IsAuthenticated))
            {
                Users UserEdit = UserBL.Details(userid);

                UserAuthorization UserAuth = new UserAuthorization();

                UserAuth.UserID = userid;

                var Roles = from r in RolesBL.Roles()
                            where r.ActiveFlag == true
                            select r;

                UserAuth.RolesList = Roles.ToList();
                
                ViewBag.UserName = UserEdit.UserName.ToString();

                return View(UserAuth);
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }

        // POST: User/Authorize/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Authorize(UserAuthorization UserAuth)
        {
            if ((Request.IsAuthenticated))
            {
                Users UserEdit = UserBL.Details(UserAuth.UserID);

                UserEdit.RoleID = UserAuth.RoleID;

                UserEdit.ActionType = "AUTH";
                UserAuth.ActionType = "AUTH";

                string InsertUser = User.Identity.GetUserName();

                var r = UserBL.UpdateUser(UserEdit, InsertUser);

                if (!r)
                {
                    ViewBag.Mensaje = "Ha ocurrido un error inesperado";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    var Roles = from rol in RolesBL.Roles()
                                where rol.ActiveFlag == true
                                select rol;

                    UserAuth.RolesList = Roles.ToList();

                    return View(UserAuth);
                }
            }
            else
            {
                return this.RedirectToAction("Login", "Account");
            }
        }
    }
}