using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using FilmUpMovie.Data;
using FilmUpMovie.Models;

namespace FilmUpMovie.Pages.Payments
{
    public class IndexModel : PageModel
    {
        private readonly FilmUpMovieContext _context;
        private readonly IConfiguration Configuration;

        public IndexModel(FilmUpMovieContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public string CurrentFilter { get; set; }
        public PaginatedList<Payment> Payments { get; set; }

        public async Task OnGetAsync(int? pageIndex, string searchString)
        {
            IQueryable<Payment> paymentsIQ = _context.Payments
                .Include(p => p.Staff);

            if (!String.IsNullOrEmpty(searchString))
            {
                paymentsIQ = paymentsIQ.Where(p => p.Staff.StaffName.Contains(searchString));
            }

            CurrentFilter = searchString;

            var pageSize = Configuration.GetValue("PageSize", 4);
            Payments = await PaginatedList<Payment>.CreateAsync(
                paymentsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
