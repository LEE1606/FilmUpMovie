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
    public class IndexModel : PageModel
    {
        private readonly FilmUpMovieContext _context;

        public IndexModel(FilmUpMovieContext context)
        {
            _context = context;
        }

        public IList<Blog> Blog { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public async Task OnGetAsync()
        {
            var blogs = from b in _context.Blogs
                        select b;

            // Apply search filter if SearchString is not empty
            if (!string.IsNullOrEmpty(SearchString))
            {
                blogs = blogs.Where(b => b.Title.Contains(SearchString));
            }

            Blog = await blogs.ToListAsync();
        }
    }

}
