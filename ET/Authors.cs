using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Authors
    {
        [Key]
        [Required]
        public int AuthorID { get; set; }

        [Required]
        [Display(Name ="Autor")]
        public string AuthorName { get; set; }

        [Required]
        [Display(Name ="Status")]
        public bool ActiveFlag { get; set; }
    }
}
