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

namespace FilmUpMovie.Pages.Combos
{
    [Authorize(Roles = "ComboAdministrators, ComboManager, ComboOwner")]
    public class DetailsModel : PageModel
    {
        private readonly FilmUpMovieContext _context;
        private readonly IAuthorizationService _authorizationService;

        public DetailsModel(FilmUpMovieContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        public Combo Combo { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Combos == null)
            {
                return NotFound();
            }

            var combo = await _context.Combos.FirstOrDefaultAsync(m => m.Combo_ID == id);

            if (combo == null)
            {
                return NotFound();
            }

            // Authorization check
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, combo, Operations.Read);
            if (!isAuthorized.Succeeded)
            {
                return Forbid(); // Return Forbidden if not authorized
            }

            Combo = combo;
            return Page();
        }
    }
}
