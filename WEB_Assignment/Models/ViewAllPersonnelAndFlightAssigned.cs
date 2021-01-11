using System.ComponentModel.DataAnnotations;

namespace WEB_Assignment.Models
{
    public class ViewAllPersonnelAndFlightAssigned
    {
        [Required(ErrorMessage = "Pls Dont leave A Blank")]
        [Display(Name = "Flight Scedule")]
        public int FlightSchedule { get; set; }

        [Required(ErrorMessage = "Pls Dont leave A Blank")]
        [Display(Name = "Role")]
        public string Role { get; set; }

        [Display(Name = "Staff ID")]
        public int StaffId { get; set; }

        [Display(Name = "Staff Name")]
        public string StaffName { get; set; }
    }
}