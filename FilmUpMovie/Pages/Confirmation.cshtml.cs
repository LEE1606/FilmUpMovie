using FilmUpMovie.Data;
using FilmUpMovie.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmUpMovie.Pages
{
    public class ConfirmationModel : PageModel
    {
        private readonly FilmUpMovieContext _context;

        public ConfirmationModel(FilmUpMovieContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public ShowTime ShowTime { get; set; }

        [BindProperty]
        public List<int> SelectedSeats { get; set; }

        public decimal TotalCost { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Retrieve the ShowTime details using ShowTimeID from query string
            var showTimeId = HttpContext.Session.GetInt32("ShowTimeID");
            if (!showTimeId.HasValue)
            {
                ErrorMessage = "Showtime not found.";
                return Page();
            }

            ShowTime = await _context.ShowTime
                .Include(st => st.Movie)
                .Include(st => st.ShowRoom)
                .FirstOrDefaultAsync(st => st.ShowTimeID == showTimeId.Value);

            if (ShowTime == null)
            {
                ErrorMessage = "Showtime not found.";
                return Page();
            }

            // Retrieve other details (e.g., SelectedSeats from session)
            var selectedSeatsString = HttpContext.Session.GetString("SelectedSeats");
            if (!string.IsNullOrEmpty(selectedSeatsString))
            {
                SelectedSeats = selectedSeatsString
                    .Split(',')
                    .Select(int.Parse)
                    .ToList();
            }

            // Retrieve TotalCost from session
            var totalCostString = HttpContext.Session.GetString("TotalCost");
            if (!string.IsNullOrEmpty(totalCostString))
            {
                TotalCost = decimal.Parse(totalCostString);
            }

            return Page();
        }
    }
}
