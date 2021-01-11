using System;
using System.ComponentModel.DataAnnotations;

namespace WEB_Assignment.Models
{
    public class BookTripViewModel
    {
        [Required(ErrorMessage = "Please select city")]
        [Display(Name = "ORIGIN")]
        public string Origin { get; set; }

        [Required(ErrorMessage = "Please select city")]
        [Display(Name = "DESTINATION")]
        public string Destination { get; set; }

        public string OriginCountry { get; set; }

        public string DestinationCountry { get; set; }

        [Required(ErrorMessage = "Cannot leave depart date blank")]
        [Display(Name = "Depart date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime DepartDate { get; set; }

        //Not in used
        [Display(Name = "Return date")]
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }

        [Required(ErrorMessage = "Need to have at least one passenger")]
        [Range(1, 10)]
        [Display(Name = "No of Adult")]
        public int NoOfAdult { get; set; }

        [Display(Name = "No of Children")]
        [Range(1, 10)]
        public int NoOfChild { get; set; }
    }
}