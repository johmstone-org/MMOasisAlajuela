using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;


namespace ET
{
    public class MusicSheets
    {
        [Key]
        public int MSID { get; set; }

        [Required]
        [Display(Name ="Tipo")]
        public int MSTypeID { get; set; }

        [Required]
        [Display(Name ="Canción")]
        public int SongID { get; set; }

        [Required]
        [Display(Name ="Instrumento")]
        public int InstrumentID { get; set; }

        [Required]
        [Display(Name ="Tonalidad")]
        public string Tonality { get; set; }

        [Required]
        [Display(Name ="Nombre")]
        public string FileName { get; set; }

        [Required]
        [Display(Name ="Archivo")]   
        public HttpPostedFile FileData { get; set; }

        [Required]
        [Display(Name = "Status")]
        public bool ActiveFlag { get; set; }

    }
}
