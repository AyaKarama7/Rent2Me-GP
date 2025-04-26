using Rent2Me.Models;
using System;

namespace Rent2Me.DTO
{
    public class FeedBackDtO
    {
        public int FeedbackId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Rating { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string LicensePlate { get; set; }

    }
}
