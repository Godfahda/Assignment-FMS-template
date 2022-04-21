using System;
using Xunit;
using FMS.Data.Models;
using FMS.Data.Services;

namespace FMS.Test
{

    public class ServiceTests
    {
        private readonly IFleetService svc;


        public ServiceTests()
        {
            // general arrangement
            svc = new FleetServiceDb();
          
            // ensure data source is empty before each test
            svc.Initialise();
        }

        // ========================== Fleet Tests =========================
   

        // write suitable tests to verify operation of the fleet service

            [Fact] 
        public void Vehicle_AddVehicle_WhenDuplicateRegNum_ShouldReturnNull()
        {
            // act 
            var v1 = svc.AddVehicle("Tesla", "Model 3",Convert.ToDateTime("01/01/2022") , "BXN74830", "Electric", "Hatch Back", "Automatic", 0, 6, "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fwww.motortrend.com%2Fuploads%2Fsites%2F5%2F2017%2F07%2FTesla-Model-3-lead-.jpg&f=1&nofb=1" );
            // this is a duplicate as the registration number as previous vehicle
            var v2 = svc.AddVehicle("Tesla", "Model 3", Convert.ToDateTime("01/01/2022") , "BXN74830", "Electric", "Hatch Back", "Automatic", 0, 6, "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fwww.motortrend.com%2Fuploads%2Fsites%2F5%2F2017%2F07%2FTesla-Model-3-lead-.jpg&f=1&nofb=1" );
            
            // assert
            Assert.NotNull(v1); // this Vehicle should have been added correctly
            Assert.Null(v2); // this Vehicle should NOT have been added        
        }

        [Fact]
        public void Vehicle_AddVehicle_WhenNone_ShouldSetAllProperties()
        {
            // act 
            var added = svc.AddVehicle("Tesla", "Model 3", Convert.ToDateTime("01/01/2022") , "BXN74830", "Electric", "Hatch Back", "Automatic", 0, 6, "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fwww.motortrend.com%2Fuploads%2Fsites%2F5%2F2017%2F07%2FTesla-Model-3-lead-.jpg&f=1&nofb=1" );            
            // retrieve Vehicle just added by using the Id returned by EF
            var v = svc.GetVehicle(added.Id);

            // assert - that Vehicle is not null
            Assert.NotNull(v);
            
            // now assert that the properties were set properly
            Assert.Equal(v.Id, v.Id);
            Assert.Equal("Tesla", v.Make);
            Assert.Equal("Model 3", v.Model);
            Assert.Equal("BXN74830", v.RegNumber);
            Assert.Equal(Convert.ToDateTime("01/01/2022"), v.Year);
            Assert.Equal("Hatch Back", v.BodyType);
            Assert.Equal("Automatic", v.TransmissionType);
            Assert.Equal(0,v.CC);
            Assert.Equal(6,v.NumberOfDoors);
            Assert.Equal("https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fwww.motortrend.com%2Fuploads%2Fsites%2F5%2F2017%2F07%2FTesla-Model-3-lead-.jpg&f=1&nofb=1",v.PhotoUrl);
        }

        [Fact]
        public void Vehicle_UpdateVehicle_ThatExists_ShouldSetAllProperties()
        {
            // arrange - create test Vehicle
            var s = svc.AddVehicle("Tesla", "Model 3", Convert.ToDateTime("01/01/2022") , "BXN74830", "Electric", "Hatch Back", "Automatic", 0, 6, "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fwww.motortrend.com%2Fuploads%2Fsites%2F5%2F2017%2F07%2FTesla-Model-3-lead-.jpg&f=1&nofb=1" );                        
            // act - create a copy and update any Vehicle properties (except Id) 
            
            var u = new Vehicle {
            
                Make = "Mercedes", 
                Model = "Class S", 
                Year = Convert.ToDateTime("01/01/2022"), 
                RegNumber = "BXNX", 
                FuelType = "Petrol", 
                BodyType = "Truck", 
                TransmissionType = "Manual", 
                CC = 4500, 
                NumberOfDoors = 5, 
                PhotoUrl = "https://mercedes.com"
            };
            // save updated Vehicle
            svc.UpdateVehicle(u); 

            // reload updated Vehicle from database into us
            var us = svc.GetVehicle(s.Id);

            // assert
            Assert.NotNull(u);           

            // now assert that the properties were set properly           
            Assert.Equal(u.Make, us.Make);
            Assert.Equal(u.Model, us.Model);
            Assert.Equal(u.Year, us.Year);
            Assert.Equal(u.RegNumber, us.RegNumber);
            Assert.Equal(u.FuelType, us.FuelType);
            Assert.Equal(u.BodyType, us.BodyType);
            Assert.Equal(u.TransmissionType, us.TransmissionType);
            Assert.Equal(u.CC, us.CC);
            Assert.Equal(u.NumberOfDoors, us.NumberOfDoors);
            Assert.Equal(u.PhotoUrl, us.PhotoUrl);
        }

