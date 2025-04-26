using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent2Me.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Rent2Me.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly AppDbContext _context;
        public SubscriptionController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost("Add")]
        public IActionResult AddPlan(SubscriptionPlan plan)
        {
            _context.Add(plan);
            _context.SaveChanges();
            return Ok("Success.");
        }
        [HttpDelete]
        public IActionResult delete(string name)
        {
            var plan = _context.SubscriptionPlans.FirstOrDefault(u => u.Name == name);
            _context.Remove(plan);
            _context.SaveChanges();
            return Ok("Success");
        }
        [HttpGet("subscription-plans")]
        public IActionResult GetSubscriptionPlans()
        {
            var subscriptionPlans = _context.SubscriptionPlans.ToList();
            return Ok(subscriptionPlans);
        }
        [HttpPost("subscribe")]
        public async Task<IActionResult> SubscribeToPlan(string customerId, string planName)
        {
            var customer = await _context.Set<Customer>()
           .FirstOrDefaultAsync(c => c.NationalID == customerId);
            var plan = _context.SubscriptionPlans.FirstOrDefault(p => p.Name == planName);

            if (customer == null || plan == null)
            {
                return NotFound(new { Error = "Customer or plan not found." });
            }
            customer.SubscriptionPlanName = planName;
            await _context.SaveChangesAsync();

            return Ok(new {Message="User subscribed to plan successfully."});
        }
        //[HttpPost("change-plan")]
        //public IActionResult ChangeSubscriptionPlan(string customerId, string planName)
        //{
        //    var customer = _context.Customers.Include(c => c.SubscriptionPlan)
        //                                     .FirstOrDefault(c => c.NationalID == customerId);
        //    var newPlan = _context.SubscriptionPlans.FirstOrDefault(p => p.Name == planName);

        //    if (customer == null || newPlan == null)
        //    {
        //        return NotFound(new { Error = "Customer or plan not found." });
        //    }

        //    if (customer.SubscriptionPlanName == planName)
        //    {
        //        return BadRequest(new { Error = "Customer is already subscribed to this plan." });
        //    }
        //    var userResponse = "continue"; // Assume user selects to continue

        //    if (userResponse == "continue")
        //    {
        //        customer.RemoveRemainingProcesses();
        //        //after successful payment
        //        customer.SubscriptionPlanName = planName;
        //        customer.SubscriptionPlan = newPlan;
        //        _context.SaveChanges();
        //        return Ok(customer);
        //    }
        //    else
        //    {
        //        return Ok(new { Message = "Subscription plan change cancelled." });
        //    }
        //}


    }
}
