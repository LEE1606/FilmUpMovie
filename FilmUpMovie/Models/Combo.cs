using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmUpMovie.Models
{
    public class Combo
    {
        [Key]
        public int Combo_ID { get; set; }

        public string? OwnerID { get; set; }


        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        public bool IsOnSale { get; set; }

        // Image as byte array (can be base64 string when displayed)
        public byte[] ComboImage { get; set; }

        public ComboStatus Status { get; set; }


    }

    public enum ComboStatus
    {
        Submitted,
        Approved,
        Rejected
    }
}

