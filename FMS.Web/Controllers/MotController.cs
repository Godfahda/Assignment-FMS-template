using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

using FMS.Data.Models;
using FMS.Data.Services;
using FMS.Web.Models;

namespace FMS.Web.Controllers
{
    [Authorize]
    public class MotController : BaseController
    {
        private readonly IFleetService svc;
        public MotController()
        {
            svc = new FleetServiceDb();
        }

        //GET /mot/index
        public IActionResult Index()
        {
            var mot = svc.GetAllMots();

            return View(mot);
        }

        public IActionResult Details(int id)
        {
            var mot = svc.GetMot(id);
            if (mot == null)
            {
                Alert("Mot Not Found", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }

            return View(mot);
        }

        //POST /Mot/create
        [Authorize(Roles = "admin, manager")]

        public IActionResult Create()
        {
            var vehicle = svc.GetVehicles();
            var mvm = new MotCreateViewModel 
            {
                Vehicle = new SelectList(vehicle,"Id","Name")
            };

            return View(mvm);
        }

        //POST /Mot/create
        [HttpPost]
        [Authorize(Roles ="admin,manager")]

        public IActionResult Create(MotCreateViewModel mvm)
        {
            if (ModelState.IsValid)
            {
                svc.CreateMot(mvm.VehicleId,mvm.TestReport);

                Alert($"MOT created",AlertType.info);
                return RedirectToAction(nameof(Index));
            }

            return View(mvm);
        }

        

    }
}