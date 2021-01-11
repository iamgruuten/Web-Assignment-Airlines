using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using WEB_Assignment.DAL;
using WEB_Assignment.Models;
using WEB_Assignment.Models.FlightSchedule;

namespace WEB_Assignment.Controllers
{
    public class StaffController : Controller
    {
        /*******************************/
        //                              /
        //           Staff              /
        //                              /
        /*******************************/


        private StaffDAL staffContext = new StaffDAL();

        public ActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") != "Staff")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["name"] = HttpContext.Session.GetString("StaffName");
            return View();
        }

        public IActionResult ViewAllPersonnelAndFlightAssigned()
        {
            //Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if (HttpContext.Session.GetString("Role") != "Staff")
            {
                return RedirectToAction("Login", "Home");
            }
            
            List<StaffViewModel> staffList = staffContext.GetAllStaff();
            
            return View(staffList);
        }

        public ActionResult AssignPersonnelToFlight()
        {
            if (HttpContext.Session.GetString("Role") != "Staff")
            {
                return RedirectToAction("Login", "Home");
            }

            List<FlightViewModel> Flightslist = staffContext.GetFlightSchedule();
            return View(Flightslist);
        }

        [HttpPost]
        public ActionResult AssignPersonnelToFlight(int flightScheduleID)
        {
            if (HttpContext.Session.GetString("Role") != "Staff")
            {
                return RedirectToAction("Login", "Home");
            }

            ViewBag.SearchID = flightScheduleID;
            List<FlightViewModel> flightsList = staffContext.SearchScheduleID(flightScheduleID);
            return View(flightsList);
        }
        // Pass in the data do Delete page
        public StaffViewModel MaptoStaffVM(Staff staff, int? id)
        {
            StaffViewModel staffVM = new StaffViewModel
            {
                StaffID = staff.StaffID,
                StaffName = staff.Name,
                ScheduleID = staffContext.GetScheduleID(id),
                Vocation = staff.Vocation
            };
            return staffVM;
        }
        public ActionResult Details(int id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if (HttpContext.Session.GetString("Role") != "Staff")
            {
                return RedirectToAction("Login", "Home");
            }
            Staff staff = staffContext.GetDetails(id);
            StaffViewModel staffVM = DetailmaptostaffVM(staff);
            return View(staffVM);
        }

