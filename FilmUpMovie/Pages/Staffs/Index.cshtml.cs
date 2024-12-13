using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FilmUpMovie.Data;
using FilmUpMovie.Models;

namespace FilmUpMovie.Pages.Staffs
{
    public class IndexModel : PageModel
    {
        private readonly FilmUpMovieContext _context;

        public IndexModel(FilmUpMovieContext context)
        {
            _context = context;
        }

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public PaginatedList<Staff> Staff { get; set; }

        public async Task OnGetAsync(string sortOrder, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";

            CurrentFilter = searchString;

            IQueryable<Staff> StaffIQ = from s in _context.Staffs
                                        select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                StaffIQ = StaffIQ.Where(s => s.StaffName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    StaffIQ = StaffIQ.OrderByDescending(s => s.StaffName);
                    break;
                case "Date":
                    StaffIQ = StaffIQ.OrderBy(s => s.StaffWorkDate);
                    break;
                case "date_desc":
                    StaffIQ = StaffIQ.OrderByDescending(s => s.StaffWorkDate);
                    break;
                default:
                    StaffIQ = StaffIQ.OrderBy(s => s.StaffName);
                    break;
            }

            var pageSize = 5; // Number of records per page
            Staff = await PaginatedList<Staff>.CreateAsync(
                StaffIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
