using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rent2Me.Models;
using System.Threading.Tasks;
using System;
using Rent2Me.DTO;
using Microsoft.EntityFrameworkCore;

namespace Rent2Me.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackService _feedbackService;
        private readonly NotificationService _notificationService;
        private readonly AppDbContext _context;
        public FeedbackController(FeedbackService feedbackService, NotificationService notificationService,AppDbContext context)
        {
            _feedbackService = feedbackService;
            _notificationService = notificationService;
            _context = context;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitFeedback([FromBody] FeedBackDtO feedbackDto)
        {
            if (feedbackDto == null || string.IsNullOrWhiteSpace(feedbackDto.Message))
            {
                return BadRequest(new { Error = "Feedback content is required." });
            }
            if (string.IsNullOrWhiteSpace(feedbackDto.FromUserId) || string.IsNullOrWhiteSpace(feedbackDto.ToUserId))
            {
                return BadRequest(new { Error = "FromUserId and ToUserId are required." });
            }

            var fromUserExists = await _context.Customers.AnyAsync(c => c.NationalID == feedbackDto.FromUserId);
            var toUserExists = await _context.Customers.AnyAsync(c => c.NationalID == feedbackDto.ToUserId);
            var car = await _context.CarDetails.AnyAsync(c => c.LicencePlate == feedbackDto.LicensePlate);
            if (!fromUserExists || !toUserExists || !car)
            {
                return BadRequest(new { Error = "check both users and license plate." });
            }

            var feedback = new UserFeedback
            {
                Message = feedbackDto.Message,
                Rating = feedbackDto.Rating,
                FromUserId = feedbackDto.FromUserId,
                ToUserId = feedbackDto.ToUserId,
                CreatedAt = DateTime.UtcNow,
                LicensePlate=feedbackDto.LicensePlate,
            };

            try
            {
                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();

                await _notificationService.CreateNotificationAsync(
                feedback.ToUserId,
                "You have a new feedback.",
                NotificationType.RentalRequest);

                return CreatedAtAction(nameof(SubmitFeedback), new { id = feedback.FeedbackId }, feedback);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while submitting feedback: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpGet("receivedFeedbacks")]
        public async Task<IActionResult> GetReceivedFeedbacks(string userId,string license)
        {
            var feedbacks = await _feedbackService.GetReceivedFeedbacks(userId,license);

            if (feedbacks == null || feedbacks.Count == 0)
            {
                return NotFound(new { Error = "No feedback received." });
            }

            return Ok(feedbacks);
        }

    }

}

