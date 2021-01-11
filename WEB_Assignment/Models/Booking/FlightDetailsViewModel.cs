using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WEB_Assignment.Models
{
    public class FlightDetailsViewModel
    {
        //This View model is used for customer to select their desired flight
        public List<FlightDetailsModel> Allflights { get; set; }

        public int bookingID { get; set; }

        public int scheduleid { get; set; }

        public int customerID { get; set; }

        public string departureCity { get; set; }
        public string arrivalCity { get; set; }

        [DataType(DataType.Text)]
        public DateTime departureTime { get; set; }

        [DataType(DataType.Text)]
        public DateTime arrivalTime { get; set; }

        [DataType(DataType.Text)]
        public DateTime departureDate { get; set; }

        [DataType(DataType.Text)]
        public DateTime arrivalDate { get; set; }

        [DataType(DataType.Text)]
        public string costSelected { get; set; }

        [DataType(DataType.Text)]
        public string typeSelected { get; set; }

        public FlightDetailsViewModel()
        {
        }
    }
}