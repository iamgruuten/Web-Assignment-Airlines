using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace WEB_Assignment.Models
{
    public class FlightViewModel
    {
        [Display(Name = "Flight Schedule ID")]
        public int FlightScheduleID { get; set; }

        [Display(Name = "Flight No")]
        public string FlightNo { get; set; }

        [Display(Name = "Route ID")]
        public int RouteID { get; set; }

        [Display(Name = "Aircraft ID")]
        public int AircraftID { get; set; }

        [Display(Name = "Departure Date")]
        public DateTime? DPD { get; set; }

        [Display(Name = "Arival DateTime")]
        public DateTime? ADT { get; set; }

        [Display(Name = "Economy Class Price")]
        public SqlMoney EconomyClassPrice { get; set; }

        [Display(Name = "Business Class Price")]
        public SqlMoney BusinessClassPrice { get; set; }

        [Display(Name = "Status of the flight schedule")]
        public string Statusflight { get; set; }
    }
}