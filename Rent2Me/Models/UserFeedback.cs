using System.ComponentModel.DataAnnotations;
using System;
using System.Text.Json.Serialization;

namespace Rent2Me.Models
{
    public class UserFeedback
    {
        [Key]
        public int FeedbackId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Rating { get; set; }
        public string FromUserId { get; set; }
        [JsonIgnore]
        public Customer FromUser { get; set; }
        public string ToUserId { get; set; }
        [JsonIgnore]
        public Customer ToUser { get; set; }
        public  string LicensePlate { get; set; }
    }
}
