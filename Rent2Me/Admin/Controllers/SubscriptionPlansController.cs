//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Rent2Me.Admin.Models;
//using Rent2Me.Models;

//namespace Rent2Me.Admin.Controllers
//{
//    //[Authorize(Policy = "DeveloperPolicy")]
//    [Route("api/[controller]")]
//    [ApiController]
//    public class SubscriptionPlansController : ControllerBase
//    {
//        private readonly SubscriptionPlanService _subscriptionPlanService;

//        public SubscriptionPlansController(SubscriptionPlanService subscriptionPlanService)
//        {
//            _subscriptionPlanService = subscriptionPlanService;
//        }

//        [HttpGet]
//        public IActionResult GetSubscriptionPlans()
//        {
//            var plans = _subscriptionPlanService.GetAllPlans();
//            return Ok(plans);
//        }

//        [HttpGet("{name}")]
//        public IActionResult GetSubscriptionPlan(string name)
//        {
//            var plan = _subscriptionPlanService.GetPlanByName(name);
//            if (plan == null)
//            {
//                return NotFound("Plan doesn't exist");
//            }
//            return Ok(plan);
//        }

//        [HttpPost]
//        public IActionResult CreateSubscriptionPlan([FromBody] SubscriptionPlan plan)
//        {
//            _subscriptionPlanService.AddPlan(plan);
//            return CreatedAtAction(nameof(GetSubscriptionPlan), new { name = plan.Name }, plan);
//        }

//        [HttpPut("{name}")]
//        public IActionResult UpdateSubscriptionPlan(string name, [FromBody] SubscriptionPlan plan)
//        {
//            if (name != plan.Name)
//            {
//                return BadRequest("Plan doesn't exist");
//            }
//            _subscriptionPlanService.UpdatePlan(plan);
//            return Ok("Plan updated successfully");
//        }

//        [HttpDelete("{name}")]
//        public IActionResult DeleteSubscriptionPlan(string name)
//        {
//            var existingPlan = _subscriptionPlanService.GetPlanByName(name);
//            if (existingPlan == null)
//            {
//                return NotFound("Plan doesn't exist");
//            }
//            _subscriptionPlanService.DeletePlan(name);
//            return Ok("Plan deleted successfully");
//        }

//    }
//}