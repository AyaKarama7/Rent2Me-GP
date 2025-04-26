using Microsoft.AspNetCore.Http;
using System;

namespace Rent2Me.DTO
{
    public class RequestOutpuParameters
    {
        public int RequestId { get; set; }
        public string RequesterId { get; set; }
        public string CarOwnerId { get; set; }
        public string LicensePlate { get; set; }
        public DateTime TimeOfDelivery { get; set; }
        public string PlaceOfDelivery { get; set; }

    }
}
