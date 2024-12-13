using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace FilmUpMovie.Models
{
    public class Movie
    {
        [Key]
        public int ID { get; set; }

        public string? MovieOwnerId { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [Required]
        [StringLength(30)]
        public string Genre { get; set; } = string.Empty;

        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        [StringLength(5)]
        [Required]
        public string Rating { get; set; } = string.Empty;

        [Display(Name = "Poster Image")]
        public byte[] PosterImage { get; set; }

        [NotMapped]
        [Display(Name = "Upload Poster Image")]
        [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Only image files are allowed.")]
        public IFormFile? PosterImageFile { get; set; }

        public MovieStatus Status { get; set; }

        public ICollection<ShowTime> ShowTimes { get; set; }

		public ICollection<Cinema> Cinemas { get; set; }

	}

    public enum MovieStatus
    {
        Pending,
        Available,
        Rejected
    }
}