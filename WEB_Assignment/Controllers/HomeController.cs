using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using WEB_Assignment.DAL;
using WEB_Assignment.Models;

namespace WEB_Assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") == "Staff")
            {
                return RedirectToAction("Index", "StaffController");
            }
            else if (HttpContext.Session.GetString("Role") == "Customer")
            {
                return RedirectToAction("Index", "CustomerController");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Login account
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Role") == "Staff")
            {
                return RedirectToAction("FlightSchedule", "Staff");
            }
            else if (HttpContext.Session.GetString("Role") == "Customer")
            {
                return RedirectToAction("Index", "Customer");
            }
            return View();
        }

        //POST: Home/Login
        //Find customer
        [HttpPost]
        public ActionResult Login(LoginViewModel login)
        {
            if (login.EmailAddress != null && login.Password != null)
            {
                string email = login.EmailAddress.ToLower();
                string password = login.Password;

                //Find in Customer First
                CustomerModel account = userContext.findCustomer(email, password);
                if (account != null)
                {
                    //Generate Session Token ID for user
                    string guid = Guid.NewGuid().ToString();
                    HttpContext.Session.SetString("AuthToken", guid);

                    //Store Login ID in session with the key "LoginID"
                    HttpContext.Session.SetInt32("LoginID", account.userID);

                    //Store Login ID in session with the key "LoginID"
                    HttpContext.Session.SetString("Name", account.Name);

                    //Store Email in Session with the key "EmailID"
                    HttpContext.Session.SetString("EmailID", account.EmailAddress);

                    //StoreLocation user role "Staff" as a string in session with the key "Role"
                    HttpContext.Session.SetString("Role", "Customer");

                    return RedirectToAction("BookTrip", "Customer");
                }
                else
                {
                    //If not found, Find in Staff
                    StaffModel staff = userContext.findStaff(email, password);
                    if (staff != null)
                    {
                        //Staff account found, Generate Token ID for staff
                        //Generate Session Token ID for staff
                        string guid = Guid.NewGuid().ToString();
                        HttpContext.Session.SetString("AuthToken", guid);

                        //Store Login ID in session with the key "LoginID"
                        HttpContext.Session.SetInt32("LoginID", staff.StaffID);

                        //Store Name in session with the key "LoginID"
                        HttpContext.Session.SetString("StaffName", staff.StaffName);

                        //Store Email in Session with the key "EmailID"
                        HttpContext.Session.SetString("EmailID", staff.EmailAddr);

                        //Store Vocation in Session with the key "VocationID"
                        HttpContext.Session.SetString("VocationID", staff.Vocation);

                        //Store Login ID in session with the key "GenderID"
                        HttpContext.Session.SetString("GenderID", staff.Gender);

                        //Store DateEmployed in Session with the key "DateEmployedID"
                        HttpContext.Session.SetString("DateEmployedID", staff.DateEmployed.ToString());

                        //Store Status in Session with the key "StatusID"
                        HttpContext.Session.SetString("StatusID", staff.Status);

                        //StoreLocation user role "Staff" as a string in session with the key "Role"
                        HttpContext.Session.SetString("Role", "Staff");

                        return RedirectToAction("Index", "Staff");
                    }
                    else
                    {
                        ModelState.AddModelError("CustomError", "The user name or password is incorrect");
                        return View(login);
                    }
                }
            }
            else
            {
                return View(login);
            }
        }

        //Logout Current Session
        public ActionResult LogOut()
        {
            // Clear all key-values pairs stored in session state
            HttpContext.Session.Clear();

            // Redirect logout user to the home page
            return RedirectToAction("Index");
        }

        //Customer Data Layer
        private UserDAL userContext = new UserDAL();

        private List<String> defaultOption = new List<String> { "Yes", "No" };

        public IActionResult Register()
        {
            ViewData["defaultpwd"] = defaultOption;

            return View("Register");
        }

        //POST: Home/Register
        //Creates a customer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Register(CustomerModel register)
        {
            ViewData["defaultpwd"] = defaultOption;

            bool phoneValid = await userContext.isPhoneNumberValidAsync(register.phoneNumber, register.countryCode);

            if (register.defaultPassword == "Yes")
            {
                register.confirmPassword = "p@55Cust";
                register.Password = "p@55Cust";
            }
            if (phoneValid)
            {
                if (ModelState.IsValid)
                {
                    register.userID = userContext.AddCustomer(register);

                    //Redirect user to login Page
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewData["defaultpwd"] = defaultOption;

                    return View(register);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please enter a valid phone number");
                return View(register);
            }
        }
    }
}