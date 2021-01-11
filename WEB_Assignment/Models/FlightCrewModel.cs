using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WEB_Assignment.Models
{
    public class FlightCrewModel
    {
        [Required(ErrorMessage = "Please select a pilot")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Please select a pilots")]
        public List<SelectListItem> FlightCaptains { get; set; }

        public List<SelectListItem> SecondPilot { get; set; }
        public List<SelectListItem> CabinCrewLeader { get; set; }
        public List<SelectListItem> CabinCrew1 { get; set; }
        public List<SelectListItem> CabinCrew2 { get; set; }
        public List<SelectListItem> CabinCrew3 { get; set; }

        [Display(Name = "Staff ID")]
        [Required(ErrorMessage = "Please select personnel")]
        public int StaffID { get; set; }

        [Required(ErrorMessage = "Please select a pilot")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Please select a pilots")]
        public int StaffID1 { get; set; }

        public int StaffID2 { get; set; }
        public int StaffID3 { get; set; }
        public int StaffID4 { get; set; }

        public int StaffID5 { get; set; }

        [Display(Name = "Name")]
        public string StaffName { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Flight Schedule ID")]
        public int FlightScheduleID { get; set; }

        [Display(Name = "Flight Number")]
        public string FlightNo { get; set; }

        [Display(Name = "Departure Date")]
        [DataType(DataType.DateTime)]
        public DateTime? DPD { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}