using System;
using System.ComponentModel.DataAnnotations;

//This validation checks whether the departure date is at least a day after the current date

namespace WEB_Assignment.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FlightScheduleValidateDate : ValidationAttribute
    {
        public DateTime MinimumDate { get; set; } //Date to compare to

        public FlightScheduleValidateDate()
        {
            MinimumDate = DateTime.Now.AddDays(1);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (value == null || (DateTime)value < MinimumDate)
            {
                return new ValidationResult("Please select a date at least a day from today.");
            }

            return ValidationResult.Success;
        }
    }
}