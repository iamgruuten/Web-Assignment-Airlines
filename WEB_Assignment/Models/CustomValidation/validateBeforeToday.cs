using System;
using System.ComponentModel.DataAnnotations;

namespace WEB_Assignment.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class validateBeforeToday : ValidationAttribute
    {
        public validateBeforeToday(string errorMessage) : base(errorMessage)
        {
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;

            DateTime? dateTime = value as DateTime?;

            if (dateTime.HasValue)
            {
                //Returns true if date is within range
                return dateTime.Value <= DateTime.Today;
            }

            return false;
        }
    }
}