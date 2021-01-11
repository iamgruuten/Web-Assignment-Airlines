using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace WEB_Assignment.Models
{
    public class PassengerViewModel
    {
        public List<int> bookingID { get; set; }

        public List<int> customerID { get; set; }

        public List<int> scheduleID { get; set; }

        [DataType(DataType.Text)]
        public List<PassengerModel> passengerDetails { get; set; }

        public List<string> seatClass { get; set; }

        public List<SqlMoney> amtPayable { get; set; }

        public List<DateTime> dateTimeCreated { get; set; }
    }
}