        public ActionResult Edit(int? id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if (HttpContext.Session.GetString("Role") != "Staff")
            {
                return RedirectToAction("Login", "Home");
            }
            StaffViewModel personnel = staffContext.GetPersonnelDetails(id.Value);
            //StaffViewModel staffVM = DetailmaptostaffVM(personnel);
            if (personnel == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(personnel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StaffViewModel personnel)
        {
            DateTime Today = DateTime.Now;
            StaffViewModel personnels = staffContext.GetPersonnelDetails(personnel.StaffID);

                // Check Each personnel that have shcedule 
                foreach (DateTime? date in personnels.listdpd)
                {
                    if (date > Today)
                    {
                        ModelState.AddModelError("Schedule", "Please Check the Flight personnel is not assigned to any fligh schedule with departure date of current date or beyond ");
                        return View(personnel);
                    }
                }
                //Update staff record to database
                staffContext.Update(personnel);
                return RedirectToAction("UpdateFlightPersonnelStatus", "Staff");
            
        }
        // Pass the data to Details page
        public StaffViewModel DetailmaptostaffVM(Staff staff)
        {
            StaffViewModel DetailsTostaffVM = new StaffViewModel
            {
                StaffID = staff.StaffID,
                StaffName = staff.Name,
                Gender = staff.Gender,
                dateOfemployed = staff.DOE,
                emailAddress = staff.EmailAddr,
                status = staff.Status,
                Vocation = staff.Vocation
            };

            return DetailsTostaffVM;
        }
        [HttpPost]
        //Search Funtion
        public ActionResult UpdateFlightPersonnelStatus(string Staffname)
        {
            ViewBag.SearchKey = Staffname;
            List<StaffViewModel> staffList = staffContext.SearchAct(Staffname);
            return View(staffList);
        }



        [HttpPost]
        //Search Funtion
        public ActionResult ViewAllPersonnelAndFlightAssigned(string Staffname)
        {
            ViewBag.SearchKey = Staffname;
            List<StaffViewModel> staffList = staffContext.SearchAct(Staffname);
            return View(staffList);
        }

        public ActionResult Delete(int? id)
        {
            //// Stop accessing the action if not logged in
            //// or account not in the "Staff" role
            if (HttpContext.Session.GetString("Role") != "Staff")
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            Staff staff = staffContext.GetDetails(id.Value);
            StaffViewModel staffVM = MaptoStaffVM(staff, id);
            return View(staffVM);
        }

        //validate the admin user
        public ActionResult CreateFlightPersonelRecord()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if
            (HttpContext.Session.GetString("Role") != "Staff")
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["StatusList"] = GetStatus();
            ViewData["VocationList"] = GetVocations();
            return View();
        }

        //data list
        private List<SelectListItem> GetVocations()
        {
            List<SelectListItem> Vocations = new List<SelectListItem>();
            Vocations.Add(new SelectListItem
            {
                Value = "Pilot",
                Text = "Pilot"
            });
            Vocations.Add(new SelectListItem
            {
                Value = "Flight Attendant",
                Text = "Flight Attendant"
            });
            Vocations.Add(new SelectListItem
            {
                Value = "Administrator",
                Text = "Administrator"
            });
            return Vocations;
        }
        // Post
        [HttpPost]
        public ActionResult Select(FlightCrewModel captainPilot)
        {
            List<StaffViewModel> listpersonnel = staffContext.PersonnelInactive();
            List<StaffViewModel> listActivepersonnel = staffContext.PersonnelActive();
            captainPilot.FlightCaptains = staffContext.DisplayCaptaiPilot();
            captainPilot.SecondPilot = staffContext.DisplayCaptaiPilot();
            captainPilot.CabinCrewLeader = staffContext.DisplayCabinCrewLeader();
            captainPilot.CabinCrew1 = staffContext.DisplayCabinCrewLeader();
            captainPilot.CabinCrew2 = staffContext.DisplayCabinCrewLeader();
            captainPilot.CabinCrew3 = staffContext.DisplayCabinCrewLeader();

            var flightCrewModel = captainPilot.FlightCaptains.Find(p => p.Value == captainPilot.StaffID.ToString());

            //validation to Check that each personnel have the exact schedule id and selected schedule id
            if (flightCrewModel != null)
            {
                if (staffContext.ValidatePersonnel(captainPilot.DPD, captainPilot.StaffID))
                {
                    ModelState.AddModelError("Conflict", "Conflict in Captain pilot schedule");
                    return View(captainPilot);
                }

                if (staffContext.ValidatePersonnel(captainPilot.DPD, captainPilot.StaffID1))
                {
                    ModelState.AddModelError("Conflict", "Conflict in Co-Pilot schedule");
                    return View(captainPilot);
                }

                if (staffContext.ValidatePersonnel(captainPilot.DPD, captainPilot.StaffID2))
                {
                    ModelState.AddModelError("Conflict", "Conflict in Crew cabin leader schedule");
                    return View(captainPilot);
                }

                if (staffContext.ValidatePersonnel(captainPilot.DPD, captainPilot.StaffID3))
                {
                    ModelState.AddModelError("Conflict", "Conflict in Flight Attendant 1 schedule");
                    return View(captainPilot);
                }

                if (staffContext.ValidatePersonnel(captainPilot.DPD, captainPilot.StaffID4))
                {
                    ModelState.AddModelError("Conflict", "Conflict in Flight Attendant 2 schedule");
                    return View(captainPilot);
                }

                if (staffContext.ValidatePersonnel(captainPilot.DPD, captainPilot.StaffID5))
                {
                    ModelState.AddModelError("Conflict", "Conflict in Flight Attendant 3 schedule");
                    return View(captainPilot);
                }

                if (captainPilot.StaffID == captainPilot.StaffID1 || captainPilot.StaffID2 == captainPilot.StaffID3 || captainPilot.StaffID2 == captainPilot.StaffID4 || captainPilot.StaffID2 == captainPilot.StaffID5 || captainPilot.StaffID3 == captainPilot.StaffID2 || captainPilot.StaffID3 == captainPilot.StaffID4 || captainPilot.StaffID3 == captainPilot.StaffID5 || captainPilot.StaffID4 == captainPilot.StaffID2 || captainPilot.StaffID4 == captainPilot.StaffID5)
                {

                    ModelState.AddModelError("Duplicate", "There is Duplicate Name Pls check again");
                    return View(captainPilot);


                }

                flightCrewModel.Selected = true;
                ViewBag.Message = "Name: " + flightCrewModel.Text;
                foreach (StaffViewModel item in listActivepersonnel)
                {
                    //Get the staffID in the list
                    int Activepersonnel = item.StaffID;
                    //this mathod is where the status update inside the bracket we put the staffid of active staff
                    staffContext.AuomatedUpdateStatusActive(Activepersonnel);


                }
                //for each personnel in the list
                foreach (StaffViewModel item in listpersonnel)
                {
                    //Get the staffID in the list
                    int inactivepersonnel = item.StaffID;
                    //this mathod is where the status update inside the bracket we put the staffid of inactive staff
                    staffContext.AuomatedUpdateStatusInactive(inactivepersonnel);
                }

                captainPilot.StaffID = staffContext.AddStaffToShedule(captainPilot);
                return RedirectToAction("UpdateFlightPersonnelStatus", "Staff");
            }
            else
            {
                return View(captainPilot);
            }
        }

        public IActionResult UpdateFlightPersonnelStatus()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Staff"))
            {
                return RedirectToAction("Index", "Home");
            }
            //list of personnel have schedule more than 4 
            List<StaffViewModel> listpersonnel = staffContext.PersonnelInactive();
            List<StaffViewModel> listActivepersonnel = staffContext.PersonnelActive();

           foreach (StaffViewModel item in listActivepersonnel)
            {
                //Get the staffID in the list
                int Activepersonnel = item.StaffID;
                //this mathod is where the status update inside the bracket we put the staffid of active staff
                staffContext.AuomatedUpdateStatusActive(Activepersonnel);
            }
            //for each personnel in the list
            foreach (StaffViewModel item in listpersonnel)
            {
                //Get the staffID in the list
                int inactivepersonnel = item.StaffID;
                //this mathod is where the status update inside the bracket we put the staffid of inactive staff
                staffContext.AuomatedUpdateStatusInactive(inactivepersonnel);
            }
            List<StaffViewModel> ActiveList = staffContext.GetAllPersonnel();
            return View(ActiveList);
        }
        public ActionResult Select(int? id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Staff"))
            {
                return RedirectToAction("Index", "Home");
            }

            FlightCrewModel captainPilot = new FlightCrewModel();
            
            captainPilot.FlightCaptains = staffContext.DisplayCaptaiPilot();

            captainPilot.SecondPilot = staffContext.DisplayCaptaiPilot();

            captainPilot.CabinCrewLeader = staffContext.DisplayCabinCrewLeader();

            captainPilot.CabinCrew1 = staffContext.DisplayCabinCrewLeader();

            captainPilot.CabinCrew2 = staffContext.DisplayCabinCrewLeader();
            
            captainPilot.CabinCrew3 = staffContext.DisplayCabinCrewLeader();
            

            if (id == null)
            { //Query string parameter not provided
              //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            //Search for Schedule ID
            FlightCrewModel FlightCrew = staffContext.GetSchduledetails(id.Value);
            FlightCrew.SecondPilot = captainPilot.SecondPilot;
            FlightCrew.FlightCaptains = captainPilot.FlightCaptains;
            FlightCrew.CabinCrewLeader = captainPilot.CabinCrewLeader;
            FlightCrew.CabinCrew1 = captainPilot.CabinCrewLeader;
            FlightCrew.CabinCrew2 = captainPilot.CabinCrewLeader;
            FlightCrew.CabinCrew3 = captainPilot.CabinCrewLeader;
            if (FlightCrew == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(FlightCrew);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFlightPersonelRecord(Staff staff)
        {
            //Get Getflightpersonnelrecord and status list for drop-down list
            //in case of the need to return to CreateFlightPersonelRecord.cshtml view
           
            ViewData["StatusList"] = GetStatus();
            ViewData["VocationList"] = GetVocations();
            if (ModelState.IsValid)
            {
                
                //Add staff record to database
                staff.StaffID = staffContext.Add(staff);
                //Redirect user to Staff/Index view
                return RedirectToAction("ViewAllPersonnelAndFlightAssigned", "Staff");
            }
            else
            {
                //Input validation fails, return to the CreateFlightPersonelRecord view
                //to display error message
                return View(staff);
            }
        }
        // list Status
        private List<SelectListItem> GetStatus()
        {
            List<SelectListItem> Status = new List<SelectListItem>();
            Status.Add(new SelectListItem
            {
                Value = "Active",
                Text = "Active"
            });
            Status.Add(new SelectListItem
            {
                Value = "Inactive",
                Text = "Inactive"
            });
            return Status;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(StaffViewModel staff)
        {
            // Delete the staff record from database
            staffContext.Delete(staff.StaffID);
            return RedirectToAction("ViewAllPersonnelAndFlightAssigned", "Staff");
        }

        /*******************************/
        //                              /
        //      Flight Scheduling       /
        //                              /
        /*******************************/

        private FlightScheduleDAL scheduleContext = new FlightScheduleDAL();

        public ActionResult FlightSchedule()
        {
            if (HttpContext.Session.GetString("Role") != "Staff")
            {
                return RedirectToAction("Index", "Home");
            }

            return View("FlightScheduling/MainPage");
        }

        public ActionResult ViewFlightRoutes()
        {
            if (HttpContext.Session.GetString("Role") != "Staff")
            {
                return RedirectToAction("Index", "Home");
            }

            List<FlightRoute> routeList = scheduleContext.GetAllFlightRoutes();
            return View("FlightScheduling/ViewFlightRoutes", routeList);
        }

        public ActionResult CreateFlightRoute()
        {
            if (HttpContext.Session.GetString("Role") != "Staff")
            {
                return RedirectToAction("Index", "Home");
            }

            return View("FlightScheduling/CreateFlightRoute");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFlightRoute(FlightRoute route)
        {
            if (ModelState.IsValid)
            {
                //Add staff record to database
                route.RouteID = scheduleContext.AddFlightRoute(route);
                //Redirect user to Staff/Index view
                return RedirectToAction("ViewFlightRoutes");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View("FlightScheduling/CreateFlightRoute", route);
            }
        }

        public ActionResult ViewFlightSchedule(int routeId)
        {
            if (HttpContext.Session.GetString("Role") != "Staff")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["routeId"] = routeId;

            List<FlightSchedule> scheduleList;
            //get all flight schedules if no flight route specified
            if (routeId == 0)
            {
                scheduleList = scheduleContext.GetAllFlightSchedules();
            }
            //get flight schedules only for specific flight route
            else
            {
                scheduleList = scheduleContext.GetFlightSchedules(routeId);
            }
            
            return View("FlightScheduling/ViewFlightSchedules", scheduleList);
        }

        public ActionResult CreateFlightSchedule(int routeId)
        {
            if (HttpContext.Session.GetString("Role") != "Staff")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["routeId"] = routeId;
            FlightSchedule schedule = new FlightSchedule { RouteID = routeId };
            return View("FlightScheduling/CreateFlightSchedules", schedule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFlightSchedule(FlightSchedule schedule)
        {
            //set status to open
            schedule.Status = "Opened";

            if (ModelState.IsValid)
            {
                //find duration of route
                List<FlightRoute> route = scheduleContext.GetSingleFlightRoute(schedule.RouteID);
                int routeDuration = (int)route[0].FlightDuration;

                //set arrival datetime
                schedule.ArrivalDateTime = ((DateTime)schedule.DepartureDateTime).AddHours(routeDuration);

                System.Diagnostics.Debug.WriteLine("Route ID: ", schedule.RouteID);

                //Add schedule record to database
                schedule.ScheduleID = scheduleContext.AddFlightSchedule(schedule);
                //Redirect user to view
                return RedirectToAction("ViewFlightSchedule", schedule.RouteID);
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View("FlightScheduling/CreateFlightSchedules", schedule);
            }
        }

        private List<SelectListItem> scheduleStatusOptions = new List<SelectListItem>
        {
            new SelectListItem{ Value = "Opened", Text = "Opened" },
            new SelectListItem{ Value = "Delayed", Text = "Delayed" },
            new SelectListItem{ Value = "Cancelled", Text = "Cancelled" },
            new SelectListItem{ Value = "Full", Text = "Full" },
        };

        public ActionResult UpdateFlightSchedule(int scheduleId)
        {
            if (HttpContext.Session.GetString("Role") != "Staff")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["options"] = scheduleStatusOptions;
            //get flight schedule
            FlightSchedule schedule = scheduleContext.GetSingleFlightSchedule(scheduleId)[0];
            UpdateScheduleStatusViewModel vm = new UpdateScheduleStatusViewModel { ScheduleID = scheduleId, Status = schedule.Status, RouteID = schedule.RouteID };
            return View("FlightScheduling/UpdateFlightScheduleStatus", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateFlightSchedule(UpdateScheduleStatusViewModel vm)
        {
            if (ModelState.IsValid)
            {
                //update schedule record to database
                int count = scheduleContext.UpdateScheduleStatus(vm.ScheduleID, vm.Status);
                //Redirect user to view
                return RedirectToAction("ViewFlightSchedule");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View("FlightScheduling/CreateFlightSchedules", vm);
            }
        }
    }
}