﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class ProgramDetails
    {
        [Key]
        public int SPID { get; set; }

        [Required]
        public int ProgramID { get; set; }

        [Required(ErrorMessage ="Este campo es obligatorio")]
        public int SongID { get; set; }

        public int AuthorID { get; set; }

        public bool ActiveFlag { get; set; }

        public List<Songs> SongsList { get; set; }

        public Songs SongsData { get; set; }

        public Authors AuthorsData { get; set; }
        public ProgramDetails()
        {
            SongsData = new Songs();
            AuthorsData = new Authors();
        }
            

    }
}
