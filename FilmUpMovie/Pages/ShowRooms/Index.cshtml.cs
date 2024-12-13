using FilmUpMovie.Authorization;
using FilmUpMovie.Data;
using FilmUpMovie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FilmUpMovie.Pages.ShowRooms
{
    [AllowAnonymous]
    public class IndexModel : DI_BasePageModel
    {
        private readonly FilmUpMovieContext _context;
        private readonly IConfiguration _configuration;

        public IndexModel(FilmUpMovieContext context, IConfiguration configuration,
        IAuthorizationService authorizationService,
        UserManager<IdentityUser> userManager) : base(context, authorizationService, userManager)
        {
            _context = context;
            _configuration = configuration;
        }

        public string CinemaName { get; set; }  // Added to accept cinema name
        public List<ShowRoom> ShowRoom { get; set; }

        public async Task OnGetAsync(string searchString, string cinemaName)
        {
            // Set the CinemaName to the route value
            CinemaName = cinemaName;

            IQueryable<ShowRoom> ShowRoomIQ = _context.ShowRoom.Include(s => s.Cinema);

            // Apply filter by Cinema Name if it's provided
            if (!String.IsNullOrEmpty(cinemaName))
            {
                ShowRoomIQ = ShowRoomIQ.Where(s => s.Cinema.Name.Contains(cinemaName));
            }

            // Apply Search filter
            if (!String.IsNullOrEmpty(searchString))
            {
                ShowRoomIQ = ShowRoomIQ.Where(s => s.Cinema.Name.Contains(searchString));
            }

            // Authorization check
            var isAuthorized = User.IsInRole(Constants.ShowRoomManagersRole) ||
                               User.IsInRole(Constants.ShowRoomAdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);

            // Only approved showrooms are shown unless authorized
            if (!isAuthorized)
            {
                ShowRoomIQ = ShowRoomIQ.Where(c => c.Status == ShowRoomStatus.Approved || c.RoomOwnerId == currentUserId);
            }

            // Retrieve the filtered list of showrooms
            ShowRoom = await ShowRoomIQ.AsNoTracking().ToListAsync();
        }
    }
}
