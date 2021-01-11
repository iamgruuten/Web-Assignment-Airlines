using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WEB_Assignment.Models
{
    public class StaffViewModel
    {
        [Display(Name = "Staff ID")]
        public int StaffID { get; set; }

        [Display(Name = "Name")]
        public string StaffName { get; set; }

        [Display(Name = "ScheduleID")]
        public int ScheduleID { get; set; }

        public string Vocation { get; set; }
        public char Gender { get; set; }

        [Display(Name = "Departure Date")]
        public DateTime? DPD { get; set; }

        public List<DateTime?> listdpd { get; set; }


        [Display(Name = "Email Address")]
        public string emailAddress { get; set; }

        [Display(Name = "Date of Employed")]
        public DateTime? dateOfemployed { get; set; }

        [Display(Name = "Status")]
        public string status { get; set; }
    }
}