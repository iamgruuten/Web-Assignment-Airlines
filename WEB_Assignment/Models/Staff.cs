using System;
using System.ComponentModel.DataAnnotations;

namespace WEB_Assignment.Models
{
    public class Staff
    {
        [Required(ErrorMessage = "Pls Dont leave A Blank")]
        [Display(Name = "Name")]
        [StringLength(50, MinimumLength = 3)]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Display(Name = "StaffID")]
        public int StaffID { get; set; }

        [Required(ErrorMessage = "Pls Dont leave A Blank")]
        [Display(Name = "Email Address")]
        [RegularExpression(@"^[\w\.=-]+@[\w\.-]+\.[\w]{2,3}$", ErrorMessage = "Email Must contain @gmail.com or @yahoo.com")]
        [ValidateEmailExist] // Validation Annotation for email address format
        // Custom Validation Attribute for checking email address exists
        public string EmailAddr { get; set; }

        [Required(ErrorMessage = "Pls Dont leave A Blank")]
        [Display(Name = "Gander")]
        public char Gender { get; set; }

        [Required(ErrorMessage = "Pls Dont leave A Blank")]
        [Display(Name = "Vocation")]
        [DataType(DataType.Text)]
        public string Vocation { get; set; }

        [Required(ErrorMessage = "Pls Dont leave A Blank")]
        [Display(Name = "Date of Employed")]
        public DateTime? DOE { get; set; }

        [Required(ErrorMessage = "Pls Dont leave A Blank")]
        [Display(Name = "Status")]
        [DataType(DataType.Text)]
        public string Status { get; set; }
    }
}