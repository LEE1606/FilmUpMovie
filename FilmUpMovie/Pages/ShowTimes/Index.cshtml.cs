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

namespace FilmUpMovie.Pages.ShowTimes
{
    [AllowAnonymous]
    public class IndexModel : DI_BasePageModel
    {
        private readonly FilmUpMovie.Data.FilmUpMovieContext _context;
        private readonly IConfiguration Configuration;

        public IndexModel(FilmUpMovieContext context, IConfiguration configuration,
        IAuthorizationService authorizationService,
        UserManager<IdentityUser> userManager) : base(context, authorizationService, userManager)
        {
            _context = context;
            Configuration = configuration;
        }

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<ShowTime> ShowTime { get; set; }

        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {

            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<ShowTime> ShowTimeIQ = from s in _context.ShowTime
                                              select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                ShowTimeIQ = ShowTimeIQ.Where(s => s.Movie.Title.Contains(searchString));

            }
            switch (sortOrder)
            {
                case "name_desc":
                    ShowTimeIQ = ShowTimeIQ.OrderByDescending(s => s.Movie.Title);
                    break;
                case "Date":
                    ShowTimeIQ = ShowTimeIQ.OrderBy(s => s.Movie.ReleaseDate);
                    break;
                case "date_desc":
                    ShowTimeIQ = ShowTimeIQ.OrderByDescending(s => s.Movie.ReleaseDate);
                    break;
                default:
                    ShowTimeIQ = ShowTimeIQ.OrderBy(s => s.Movie.Title);
                    break;
            }

            var pageSize = Configuration.GetValue("PageSize", 4);


            var isAuthorized = User.IsInRole(Constants.ShowTimeManagersRole) ||
                          User.IsInRole(Constants.ShowTimeAdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);

            // Only approved contacts are shown UNLESS you're authorized to see them
            // or you are the owner.
            if (!isAuthorized)
            {
                ShowTimeIQ = ShowTimeIQ.Where(c => c.Status == ShowTimeStatus.Approved
                                          || c.TimeOwnerId == currentUserId);
            }
  

            ShowTime = await PaginatedList<ShowTime>.CreateAsync(
                           
                ShowTimeIQ.Include(s => s.Movie).Include(c => c.ShowRoom).AsNoTracking(), pageIndex ?? 1, pageSize);

        }
    }
}
