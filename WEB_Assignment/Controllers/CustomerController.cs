using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using WEB_Assignment.DAL;
using WEB_Assignment.Models;

namespace WEB_Assignment.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private countryList GetAllCountriesRoute()
        {
            //Get a list of branches from database
            countryList countriesList = bookContext.getAvailableCountry();

            return countriesList;
        }

        private List<FlightDetailsModel> GetAllFlightRoute()
        {
            //Get a list of branches from database
            List<FlightDetailsModel> fdm = bookContext.getAllFlights();

            return fdm;
        }

        private BookingDAL bookContext = new BookingDAL();

        public IActionResult updatePassword()
        {
            if (HttpContext.Session.GetString("AuthToken") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        private UserDAL userContent = new UserDAL();

        [HttpPost]
        public IActionResult updatePassword(updatePasswordViewModel updatePassword)
        {
            if (HttpContext.Session.GetString("AuthToken") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            int userID = (int)HttpContext.Session.GetInt32("LoginID");
            int count = userContent.updatePassword(userID, updatePassword.oldPassword, updatePassword.newPassword);

            if (count > 0)
            {
                ModelState.AddModelError("passwordUpdateError", "The password updated successfully");

                return View();
            }
            else
            {
                ModelState.AddModelError("passwordUpdateError", "The password does not match to your account");
                return View();
            }
        }

        [HttpPost]
        public ActionResult BookTrip(BookTripViewModel tripModel)
        {
            if (HttpContext.Session.GetString("AuthToken") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(tripModel);
            }

            if (tripModel.Origin.Contains(",") && tripModel.Destination.Contains(","))
            {
                string[] dest = tripModel.Origin.Split(",");
                string[] arriv = tripModel.Destination.Split(",");

                tripModel.OriginCountry = dest[0];
                tripModel.Origin = dest[1].Trim();

                tripModel.DestinationCountry = arriv[0];
                tripModel.Destination = arriv[1].Trim();
            }

            if (bookContext.IsRouteExist(tripModel.Origin, tripModel.Destination, tripModel.DepartDate))
            {
                //TODO one way trip

                //Two trip only
                HttpContext.Session.SetString("departCity", tripModel.Origin);
                HttpContext.Session.SetString("arrivalCity", tripModel.Destination);
                HttpContext.Session.SetString("departDate", tripModel.DepartDate.ToString());
                HttpContext.Session.SetString("returnDate", tripModel.ReturnDate.ToString());
                HttpContext.Session.SetInt32("numberOfPassenger", (tripModel.NoOfAdult + tripModel.NoOfChild));

                return RedirectToAction("SelectFlight");
            }
            else
            {
                ModelState.AddModelError("routeValid", "Sorry! We do not have route available for now");
                ViewData["AllAvailableFlight"] = GetAllFlightRoute();
                return View(tripModel);
            }
        }

        [HttpPost]
        public ActionResult SelectBookTrip(BookTripViewModel detailsModel)
        {
            if (HttpContext.Session.GetString("AuthToken") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            BookTripViewModel tripViewModel = new BookTripViewModel();
            tripViewModel.Origin = detailsModel.OriginCountry + " ," + detailsModel.Origin;
            tripViewModel.Destination = detailsModel.DestinationCountry + " ," + detailsModel.Destination;
            tripViewModel.DepartDate = detailsModel.DepartDate;
            ViewData["AllAvailableFlight"] = GetAllFlightRoute();

            return View("BookTrip", tripViewModel);
        }

        public IActionResult BookTrip()
        {
            if (HttpContext.Session.GetString("AuthToken") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            TempData["CountryList"] = JsonConvert.SerializeObject(GetAllCountriesRoute());
            ViewData["AllAvailableFlight"] = GetAllFlightRoute();

            return View();
        }

        public IActionResult SelectFlight()
        {
            if (HttpContext.Session.GetString("AuthToken") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            BookTripViewModel bookTripViewModel = new BookTripViewModel();
            bookTripViewModel.DepartDate = Convert.ToDateTime(HttpContext.Session.GetString("departDate"));
            bookTripViewModel.ReturnDate = Convert.ToDateTime(HttpContext.Session.GetString("returnDate"));
            bookTripViewModel.Origin = HttpContext.Session.GetString("departCity");
            bookTripViewModel.Destination = HttpContext.Session.GetString("arrivalCity");

            List<FlightDetailsModel> flightDetailsList = bookContext.getAvailableFlight(bookTripViewModel);
            FlightDetailsViewModel fdvm = new FlightDetailsViewModel();
            fdvm.Allflights = flightDetailsList;
            return View(fdvm);
        }

        [HttpPost]
        public ActionResult SelectFlight(FlightDetailsModel flightDetailsModel)
        {
            if (HttpContext.Session.GetString("AuthToken") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            HttpContext.Session.SetInt32("scheduleID", flightDetailsModel.scheduleid);
            HttpContext.Session.SetString("SeatType", flightDetailsModel.typeSelected);

            FlightDetailsViewModel dvm = new FlightDetailsViewModel();
            dvm.customerID = (int)HttpContext.Session.GetInt32("LoginID");
            dvm.scheduleid = flightDetailsModel.scheduleid;
            dvm.arrivalCity = flightDetailsModel.arrivalCity;
            dvm.arrivalDate = flightDetailsModel.arrivalDate;
            dvm.arrivalTime = flightDetailsModel.arrivalTime;
            dvm.departureCity = flightDetailsModel.departureCity;
            dvm.departureDate = flightDetailsModel.departureDate;
            dvm.departureTime = flightDetailsModel.departureTime;
            dvm.typeSelected = flightDetailsModel.typeSelected;
            dvm.costSelected = flightDetailsModel.costSelected;

            //Storing flight details model that is selected into a session for later use
            HttpContext.Session.SetString("FlightDetailsSelected", JsonConvert.SerializeObject(dvm));

            TempData["arrivalCity"] = flightDetailsModel.arrivalCity;
            TempData["departureCity"] = flightDetailsModel.departureCity;
            TempData["arrivalTime"] = flightDetailsModel.arrivalTime.ToString("HH:mm");
            TempData["arrivalDate"] = flightDetailsModel.arrivalDate.ToString("dd MMMM") + " " + flightDetailsModel.arrivalDate.ToString("dddd");
            TempData["departTime"] = flightDetailsModel.departureTime.ToString("HH:mm");
            TempData["departDate"] = flightDetailsModel.departureDate.ToString("dd MMMM") + " " + flightDetailsModel.departureDate.ToString("dddd");
            TempData["costType"] = flightDetailsModel.costSelected;

            return RedirectToAction("PassengerDetails");
        }

        public IActionResult PassengerDetails()
        {
            if (HttpContext.Session.GetString("AuthToken") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            String passengerDetails = HttpContext.Session.GetString("PassengerDetailsEntered");
            List<PassengerModel> pm = new List<PassengerModel>();
            PassengerViewModel passengerViewModel = new PassengerViewModel();
            if (!String.IsNullOrEmpty(passengerDetails))
            {
                passengerViewModel.passengerDetails = new List<PassengerModel>();

                pm = JsonConvert.DeserializeObject<List<PassengerModel>>(passengerDetails);
                for (int i = 0; i < pm.Count; i++)
                {
                    PassengerModel item = new PassengerModel();
                    item.passengerName = pm[i].passengerName;
                    item.nationality = pm[i].nationality;
                    item.passportNumber = pm[i].passportNumber;
                    item.remarks = pm[i].remarks;
                    passengerViewModel.passengerDetails.Add(item);
                }
            }

            return View(passengerViewModel);
        }

        [HttpPost]
        public ActionResult PassengerDetails(PassengerViewModel passenger)
        {
            if (HttpContext.Session.GetString("AuthToken") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessages"] = ModelState;
                return View(passenger);
            }

            //Flight details selected baased on customer
            string fd = HttpContext.Session.GetString("FlightDetailsSelected");
            FlightDetailsViewModel fdm = JsonConvert.DeserializeObject<FlightDetailsViewModel>(fd);

            //Formatting on the passnger mode in to a list

            List<PassengerModel> passengerList = new List<PassengerModel>();
            for (int i = 0; i < passenger.passengerDetails.Count; i++)
            {
                PassengerModel passengerModel = new PassengerModel();

                string passengerName = passenger.passengerDetails[i].passengerName.Trim();
                passengerModel.passengerName = passengerName;

                string nationality = passenger.passengerDetails[i].nationality.Trim();
                passengerModel.nationality = nationality;

                string passportNumber = passenger.passengerDetails[i].passportNumber.Trim();
                passengerModel.passportNumber = passportNumber;

                if (String.IsNullOrWhiteSpace(passenger.passengerDetails[i].remarks))
                {
                    passenger.passengerDetails[i].remarks = "No remarks";
                }

                passengerModel.remarks = passenger.passengerDetails[i].remarks;
                passengerModel.flightDetailsViewModel = fdm;

                passengerList.Add(passengerModel);
            }

            HttpContext.Session.SetString("PassengerDetailsEntered", JsonConvert.SerializeObject(passengerList));

            return RedirectToAction("ReviewPassenger");
        }

        public IActionResult ReviewPassenger()
        {
            if (HttpContext.Session.GetString("AuthToken") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            String passengerDetails = HttpContext.Session.GetString("PassengerDetailsEntered");
            List<PassengerModel> pm = JsonConvert.DeserializeObject<List<PassengerModel>>(passengerDetails);

            return View(pm);
        }

        public IActionResult CompleteBooking()
        {
            if (HttpContext.Session.GetString("AuthToken") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            String passengerDetails = HttpContext.Session.GetString("PassengerDetailsEntered");
            List<PassengerModel> pm = JsonConvert.DeserializeObject<List<PassengerModel>>(passengerDetails);
            bool complete = bookContext.submitBooking(pm);

            return View();
        }

        [HttpGet]
        public IActionResult viewBooking()
        {
            if (HttpContext.Session.GetString("AuthToken") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            List<FlightDetailsModel> fdm = new List<FlightDetailsModel>();
            fdm = bookContext.viewBooking((int)HttpContext.Session.GetInt32("LoginID"));

            return View(fdm);
        }

        [HttpGet]
        public IActionResult viewBookingPassengers(int scheduleID)
        {
            if (HttpContext.Session.GetString("AuthToken") == null)
            {
                return RedirectToAction("Login", "Home");
            }

            List<PassengerModel> fdm = new List<PassengerModel>();
            fdm = bookContext.getTripPassengers((int)HttpContext.Session.GetInt32("LoginID"), scheduleID);
            ViewData["id"] = scheduleID;

            return View(fdm);
        }

        public async System.Threading.Tasks.Task<IActionResult> PassengerPrintDetailsAsync(int id)
        {
            List<PassengerModel> fdm = new List<PassengerModel>();

            fdm = bookContext.getTripPassengers((int)HttpContext.Session.GetInt32("LoginID"), id);
            fdm = await bookContext.getTripPassengersQRAsync(fdm);

            return new ViewAsPdf("~/Views/Customer/PassengerPrintDetails.cshtml", fdm);
        }

        private CovidDAL covid = new CovidDAL();

        public async System.Threading.Tasks.Task<ActionResult> ViewWorld()
        {
            CovidModel covidCases = await covid.getCovidStatusAsync();

            return View(covidCases);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> ViewWorld(string Country)
        {
            CovidModel covidCases = await covid.getCovidStatusAsync();
            Countries a = covidCases.Countries.Find(x => x.Country.ToUpper() == Country.ToUpper());

            ViewData["CountryStats"] = a;
            if (a == null)
            {
                ModelState.AddModelError("Stats", "No Country Found");
            }
            else
            {
                string imageURL = await covid.getCountryFlagLink(Country);
                int countryPopulation = await covid.GetPopulation(Country);

                ViewData["countryImg"] = imageURL;
                ViewData["affectPop"] = String.Format("{0:n0}", countryPopulation);
            }
            return View(covidCases);
        }
    }
}