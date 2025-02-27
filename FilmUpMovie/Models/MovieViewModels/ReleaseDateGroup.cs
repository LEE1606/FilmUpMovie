﻿using System;
using System.ComponentModel.DataAnnotations;

namespace FilmUpMovie.Models.MovieViewModels
{
    public class ReleaseDateGroup
    {
        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        public int MovieCount { get; set; }
    }
}
