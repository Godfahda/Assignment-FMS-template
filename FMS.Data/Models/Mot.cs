using System;
using System.ComponentModel.DataAnnotations;

namespace FMS.Data.Models
{
    public enum MotStatus {PASSED, FAILED, ALL}

    public class Mot
    {
        public int Id { get; set; }
        
        // suitable mot attributes / relationships

        public DateTime IssueDate {get; set;} = DateTime.Today; //Inputs the date MOT was issued using present date

        public DateTime DueDate {get; set;} = DateTime.Now.AddYears(1) ; // Calculates MOT due date by adding a year to present date

        public string IssuedBy {get; set;} //Gets the name of user who issued the MOT

        public Boolean TestResult { get; set;} = false; //Gives the state of the test; pass or fail

        public int Mileage {get; set;} //Shows presnt milage of the vehicle

        public string TestReport {get; set;} //Free text field to add comments or report about MOT 

        public int VehicleId {get; set;} //foreign key

        public Vehicle Vehicle {get; set;} //Navigation Property

    }
}
