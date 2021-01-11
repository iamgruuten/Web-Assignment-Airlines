using System;
using System.ComponentModel.DataAnnotations;

namespace WEB_Assignment.Models.FlightSchedule
{
    public class FlightRoute
    {
        public int RouteID { get; set; }

        [Required]
        [Display(Name = "Departure City")]
        public string DepartureCity { get; set; }

        [Required]
        [Display(Name = "Departure Country")]
        public string DepartureCountry { get; set; }

        [Required]
        [Display(Name = "Arrival City")]
        public string ArrivalCity { get; set; }

        [Required]
        [Display(Name = "Arrival Country")]
        public string ArrivalCountry { get; set; }

        [Required]
        [Display(Name = "Duration")]
        [Range(1, 100)]
        public int? FlightDuration { get; set; }

        public override string ToString()
        {
            return RouteID.ToString();
        }
    }
}