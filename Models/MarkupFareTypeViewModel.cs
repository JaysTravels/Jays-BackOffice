namespace Jays_BackOffice.Models
{
    public class MarkupFareTypeViewModel
    {
        public int MarkupId { get; set; }
        public int FareTypeId { get; set; }
        public string FareTypeName { get; set; }
        public bool IsSelected { get; set; } // To track selection
    }
}
