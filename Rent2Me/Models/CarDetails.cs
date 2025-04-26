using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rent2Me.Models
{
    public class CarDetails
    {
        [Key]
        public string LicencePlate { get; set; }
        public string CarType { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public string CurrentMileage { get; set; }
        public string Year { get; set; }
        public string Model { get; set; }
        public string ImagePath { get; set; }
        public string SeatingCapacity { get; set; }
        public string RentingPrice { get; set; }
        public string Deposite { get; set; }

        public string PropertyDeedPath { get; set; }
        public bool Avilability { get; set; }
        public string CustomerId { get; set; } 
        public Customer Customer { get; set; }
    }
}
