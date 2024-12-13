using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FilmUpMovie.Models;

namespace FilmUpMovie.ViewModels
{
	public class CinemaViewModels
	{
		public int CinemaID { get; set; }

		public string? CinemaOwnerid { get; set; }

		[StringLength(50)]
		public string Name { get; set; }

		[StringLength(50)]
		[Display(Name = "Cinema Location")]
		public string Location { get; set; }

		public byte[]? CinemaImage { get; set; }

		[NotMapped]
		[Display(Name = "Upload Cinema Image")]
		public IFormFile? CinemaImageFile { get; set; }

		public int NumOfHall { get; set; }

		public CinemaStatus Status { get; set; }

		// Authorization flags
		public bool CanEdit { get; set; }    // True if the user has permission to edit
		public bool CanDelete { get; set; } // True if the user has permission to delete
	}
}
