using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using FMS.Data.Models;
using FMS.Data.Repository;
using FMS.Data.Security;

namespace FMS.Data.Services
{
    public class FleetServiceDb : IFleetService
    {
        private readonly DataContext db;

        public FleetServiceDb()
        {
            db = new DataContext();
        }

        public void Initialise()
        {
            db.Initialise(); // recreate database
        }

        // ==================== Fleet Management ==================
       
        // implement IFleetService methods here

        //Recalling all vehicles 
        public IList<Vehicle> GetVehicles()
        {
            return db.Vehicles.ToList();
        }

        //Retrieve Vehicles by ID and related MOTs

        public Vehicle GetVehicle (int id) 
        {
            return db.Vehicles
                     .Include(v => v.Mot) 
                     .FirstOrDefault(v => v.Id == id);
        }

        //Add a new vehicle but first checking for distinct Registration number
        public Vehicle AddVehicle(string make, string model, DateTime year, string regNumber, string fuelType, string bodyType, string transmissionType, int cc, int numberOfDoors, string photoUrl)
        {
            //Checking Distinct Registation number
            var exists = GetVehicleByRegNumber(regNumber);
            if (exists != null)
            {
                return null;
            }

            var v = new Vehicle
            {
                Make = make, 
                Model = model, 
                Year = year, 
                RegNumber = regNumber, 
                FuelType = fuelType, 
                BodyType = bodyType, 
                TransmissionType = transmissionType, 
                CC = cc, 
                NumberOfDoors = numberOfDoors, 
                PhotoUrl = photoUrl
            };
            db.Vehicles.Add(v); //add newly created vehicle
            db.SaveChanges();
            return v; //Returns newly created/added vehicle

        }

        // Delete the vehicle identified by Id returning true if deleted and false if not found
        public bool DeleteVehicle(int id)
        {
            var v = GetVehicle(id);
            if (v == null)
            {
                return false;
            }
            db.Vehicles.Remove(v);
            db.SaveChanges();
            return true;
        }

        // Update vehicle details with new details
        public Vehicle UpdateVehicle (Vehicle updated)
        {
            //Check if the vehicle exists
            var vehicle = GetVehicle(updated.Id);
            if (vehicle == null)
            {
                return null;
            }
            //Populating vehicle details with new values
            vehicle.Make = updated.Make; 
            vehicle.Model = updated.Model; 
            vehicle.Year = updated.Year; 
            vehicle.RegNumber = updated.RegNumber; 
            vehicle.FuelType = updated.FuelType; 
            vehicle.BodyType = updated.BodyType; 
            vehicle.TransmissionType = updated.TransmissionType;
            vehicle.CC = updated.CC; 
            vehicle.NumberOfDoors = updated.NumberOfDoors; 
            vehicle.PhotoUrl = updated.PhotoUrl;

            db.SaveChanges();
            return vehicle;
        }

        public Vehicle GetVehicleByRegNumber(string regNumber)
        {
            return db.Vehicles.FirstOrDefault(v => v.RegNumber == regNumber);
        }

        public bool IsDuplicateRegNumber (string regNumber, int vehicleId)
        {
            var existing = GetVehicleByRegNumber(regNumber);
            // if a vehicle with Registation Number exist and the Id does not match the vehicle ID (if provided); then the Reg Number is invalid
            return existing != null && vehicleId != existing.Id;
        }

        public Mot CreateMot (int vehicleId, string status)
        {
            var vehicle = GetVehicle (vehicleId);
            if (vehicle == null) return null;

            var Mot = new Mot
            {
                VehicleId = vehicleId,
                TestReport = status,
            };
            db.Mots.Add(Mot);
            db.SaveChanges(); //write to database
            return Mot;
        }

        public Mot GetMot(int id)
        {
            return db.Mots
                     .Include(m => m.Vehicle)
                     .FirstOrDefault(m => m.Id == id);
        }

        public bool DeleteMot(int id )
        {
            var mot = GetMot(id);
            if (mot == null) return false;

            var result = db.Mots.Remove(mot);

            db.SaveChanges();
            return true;
        }

        public IList<Mot> GetAllMots()
        {
            return db.Mots
                     .Include(m => m.Vehicle)
                     .ToList();
        }


        // ==================== User Authentication/Registration Management ==================
        public User Authenticate(string email, string password)
        {
            // retrieve the user based on the EmailAddress (assumes EmailAddress is unique)
            var user = GetUserByEmail(email);

            // Verify the user exists and Hashed User password matches the password provided
            return (user != null && Hasher.ValidateHash(user.Password, password)) ? user : null;
        }

        public User Register(string name, string email, string password, Role role)
        {
            // check that the user does not already exist (unique user name)
            var exists = GetUserByEmail(email);
            if (exists != null)
            {
                return null;
            }

            // Custom Hasher used to encrypt the password before storing in database
            var user = new User 
            {
                Name = name,
                Email = email,
                Password = Hasher.CalculateHash(password),
                Role = role   
            };
   
            db.Users.Add(user);
            db.SaveChanges();
            return user;
        }

        public User GetUserByEmail(string email)
        {
            return db.Users.FirstOrDefault(u => u.Email == email);
        }

    }
}
