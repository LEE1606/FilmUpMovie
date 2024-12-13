using FilmUpMovie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmUpMovie.Pages
{
    public class DownloadReceiptModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public decimal TotalCost { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Seats { get; set; }

        public string ReceiptContent { get; set; }

        public void OnGet()
        {
            // Generate the content for the receipt
            ReceiptContent = $"Receipt\n\nMovie Booking\n" +
                             $"------------------------\n" +
                             $"Seats Booked: {Seats}\n" +
                             $"Total Price: RM {TotalCost.ToString("0.00")}\n" +
                             $"------------------------\n" +
                             $"Thank you for your booking!\n" +
                             $"Enjoy your movie experience!";
        }
    }
}
