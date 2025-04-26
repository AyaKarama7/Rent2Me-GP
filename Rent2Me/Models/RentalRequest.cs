using System;
using System.ComponentModel.DataAnnotations;

namespace Rent2Me.Models
{
    public class RentalRequest
    {
        [Key]
        public int RentalRequestId { get; set; }
        public string RequesterId { get; set; }
        public string CarOwnerId { get; set; }
        public DateTime RequestedDate { get; set; }
        public bool IsAccepted { get; set; } = true;
        public bool IsRejected { get; set; } = false;
        public string LicensePlate { get; set; }
        public DateTime TimeOfDelivery { get; set; }
        public string PlaceOfDelivery { get; set; }
        public string RenterContractPath { get; set; }
        public string Status { get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
