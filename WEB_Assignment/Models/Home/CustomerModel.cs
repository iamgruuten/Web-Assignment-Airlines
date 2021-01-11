using System;
using System.ComponentModel.DataAnnotations;

namespace WEB_Assignment.Models
{
    public class CustomerModel
    {
        [Display(Name = "ID")]
        public int userID { get; set; }

        [Required(ErrorMessage = "Cannot leave name blank")]
        [Display(Name = "Name")]
        [DataType(DataType.Text)]
        public String Name { get; set; }

        [Required(ErrorMessage = "Cannot leave nationality blank")]
        [Display(Name = "Nationality")]
        public String Nationality { get; set; }

        [Display(Name = "Phone number, Include Country Code")]
        [Required(ErrorMessage = "Cannot leave phone number blank")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Enter a valid phone number")]
        [DataType(DataType.PhoneNumber)]
        public String phoneNumber { get; set; }

        [Required(ErrorMessage = "Cannot leave country code blank")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Enter a valid phone number")]
        public String countryCode { get; set; }

        [Display(Name = "Passport Number")]
        public String passportNo { get; set; }

        [Required(ErrorMessage = "Cannot leave email blank")]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [ValidateEmailExist]
        public String EmailAddress { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        [validateBeforeToday("Date Of Birth is invalid")]
        public DateTime dob { get; set; }

        [Required(ErrorMessage = "Cannot leave password blank")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        [Display(Name = "Set Password as default (Password will be set to p@55Cust)")]
        public String defaultPassword { get; set; }

        [Required(ErrorMessage = "Cannot leave password blank")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public String confirmPassword { get; set; }
    }
}