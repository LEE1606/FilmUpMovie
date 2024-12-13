using System;
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
    public class DetailsModel : PageModel
    {
        private readonly FilmUpMovie.Data.FilmUpMovieContext _context;
        private readonly IAuthorizationService _authorizationService;

        public DetailsModel(FilmUpMovie.Data.FilmUpMovieContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

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

            // Authorization check to ensure the user can view the beverage details
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, beverage, Operations.Read);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            Beverage = beverage;
            return Page();
        }
    }
}
