using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using WEB_Assignment.Models.FlightSchedule;

namespace WEB_Assignment.DAL
{
    public class FlightScheduleDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public FlightScheduleDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("AirFlightConnectionStrings");

            //Instantiate a SqlConnection object with the Connection String read
            conn = new SqlConnection(strConn);
        }

        /*******************************/
        //                              /
        //        Flight Routes         /
        //                              /
        /*******************************/

        public List<FlightRoute> GetSingleFlightRoute(int routeId)
        {
            //Create a SQL Command object from the connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL Statement
            cmd.CommandText = @"SELECT * FROM FlightRoute WHERE RouteID = @routeId";
            //Define the parameters used in SQL statement, value for each parameter
            cmd.Parameters.AddWithValue("@routeId", routeId);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a staff list
            List<FlightRoute> routeList = new List<FlightRoute>();
            using (reader)
            {
                while (reader.Read())
                {
                    routeList.Add(
                        new FlightRoute
                        {
                            RouteID = !reader.IsDBNull(0) ? reader.GetInt32(0) : 0,    //0: 1st column
                            DepartureCity = !reader.IsDBNull(1) ? reader.GetString(1) : null,      //1: 2nd column
                            DepartureCountry = !reader.IsDBNull(2) ? reader.GetString(2) : null, //2: 3rd column
                            ArrivalCity = !reader.IsDBNull(3) ? reader.GetString(3) : null,     //3: 4th column
                            ArrivalCountry = !reader.IsDBNull(4) ? reader.GetString(4) : null,   //5: 6th column
                            FlightDuration = !reader.IsDBNull(5) ? reader.GetInt32(5) : 0,  //6: 7th column
                        }
                    );
                }
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return routeList;
        }

        public List<FlightRoute> GetAllFlightRoutes()
        {
            //Create a SQL Command object from the connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL Statement
            cmd.CommandText = @"SELECT * FROM FlightRoute ORDER BY RouteID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a staff list
            List<FlightRoute> routeList = new List<FlightRoute>();
            using (reader)
            {
                while (reader.Read())
                {
                    routeList.Add(
                        new FlightRoute
                        {
                            RouteID = !reader.IsDBNull(0) ? reader.GetInt32(0) : 0,    //0: 1st column
                            DepartureCity = !reader.IsDBNull(1) ? reader.GetString(1) : null,      //1: 2nd column
                            DepartureCountry = !reader.IsDBNull(2) ? reader.GetString(2) : null, //2: 3rd column
                            ArrivalCity = !reader.IsDBNull(3) ? reader.GetString(3) : null,     //3: 4th column
                            ArrivalCountry = !reader.IsDBNull(4) ? reader.GetString(4) : null,   //5: 6th column
                            FlightDuration = !reader.IsDBNull(5) ? reader.GetInt32(5) : 0,  //6: 7th column
                        }
                    );
                }
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return routeList;
        }

        public int AddFlightRoute(FlightRoute route)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify an INSERT SQL statement which will
            //return the auto-generated RouteID after insertion
            cmd.CommandText = @"INSERT INTO FlightRoute (DepartureCity, DepartureCountry, ArrivalCity, ArrivalCountry, FlightDuration)
                                OUTPUT INSERTED.RouteID
                                VALUES(@dCity, @dCountry, @aCity, @aCountry, @duration)";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@dCity", route.DepartureCity);
            cmd.Parameters.AddWithValue("@dCountry", route.DepartureCountry);
            cmd.Parameters.AddWithValue("@aCity", route.ArrivalCity);
            cmd.Parameters.AddWithValue("@aCountry", route.ArrivalCountry);
            cmd.Parameters.AddWithValue("@duration", route.FlightDuration);

            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //RouteID after executing the INSERT SQL statement
            route.RouteID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return route.RouteID;
        }

        /*******************************/
        //                              /
        //       Flight Schedules       /
        //                              /
        /*******************************/

        public List<FlightSchedule> GetAllFlightSchedules()
        {
            //Create a SQL Command object from the connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL Statement
            cmd.CommandText = @"SELECT * FROM FlightSchedule ORDER BY ScheduleID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a staff list
            List<FlightSchedule> scheduleList = new List<FlightSchedule>();
            using (reader)
            {
                while (reader.Read())
                {
                    scheduleList.Add(
                        new FlightSchedule
                        {
                            ScheduleID = !reader.IsDBNull(0) ? reader.GetInt32(0) : 0,
                            FlightNumber = !reader.IsDBNull(1) ? reader.GetString(1) : null,
                            RouteID = !reader.IsDBNull(2) ? reader.GetInt32(2) : 0,
                            AircraftID = !reader.IsDBNull(3) ? reader.GetInt32(3) : 0,
                            DepartureDateTime = !reader.IsDBNull(4) ? reader.GetDateTime(4) : (DateTime?)null,
                            ArrivalDateTime = !reader.IsDBNull(5) ? reader.GetDateTime(5) : (DateTime?)null,
                            EconomyClassPrice = !reader.IsDBNull(6) ? reader.GetDecimal(6) : 0,
                            BusinessClassPrice = !reader.IsDBNull(7) ? reader.GetDecimal(7) : 0,
                            Status = !reader.IsDBNull(8) ? reader.GetString(8) : null,
                        }
                    );
                }
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return scheduleList;
        }

        public List<FlightSchedule> GetFlightSchedules(int routeId)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL Statement
            cmd.CommandText = @"SELECT * FROM FlightSchedule WHERE RouteID = @routeId ORDER BY ScheduleID";
            //Define the parameters used in SQL statement, value for each parameter
            cmd.Parameters.AddWithValue("@routeId", routeId);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a staff list
            List<FlightSchedule> scheduleList = new List<FlightSchedule>();
            using (reader)
            {
                while (reader.Read())
                {
                    scheduleList.Add(
                        new FlightSchedule
                        {
                            ScheduleID = !reader.IsDBNull(0) ? reader.GetInt32(0) : 0,
                            FlightNumber = !reader.IsDBNull(1) ? reader.GetString(1) : null,
                            RouteID = !reader.IsDBNull(2) ? reader.GetInt32(2) : 0,
                            AircraftID = !reader.IsDBNull(3) ? reader.GetInt32(3) : 0,
                            DepartureDateTime = !reader.IsDBNull(4) ? reader.GetDateTime(4) : (DateTime?)null,
                            ArrivalDateTime = !reader.IsDBNull(5) ? reader.GetDateTime(5) : (DateTime?)null,
                            EconomyClassPrice = !reader.IsDBNull(6) ? reader.GetDecimal(6) : 0,
                            BusinessClassPrice = !reader.IsDBNull(7) ? reader.GetDecimal(7) : 0,
                            Status = !reader.IsDBNull(8) ? reader.GetString(8) : null,
                        }
                    );
                }
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return scheduleList;
        }

        public List<FlightSchedule> GetSingleFlightSchedule(int scheduleId)
        {
            //Create a SQL Command object from the connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL Statement
            cmd.CommandText = @"SELECT * FROM FlightSchedule WHERE ScheduleID = @scheduleId";
            //Define the parameters used in SQL statement, value for each parameter
            cmd.Parameters.AddWithValue("@scheduleId", scheduleId);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a staff list
            List<FlightSchedule> scheduleList = new List<FlightSchedule>();
            using (reader)
            {
                while (reader.Read())
                {
                    scheduleList.Add(
                        new FlightSchedule
                        {
                            ScheduleID = !reader.IsDBNull(0) ? reader.GetInt32(0) : 0,
                            FlightNumber = !reader.IsDBNull(1) ? reader.GetString(1) : null,
                            RouteID = !reader.IsDBNull(2) ? reader.GetInt32(2) : 0,
                            AircraftID = !reader.IsDBNull(3) ? reader.GetInt32(3) : 0,
                            DepartureDateTime = !reader.IsDBNull(4) ? reader.GetDateTime(4) : (DateTime?)null,
                            ArrivalDateTime = !reader.IsDBNull(5) ? reader.GetDateTime(5) : (DateTime?)null,
                            EconomyClassPrice = !reader.IsDBNull(6) ? reader.GetDecimal(6) : 0,
                            BusinessClassPrice = !reader.IsDBNull(7) ? reader.GetDecimal(7) : 0,
                            Status = !reader.IsDBNull(8) ? reader.GetString(8) : null,
                        }
                    );
                }
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return scheduleList;
        }

        public int AddFlightSchedule(FlightSchedule schedule)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify an INSERT SQL statement which will
            //return the auto-generated RouteID after insertion
            cmd.CommandText = @"INSERT INTO FlightSchedule (FlightNumber, AircraftID, RouteID, DepartureDateTime, ArrivalDateTime, EconomyClassPrice, BusinessClassPrice, Status)
                                OUTPUT INSERTED.RouteID
                                VALUES(@fNumber, @aId, @routeId, @dDateTime, @aDateTime, @ePrice, @bPrice, @status)";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@fNumber", schedule.FlightNumber);
            cmd.Parameters.AddWithValue("@aId", schedule.AircraftID);
            cmd.Parameters.AddWithValue("@routeId", schedule.RouteID);
            cmd.Parameters.AddWithValue("@dDateTime", schedule.DepartureDateTime);
            cmd.Parameters.AddWithValue("@aDateTime", schedule.ArrivalDateTime);
            cmd.Parameters.AddWithValue("@ePrice", schedule.EconomyClassPrice);
            cmd.Parameters.AddWithValue("@bPrice", schedule.BusinessClassPrice);
            cmd.Parameters.AddWithValue("@status", schedule.Status);

            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //RouteID after executing the INSERT SQL statement
            schedule.ScheduleID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return schedule.RouteID;
        }

        public int UpdateScheduleStatus(int scheduleId, string status)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE FlightSchedule SET Status=@status WHERE ScheduleID = @sId";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@sId", scheduleId);

            //Open a database connection
            conn.Open();
            //ExecuteNonQUery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();

            return count;
        }

        /*******************************/
        //                              /
        //           Booking            /
        //                              /
        /*******************************/

        public int GetBookingPerSchedule(int scheduleId)
        {
            //Create a SQL Command object from the connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL Statement
            cmd.CommandText = @"SELECT COUNT(BookingID) FROM Booking WHERE ScheduleID = @scheduleId";
            //Define the parameters used in SQL statement, value for each parameter
            cmd.Parameters.AddWithValue("@scheduleId", scheduleId);
            //Open a database connection
            conn.Open();
            //Execute
            int bookings = (int)cmd.ExecuteScalar();
            //Close the database connection
            conn.Close();

            return bookings;
        }
    }
}