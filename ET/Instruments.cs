using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Instruments
    {
        [Key]
        public int InstrumentID { get; set; }

        [Required]
        [Display(Name ="Instrumento")]
        public string Instrument { get; set; }

        [Required]
        [Display(Name = "Status")]
        public bool ActiveFlag { get; set; }
    }
}
