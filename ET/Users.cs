using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Users
    {
        [Key]
        [Required]
        public int UserID { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name ="Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Status")]
        public bool ActiveFlag { get; set; }

        [Required]
        [Display(Name = "Usuario Autorizado")]
        public bool AuthorizationFlag { get; set; }

        [Display(Name = "Role")]
        public int? RoleID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        public List<Roles> RolesList { get; set; }

        public Roles RolesData { get; set; }

        public Users()
        {
            RolesData = new Roles();
        }
    }
}
