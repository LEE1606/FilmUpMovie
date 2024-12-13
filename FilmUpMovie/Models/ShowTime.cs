using System.ComponentModel.DataAnnotations;

namespace
    FilmUpMovie.Models
{
    public class ShowTime
    {
		public int ShowTimeID { get; set; }

        public string? TimeOwnerId { get; set; }

        //public Owner Owner { get; set; }

        public int MovieID { get; set; }

		[StringLength(30)]
		public string Time { get; set; } 

		//public int CinemaID { get; set; }

		public int ShowRoomID { get; set; }

		public ShowRoom ShowRoom { get; set; }

		public Movie Movie { get; set; }

        public ShowTimeStatus Status { get; set; }
        public ICollection<Seat> Seats { get; set; }

    }
    public enum ShowTimeStatus
    {
        Submitted,
        Approved,
        Rejected
    }
}
