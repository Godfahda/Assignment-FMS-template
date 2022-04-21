using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using FMS.Data.Models;
using FMS.Data.Services;
using FMS.Web.Controllers;

namespace FMS.Web.Controllers
{
    [Authorize]
    public class VehicleController : BaseController
    {
        // provide suitable controller actions
        private IFleetService svc;

        public VehicleController()
        {
            svc = new FleetServiceDb();
        }

        public IActionResult Index()
        {
            var vehicle = svc.GetVehicles();
            return View(vehicle);
        }

        public IActionResult Details(int id)
        {   //Receive vehichle with specifid id from the service
            var v = svc.GetVehicle(id);

            //Replace not found with alert and redirection
            if (v == null)
            {
                //Dislay suitable waring alert and redirect
                Alert($"Vehicle {id} not found", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }
            //PAss vehichle as parameter to the view
            return View(v);
        }

        //GET: /Vehichle/create
        [Authorize(Roles = "admin")]

        public IActionResult Create()
        {
            //Display blank form to add new vehicle
            return View();
        }

        //POST /Vehicle/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="admin")]

        public IActionResult Create ([Bind("make,model,year,regNumber,fuelType,bodyType,transmissionType,CC,numberOfDoors,photoUrl")] Vehicle v)
        {
            //Check unique Reg Number for vehicle
            if (svc.IsDuplicateRegNumber(v.RegNumber, v.Id))
            {
                //Adding manual validation error
                ModelState.AddModelError(nameof(v.RegNumber)," This Registration number is already in use");
            }

             //Complete post action to add vehicle
            if (ModelState.IsValid)
            {
                //pass data from service to store
                v = svc.AddVehicle(v.Make, v.Model, v.Year, v.RegNumber, v.FuelType, v.BodyType, v.TransmissionType, v.CC,v.NumberOfDoors, v.PhotoUrl);
                Alert($"Vehicle added sucessfully" , AlertType.success);

                return RedirectToAction(nameof(Details), new { Id = v.Id});

            }

            //Redisplay the form for editing as there are validation errors
            return View(v);
        }   

        //GET /vehicle/edit/(id)
        [Authorize(Roles ="admin,manager")]
        public IActionResult Edit(int id)
        {
            //Retrieve student from service
            var v = svc.GetVehicle(id);

            //Check for null and alert as needed
            if (v == null)
            {
                Alert($"Vehicle {id} not found", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }
            return View(v);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="admin,manager")]

        public IActionResult Edit (int id, [Bind("make,model,year,regNumber,fuelType,bodyType,transmissionType,CC,numberOfDoors,photoUrl")] Vehicle v)
        {

            //Check for unique RegNumber
            if (svc.IsDuplicateRegNumber(v.RegNumber,v.Id))
            {
                //Adding manual validation error
                ModelState.AddModelError("Registration Number","This Reguistation number is already used");
            
            }

            //Validate and complete POST action to save vehicle changes
            if (ModelState.IsValid)
            {
                //Pass data to service to update
                svc.UpdateVehicle(v);
                Alert("Vehicle updated sucessfully", AlertType.info);

                return RedirectToAction(nameof(Details), new { Id = v.Id});

            }

                //Redisplay form
                return View(v);
        }

        //GET /vehicle/delete/{id}
        [Authorize(Roles = "admin")]

        public IActionResult Delete (int id)
        {
            //load the vehicle using the service
            var v = svc.GetVehicle(id);
            // check the returned student is not null
            //Check for null and alert as needed
         
            if (v == null)
            {
                Alert($"Vehicle {id} not found", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }

            //Pass vehicle to view for deletion confirmation
            return View(v);
        }

        //POST /vehicle/delete/{id}
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]

        public IActionResult DeleteConfirm(int id)
        {
            //Delete student via service
            svc.DeleteVehicle(id);

            Alert("Student delete successfully", AlertType.info);

            //Return to the index view
            return RedirectToAction(nameof(Index));
        }

        //==================Vehicle MOT management ======================

        //GET /vehicle/createMOT/{id}

        public IActionResult MOTCreate(int id)
        {
            var v = svc.GetVehicle(id);
            //Check the reurned vehicle is not null and alert if so
            if (v == null)
            {
                Alert($"Vehicle {id} not found", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }

            var mot = new Mot { VehicleId = id};

            return View (mot);
        }

        //POST /student/create
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult MotCreate([Bind("VehicleID, status")] Mot m)
        {
            if (ModelState.IsValid)
            {
                var mot = svc.CreateMot(m.VehicleId, m.TestReport);
                Alert($"MOT created successfully for Vrhicle {m.VehicleId}",AlertType.info);
                return RedirectToAction (nameof(Details), new{Id = mot.VehicleId});
            }

            //Residplay form for editing
            return View(m);
        }

        //GET /vehicle/MOT delete/{id}

        public IActionResult MotDelete(int id)
        {
            //load the mot using the service
            var mot = svc.GetMot(id);
            // check the returned MOT state and alert as required
            if (mot == null)
            {
                Alert($"Mot {id} not found", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }

            //Pass mot to view for deletion confirmation
            return View(mot);
        }

        //POST /vehicle/motdeleteconfrim/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult MotDeleteCOnfirm(int id, int vehicleId)
        {
            //delete vehicle via service
            svc.DeleteMot(id);
            Alert($"Mot deleted sucessfully for student {vehicleId}", AlertType.info);

            //redirect to the mot index view
            return RedirectToAction(nameof(Details), new { Id = vehicleId});
        }
    }


}
