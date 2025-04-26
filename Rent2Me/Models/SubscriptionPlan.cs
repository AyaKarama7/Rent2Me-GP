using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Rent2Me.Models
{
    public class SubscriptionPlan
    {
        [Key]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int NumOfProcesses { get; set; }
        [JsonIgnore]
        public List<Customer> Customers { get; set; }
    }
}