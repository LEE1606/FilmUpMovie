using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace FilmUpMovie.Models
{
    public class Beverage
    {
        [Key]
        [Column("B_ID")] // Custom primary key name
        public int B_ID { get; set; } // Primary key for Beverage

        public string? OwnerID { get; set; }


        [Required]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public bool IsNewArrival { get; set; }

        public bool IsOnSale { get; set; }

        public byte[] BeverageImage { get; set; } // This stores the image as binary data in the database.

        public BeverageStatus Status { get; set; }
    }
    public enum BeverageStatus
    {
        Submitted,
        Approved,
        Rejected
    }

}


