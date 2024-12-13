using FilmUpMovie.Data;
using FilmUpMovie.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmUpMovie.Pages.Select
{
    public class SelectModel : PageModel
    {
        private readonly FilmUpMovieContext _context;

        public SelectModel(FilmUpMovieContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<int> SelectedSeats { get; set; } = new List<int>();

        [BindProperty(SupportsGet = true)]
        public ShowTime ShowTime { get; set; }

        public List<Seat> Seats { get; set; }
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(int showtimeId)
        {
            ShowTime = await _context.ShowTime
                .Include(st => st.Movie)
                 .Include(st => st.ShowRoom)
                .FirstOrDefaultAsync(st => st.ShowTimeID == showtimeId);

            if (ShowTime == null)
            {
                ErrorMessage = "Showtime not found.";
                return;
            }

            Seats = await _context.Seat
                .Where(s => s.ShowTimeID == showtimeId)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (SelectedSeats == null || SelectedSeats.Count == 0)
            {
                ErrorMessage = "Please select at least one seat.";
                return Page();
            }

            var selectedSeatObjects = await _context.Seat
                .Where(s => SelectedSeats.Contains(s.SeatID))
                .ToListAsync();

            if (selectedSeatObjects.Count == 0)
            {
                ErrorMessage = "Some of the selected seats are not available.";
                return Page();
            }

            foreach (var seat in selectedSeatObjects)
            {
                if (seat.Status == "Booked")
                {
                    ErrorMessage = $"Seat {seat.SeatID} is already booked.";
                    return Page();
                }
                seat.Status = "Booked"; // Mark seat as booked
            }

            await _context.SaveChangesAsync();

            decimal totalCost = selectedSeatObjects.Sum(seat => seat.Price);

            // Store SelectedSeats and TotalCost in session (optional)
            HttpContext.Session.SetString("SelectedSeats", string.Join(",", SelectedSeats));
            HttpContext.Session.SetString("TotalCost", totalCost.ToString("F2")); // Store as string

            // Redirect to the Review page with ShowTimeID in the query string
            return RedirectToPage("/Review", new { showTimeId = ShowTime.ShowTimeID });
        }


    }
}
