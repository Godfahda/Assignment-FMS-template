using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FMS.Web.Models
{
    public class MotCreateViewModel
    {
        public SelectList Vehicle {set; get;}

        [Required(ErrorMessage = "Please Select a Vehicle")]
        [Display(Name = "Select Vehicle")]

        public int VehicleId {get; set;}

        [Required]
        [StringLength(100)]

        public string TestReport {get; set;}
    }
}