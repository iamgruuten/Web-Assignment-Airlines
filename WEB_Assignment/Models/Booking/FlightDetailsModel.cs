using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace WEB_Assignment.Models
{
    public class FlightDetailsModel
    {
        public int scheduleid { get; set; }

        public string departureCountry { get; set; }
        public string arrivalCountry { get; set; }

        [Display(Name = "Departure")]
        public string departureCity { get; set; }

        [Display(Name = "Arrival")]
        public string arrivalCity { get; set; }

        public DateTime departureTime { get; set; }

        public DateTime arrivalTime { get; set; }

        public DateTime departureDate { get; set; }

        public DateTime arrivalDate { get; set; }

        public string timeTaken { get; set; }

        public SqlMoney businessPrice { get; set; }

        public SqlMoney economyPrice { get; set; }

        public string flightNo { get; set; }

        [Display(Name = "Aircraft")]
        public string flightModel { get; set; }

        public int noOfEcoSeats { get; set; }

        public int noOfBusSeats { get; set; }

        public int noOfEcoSeatsBooked { get; set; }

        public int noOfBusSeatsBooked { get; set; }

        public string costSelected { get; set; }

        public string typeSelected { get; set; }

        [Display(Name = "Booking Date: ")]
        public DateTime BookingTimeCreated { get; set; }

        public FlightDetailsModel()
        {
        }

        public FlightDetailsModel(
            int scheduleid,
            string flightModel,
            string arrivalCountry,
            string departureCountry,
            string departureCity,
            string arrivalCity,
            DateTime departTime,
            DateTime arrivalTime)
        {
            this.scheduleid = scheduleid;
            this.flightModel = flightModel;
            this.departureCity = departureCity;
            this.arrivalCity = arrivalCity;
            departureTime = departTime;
            this.arrivalTime = arrivalTime;
            this.arrivalCountry = arrivalCountry;
            this.departureCountry = departureCountry;
        }
    }
}