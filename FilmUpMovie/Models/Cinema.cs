using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmUpMovie.Models
{
    public class Cinema
    {
        [Key]
        public int CinemaID { get; set; }

        public string? CinemaOwnerid { get; set; }
        //public Owner Owner { get; set; }


        [StringLength(50)]
        public string Name { get; set; }

		[StringLength(50)]
        [Display(Name = "Cinema Location")]
        public string Location { get; set; }

        public byte[] CinemaImage { get; set; }

        [NotMapped]
        [Display(Name = "Upload Cinema Image")]
        [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Only image files are allowed.")]
        public IFormFile? CinemaImageFile { get; set; }

        public int NumOfHall { get; set; }
        public CinemaStatus Status { get; set; }

        //public ICollection<ShowTime> ShowTimes { get; set; }

        public ICollection<Movie> Movies { get; set; }

        public ICollection<ShowRoom> ShowRooms { get; set; }
    }
    public enum CinemaStatus
    {
        Submitted,
        Approved,
        Rejected
    }
}
