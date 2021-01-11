using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net.Http;
using WEB_Assignment.Models;

namespace WEB_Assignment.DAL
{
    public class BookingDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        //Getting available country
        public countryList getAvailableCountry()
        {
            countryList country = new countryList();
            country.arrival = new Dictionary<string, List<string>>();
            country.depart = new Dictionary<string, List<string>>();

            string arrivalCountry, arrivalCity, departCountry, departCity;

            using (SqlCommand cmd = conn.CreateCommand())
            {
                //Getting the availale arrival city and country
                SqlDataReader dr;
                cmd.CommandText = @"SELECT DISTINCT FlightRoute.ArrivalCity, FlightRoute.ArrivalCountry
                                    FROM FlightRoute";

                conn.Open();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        arrivalCity = dr.GetString(0);
                        arrivalCountry = dr.GetString(1);

                        //Add if city available if the key exist, True value
                        if (country.arrival.ContainsKey(arrivalCountry))
                        {
                            country.arrival[arrivalCountry].Add(arrivalCity);
                        }
                        else
                        {
                            country.arrival.Add(arrivalCountry, new List<string>());
                            country.arrival[arrivalCountry].Add(arrivalCity);
                        }
                    }
                }

                //Getting the availale departure city and country
                cmd.CommandText = @"SELECT DISTINCT FlightRoute.DepartureCity, FlightRoute.DepartureCountry
                                    FROM FlightRoute";
                dr.Close();

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        departCity = dr.GetString(0);
                        departCountry = dr.GetString(1);

