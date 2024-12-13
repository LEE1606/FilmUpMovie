using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace FilmUpMovie.Models
{
    
        public class Cart
        {
            [Key]
            public int CartId { get; set; }

            public List<CartItem> Items { get; set; }
        }

  }
