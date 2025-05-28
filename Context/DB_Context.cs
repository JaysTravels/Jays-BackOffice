using Jays_BackOffice.DB_Models;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Domain.DB_Models;

namespace Jays_BackOffice.Context
{
    public class DB_Context : DbContext
    {
        public DB_Context(DbContextOptions<DB_Context> options) : base(options) { }
        public DbSet<SearchAvailabilityResults> AvailabilityResults { get; set; }
        public DbSet<FlightMarkup> FlightMarkups { get; set; }

        public DbSet<ReservationFlow> ReservationFlow { get; set; }

        public DbSet<FlightInfo> FlightsInfo { get; set; }
        public DbSet<PassengerInfo> PassengersInfo { get; set; }

        public DbSet<BookingInfo> BookingInfo { get; set; }

        public DbSet<Users> Users { get; set; }
       
        public DbSet<ManualPayment> ManulPayments { get; set; }

        public DbSet<MarketingSource> MarketingSources { get; set; }
        public DbSet<GDS> GDS { get; set; }

        public DbSet<DayName> DayName { get; set; }

        public DbSet<FareType> FareTypes { get; set; }

        public DbSet<ApplyMarkup> Markups { get; set; }

        public DbSet<MarkupDay> MarkupDay { get; set; }

        public DbSet<MarkupGDS> MarkupGds { get; set; }

        public DbSet<MarkupFareType> MarkupFareTypes { get; set; }

        public DbSet<MarkupMarketingSource> MarkupMarketingSources { get; set; }

        public DbSet<MarkupJournyType> MarkupJournyTypes { get; set; }

        public DbSet<JourneyType> JourneyTypes { get; set; }

        public DbSet<Deeplink> Deeplinks { get; set; }
        public DbSet<Jays_BackOffice.DB_Models.ActiveUser> ActiveUser { get; set; } = default!;
    }
}
