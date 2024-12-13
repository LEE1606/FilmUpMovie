using FilmUpMovie.Data;
using FilmUpMovie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
//using BlogModel = Blog.Models.Blog;

namespace FilmUpMovie.Pages.Blogging
{
    public class BlogPageModel : PageModel
    {
        private readonly FilmUpMovieContext _context;

        public BlogPageModel(FilmUpMovieContext context)
        {
            _context = context;
        }

        // List of blogs to display
        public IList<Blog> Blogs { get; set; } = new List<Blog>();

        // Recommended hottest trend post
        public Blog HottestTrend { get; set; }

        // Search term
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public void OnGet()
        {
            // Fetch blogs from the database
            var query = _context.Blogs.Include(b => b.Comments).AsQueryable();

            // Filter by search term if provided
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                query = query.Where(b => b.Title.Contains(SearchTerm) ||
                                         b.Content.Contains(SearchTerm));
            }

            // Order by most recent and convert to list
            Blogs = query.OrderByDescending(b => b.DatePosted).ToList();

            // Find the hottest trend (based on likes + comments count)
            HottestTrend = query
                .OrderByDescending(b => b.LikeCount + b.Comments.Count)
                .FirstOrDefault();
        }
    }
}