                        if (country.depart.ContainsKey(departCountry))
                        {
                            country.depart[departCountry].Add(departCity);
                        }
                        else
                        {
                            country.depart.Add(departCountry, new List<string>());
                            country.depart[departCountry].Add(departCity);
                        }
                    }
                }
                conn.Close();
                dr.Close();
            }

            return country;
        }

        public BookingDAL()
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

        //Check if route exist
        public bool IsRouteExist(string DepartureCity, string ArrivalCity, DateTime DepartDate)
        {
            bool routeExist = false;
            using (SqlCommand cmd = conn.CreateCommand())
            {
                SqlDataReader dr;

                //Add date validation TODO

                cmd.CommandText = @"SELECT *
                                    From FlightRoute
                                    INNER JOIN FlightSchedule ON FlightRoute.RouteID = FlightSchedule.RouteID
                                    INNER JOIN Aircraft ON Aircraft.AircraftID = FlightSchedule.AircraftID
                                    WHERE FlightSchedule.Status = 'Opened' AND
                                    FlightRoute.ArrivalCity = @arrivalCity AND
                                    FlightRoute.DepartureCity = @departureCity AND
                                    CONVERT(date, FlightSchedule.ArrivalDateTime) = @arrivalDate";

                cmd.Parameters.AddWithValue("@departureCity", DepartureCity);
                cmd.Parameters.AddWithValue("@arrivalCity", ArrivalCity);
                cmd.Parameters.AddWithValue("@arrivalDate", DepartDate.ToString("yyyy-MM-dd"));

                conn.Open();

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {              //Route Exist
                    routeExist = true;
                }
                else
                {
                    //Route does not exist
                    routeExist = false;
                }
                dr.Close();
            }

            conn.Close();
            return routeExist;
        }

        //Get available routes based on the dates
        public List<FlightDetailsModel> getAvailableFlight(BookTripViewModel bm)
        {
            List<FlightDetailsModel> listFlight = new List<FlightDetailsModel>();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                SqlDataReader dr;

                //To make it readable, the code will be separated into Find Flights then Get number of seats per condition

                cmd.CommandText = @"SELECT FlightRoute.RouteID, FlightRoute.ArrivalCity, FlightRoute.DepartureCity,
                                    FlightSchedule.ArrivalDateTime, FlightSchedule.DepartureDateTime,
                                    FlightSchedule.BusinessClassPrice, FlightSchedule.EconomyClassPrice,
                                    FlightSchedule.FlightNumber, Aircraft.MakeModel, FlightRoute.FlightDuration,
                                    FlightRoute.ArrivalCountry, FlightRoute.DepartureCountry
                                    From FlightRoute
                                    INNER JOIN FlightSchedule ON FlightRoute.RouteID = FlightSchedule.RouteID
                                    INNER JOIN Aircraft ON Aircraft.AircraftID = FlightSchedule.AircraftID
                                    WHERE FlightSchedule.Status = 'Opened' AND
                                    FlightRoute.ArrivalCity = @arrivalCity AND
                                    FlightRoute.DepartureCity = @departureCity AND
                                    CONVERT(date, FlightSchedule.ArrivalDateTime) = @arrivalDate";

                cmd.Parameters.AddWithValue("@departureCity", bm.Origin);
                cmd.Parameters.AddWithValue("@arrivalCity", bm.Destination);
                cmd.Parameters.AddWithValue("@arrivalDate", bm.DepartDate.ToString("yyyy-MM-dd"));

                conn.Open();

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    do
                    {
                        while (dr.Read())
                        {
                            FlightDetailsModel flightDetailsModel = new FlightDetailsModel();

                            flightDetailsModel.scheduleid = dr.GetInt32(0);
                            flightDetailsModel.arrivalCity = dr.GetString(1);
                            flightDetailsModel.departureCity = dr.GetString(2);
                            flightDetailsModel.arrivalDate = dr.GetDateTime(3);
                            flightDetailsModel.departureDate = dr.GetDateTime(4);
                            flightDetailsModel.arrivalTime = dr.GetDateTime(3);
                            flightDetailsModel.departureTime = dr.GetDateTime(4);
                            flightDetailsModel.businessPrice = dr.GetSqlMoney(5);
                            flightDetailsModel.economyPrice = dr.GetSqlMoney(6);
                            flightDetailsModel.flightNo = dr.GetString(7);
                            flightDetailsModel.flightModel = dr.GetString(8);
                            flightDetailsModel.timeTaken = dr.GetInt32(9).ToString();
                            flightDetailsModel.arrivalCountry = dr.GetString(10);
                            flightDetailsModel.departureCountry = dr.GetString(11);
                            listFlight.Add(flightDetailsModel);
                        }
                    } while (dr.NextResult());
                }
                else
                {
                    listFlight = null;
                }
                dr.Close();

                if (listFlight != null)
                {
                    for (int i = 0; i < listFlight.Count; i++)
                    {
                        //Next will be getting the number of seats available
                        cmd.CommandText = @"SELECT FlightSchedule.ScheduleID, Aircraft.NumBusinessSeat, Aircraft.NumEconomySeat, COUNT(CASE WHEN Booking.SeatClass LIKE 'Business' THEN 1 END) AS No_of_business, COUNT(CASE WHEN Booking.SeatClass LIKE 'Economy' THEN 1 END) AS Number_of_Economy
                                    from Aircraft
                                    INNER JOIN FlightSchedule ON FlightSchedule.AircraftID = Aircraft.AircraftID
                                    LEFT JOIN Booking ON Booking.ScheduleID = FlightSchedule.ScheduleID
                                    WHERE FlightSchedule.ScheduleID = @scheduleID
                                    GROUP BY Booking.ScheduleID, FlightSchedule.ScheduleID, Aircraft.NumBusinessSeat, Aircraft.NumEconomySeat";

                        cmd.Parameters.AddWithValue("@scheduleID", listFlight[i].scheduleid);
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                listFlight[i].noOfBusSeats = dr.GetInt32(1);
                                listFlight[i].noOfEcoSeats = dr.GetInt32(2);
                                listFlight[i].noOfBusSeatsBooked = dr.GetInt32(3);
                                listFlight[i].noOfEcoSeatsBooked = dr.GetInt32(4);
                            }
                        }
                        cmd.Parameters.Clear();
                    }

                    //A connection should be closed after operations.
                    dr.Close();
                    conn.Close();
                }

                return listFlight;
            }
        }

        public List<FlightDetailsModel> getAllFlights()
        {
            List<FlightDetailsModel> listAllFlight = new List<FlightDetailsModel>();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                SqlDataReader dr;

                //To make it readable, the code will be separated into Find Flights then Get number of seats per condition

                cmd.CommandText = @"SELECT FlightRoute.RouteID, FlightRoute.ArrivalCity, FlightRoute.DepartureCity,
                                    FlightSchedule.ArrivalDateTime, FlightSchedule.DepartureDateTime,
                                    FlightSchedule.BusinessClassPrice, FlightSchedule.EconomyClassPrice,
                                    FlightSchedule.FlightNumber, Aircraft.MakeModel, FlightRoute.FlightDuration,
                                    FlightRoute.ArrivalCountry, FlightRoute.DepartureCountry
                                    From FlightRoute
                                    INNER JOIN FlightSchedule ON FlightRoute.RouteID = FlightSchedule.RouteID
                                    INNER JOIN Aircraft ON Aircraft.AircraftID = FlightSchedule.AircraftID
                                    WHERE FlightSchedule.Status = 'Opened'";

                conn.Open();

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    do
                    {
                        while (dr.Read())
                        {
                            FlightDetailsModel flightDetailsModel = new FlightDetailsModel();

                            flightDetailsModel.scheduleid = dr.GetInt32(0);
                            flightDetailsModel.arrivalCity = dr.GetString(1);
                            flightDetailsModel.departureCity = dr.GetString(2);
                            flightDetailsModel.arrivalDate = dr.GetDateTime(3);
                            flightDetailsModel.departureDate = dr.GetDateTime(4);
                            flightDetailsModel.arrivalTime = dr.GetDateTime(3);
                            flightDetailsModel.departureTime = dr.GetDateTime(4);
                            flightDetailsModel.businessPrice = dr.GetSqlMoney(5);
                            flightDetailsModel.economyPrice = dr.GetSqlMoney(6);
                            flightDetailsModel.flightNo = dr.GetString(7);
                            flightDetailsModel.flightModel = dr.GetString(8);
                            flightDetailsModel.timeTaken = dr.GetInt32(9).ToString();
                            flightDetailsModel.arrivalCountry = dr.GetString(10);
                            flightDetailsModel.departureCountry = dr.GetString(11);
                            listAllFlight.Add(flightDetailsModel);
                        }
                    } while (dr.NextResult());
                }
                else
                {
                    listAllFlight = null;
                }

                dr.Close();

                if (listAllFlight != null)
                {
                    for (int i = 0; i < listAllFlight.Count; i++)
                    {
                        //Next will be getting the number of seats available
                        cmd.CommandText = @"SELECT FlightSchedule.ScheduleID, Aircraft.NumBusinessSeat, Aircraft.NumEconomySeat, COUNT(CASE WHEN Booking.SeatClass LIKE 'Business' THEN 1 END) AS No_of_business, COUNT(CASE WHEN Booking.SeatClass LIKE 'Economy' THEN 1 END) AS Number_of_Economy
                                    from Aircraft
                                    INNER JOIN FlightSchedule ON FlightSchedule.AircraftID = Aircraft.AircraftID
                                    LEFT JOIN Booking ON Booking.ScheduleID = FlightSchedule.ScheduleID
                                    WHERE FlightSchedule.ScheduleID = @scheID
                                    GROUP BY Booking.ScheduleID, FlightSchedule.ScheduleID, Aircraft.NumBusinessSeat, Aircraft.NumEconomySeat";

                        cmd.Parameters.AddWithValue("@scheID", listAllFlight[i].scheduleid);

                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                listAllFlight[i].noOfBusSeats = dr.GetInt32(1);
                                listAllFlight[i].noOfEcoSeats = dr.GetInt32(2);
                                listAllFlight[i].noOfBusSeatsBooked = dr.GetInt32(3);
                                listAllFlight[i].noOfEcoSeatsBooked = dr.GetInt32(4);
                            }
                        }
                        cmd.Parameters.Clear();
                        dr.Close();
                    }
                }

                //A connection should be closed after operations.
                dr.Close();
                conn.Close();

                return listAllFlight;
            }
        }

        //submit booking
        public bool submitBooking(List<PassengerModel> flightDetailsViewModels)
        {
            int count = 0;
            for (int i = 0; i < flightDetailsViewModels.Count; i++)
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Booking (CustomerID, ScheduleID, PassengerName, PassportNumber, Nationality, SeatClass, AmtPayable, Remarks, DateTimeCreated)
                                OUTPUT INSERTED.BookingID
                                VALUES(@CustomerID, @ScheduleID, @PassengerName, @PassportNumber, @Nationality, @SeatClass,@AmtPayable, @Remarks, getdate())";

                    cmd.Parameters.AddWithValue("@CustomerID", flightDetailsViewModels[i].flightDetailsViewModel.customerID);
                    cmd.Parameters.AddWithValue("@ScheduleID", flightDetailsViewModels[i].flightDetailsViewModel.scheduleid);
                    cmd.Parameters.AddWithValue("@PassengerName", flightDetailsViewModels[i].passengerName);
                    cmd.Parameters.AddWithValue("@PassportNumber", flightDetailsViewModels[i].passportNumber);
                    cmd.Parameters.AddWithValue("@Nationality", flightDetailsViewModels[i].nationality);
                    cmd.Parameters.AddWithValue("@SeatClass", flightDetailsViewModels[i].flightDetailsViewModel.typeSelected);
                    cmd.Parameters.AddWithValue("@AmtPayable", flightDetailsViewModels[i].flightDetailsViewModel.costSelected);
                    cmd.Parameters.AddWithValue("@Remarks", flightDetailsViewModels[i].remarks);

                    //A connection to database must be opened before any operations made.
                    conn.Open();

                    //ExecuteScalar is used to retrieve the auto-generated
                    //StaffID after executing the INSERT SQL statement
                    count = (int)cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            if (count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //View Booking based on customer details
        public List<FlightDetailsModel> viewBooking(int customerID)
        {
            List<FlightDetailsModel> fdmList = new List<FlightDetailsModel>();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                SqlDataReader dr;
                cmd.CommandText = @"SELECT Booking.ScheduleID, FlightSchedule.ArrivalDateTime, FlightSchedule.DepartureDateTime, FlightRoute.ArrivalCountry,
                                    FlightRoute.ArrivalCity, FlightRoute.DepartureCity, FlightRoute.DepartureCountry,Aircraft.MakeModel From Booking
                                    INNER JOIN FlightSchedule ON FlightSchedule.ScheduleID = Booking.ScheduleID
                                    INNER JOIN FlightRoute ON FlightRoute.RouteID = FlightSchedule.RouteID
                                    INNER JOIN Aircraft ON Aircraft.AircraftID = FlightSchedule.AircraftID
                                    WHERE Booking.CustomerID = @custID GROUP BY Booking.ScheduleID, FlightSchedule.ArrivalDateTime, FlightSchedule.DepartureDateTime, FlightRoute.ArrivalCountry,
                                    FlightRoute.ArrivalCity, FlightRoute.DepartureCity, FlightRoute.DepartureCountry,Aircraft.MakeModel";

                cmd.Parameters.AddWithValue("@custID", customerID);
                conn.Open();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        int scheduleid = dr.GetInt32(0);
                        DateTime arrivalDateTime = dr.GetDateTime(1);
                        DateTime departDateTime = dr.GetDateTime(2);
                        string arrivalCountry = dr.GetString(3);
                        string arrivalCity = dr.GetString(4);
                        string departCity = dr.GetString(5);
                        string departCountry = dr.GetString(6);
                        string makeModel = dr.GetString(7);

                        FlightDetailsModel fdm = new FlightDetailsModel(scheduleid, makeModel, arrivalCountry, departCountry, departCity, arrivalCity, departDateTime, arrivalDateTime);
                        fdmList.Add(fdm);
                    }
                }
                dr.Close();
                conn.Close();
            }

            return fdmList;
        }

        public List<PassengerModel> getTripPassengers(int customerID, int scheduleID)
        {
            List<PassengerModel> pmList = new List<PassengerModel>();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                SqlDataReader dr;
                cmd.CommandText = @"SELECT Booking.PassengerName, Booking.PassportNumber, Booking.Nationality, Booking.DateTimeCreated, FlightSchedule.DepartureDateTime,
                                    FlightSchedule.ArrivalDateTime, FlightRoute.ArrivalCity, FlightRoute.ArrivalCountry, FlightRoute.DepartureCity, FlightRoute.DepartureCountry, Aircraft.MakeModel, FlightSchedule.FlightNumber,
                                    Booking.BookingID, Booking.ScheduleID, Booking.SeatClass, Booking.Remarks
                                    FROM Booking
                                    INNER JOIN FlightSchedule on FlightSchedule.ScheduleID = Booking.ScheduleID
                                    INNER JOIN FlightRoute on FlightRoute.RouteID = FlightSchedule.RouteID
                                    INNER JOIN Aircraft on Aircraft.AircraftID = FlightSchedule.AircraftID
                                    WHERE Booking.CustomerID = @custID AND Booking.ScheduleID = @schdID";

                cmd.Parameters.AddWithValue("@custID", customerID);
                cmd.Parameters.AddWithValue("@schdID", scheduleID);

                conn.Open();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        FlightDetailsModel fdm = new FlightDetailsModel();
                        PassengerModel passengerModel = new PassengerModel();
                        passengerModel.passengerName = dr.GetString(0);
                        passengerModel.passportNumber = dr.GetString(1);
                        passengerModel.nationality = dr.GetString(2);
                        passengerModel.dateTimeCreated = dr.GetDateTime(3);
                        fdm.departureTime = dr.GetDateTime(4);
                        fdm.arrivalTime = dr.GetDateTime(5);
                        fdm.arrivalCity = dr.GetString(6);
                        fdm.arrivalCountry = dr.GetString(7);
                        fdm.departureCity = dr.GetString(8);
                        fdm.departureCountry = dr.GetString(9);
                        fdm.flightModel = dr.GetString(10);
                        fdm.flightNo = dr.GetString(11);
                        fdm.scheduleid = dr.GetInt32(13);
                        fdm.typeSelected = dr.GetString(14);
                        passengerModel.remarks = dr.GetString(15);

                        passengerModel.flightDetailsModel = fdm;

                        pmList.Add(
                            passengerModel
                            );
                    }
                }
                dr.Close();
                conn.Close();
            }

            return pmList;
        }

        public int getBookingID(PassengerModel passenger)
        {
            int bookingID = 0;

            using (SqlCommand cmd = conn.CreateCommand())
            {
                SqlDataReader dr;
                cmd.CommandText = @"SELECT Booking.BookingID
                                    FROM Booking
                                    WHERE Booking.PassportNumber = @passNo AND Booking.ScheduleID = @schdID";

                cmd.Parameters.AddWithValue("@passNo", passenger.passportNumber);
                cmd.Parameters.AddWithValue("@schdID", passenger.flightDetailsModel.scheduleid);

                conn.Open();
                dr = cmd.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    bookingID = dr.GetInt32(0);
                }
                dr.Close();
                conn.Close();
            }

            return bookingID;
        }

        public async System.Threading.Tasks.Task<List<PassengerModel>> getTripPassengersQRAsync(List<PassengerModel> models)
        {
            foreach (PassengerModel passengerModel in models)
            {
                String dataQr = getBookingID(passengerModel).ToString();
                String uri = "https://api.qrserver.com/v1/create-qr-code/?size=150x150&data=" + dataQr;

                ImageConverter converter = new ImageConverter();

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        using (var response = await client.GetAsync(uri))
                        {
                            response.EnsureSuccessStatusCode();

                            //Converting from stream reader to byte array
                            passengerModel.imageBuffer = StreamToByteArray(await response.Content.ReadAsStreamAsync())
;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: ", ex);
                }
            }

            return models;
        }

        public byte[] StreamToByteArray(Stream input)
        {
            input.Position = 0;
            using (var ms = new MemoryStream())
            {
                int length = System.Convert.ToInt32(input.Length);
                input.CopyTo(ms, length);
                return ms.ToArray();
            }
        }
    }
}