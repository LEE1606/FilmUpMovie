using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting; // Use this namespace for IWebHostEnvironment
using System.IO;
using System.Threading.Tasks;
using FilmUpMovie.Data;
using FilmUpMovie.Models;

namespace FilmUpMovie.Pages.Blogging;
public class CreateModel : PageModel
{
    private readonly FilmUpMovieContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CreateModel(FilmUpMovieContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public Blog Blog { get; set; } = default!;

    [BindProperty]
    public IFormFile? ImageUpload { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || _context.Blogs == null || Blog == null)
        {
            return Page();
        }

        // Handle the file upload if there's an image selected
        if (ImageUpload != null)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images"); // Images folder under wwwroot
            var filePath = Path.Combine(uploadsFolder, ImageUpload.FileName);

            // Create the directory if it doesn't exist
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Save the uploaded file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ImageUpload.CopyToAsync(stream);
            }

            // Save the file path (URL) in the blog's ImageUrl property
            Blog.ImageUrl = "/images/" + ImageUpload.FileName;  // Relative URL to the uploaded image
        }

        _context.Blogs.Add(Blog);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
