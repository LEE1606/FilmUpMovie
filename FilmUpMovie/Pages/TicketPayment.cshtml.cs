using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using FilmUpMovie.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmUpMovie.Models;
using System.Security.Claims; // Add this for claim access

namespace FilmUpMovie.Pages
{
    public class TicketPaymentModel : PageModel
    {
        private readonly FilmUpMovieContext _context;

        public TicketPaymentModel(FilmUpMovieContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public decimal Amount { get; set; }

        [BindProperty]
        public int ShowTimeID { get; set; }

        public ShowTime Showtime { get; set; }

        [BindProperty]
        public List<string> SelectedSeats { get; set; }

        public decimal TotalCost { get; set; }

        // OnGetAsync to handle payment page setup
        public async Task<IActionResult> OnGetAsync(decimal? totalCost, string seats)
        {
            if (totalCost.HasValue && !string.IsNullOrEmpty(seats))
            {
                // Handle new payment page request
                TotalCost = totalCost.Value;
                SelectedSeats = seats.Split(',').ToList();

                // Store data in session
                HttpContext.Session.SetString("TotalCost", TotalCost.ToString());
                HttpContext.Session.SetString("SelectedSeats", string.Join(",", SelectedSeats));
                int? showTimeId = HttpContext.Session.GetInt32("ShowTimeID");

                if (showTimeId.HasValue)
                {
                    ShowTimeID = showTimeId.Value;  // Assign ShowTimeID
                }
                // Retrieve the email of the logged-in user (from claims)
                Email = User.FindFirstValue(ClaimTypes.Email); // This will fetch the email from the claims
            }
            else
            {
                // Handle case where data is missing
                return RedirectToPage("/Error");
            }

            return Page();
        }

    }
}
