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
using Microsoft.AspNetCore.Identity;
namespace FilmUpMovie.Pages.Combos
{
    [Authorize(Roles = Constants.ComboAdministratorsRole)]
    public class IndexModel : PageModel
    {
        private readonly FilmUpMovieContext _context;
        private readonly IAuthorizationService _authorizationService;

        public IndexModel(FilmUpMovieContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        public IList<Combo> Combos { get; set; }

        public async Task OnGetAsync()
        {
            // Start with all combos
            var combosQuery = _context.Combos.AsQueryable();

            // Check if the user is authorized as an admin, manager, or owner
            var isAuthorized = User.IsInRole("ComboAdministrators") || User.IsInRole("ComboManager") || User.IsInRole("ComboOwner");

            // If the user is not authorized, show only approved combos
            if (!isAuthorized)
            {
                combosQuery = combosQuery.Where(c => c.Status == ComboStatus.Approved);
            }

            // Retrieve the filtered list of combos
            Combos = await combosQuery.ToListAsync();
        }
    }
}
