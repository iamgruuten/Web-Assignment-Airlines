using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using WEB_Assignment.DAL;

namespace WEB_Assignment.Models
{
    public class ValidateEmailExist : ValidationAttribute
    {
        private UserDAL userDAL = new UserDAL();
        private StaffDAL staffDAL = new StaffDAL();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string email = value.ToString();
            Regex regex = new Regex("^[A-Za-z0-9._%+-]+@lca.com$");

            if (ReferenceEquals(validationContext.ObjectInstance.GetType(), typeof(Staff)))
            {
                Staff staff = (Staff)validationContext.ObjectInstance;
                int staffID = staff.StaffID;

                if (staffDAL.IsEmailExist(email, staffID) == true)
                {
                    //Validation failed
                    return new ValidationResult("Email address already exists!");
                }
                else if (!regex.IsMatch(email))
                {
                    //Invalid Domain Email Address
                    return new ValidationResult("Email Domain must be @lca.com!");
                }
                else
                {
                    //Validation passed
                    return ValidationResult.Success;
                }
            }
            if (userDAL.IsCustEmailExist(email) == true)
            {
                //Validation failed
                return new ValidationResult("Email address already exists!");
            }
            else if (regex.IsMatch(email))
            {
                //Invalid Domain Email Address
                return new ValidationResult("Email Domain is not accepted!");
            }
            else
            {
                //Validation passed
                return ValidationResult.Success;
            }
        }
    }
}