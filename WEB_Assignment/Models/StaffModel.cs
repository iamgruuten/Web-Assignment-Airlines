using System;
using System.ComponentModel.DataAnnotations;

namespace WEB_Assignment.Models
{
    public class StaffModel
    {
        public int StaffID { get; set; }

        public string StaffName { get; set; }

        public String Gender { get; set; }

        public DateTime DateEmployed { get; set; }

        public string Vocation { get; set; }

        public string EmailAddr { get; set; }

        public string Password { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Flight Schedule ID")]
        public int FlightScheduleID { get; set; }

        [Display(Name = "Departure City")]
        public string DprtCity { get; set; }

        [Display(Name = "Departure Country")]
        public string DprtCountry { get; set; }

        [Display(Name = "Arrival Country")]
        public string ArrivalCountry { get; set; }

        [Display(Name = "Arrival City")]
        public string ArrivalCity { get; set; }

        [Display(Name = "Flight No")]
        public string FlightNo { get; set; }

        [Display(Name = "Departure Date")]
        public DateTime? DPD { get; set; }

        [Display(Name = "Arival DateTime")]
        public DateTime? ADT { get; set; }

        [Display(Name = "Economy Class Price")]
        public decimal EconomyClassPrice { get; set; }

        [Display(Name = "Business Class Price")]
        public decimal BusinessClassPrice { get; set; }

        [Display(Name = "Route ID")]
        public int RouteID { get; set; }

        [Display(Name = "Aircraft ID")]
        public int AircraftID { get; set; }
    }
}