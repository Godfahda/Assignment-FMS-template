using System;
using System.ComponentModel.DataAnnotations;
using FMS.Data.Validators;

namespace FMS.Data.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        
        // suitable vehicle properties/relationships

        [Required]
        public string Make {get; set;}

        [Required]
        public string Model {get; set;}

        [Required]
        public DateTime Year {get; set;} 

        [Required]
        public string RegNumber {get; set;}
        
        [Required]
        public string FuelType {get; set;}

        [Required]
        public string BodyType {get; set;}

        [Required]
        public string TransmissionType {get; set;}

        [Required]
        public int CC {get; set;}

        [Required]
        public int NumberOfDoors {get; set;}

        [Required]
        [UrlResource]
        public string PhotoUrl {get; set; }

        public IList<Mot> Mot {get; set;} = new List<Mot>();
    }

}
