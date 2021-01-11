using System;
using System.ComponentModel.DataAnnotations;

namespace WEB_Assignment.Models
{
    [Serializable]
    public class PassengerModel
    {
        [Required(ErrorMessage = "Cannot leave name field blank")]
        [Display(Name = "Name (as in passport)")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Invalid name")]
        public string passengerName { get; set; }

        [Required(ErrorMessage = "Cannot leave nationality field blank")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Invalid Nationality")]
        [Display(Name = "Nationality")]
        [DataType(DataType.Text)]
        public string nationality { get; set; }

        [Required(ErrorMessage = "Cannot leave passport number field blank")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Invalid Passport Number")]
        [Display(Name = "Passport Number")]
        public string passportNumber { get; set; }

        [StringLength(3000)]
        [Display(Name = "Remarks, if any")]
        [DataType(DataType.MultilineText)]
        public string remarks { get; set; }

        public DateTime dateTimeCreated { get; set; }

        public byte[] imageBuffer { get; set; }

        public FlightDetailsViewModel flightDetailsViewModel { get; set; }

        public FlightDetailsModel flightDetailsModel { get; set; }

        public PassengerModel()
        {
        }
    }
}