using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FilmUpMovie.Authorization;
using FilmUpMovie.Data;
using FilmUpMovie.Models;
using FilmUpMovie.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FilmUpMovie.Pages.Combos
{
    [Authorize(Roles = "ComboAdministrators")]
    public class EditModel : PageModel
    {
        private readonly FilmUpMovieContext _context;
        private readonly IAuthorizationService _authorizationService;

        public EditModel(FilmUpMovieContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        [BindProperty]
        public ComboViewModel ComboViewModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var combo = await _context.Combos.FindAsync(id);
            if (combo == null)
            {
                return NotFound();
            }

            // Authorization check before editing
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, combo, Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            // Map entity data to the view model
            ComboViewModel = new ComboViewModel
            {
                Name = combo.Name,
                Price = combo.Price,
                Description = combo.Description,
                IsOnSale = combo.IsOnSale
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var combo = await _context.Combos.FindAsync(id);
            if (combo == null)
            {
                return NotFound();
            }

            // Authorization check before updating
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, combo, Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            // Update fields
            combo.Name = ComboViewModel.Name;
            combo.Price = ComboViewModel.Price;
            combo.Description = ComboViewModel.Description;
            combo.IsOnSale = ComboViewModel.IsOnSale;

            // Handle optional file upload
            if (ComboViewModel.ComboImageFile != null)
            {
                // Validate the file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(ComboViewModel.ComboImageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("ComboViewModel.ComboImageFile", "Only .jpg, .jpeg, or .png files are allowed.");
                    return Page();
                }

                // Convert image to byte array
                using (var memoryStream = new MemoryStream())
                {
                    await ComboViewModel.ComboImageFile.CopyToAsync(memoryStream);
                    combo.ComboImage = memoryStream.ToArray();
                }
            }

            try
            {
                _context.Combos.Update(combo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Combos.Any(e => e.Combo_ID == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
    }
}

