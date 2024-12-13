using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;  // Add this namespace
using System.IO;
using System.Threading.Tasks;
using FilmUpMovie.Data;
using FilmUpMovie.Models;


namespace FilmUpMovie.Pages.Blogging
{
    public class EditModel : PageModel
    {
        private readonly FilmUpMovieContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;  // Use IWebHostEnvironment

        public EditModel(FilmUpMovieContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;  // Inject IWebHostEnvironment
        }

        [BindProperty]
        public Blog Blog { get; set; } = default!;

        [BindProperty]
        public IFormFile? ImageUpload { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Blog = await _context.Blogs.FindAsync(id);

            if (Blog == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var blogInDb = await _context.Blogs.FindAsync(Blog.Id);

            if (blogInDb == null)
            {
                return NotFound();
            }

            // Update fields
            blogInDb.Title = Blog.Title;
            blogInDb.Content = Blog.Content;
            blogInDb.DatePosted = Blog.DatePosted;
      

            // Handle new image upload if available
            if (ImageUpload != null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");  // Corrected usage
                var filePath = Path.Combine(uploadsFolder, ImageUpload.FileName);

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageUpload.CopyToAsync(stream);
                }

                // Update ImageUrl property
                blogInDb.ImageUrl = "/images/" + ImageUpload.FileName;
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
