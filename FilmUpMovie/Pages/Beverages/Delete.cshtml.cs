using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FilmUpMovie.Data;
using FilmUpMovie.Models;
using Microsoft.AspNetCore.Authorization;
using FilmUpMovie.Authorization;

namespace FilmUpMovie.Pages.Beverages
{
    [Authorize(Roles = Constants.BeverageAdministratorsRole)]
    public class DeleteModel : PageModel
    {
        private readonly FilmUpMovie.Data.FilmUpMovieContext _context;
        private readonly IAuthorizationService _authorizationService;

        public DeleteModel(FilmUpMovie.Data.FilmUpMovieContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        [BindProperty]
        public Beverage Beverage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Beverages == null)
            {
                return NotFound();
            }

            var beverage = await _context.Beverages.FirstOrDefaultAsync(m => m.B_ID == id);

            if (beverage == null)
            {
                return NotFound();
            }

            // Authorization check for viewing the beverage before deletion
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, beverage, Operations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            Beverage = beverage;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Beverages == null)
            {
                return NotFound();
            }

            var beverage = await _context.Beverages.FindAsync(id);

            if (beverage == null)
            {
                return NotFound();
            }

            // Authorization check before deleting the beverage
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, beverage, Operations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            _context.Beverages.Remove(beverage);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
