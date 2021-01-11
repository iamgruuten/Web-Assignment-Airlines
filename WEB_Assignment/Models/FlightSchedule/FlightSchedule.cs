using System;
using System.ComponentModel.DataAnnotations;

namespace WEB_Assignment.Models.FlightSchedule
{
    public class FlightSchedule
    {
        public int ScheduleID { get; set; }

        [Required]
        [Display(Name = "Flight Number")]
        public string FlightNumber { get; set; }

        [Required]
        [Display(Name = "Route ID")]
        public int RouteID { get; set; }

        [Required]
        [Display(Name = "Aircraft ID")]
        public int? AircraftID { get; set; }

        [Required]
        [FlightScheduleValidateDate]
        [Display(Name = "Departure Date/Time")]
        public DateTime? DepartureDateTime { get; set; }

        public DateTime? ArrivalDateTime { get; set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Price needs to be more than 0")]
        [Display(Name = "Economy Class Price")]
        public decimal EconomyClassPrice { get; set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Price needs to be more than 0")]
        [Display(Name = "Business Class Price")]
        public decimal BusinessClassPrice { get; set; }

        public string Status { get; set; }
    }
}