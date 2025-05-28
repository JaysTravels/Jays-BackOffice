using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Jays_BackOffice.Models
{
    public class DeeplinkModel
    {
        
        [Display(Name = "Deeplink ID")]
        public int DeeplinkId { get; set; }

        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Country Name")]
        public string? CountryName { get; set; }

        [Display(Name = "City Name 1")]
        public string? CityName1 { get; set; }

        [Display(Name = "Price 1")]
        public decimal? Price1 { get; set; }

        [Display(Name = "City Name 2")]
        public string? CityName2 { get; set; }

        [Display(Name = "Price 2")]
        public decimal? Price2 { get; set; }

        [Display(Name = "City Name 3")]
        public string? CityName3 { get; set; }

        [Display(Name = "Price 3")]
        public decimal? Price3 { get; set; }

        [Display(Name = "Adults")]
        public int? Adults { get; set; } // Corrected "Aduts" to "Adults"

        [Display(Name = "Children")]
        public int? Children { get; set; }

        [Display(Name = "Infant")]
        public int? Infant { get; set; }

        [Display(Name = "Departure Date")]
        public string? DepartureDate { get; set; }

        [Display(Name = "Return Date")]
        public string? ReturnDate { get; set; }

        [Display(Name = "Origin")]
        public string? Origin { get; set; }

        [Display(Name = "Destination")]
        public string? Destination { get; set; }

        [Display(Name = "Is Active")]
        public bool? IsActive { get; set; }

        [Display(Name = "Created On")]
        public DateTime? CreatedOn { get; set; }

      
        [Display(Name = "Cabin Class")]
        public string? CabinClass { get; set; }

        [Display(Name = "Flight Type")]
        public string? FlightType { get; set; }
    }
}
