using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using WEB_Assignment.Models;
using System.Data;
using WEB_Assignment.Controllers;
using System.Text.RegularExpressions;
using System.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WEB_Assignment.DAL
{
    public class StaffDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        
        // get Schedule id by staff id
        public int GetScheduleID(int? ID)
        {
            SqlConnection conn2;
            string strConnection = Configuration.GetConnectionString("AirFlightConnectionStrings");
            //Represent SqlConnection Object with connection read
            conn2 = new SqlConnection(strConnection);
            int ScheduleID = 0;
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn2.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"Select ScheduleID From FlightCrew Where StaffID =@ID";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@ID", ID);
            //A connection to database must be opened before any operations made.
            conn2.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            while (reader.Read())
            {
                ScheduleID = reader.GetInt32(0);
            }
            conn2.Close();
            //Close DataReader
            conn2.Close();
            reader.Close();
            //Close the database connection
            return ScheduleID;
        }

        //get flgth number by Schedule id
        public string GetFlightNumber(int? ID)
        {
            SqlConnection conn2;
            string strConnection = Configuration.GetConnectionString("AirFlightConnectionStrings");
            //Represent SqlConnection Object with connection read
            conn2 = new SqlConnection(strConnection);
            string FlightNumber = "";
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn2.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"Select FlightNumber From FlightSchedule Where ScheduleID =@ID";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@ID", ID);
            //A connection to database must be opened before any operations made.
            conn2.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            while (reader.Read())
            {
                FlightNumber = reader.GetString(0);
            }
            conn2.Close();
            //Close DataReader
            conn2.Close();
            reader.Close();
            //Close the database connection
            return FlightNumber;
        }
        //get Date of departure by selected schedule ID
        public DateTime? getDPD(int? ID)
        {
            SqlConnection conn2;
            string strConnection = Configuration.GetConnectionString("AirFlightConnectionStrings");
            //Represent SqlConnection Object with connection read
            conn2 = new SqlConnection(strConnection);
            DateTime? DPD = new DateTime();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn2.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"Select DepartureDateTime From FlightSchedule Where ScheduleID =@ID";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@ID", ID);
            //A connection to database must be opened before any operations made.
            conn2.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            while (reader.Read())
            {
                DPD = reader.GetDateTime(0);
            }
            conn2.Close();
            //Close DataReader
            //conn2.Close();
            reader.Close();
            int schid = GetScheduleID(ID);
            //For personnel with no schedules assigned to them
            //if (schid == null)
            //{
            //    DPD = null;
            //}

            //Close the database connection
            return DPD;
        }

        // get Role by selected Staff ID
        public string GetRole(int? ID)
        {
            SqlConnection conn2;
            string strConnection = Configuration.GetConnectionString("AirFlightConnectionStrings");
            //Represent SqlConnection Object with connection read
            conn2 = new SqlConnection(strConnection);
            string Role = "";
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn2.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"Select Role From FlightCrew Where StaffID =@ID";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@ID", ID);
            //A connection to database must be opened before any operations made.
            conn2.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            while (reader.Read())
            {
                Role = reader.GetString(0);
            }
            conn2.Close();
            //Close DataReader
            conn2.Close();
            reader.Close();
            //Close the database connection
            return Role;
        }

        public string GetStatus(int? ID)
        {
            SqlConnection conn2;
            string strConnection = Configuration.GetConnectionString("AirFlightConnectionStrings");
            //Represent SqlConnection Object with connection read
            conn2 = new SqlConnection(strConnection);
            string Status = "";
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn2.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"Select Status From Staff Where StaffID =@ID";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@ID", ID);
            //A connection to database must be opened before any operations made.
            conn2.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            while (reader.Read())
            {
                Status = reader.GetString(0);
            }
            conn2.Close();
            //Close DataReader
            conn2.Close();
            reader.Close();
            //Close the database connection
            return Status;
        }

        public StaffDAL()
        {
            //To read connectionString set in appsettings.json file for flight system
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var builder = configurationBuilder;

            Configuration = builder.Build();

            //Getting the connection string on startup file
            string strConnection = Configuration.GetConnectionString("AirFlightConnectionStrings");

            //Represent SqlConnection Object with connection read
            conn = new SqlConnection(strConnection);
        }
        
        // Flight Schedule list
        public List<FlightViewModel> GetFlightSchedule()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM FlightSchedule";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            List<FlightViewModel> Flightslist = new List<FlightViewModel>();
            while (reader.Read())
            {
                FlightViewModel flightVM = new FlightViewModel();
                // Fill staff object with values from the data reader
                flightVM.FlightScheduleID = reader.GetInt32(0);

                flightVM.FlightNo = reader.GetString(1);

                flightVM.RouteID = reader.GetInt32(2);

                flightVM.AircraftID = reader.GetInt32(3);

                flightVM.DPD = !reader.IsDBNull(4) ? reader.GetDateTime(4) : (DateTime?)null;

                flightVM.ADT = !reader.IsDBNull(5) ? reader.GetDateTime(5) : (DateTime?)null;

                flightVM.EconomyClassPrice = reader.GetSqlMoney(6);

                flightVM.BusinessClassPrice = reader.GetSqlMoney(7);

                flightVM.Statusflight = !reader.IsDBNull(8) ? reader.GetString(8) : null;
                
                Flightslist.Add(flightVM);
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return Flightslist;
        }
        // get selected Schdule details
        public FlightCrewModel GetSchduledetails(int FlightScheduleID)
        {
            FlightCrewModel flightVM = new FlightCrewModel();
            List<FlightCrewModel> Flightslist = new List<FlightCrewModel>();
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM FlightSchedule WHERE ScheduleID = @Search";

            cmd.Parameters.AddWithValue("@Search", FlightScheduleID);
            conn.Open();
            //Execute SELeCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    // Fill staff object with values from the data reader
                    flightVM.FlightNo = GetFlightNumber(reader.GetInt32(0));
                    flightVM.FlightScheduleID = reader.GetInt32(0);
                    flightVM.DPD = getDPD(reader.GetInt32(0));
                    //flightVM.StaffID = reader.GetInt32(1);

                    //flightVM.Role = reader.GetString(2);

                    Flightslist.Add(flightVM);
                }
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return flightVM;
        }
        //Search Shedule ID
        public List<FlightViewModel> SearchScheduleID(int FlightScheduleID)
        {
            FlightViewModel flightVM = new FlightViewModel();
            List<FlightViewModel> Flightslist = new List<FlightViewModel>();
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM FlightSchedule WHERE ScheduleID = @Search";

            cmd.Parameters.AddWithValue("@Search", FlightScheduleID);
            conn.Open();
            //Execute SELeCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    // Fill staff object with values from the data reader
                    flightVM.FlightScheduleID = reader.GetInt32(0);

                    flightVM.FlightNo = reader.GetString(1);

                    flightVM.RouteID = reader.GetInt32(2);

                    flightVM.AircraftID = reader.GetInt32(3);

                    flightVM.DPD = !reader.IsDBNull(4) ? reader.GetDateTime(4) : (DateTime?)null;

                    flightVM.ADT = !reader.IsDBNull(5) ? reader.GetDateTime(5) : (DateTime?)null;

                    flightVM.EconomyClassPrice = reader.GetSqlMoney(6);

                    flightVM.BusinessClassPrice = reader.GetSqlMoney(7);

                    flightVM.Statusflight = !reader.IsDBNull(8) ? reader.GetString(8) : null;
                    Flightslist.Add(flightVM);
                }
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return Flightslist;
        }

        //display Captian Pilot list
        public List<SelectListItem> DisplayCaptaiPilot()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT StaffID,StaffName From Staff WHERE Vocation = 'Pilot' And Status = 'Active'";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            while (reader.Read())
            {
                items.Add(new SelectListItem
                {
                    Text = reader.GetString(1),
                    Value = reader.GetInt32(0).ToString()
                });
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return items;
        }
        //display Captian Flight attendant  list
        public List<SelectListItem> DisplayCabinCrewLeader()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT StaffID,StaffName From Staff WHERE Vocation = 'Flight Attendant' And Status = 'Active'";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            while (reader.Read())
            {
                items.Add(new SelectListItem
                {
                    Text = reader.GetString(1).ToString(),
                    Value = reader.GetInt32(0).ToString()
                    
                });
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return items;
        }
        
        //Staff list
        public List<StaffViewModel> GetAllStaff()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Staff ORDER BY StaffID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            List<StaffViewModel> staffList = new List<StaffViewModel>();
            while (reader.Read())
            {
                StaffViewModel staff = new StaffViewModel();
                // Fill staff object with values from the data reader
                staff.ScheduleID = GetScheduleID(reader.GetInt32(0));

                staff.StaffID = reader.GetInt32(0);

                staff.StaffName = !reader.IsDBNull(1) ? reader.GetString(1) : null;
                // (char) 0 - ASCII Code 0 - null value
                //[0]
                staff.Gender = !reader.IsDBNull(2) ?
                reader.GetString(2)[0] : (char)0;

                staff.dateOfemployed = !reader.IsDBNull(3) ?
                reader.GetDateTime(3) : (DateTime?)null;

                staff.Vocation = !reader.IsDBNull(4) ? reader.GetString(4) : null;

                staff.emailAddress = !reader.IsDBNull(5) ?
                reader.GetString(5) : null;

                staff.status = !reader.IsDBNull(6) ? reader.GetString(7) : null;
                staffList.Add(staff);
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return staffList;
        }

        // List of personnel Pilot and flight attendant
        public List<StaffViewModel> GetAllPersonnel()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"select * from staff where Staff.Vocation != 'Administrator'";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            List<StaffViewModel> staffList = new List<StaffViewModel>();
            while (reader.Read())
            {
                StaffViewModel staff = new StaffViewModel();
                // Fill staff object with values from the data reader
                staff.ScheduleID = GetScheduleID(reader.GetInt32(0));

                staff.DPD = getDPD(reader.GetInt32(0));

                staff.StaffID = reader.GetInt32(0);

                staff.StaffName = !reader.IsDBNull(1) ? reader.GetString(1) : null;
                // (char) 0 - ASCII Code 0 - null value
                //[0]
                staff.Gender = !reader.IsDBNull(2) ?
                reader.GetString(2)[0] : (char)0;

                staff.dateOfemployed = !reader.IsDBNull(3) ?
                reader.GetDateTime(3) : (DateTime?)null;

                staff.Vocation = !reader.IsDBNull(4) ? reader.GetString(4) : null;

                staff.emailAddress = !reader.IsDBNull(5) ?
                reader.GetString(5) : null;

                staff.status = !reader.IsDBNull(6) ? reader.GetString(7) : null;
                staffList.Add(staff);
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return staffList;
        }

        //Create Staff
        public int Add(Staff staff)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO Staff (StaffName, Gender,DateEmployed,Vocation,EmailAddr,Status) OUTPUT INSERTED.StaffID VALUES(@name, @gender,@dateEmployed,@vocation,@email,@status)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@name", staff.Name);
            cmd.Parameters.AddWithValue("@gender", staff.Gender);
            cmd.Parameters.AddWithValue("@dateEmployed", staff.DOE);
            cmd.Parameters.AddWithValue("@email", staff.EmailAddr);
            cmd.Parameters.AddWithValue("@vocation", staff.Vocation);
            cmd.Parameters.AddWithValue("@status", staff.Status);

            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            staff.StaffID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return staff.StaffID;
        }

        //Validate Each personnel bettwen His/ her current Date of departure and selected date of departure
        public bool ValidatePersonnel(DateTime? date, int staffID)
        {
            bool hasRow = false;
            String dates = date?.ToString("yyyy-MM-dd HH:mm:ss.000");
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"select DepartureDateTime from FlightCrew INNER JOIN FlightSchedule on FlightCrew.ScheduleID = FlightSchedule.ScheduleID WHERE FlightCrew.StaffID = @StaffID and (FlightSchedule.DepartureDateTime <= @selectedDate AND FlightSchedule.ArrivalDateTime >= @selectedDate)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@selectedDate", dates);
            cmd.Parameters.AddWithValue("@StaffID", staffID);
            conn.Open();
            //Execute SELeCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                hasRow = true;

            }
            else
            {
                hasRow = false;
            }
            reader.Close();
            //Close the database connection
            conn.Close();
            return hasRow;
        }
        //insert personnel to a selected schedule and role
        public int AddStaffToShedule(FlightCrewModel Flighcrew)
        {
            
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO FlightCrew (StaffID,ScheduleID,Role) OUTPUT INSERTED.StaffID VALUES(@staffid, @FlightScheduleID,@Role)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@staffid", Flighcrew.StaffID);
            cmd.Parameters.AddWithValue("@FlightScheduleID", Flighcrew.FlightScheduleID);
            cmd.Parameters.AddWithValue("@Role", "Flight Captain");
            
            SqlCommand cmd1 = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd1.CommandText = @"INSERT INTO FlightCrew (StaffID,ScheduleID,Role) OUTPUT INSERTED.StaffID VALUES(@staffid, @FlightScheduleID,@Role)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd1.Parameters.AddWithValue("@staffid", Flighcrew.StaffID1);
            cmd1.Parameters.AddWithValue("@FlightScheduleID", Flighcrew.FlightScheduleID);
            cmd1.Parameters.AddWithValue("@Role", "Second Pilot");

            SqlCommand cmd2 = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd2.CommandText = @"INSERT INTO FlightCrew (StaffID,ScheduleID,Role) OUTPUT INSERTED.StaffID VALUES(@staffid, @FlightScheduleID,@Role)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd2.Parameters.AddWithValue("@staffid", Flighcrew.StaffID2);
            cmd2.Parameters.AddWithValue("@FlightScheduleID", Flighcrew.FlightScheduleID);
            cmd2.Parameters.AddWithValue("@Role", "Cabin Crew Leader");

            SqlCommand cmd3 = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd3.CommandText = @"INSERT INTO FlightCrew (StaffID,ScheduleID,Role) OUTPUT INSERTED.StaffID VALUES(@staffid, @FlightScheduleID,@Role)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd3.Parameters.AddWithValue("@staffid", Flighcrew.StaffID3);
            cmd3.Parameters.AddWithValue("@FlightScheduleID", Flighcrew.FlightScheduleID);
            cmd3.Parameters.AddWithValue("@Role", "Flight Attendant");

            SqlCommand cmd4 = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd4.CommandText = @"INSERT INTO FlightCrew (StaffID,ScheduleID,Role) OUTPUT INSERTED.StaffID VALUES(@staffid, @FlightScheduleID,@Role)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd4.Parameters.AddWithValue("@staffid", Flighcrew.StaffID4);
            cmd4.Parameters.AddWithValue("@FlightScheduleID", Flighcrew.FlightScheduleID);
            cmd4.Parameters.AddWithValue("@Role", "Flight Attendant");

            SqlCommand cmd5 = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd5.CommandText = @"INSERT INTO FlightCrew (StaffID,ScheduleID,Role) OUTPUT INSERTED.StaffID VALUES(@staffid, @FlightScheduleID,@Role)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd5.Parameters.AddWithValue("@staffid", Flighcrew.StaffID5);
            cmd5.Parameters.AddWithValue("@FlightScheduleID", Flighcrew.FlightScheduleID);
            cmd5.Parameters.AddWithValue("@Role", "Flight Attendant");


            //A connection to database must be opened before any operations made.
            conn.Open();
            int rowAffected = 0;
            //Execute the Add sql to Add the staff to flighcrew
            rowAffected += (int)cmd.ExecuteScalar();
            rowAffected += (int)cmd1.ExecuteScalar();
            rowAffected += (int)cmd2.ExecuteScalar();
            rowAffected += (int)cmd3.ExecuteScalar();
            rowAffected += (int)cmd4.ExecuteScalar();
            rowAffected += (int)cmd5.ExecuteScalar();
            //Close database connection
            conn.Close();
            //Return number of row of staff that just added 
            return rowAffected;
            ////ExecuteScalar is used to retrieve the auto-generated
            ////StaffID after executing the INSERT SQL statement
            //Flighcrew.StaffID = (int)cmd.ExecuteScalar();
            ////A connection should be closed after operations.
            //conn.Close();
            ////Return id when no error occurs.
            //return Flighcrew.StaffID;

        }

        // Search staff
        public List<StaffViewModel> SearchAct(string Name)
        {
            StaffViewModel staff = new StaffViewModel();
            List<StaffViewModel> staffList = new List<StaffViewModel>();
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM Staff WHERE StaffName = @Search";

            cmd.Parameters.AddWithValue("@Search", Name);
            conn.Open();
            //Execute SELeCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    staff.ScheduleID = GetScheduleID(reader.GetInt32(0));

                    staff.StaffID = reader.GetInt32(0);

                    staff.StaffName = !reader.IsDBNull(1) ? reader.GetString(1) : null;
                    // (char) 0 - ASCII Code 0 - null value
                    //[0]
                    staff.Gender = !reader.IsDBNull(2) ?
                    reader.GetString(2)[0] : (char)0;

                    staff.dateOfemployed = !reader.IsDBNull(3) ?
                    reader.GetDateTime(3) : (DateTime?)null;

                    staff.Vocation = !reader.IsDBNull(4) ? reader.GetString(4) : null;

                    staff.emailAddress = !reader.IsDBNull(5) ?
                    reader.GetString(5) : null;

                    staff.status = !reader.IsDBNull(6) ? reader.GetString(7) : null;
                    staffList.Add(staff);
                }
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return staffList;
        }
        //search personnel
        public List<FlightCrewModel> SearchPersonnel(int ID)
        {
            FlightCrewModel Personnel = new FlightCrewModel();
            List<FlightCrewModel> PersonnelList = new List<FlightCrewModel>();
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM Staff WHERE StaffName = @Search";

            cmd.Parameters.AddWithValue("@Search", ID);
            conn.Open();
            //Execute SELeCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    Personnel.FlightScheduleID = GetScheduleID(reader.GetInt32(0));

                    Personnel.StaffID = reader.GetInt32(0);

                    Personnel.Role = GetRole(ID);
                    // (char) 0 - ASCII Code 0 - null value
                    //[0]
                    Personnel.StaffName = reader.GetString(1);

                    Personnel.Status = !reader.IsDBNull(6) ? reader.GetString(7) : null;
                    PersonnelList.Add(Personnel);
                }
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return PersonnelList;
        }
        //update personnel status
        public int Update(StaffViewModel Personnel)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE Staff SET Status=@status WHERE StaffID = @selectedStaffID";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@status", Personnel.status);
            cmd.Parameters.AddWithValue("@selectedStaffID", Personnel.StaffID);
            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
            return count;
        }
        // this is a list for personnel that have more than 4 schedule
        public List<StaffViewModel> PersonnelInactive()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT StaffID FROM FlightCrew GROUP BY StaffID having Count(*)>4";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            List<StaffViewModel> PersonnelList = new List<StaffViewModel>();
            while (reader.Read())
            {
                StaffViewModel personnel = new StaffViewModel();
                // Fill staff object with values from the data reader
                personnel.StaffID = reader.GetInt32(0);
                PersonnelList.Add(personnel);
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return PersonnelList;
        }
        public List<StaffViewModel> PersonnelActive()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT StaffID FROM FlightCrew GROUP BY StaffID having Count(*)< 5";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            List<StaffViewModel> PersonnelList = new List<StaffViewModel>();
            while (reader.Read())
            {
                StaffViewModel personnel = new StaffViewModel();
                // Fill staff object with values from the data reader
                personnel.StaffID = reader.GetInt32(0);
                PersonnelList.Add(personnel);
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return PersonnelList;
        }

        //this to Change the personnel status to inactive. Selected staffID we use the PersonnelHaveShedule list
        public int AuomatedUpdateStatusInactive(int Personnel)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE Staff SET Status=@status WHERE StaffID = @selectedStaffID";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@status", "Inactive");
            cmd.Parameters.AddWithValue("@selectedStaffID", Personnel);
            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
            return count;
        }
        public int AuomatedUpdateStatusActive(int Personnel)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE Staff SET Status=@status WHERE StaffID = @selectedStaffID";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@status", "Active");
            cmd.Parameters.AddWithValue("@selectedStaffID", Personnel);
            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
            return count;
        }
        public Staff GetDetails(int StaffID)
        {
            Staff staff = new Staff();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a staff record.
            cmd.CommandText = @"SELECT * FROM Staff WHERE StaffID = @selectedStaffID";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “staffId”.
            cmd.Parameters.AddWithValue("@selectedStaffID", StaffID);

            //Open a database connection
            conn.Open();
            //Execute SELeCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    // Fill staff object with values from the data reader
                    staff.StaffID = StaffID;
                    staff.Name = !reader.IsDBNull(1) ? reader.GetString(1) : null;
                    // (char) 0 - ASCII Code 0 - null value
                    //[0]
                    staff.Gender = !reader.IsDBNull(2) ?
                    reader.GetString(2)[0] : (char)0;

                    staff.DOE = !reader.IsDBNull(3) ?
                    reader.GetDateTime(3) : (DateTime?)null;

                    staff.Vocation = !reader.IsDBNull(4) ? reader.GetString(4) : null;

                    staff.EmailAddr = !reader.IsDBNull(5) ?
                    reader.GetString(5) : null;

                    staff.Status = !reader.IsDBNull(6) ? reader.GetString(7) : null;
                }
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return staff;
        }
        public StaffViewModel GetPersonnelDetails(int StaffID)
        {
            StaffViewModel personnel = new StaffViewModel();
            //list of schedule that each personnel have
            List<DateTime?> listdpd = new List<DateTime?>();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a staff record.
            cmd.CommandText = @"SELECT * FROM Staff WHERE StaffID = @selectedStaffID";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “staffId”.
            cmd.Parameters.AddWithValue("@selectedStaffID", StaffID);

            //Open a database connection
            conn.Open();
            //Execute SELeCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    // Fill staff object with values from the data reader
                    personnel.StaffID = StaffID;
                    personnel.ScheduleID = GetScheduleID(reader.GetInt32(0));

                    listdpd.Add(getDPD(personnel.ScheduleID));
                    personnel.StaffName = !reader.IsDBNull(1) ? reader.GetString(1) : null;
                    // (char) 0 - ASCII Code 0 - null value
                    //[0]
                    personnel.Gender = !reader.IsDBNull(2) ?
                    reader.GetString(2)[0] : (char)0;

                    personnel.dateOfemployed = !reader.IsDBNull(3) ?
                    reader.GetDateTime(3) : (DateTime?)null;

                    personnel.Vocation = !reader.IsDBNull(4) ? reader.GetString(4) : null;

                    personnel.emailAddress = !reader.IsDBNull(5) ?
                    reader.GetString(5) : null;

                    personnel.status = !reader.IsDBNull(6) ? reader.GetString(7) : null;
                }
            }

            personnel.listdpd = listdpd;

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return personnel;
        }

        public int Delete(int StaffID)
        {
            //Create sqlcommand object, supply it with a DELETE SQL statement
            //to delete staff contact records specified by a Staff ID
            SqlCommand cmd1 = conn.CreateCommand();
            cmd1.CommandText = @"DELETE FROM FlightCrew WHERE StaffID = @selectStaffID";
            cmd1.Parameters.AddWithValue("@selectStaffID", StaffID);

            //Instantiate a SqlCommand object, supply it with a DELETE SQL statement
            //to delete a staff record specified by a Staff ID
            SqlCommand cmd2 = conn.CreateCommand();
            cmd2.CommandText = @"DELETE FROM Staff
                              WHERE StaffID = @selectStaffID";
            cmd2.Parameters.AddWithValue("@selectStaffID", StaffID);

            //Open a database connection
            conn.Open();
            int rowAffected = 0;
            //Execute the DELETE SQL to remove the staff record
            rowAffected += cmd1.ExecuteNonQuery();
            rowAffected += cmd2.ExecuteNonQuery();
            //Close database connection
            conn.Close();
            //Return number of row of staff record updated or deleted
            return rowAffected;
        }

        public bool IsEmailExist(string EmailAddr, int staffID)
        {
            bool emailFound = false;
            //Create a SqlCommand object and specify the SQL statement
            //to get a staff record with the email address to be validated
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT StaffID FROM Staff
                WHERE EmailAddr=@selectedEmail";
            cmd.Parameters.AddWithValue("@selectedEmail", EmailAddr);
            //Open a database connection and excute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {
                    if (reader.GetInt32(0) != staffID)
                        //The email address is used by another staff
                        emailFound = true;
                }
            }
            else
            { //No record
                emailFound = false; // The email address given does not exist
            }
            reader.Close();
            conn.Close();
            return emailFound;
        }
    }
}