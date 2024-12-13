using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FilmUpMovie.Data;
using FilmUpMovie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FilmUpMovie.Authorization;

namespace FilmUpMovie.Pages.Foods
{
    [AllowAnonymous]
    public class IndexModel : DI_BasePageModel
    {
        private readonly FilmUpMovieContext _context;

        public IndexModel(
            FilmUpMovieContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager) : base(context, authorizationService, userManager)
        {
            _context = context;
        }

        public IList<Food> Food { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var FoodsIQ = _context.Foods.AsQueryable();

            // Authorization check for FoodManager or FoodAdmin roles
            var isAuthorized = User.IsInRole(Constants.FoodManagerRole) || User.IsInRole(Constants.FoodAdministratorsRole);
            var currentUserId = UserManager.GetUserId(User);

            // Show only approved foods or foods owned by the current user if not authorized
            if (!isAuthorized)
            {
                FoodsIQ = FoodsIQ.Where(f => f.Status == FoodStatus.Approved || f.OwnerID == currentUserId);
            }

            // Retrieve the filtered list of foods
            Food = await FoodsIQ.ToListAsync();
        }
    }
}