        [Fact] 
        public void Vehicle_GetAllVehicles_WhenNone_ShouldReturn0()
        {
            // act 
            var vehicles = svc.GetVehicles();
            var count = vehicles.Count;

            // assert
            Assert.Equal(0, count);
        }

         [Fact]
        public void Vehicle_GetVehicles_When2Exist_ShouldReturn2()
        {
            // arrange
            var v1 = svc.AddVehicle("Tesla", "Model 3", Convert.ToDateTime("01/01/2022") , "BXN74830", "Electric", "Hatch Back", "Automatic", 0, 6, "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fwww.motortrend.com%2Fuploads%2Fsites%2F5%2F2017%2F07%2FTesla-Model-3-lead-.jpg&f=1&nofb=1" );
            var v2 = svc.AddVehicle("Mercedes", "Mercedes-AMG E63 S 4Matic+", Convert.ToDateTime("20/04/2017") , "SEK 2132", "Petrol", "Sedan", "Automatic", 3982, 5, "https://car-images.bauersecure.com/pagefiles/68234/1056x594/zmer-001.jpg" );

            // act
            var vehicles = svc.GetVehicles();
            var count = vehicles.Count;

            // assert
            Assert.Equal(2, count);
        }

        [Fact] 
        public void Vehicle_GetVehicle_WhenNonExistent_ShouldReturnNull()
        {
            // act 
            var vehicle = svc.GetVehicle(1); // non existent vehicle

            // assert
            Assert.Null(vehicle);
        }

        [Fact] 
        public void Vehicle_GetVehicle_ThatExists_ShouldReturnVehicle()
        {
            // act 
            var v = svc.AddVehicle("Mercedes", "Mercedes-AMG E63 S 4Matic+", Convert.ToDateTime("20/04/2017") , "SEK 2132", "Petrol", "Sedan", "Automatic", 3982, 5, "https://car-images.bauersecure.com/pagefiles/68234/1056x594/zmer-001.jpg" );

            var vs = svc.GetVehicle(v.Id);

            // assert
            Assert.NotNull(vs);
            Assert.Equal(v.Id, vs.Id);
        }

        [Fact] 
        public void Vehicle_GetVehicle_ThatExistsWithMot_ShouldReturnVehicleWithMots()
        {
            // arrange 
            var v = svc.AddVehicle("Mercedes", "Mercedes-AMG E63 S 4Matic+", Convert.ToDateTime("20/04/2017") , "SEK 2132", "Petrol", "Sedan", "Automatic", 3982, 5, "https://car-images.bauersecure.com/pagefiles/68234/1056x594/zmer-001.jpg" );
            svc.CreateMot(v.Id, "Test Mot 1");
            svc.CreateMot(v.Id, "Test Mot 2");
            
            // act
            var vehicle = svc.GetVehicle(v.Id);

            // assert
            Assert.NotNull(v);    
            Assert.Equal(2, vehicle.Mot.Count);
        }

