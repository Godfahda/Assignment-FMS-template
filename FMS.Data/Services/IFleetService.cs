using System;
using System.Collections.Generic;
	
using FMS.Data.Models;
	
namespace FMS.Data.Services
{
    // This interface describes the operations that a FleetService class should implement
    public interface IFleetService
    {
        void Initialise();
        
        // add suitable method definitions to implement assignment requirements            
        IList<Vehicle> GetVehicles();
        Vehicle GetVehicle(int id);
        Vehicle GetVehicleByRegNumber (string regNumber);
        Vehicle AddVehicle (string make, string model, DateTime year, string regNumber, string fuelType, string bodyType, string transmissionType, int CC, int numberOfDoors, string photoUrl);
        Vehicle UpdateVehicle(Vehicle updated);
        bool DeleteVehicle(int id);
        bool IsDuplicateRegNumber (string RegNumber, int VehicleId);

        // ------------- User Management -------------------
        User Authenticate(string email, string password);
        User Register(string name, string email, string password, Role role);
        User GetUserByEmail(string email);
    
        //-------------- MOT Management --------------------
        Mot CreateMot (int id, string TestReport);
        Mot GetMot (int id);
        //Mot UpdateMot (int id, bool TestResult);
        bool DeleteMot(int id);
        IList<Mot> GetAllMots();
        //IList<Mot> SearchMots(MotStatus status, string query);

    }
    
}