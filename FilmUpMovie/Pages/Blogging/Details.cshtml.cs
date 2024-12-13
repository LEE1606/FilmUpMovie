using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FilmUpMovie.Data;
using FilmUpMovie.Models;

namespace FilmUpMovie.Pages.Blogging
{
    public class DetailsModel : PageModel
    {
        private readonly FilmUpMovieContext _context;

        public DetailsModel(FilmUpMovieContext context)
        {
            _context = context;
        }

        public Blog Blog { get; set; } = default!; // Specify the full namespace for Blog

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FirstOrDefaultAsync(m => m.Id == id); // Use _context.Blogs
            if (blog == null)
            {
                return NotFound();
            }
            else
            {
                Blog = blog;
            }
            return Page();
        }
    }
}
