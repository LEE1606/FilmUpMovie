using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FilmUpMovie.Models
{
    public class ShowRoom
    {
        public int ShowRoomID { get; set; }

        public string? RoomOwnerId { get; set; }
        //public Owner Owner { get; set; }

        [Required]
        public string RoomNum { get; set; }

        public int TotalSeat { get; set; }

        [Required]
        [StringLength(30)]
        public string Experience { get; set; }

        public int CinemaID { get; set; } 

        public Cinema Cinema { get; set; }

        public ICollection<ShowTime> ShowTimes { get; set; }

        public ShowRoomStatus Status { get; set; }

    }
    public enum ShowRoomStatus
    {
        Submitted,
        Approved,
        Rejected
    }
}
