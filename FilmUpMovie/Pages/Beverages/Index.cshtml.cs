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

namespace FilmUpMovie.Pages.Beverages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly FilmUpMovieContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(FilmUpMovieContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        public IList<Beverage> Beverage { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var beveragesIQ = _context.Beverages.AsQueryable();

            // Check if the user is authorized as a Beverage Manager or Administrator
            var isAuthorized = User.IsInRole(Constants.BeverageAdministratorsRole) || User.IsInRole(Constants.BeverageManagerRole);
            var currentUserId = _userManager.GetUserId(User);

            // Filter beverages based on authorization
            if (!isAuthorized)
            {
                // Show only approved beverages or beverages owned by the current user
                beveragesIQ = beveragesIQ.Where(b => b.Status == BeverageStatus.Approved || b.OwnerID == currentUserId);
            }

            // Fetch the filtered list of beverages
            Beverage = await beveragesIQ.ToListAsync();
        }
    }
}
