using System;
using System.Text;
using System.Collections.Generic;
using FMS.Data.Models;

namespace FMS.Data.Services
{
    public static class FleetServiceSeeder
    {
        // use this class to seed the database with dummy test data using an IFleetService
        public static void Seed(IFleetService svc)
        {
            svc.Initialise();

            // Add vehicles
            var v1=svc.AddVehicle("Tesla", "Model 3", Convert.ToDateTime("01/01/2022") , "BXN74830", "Electric", "Hatch Back", "Automatic", 0, 6, "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fwww.motortrend.com%2Fuploads%2Fsites%2F5%2F2017%2F07%2FTesla-Model-3-lead-.jpg&f=1&nofb=1" );
            var v2=svc.AddVehicle("Tesla", "Model Y", Convert.ToDateTime("01/01/2020") , "YII BGH", "Electric", "Hatch Back", "Automatic", 0, 6, "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi0.wp.com%2Fblurredculture.com%2Fwp-content%2Fuploads%2F2019%2F03%2FAnd-Teslas-small-SUV-is-go.jpg&f=1&nofb=1" );
            var v3=svc.AddVehicle("Mercedes", "Mercedes-AMG E63 S 4Matic+", Convert.ToDateTime("20/04/2017") , "SEK 2132", "Petrol", "Sedan", "Automatic", 3982, 5, "https://car-images.bauersecure.com/pagefiles/68234/1056x594/zmer-001.jpg" );
            
            // seed Mot for Vehicles
            var m1 = svc.CreateMot(v1.Id, "Nosie from the front of vehicle");
            var m2 = svc.CreateMot(v3.Id, "No issues. I just wished I bought the Tesla instead");

              // add users
            var u1 = svc.Register("Guest", "guest@fms.com", "guest", Role.guest);
            var u2 = svc.Register("Administrator", "admin@fms.com", "admin", Role.admin);
            var u3 = svc.Register("Manager", "manager@fms.com", "manager", Role.manager);


        }
    }
}
