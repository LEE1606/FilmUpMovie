using FilmUpMovie.Data;
using FilmUpMovie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
//using BlogModel = Blog.Models.Blog;

namespace FilmUpMovie.Pages.Blogging
{
    public class BlogDetailsModel : PageModel
    {
        private readonly FilmUpMovieContext _context;

        public BlogDetailsModel(FilmUpMovieContext context)
        {
            _context = context;
        }

        // The blog details to be displayed
        public Blog Blog { get; set; }

        // Comment text input by the user (for binding in the form)
        [BindProperty]
        public string NewComment { get; set; }

        public void OnGet(int blogId)
        {
            // Fetch the blog with its comments
            Blog = _context.Blogs
                .Include(b => b.Comments)
                .FirstOrDefault(b => b.Id == blogId);

            // If blog is not found, redirect to the error page
            if (Blog == null)
            {
                RedirectToPage("/Error");
            }
        }

        // Method to handle Likes
        public IActionResult OnPostLike(int blogId)
        {
            Blog = _context.Blogs.FirstOrDefault(b => b.Id == blogId);
            if (Blog != null)
            {
                Blog.LikeCount++; // Increment the like count
                _context.SaveChanges(); // Save changes to the database
            }
            return RedirectToPage(new { blogId }); // Redirect back to the same page
        }

        // Method to handle adding a new comment
        public IActionResult OnPostComment(int blogId)
        {
            // Fetch the blog with its comments
            Blog = _context.Blogs.Include(b => b.Comments).FirstOrDefault(b => b.Id == blogId);

            if (Blog != null && !string.IsNullOrWhiteSpace(NewComment))
            {
                // Create and add the new comment
                var comment = new Comment
                {
                    Text = NewComment,
                    BlogId = blogId,
                    DatePosted = DateTime.Now
                };
                _context.Comments.Add(comment); // Add the comment to the database
                _context.SaveChanges(); // Save changes

                // Reload the blog's comments to ensure the count is updated
                Blog = _context.Blogs.Include(b => b.Comments).FirstOrDefault(b => b.Id == blogId);
            }

            // Redirect back to the same page
            return RedirectToPage(new { blogId });
        }

    }
}
