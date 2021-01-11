using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WEB_Assignment.Models
{
    public class ValidateCustomListLength
    {
        public class ValidateRemarksListLength : ValidationAttribute
        {
            private readonly int _maxElements;

            public ValidateRemarksListLength(int maxElements)
            {
                _maxElements = maxElements;
            }

            public override bool IsValid(object value)
            {
                var list = value as IList;
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null)
                        {
                            return true;
                        }
                        else
                        {
                            return item.ToString().Length <= _maxElements;
                        }
                    }
                }
                return false;
            }
        }

        public class CannotBeEmptyAttribute : ValidationAttribute
        {
            private const string defaultError = "'{0}' must have at least one element.";

            public override bool IsValid(object value)
            {
                IEnumerable list = value as IEnumerable;

                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item != null)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            public override string FormatErrorMessage(string name)
            {
                return String.Format(this.ErrorMessageString, name);
            }
        }

        public class NoStringInListBiggerThanAttribute : ValidationAttribute
        {
            private readonly int length;

            public NoStringInListBiggerThanAttribute(int length)
            {
                this.length = length;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var strings = value as IEnumerable<string>;
                if (strings == null)
                    return ValidationResult.Success;

                var invalid = strings.Where(s => s.Length > length).ToArray();
                if (invalid.Length > 0)
                    return new ValidationResult("The following strings exceed the value: " + string.Join(", ", invalid));

                return ValidationResult.Success;
            }
        }
    }
}