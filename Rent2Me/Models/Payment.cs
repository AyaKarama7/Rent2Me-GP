using System.ComponentModel.DataAnnotations;
using System;

namespace Rent2Me.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }
        public string PaymentDescription { get; set; }
        public string PaymentGatewayResponse { get; set; }
        public string BillingAddress { get; set; }
        public string AuthorizationCode { get; set; }
        public string CaptureId { get; set; }
        public string RefundId { get; set; }
        public string CustomerId { get; set; }
        public string SubscriptionPlanName { get; set; }
        public int RentalRequestId { get; set; }
        public Customer Customer { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; }
    }
}
