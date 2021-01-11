using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;
using WEB_Assignment.Models;

namespace WEB_Assignment.DAL
{
    public class UserDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        //Empty Constructor for customer details
        public UserDAL()
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

        //Get Customer Details
        public CustomerModel findCustomer(string EmailAddress, string Password)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                SqlDataReader dr;

                cmd.CommandText = @"SELECT * from Customer WHERE LOWER(EmailAddr) LIKE LOWER(@emailaddr) COLLATE SQL_Latin1_General_CP1_CS_AS and Password = @password COLLATE SQL_Latin1_General_CP1_CS_AS";

                cmd.Parameters.AddWithValue("@emailaddr", EmailAddress);
                cmd.Parameters.AddWithValue("@password", Password);

                //A connection to database must be opened before any operations made.
                conn.Open();

                //ExecuteScalar is used to retrieve the auto-generated
                //StaffID after executing the INSERT SQL statement
                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    CustomerModel customer = new CustomerModel();
                    customer.userID = dr.GetInt32(0);
                    customer.Name = dr.GetString(1);
                    customer.Nationality = dr.GetString(2);
                    customer.dob = dr.GetDateTime(3);
                    customer.phoneNumber = dr.GetString(4);
                    customer.EmailAddress = dr.GetString(5);
                    customer.Password = dr.GetString(6);

                    //A connection should be closed after operations.
                    dr.Close();
                    conn.Close();

                    return customer;
                }
                else
                {
                    //A connection should be closed after operations.
                    conn.Close();

                    return null;
                }
            }
        }

        //TODO Check for email if exist
        public bool IsCustEmailExist(string email)
        {
            bool emailExist = false;

            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM Customer
                                WHERE LOWER(EmailAddr)=LOWER(@emailUser)";
                cmd.Parameters.AddWithValue("@emailUser", email);

                conn.Open();
                SqlDataReader sqlDataReader = cmd.ExecuteReader();

                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        if (sqlDataReader.GetString(5) == email)
                        {
                            //Email Exist
                            emailExist = true;
                        }
                    }
                }
                else
                {
                    //Email Does not exist
                    emailExist = false;
                }

                sqlDataReader.Close();
            }
            conn.Close();

            return emailExist;
        }

        //TODO Check if phone number is valid
        public async System.Threading.Tasks.Task<bool> isPhoneNumberValidAsync(string PhoneNumber, string AreaCode)
        {
            //API Key to validate phone number
            string API_KEY = "vDHJVHzmTCCWcseT0xpPEJqFt4RahiY6ClnLxYxzhdqUftWw";
            string USER_ID = "trioTrufffle";
            HttpClient client = new HttpClient();
            var responseString = await client.GetStringAsync("https://neutrinoapi.net/phone-validate?number=" + AreaCode + PhoneNumber + "&api-key=" + API_KEY + "&user-id=" + USER_ID);

            JObject obj = JObject.Parse(responseString);
            System.Console.WriteLine(obj["valid"]); ;
            return (Boolean)obj["valid"];
        }

        //The add method is used to return auto-generated ID of the customer
        public int AddCustomer(CustomerModel register)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO Customer (CustomerName, Nationality, BirthDate, TelNo, EmailAddr, Password)
                                OUTPUT INSERTED.CustomerID
                                VALUES(@customername, @nationality, @birthdate, @telno, @emailaddr, @password)";

                cmd.Parameters.AddWithValue("@customername", register.Name);
                cmd.Parameters.AddWithValue("@nationality", register.Nationality);
                cmd.Parameters.AddWithValue("@birthdate", register.dob);
                cmd.Parameters.AddWithValue("@telno", "+" + register.countryCode + register.phoneNumber);
                cmd.Parameters.AddWithValue("@emailaddr", register.EmailAddress.ToLower());
                cmd.Parameters.AddWithValue("@password", register.Password);

                //A connection to database must be opened before any operations made.
                conn.Open();

                //ExecuteScalar is used to retrieve the auto-generated
                //StaffID after executing the INSERT SQL statement
                register.userID = (int)cmd.ExecuteScalar();
            }

            //A connection should be closed after operations.
            conn.Close();

            //Return id when no error occurs.
            return register.userID;
        }

        //TODO Update Customer Password
        public int updatePassword(int userID, string oldPassword, string newPassword)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE Customer SET Password=@newPassword
                                WHERE CustomerID = @CustomerID
                                AND Password = @oldPassword";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@newPassword", newPassword);
            cmd.Parameters.AddWithValue("@oldPassword", oldPassword);
            cmd.Parameters.AddWithValue("@CustomerID", userID);

            conn.Open();

            int count = (int)cmd.ExecuteNonQuery();

            conn.Close();

            return count;
        }

        //Get Staff Details
        public StaffModel findStaff(string EmailAddress, string Password)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                SqlDataReader dr;

                cmd.CommandText = @"SELECT * from Staff WHERE EmailAddr = @emailaddr and Password = @password and Vocation  = 'Administrator'";

                cmd.Parameters.AddWithValue("@emailaddr", EmailAddress);
                cmd.Parameters.AddWithValue("@password", Password);

                //A connection to database must be opened before any operations made.
                conn.Open();

                //ExecuteScalar is used to retrieve the auto-generated
                //StaffID after executing the INSERT SQL statement
                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    StaffModel staff = new StaffModel();
                    staff.StaffID = dr.GetInt32(0);
                    staff.StaffName = dr.GetString(1);
                    staff.Gender = dr.GetString(2);
                    staff.DateEmployed = dr.GetDateTime(3);
                    staff.Vocation = dr.GetString(4);
                    staff.EmailAddr = dr.GetString(5);
                    staff.Password = dr.GetString(6);
                    staff.Status = dr.GetString(7);

                    //A connection should be closed after operations.
                    dr.Close();
                    conn.Close();

                    return staff;
                    //TODO: VALID EMAIL OR PASSWORD
                }
                else
                {
                    //A connection should be closed after operations.
                    conn.Close();

                    return null;
                }
            }
        }
    }
}