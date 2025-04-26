using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rent2Me.Models
{
    public class Customer
    {
        [Key]
        public string NationalID { get; set; }
        public bool IsNationalIdPublic { get; set; } = false;
        public string Name { get; set; }//public
        public string Address { get; set; }
        public bool IsAddressPublic { get; set; } = true;
        public string Phone { get; set; }
        public bool IsPhonePublic { get; set; } = true;
        public string Mail { get; set; }
        public bool IsMailPublic { get; set; } = true;
        public string Password { get; set; }
        public string ImagePath { get; set; }
        public string DrivingLicensePath { get; set; }
        public DateTime Birthdate { get; set; }
        public int Age { get; set; }
        public bool IsAgePublic { get; set; } = true;
        public string Gender { get; set; }
        public bool IsGenderPublic { get; set; } = true;
        public string SubscriptionPlanName { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; } 
        public List<CarDetails> Cars { get; set; } = new List<CarDetails>();
        public List<RentalRequest> Requests { get; set; } = new List<RentalRequest>();
        public void RemoveRemainingProcesses()
        {
            this.SubscriptionPlan.NumOfProcesses = 0;
        }
        public List<UserFeedback> GivenFeedbacks { get; set; } 
        public List<UserFeedback> ReceivedFeedbacks { get; set; } 
        public List<Notification> Notifications { get; set; }
    }
}
