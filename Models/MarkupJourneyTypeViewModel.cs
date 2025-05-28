namespace Jays_BackOffice.Models
{
    public class MarkupJourneyTypeViewModel
    {
        public int MarkupId { get; set; }
        public int JourneyTypeId { get; set; }
        public string JourneyType { get; set; }
        public bool IsSelected { get; set; } // To track selection
    }
}