        [Fact]
        public void Vehicle_DeleteVehicle_ThatExists_ShouldReturnTrue()
        {
            // act 
            var v = svc.AddVehicle("Mercedes", "Mercedes-AMG E63 S 4Matic+", Convert.ToDateTime("20/04/2017") , "SEK 2132", "Petrol", "Sedan", "Automatic", 3982, 5, "https://car-images.bauersecure.com/pagefiles/68234/1056x594/zmer-001.jpg" );
            var deleted = svc.DeleteVehicle(v.Id);

            // try to retrieve deleted vehicle
            var v1 = svc.GetVehicle(v.Id);

            // assert
            Assert.True(deleted); // delete vehicle should return true
            Assert.Null(v1);      // v1 should be null
        }

        [Fact]
        public void Vehicle_DeleteStudent_ThatDoesntExist_ShouldReturnFalse()
        {
            // act 	
            var deleted = svc.DeleteVehicle(0);

            // assert
            Assert.False(deleted);
        } 

     // ---------------------- MOT Tests ------------------------

        [Fact] 
        public void MOT_CreateMOT_ForExistingVehicle_ShouldBeCreated()
        {
            // arrange
            var v = svc.AddVehicle("Mercedes", "Mercedes-AMG E63 S 4Matic+", Convert.ToDateTime("20/04/2017") , "SEK 2132", "Petrol", "Sedan", "Automatic", 3982, 5, "https://car-images.bauersecure.com/pagefiles/68234/1056x594/zmer-001.jpg" );
         
            // act
            var m = svc.CreateMot(v.Id, "TEST MOT 1");
           
            // assert
            Assert.NotNull(m);
            Assert.Equal(v.Id, m.VehicleId);
            Assert.False(m.TestResult); 
        }

        // --- GetMot should include Vehicle
        [Fact] 
        public void MOT_GetMOT_WhenExists_ShouldReturnMOTAndVehicle()
        {
            // arrange
            var v = svc.AddVehicle("Mercedes", "Mercedes-AMG E63 S 4Matic+", Convert.ToDateTime("20/04/2017") , "SEK 2132", "Petrol", "Sedan", "Automatic", 3982, 5, "https://car-images.bauersecure.com/pagefiles/68234/1056x594/zmer-001.jpg" );
            var m = svc.CreateMot(v.Id, "Test MOT");

            // act
            var mot = svc.GetMot(m.Id);

            // assert
            Assert.NotNull(mot);
            Assert.NotNull(mot.Vehicle);
            Assert.Equal(v.Make, mot.Vehicle.Make); 
        }

        [Fact] 
        public void Mot_DeleteMot_WhenExists_ShouldReturnTrue()
        {
            // arrange
            var v = svc.AddVehicle("Mercedes", "Mercedes-AMG E63 S 4Matic+", Convert.ToDateTime("20/04/2017") , "SEK 2132", "Petrol", "Sedan", "Automatic", 3982, 5, "https://car-images.bauersecure.com/pagefiles/68234/1056x594/zmer-001.jpg" );
            var m = svc.CreateMot(v.Id, "Test MOT");

            // act
            var deleted = svc.DeleteMot(m.Id);     // delete MOT    
            
            // assert
            Assert.True(deleted);                    // MOT should be deleted
        }  

         [Fact] 
        public void Mot_DeleteMot_WhenNonExistant_ShouldReturnFalse()
        {
    
           
            // act
            var deleted = svc.DeleteMot(1);     // delete non-existent MOT    
            
            // assert
            Assert.False(deleted);                  // MOT should not be deleted
        }  

         [Fact] // --- Authenticate Invalid Test
        public void User_Authenticate_WhenInValidCredentials_ShouldReturnNull()
        {
            // arrange 
            var s1 = svc.Register("XXX", "xxx@email.com", "admin", Role.admin);
        
            // act
            var user = svc.Authenticate("xxx@email.com", "guest");
            // assert
            Assert.Null(user);

        } 

        [Fact] // --- Authenticate Valid Test
        public void User_Authenticate_WhenValidCredentials_ShouldReturnUser()
        {
            // arrange 
            var s1 = svc.Register("XXX", "xxx@email.com", "admin", Role.admin);
        
            // act
            var user = svc.Authenticate("xxx@email.com", "admin");
            
            // assert
            Assert.NotNull(user);
        }

    }
}