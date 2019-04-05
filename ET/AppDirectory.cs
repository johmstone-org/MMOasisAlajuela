using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class AppDirectory
    {
        public int RARProfileID { get; set; }

        [Required]
        public int ApplicationID { get; set; }

        [Required]
        [Display(Name ="Clase Principal")]
        public string MainClass { get; set; }

        [Display(Name ="Nombre")]
        public string AppName { get; set; }

        public string Controller { get; set; }

        [Display(Name ="Página")]
        public string ViewPage { get; set; }

        public string Parameter { get; set; }

        public bool Status { get; set; }
    }
}
