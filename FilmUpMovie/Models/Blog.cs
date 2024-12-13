using System.ComponentModel.DataAnnotations;

namespace FilmUpMovie.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [DataType(DataType.Date)]
        public DateTime DatePosted { get; set; }

        public string? ImageUrl { get; set; }

        public int LikeCount { get; set; } = 0; // Number of likes

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        // New read-only property for Comment Count
        public int CommentCount => Comments?.Count ?? 0;
    }
}
