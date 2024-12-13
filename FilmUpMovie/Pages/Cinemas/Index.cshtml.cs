using FilmUpMovie.Authorization;
using FilmUpMovie.Data;
using FilmUpMovie.Models;
using FilmUpMovie.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmUpMovie.Pages.Cinemas
{
    [AllowAnonymous]
    public class IndexModel : DI_BasePageModel
    {
        private readonly FilmUpMovieContext _context;
        private readonly IConfiguration Configuration;

        public IndexModel(FilmUpMovieContext context, IConfiguration configuration,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager) : base(context, authorizationService, userManager)
        {
            _context = context;
            Configuration = configuration;
        }

        
        public string CurrentFilter { get; set; }
        public List<string> Locations { get; set; }

		public List<CinemaViewModels> Cinemas { get; set; }

        public async Task OnGetAsync(string searchString, string location, int? pageIndex)
        {
            // Start building the query for filtering data
            IQueryable<Cinema> cinemasQuery = _context.Cinema.AsQueryable();

            // Search by cinema name
            if (!string.IsNullOrEmpty(searchString))
            {
                CurrentFilter = searchString;
                cinemasQuery = cinemasQuery.Where(c => c.Name.Contains(searchString));
            }

            // Filter by location
            if (!string.IsNullOrEmpty(location))
            {
                cinemasQuery = cinemasQuery.Where(c => c.Location == location);
            }

            // Fetch cinemas from the database
            var cinemas = await cinemasQuery.ToListAsync();

            // Initialize the Cinemas list
            Cinemas = new List<CinemaViewModels>();

            // Get the current user's ID
            var currentUserId = UserManager.GetUserId(User);

            // Compute permissions asynchronously for each cinema
            foreach (var cinema in cinemas)
            {
                // Default: Rejected cinemas are hidden for general users
                var canRead = cinema.Status != CinemaStatus.Rejected || // Not rejected
                              User.IsInRole(Constants.CinemaManagerRole) || // Managers can view all statuses
                              User.IsInRole(Constants.CinemaAdministratorsRole) || // Admins can view all statuses
                              cinema.CinemaOwnerid == currentUserId || // Owners can view their own cinemas
                              (await AuthorizationService.AuthorizeAsync(User, cinema, Operations.Read)).Succeeded; // Explicit authorization

                if (!canRead)
                {
                    continue;
                }

                // Check if the user can edit or delete the cinema
                var canEdit = (await AuthorizationService.AuthorizeAsync(User, cinema, Operations.Update)).Succeeded;
                var canDelete = (await AuthorizationService.AuthorizeAsync(User, cinema, Operations.Delete)).Succeeded;

                // Add cinema to the ViewModel list
                Cinemas.Add(new CinemaViewModels
                {
                    CinemaID = cinema.CinemaID,
                    Name = cinema.Name,
                    Location = cinema.Location,
                    CinemaImage = cinema.CinemaImage,
                    NumOfHall = _context.ShowRoom.Count(s => s.CinemaID == cinema.CinemaID),
                    Status = cinema.Status,
                    CinemaOwnerid = cinema.CinemaOwnerid,

                    // Assign computed permissions
                    CanEdit = canEdit,
                    CanDelete = canDelete
                });
            }

            // Populate locations for the dropdown
            Locations = await _context.Cinema
                .Select(c => c.Location)
                .Distinct()
                .ToListAsync();
        }


    }
}
