using Jays_BackOffice.DB_Models;
using Microsoft.AspNetCore.Mvc;

namespace Jays_BackOffice.Models
{
    public class MarkupViewModel
    {
        public ApplyMarkup Markup { get; set; }

        [BindProperty]
        public List<MarkupGdsViewModel> SelectedGDS { get; set; }
        [BindProperty]
        public List<MarkupFareTypeViewModel> SelectedFareType { get; set; }
        [BindProperty]
        public List<MarkupDayNameViewModel> SelectedDayName { get; set; }
        [BindProperty]
        public List<MarkupJourneyTypeViewModel> SelectedJournyType { get; set; }
        [BindProperty]
        public List<MarkupAdvertiserViewModel> SelectedSource { get; set; }

    }
}
