using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Rent2Me.DTO
{
    public class CarInputParameters
    {
        [Required(ErrorMessage ="License plate is required.")]
        public string LicencePlate { get; set; }
        [Required(ErrorMessage = "Car Type is required.")]
        public string CarType { get; set; }
        [Required(ErrorMessage = "Brand is required.")]
        public string Brand { get; set; }
        [Required(ErrorMessage = "Color is required.")]
        public string Color { get; set; }
        public string CurrentMileage { get; set; }
        [Required(ErrorMessage = "Year is required.")]
        public string Year { get; set; }
        [Required(ErrorMessage = "Model is required.")]
        public string Model { get; set; }
        public IFormFile Image { get; set; }
        public string SeatingCapacity { get; set; }
        [Required(ErrorMessage = "Renting Price is required.")]
        public string RentingPrice { get; set; }
        public string Deposite { get; set; }
        [Required(ErrorMessage = "Property deed is required")]
        public IFormFile PropertyDeed{ get; set; }
        public  string UserrID { get; set; }
    }
}
