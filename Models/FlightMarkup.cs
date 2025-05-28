using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reservation_Admin.Models;

public partial class FlightMarkup
{
    [Column("markup_id")]
    public long MarkupId { get; set; }

    [Column("adutl_markup")]
    [Display(Name = "Markup on Adult")]    
    public decimal? AdultMarkup { get; set; }

    [Display(Name = "Markup on Child")]
    [Column("child_markup")]
    public decimal? ChildMarkup { get; set; }
    [Display(Name = "Markup on Infant")]
    [Column("infant_markup")]
    public decimal? InfantMarkup { get; set; }
   // [Column("apply_markup")]
    public bool? ApplyMarkup { get; set; }

    [Display(Name = "Is Percentage")]
    [Column("is_percentage")]
    public bool? IsPercentage { get; set; }
    [Column("airline")]
    public string? Airline { get; set; }
    [Display(Name = "Airline Markup")]
    [Column("airline_markup")]
    public decimal? AirlineMarkup { get; set; }


    [Display(Name = "Airline Discount")]
    [Column("discount_on_airline")]
    public decimal? DiscountOnAirline { get; set; }
    [Display(Name = "Apply Airline Discount")]
    [Column("apply_airline_discount")]
    public bool? ApplyAirlineDiscount { get; set; }
    [Column("meta")]
    public string? Meta { get; set; }
    [Column("meta_markup")]
    [Display(Name = "Meta Markup")]
    public decimal? MetaMarkup { get; set; }

    [Display(Name = "Discount on Meta")]
    [Column("discount_on_meta")]
    public decimal? DiscountOnMeta { get; set; }
    [Column("gds")]
    public string? Gds { get; set; }
    [Display(Name = "Gds markup")]
    [Column("gds_markup")]
    public decimal? GdsMarkup { get; set; }

    [Display(Name = "Marketing Source")]
    [Column("marketing_source")]
    public string? MarketingSource { get; set; }

    [Display(Name = "Marketing Source Markup")]
    [Column("marketing_source_markup")]
    public decimal? MarketingSourceMarkup { get; set; }

    [Display(Name = "Fare type")]
    [Column("fare_type")]
    public string? FareType { get; set; }

    [Display(Name = "Fare type markup")]
    [Column("fare_type_markup")]
    public decimal? FareTypeMarkup { get; set; }

    [Display(Name = "Journy type")]
    [Column("journy_type")]
    public string? JournyType { get; set; }

    [Display(Name = "Journy type markup")]
    [Column("journy_type_markup")]
    public decimal? JournyTypeMarkup { get; set; }

    [Display(Name = "Between hours")]
    [Column("between_hours")]
    public string? BetweenHours { get; set; }

    [Display(Name = "Between hours markup")]
    [Column("between_hours_markup")]
    public decimal? BetweenHoursMarkup { get; set; }

    [Display(Name = "Start Airport")]
    [Column("start_airport")]
    public string? StartAirport { get; set; }

    [Display(Name ="Start Airport markup")]
    [Column("start_airport_markup")]
    public decimal? StartAirportMarkup { get; set; }

    [Display(Name ="End Airport")]
    [Column("end_airport")]
    public string? EndAirport { get; set; }
    [Column("end_airport_markup")]

    [Display(Name = "End Airport Markup")]
    public decimal? EndAirportMarkup { get; set; }
    [Column("created_on")]
    public DateTime CreatedOn { get; set; }

    [Column("from_date")]
    [Display(Name = "FromDate")]
    public DateOnly? FromDate { get; set; }
    [Column("to_date")]
    [Display(Name = "ToDate")]
    public DateOnly? ToDate { get; set; }

    [Display(Name = "Date Markup")]
    [Column("date_markup")]
    public decimal? DateMarkup { get; set; }
}
