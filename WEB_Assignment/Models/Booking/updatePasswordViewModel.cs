using System.ComponentModel.DataAnnotations;

namespace WEB_Assignment.Models
{
    public class updatePasswordViewModel
    {
        [Required(ErrorMessage = "Enter old password")]
        [Display(Name = "Old Password")]
        [DataType(DataType.Password)]
        public string oldPassword { get; set; }

        [Required(ErrorMessage = "Enter new password")]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string newPassword { get; set; }

        [Required(ErrorMessage = "Enter confirm new password")]
        [Display(Name = "Confirm new password")]
        [DataType(DataType.Password)]
        [Compare("newPassword", ErrorMessage = "New password is not matched")]
        public string confirmNewPassword { get; set; }
    }
}