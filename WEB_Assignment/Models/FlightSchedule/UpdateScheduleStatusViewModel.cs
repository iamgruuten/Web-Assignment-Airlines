using System.ComponentModel.DataAnnotations;

namespace WEB_Assignment.Models.FlightSchedule
{
    public class UpdateScheduleStatusViewModel
    {
        [Display(Name = "Schedule ID")]
        public int ScheduleID { get; set; }

        [Display(Name = "Route ID")]
        public int RouteID { get; set; }

        public string Status { get; set; }
    }
}