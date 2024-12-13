using System.ComponentModel.DataAnnotations;

namespace FilmUpMovie.Models
{
    public class Comment
    {


        [Key]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        public DateTime DatePosted { get; set; } = DateTime.Now;
        public int BlogId { get; set; }
        public Blog Blog { get; set; }

    }

}
