using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FilmUpMovie.Data;
using FilmUpMovie.Models;
using FilmUpMovie.Data;

namespace FilmUpMovie.Pages.Blogging
{
    public class DeleteModel : PageModel
    {
        private readonly FilmUpMovieContext _context;

        public DeleteModel(FilmUpMovieContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Blog Blog { get; set; } = default!; // Specify the full namespace for Blog

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FirstOrDefaultAsync(m => m.Id == id);

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }
            var blog = await _context.Blogs.FindAsync(id);

            if (blog != null)
            {
                Blog = blog;
                _context.Blogs.Remove(Blog);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
