using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using Rent2Me.DTO;
using Rent2Me.Models;
using System;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

namespace Rent2Me.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalRequestController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly NotificationService _notificationService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public RentalRequestController(AppDbContext context, NotificationService notificationService, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _notificationService = notificationService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("request")]
        public async Task<IActionResult> AddRequest([FromForm] RentalRequestDto input)
        {
            var renter = await _context.Set<Customer>()
           .FirstOrDefaultAsync(c => c.NationalID == input.RequesterId);
            var owner = await _context.Set<Customer>()
           .FirstOrDefaultAsync(c => c.NationalID == input.CarOwnerId);

            if (renter==null||owner==null)
            {
                return BadRequest(new { Error = "owner or renter not found" });
            }
            var request = new RentalRequest
            {
                RequesterId=input.RequesterId,
                CarOwnerId=input.CarOwnerId,
                PlaceOfDelivery=input.PlaceOfDelivery,
                TimeOfDelivery=input.TimeOfDelivery,
                LicensePlate=input.LicensePlate,
                Status="2",

            };
            _context.Add(request);
            _context.SaveChanges();
            if (input.RenterContract != null && input.RenterContract.Length > 0)
            {

                var imageName = $"{request.RentalRequestId}_RequestContract.jpg";
                var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "Files", imageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await input.RenterContract.CopyToAsync(stream);
                }
                request.RenterContractPath = imagePath;
            }
            if (renter != null)
            {
                renter.Requests.Add(request);
                owner.Requests.Add(request);
                _context.SaveChanges();
            }
            var r = new RequestOutpuParameters
            {
                RequestId=request.RentalRequestId,
                RequesterId=input.RequesterId,
                CarOwnerId=input.CarOwnerId,
                PlaceOfDelivery = input.PlaceOfDelivery,
                TimeOfDelivery = input.TimeOfDelivery,
                LicensePlate = input.LicensePlate,
            };
            await _notificationService.CreateNotificationAsync(
                request.CarOwnerId,
                "You have a new rental request",
                NotificationType.RentalRequest
            );
            return Ok(r);
        }

        [HttpPost("accept")]
        public async Task<IActionResult> AcceptRentalRequest(int id,[FromForm]IFormParameter input)
        {
            var rentalRequest = await _context.RentalRequests.FindAsync(id);
            if (rentalRequest == null)
            {
                return NotFound();
            }

            rentalRequest.IsAccepted = true;
            rentalRequest.Status = "1";

            if (input.Image != null && input.Image.Length > 0)
            {

                var imageName = $"{rentalRequest.RentalRequestId}_FinalContract.jpg";
                var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "Files", imageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await input.Image.CopyToAsync(stream);
                }
                rentalRequest.RenterContractPath = imagePath;
            }
            else return BadRequest(new {Error="Enter your signed contract"});
            await _context.SaveChangesAsync();

            // Notify the requester that the request has been rejected
            await _notificationService.CreateNotificationAsync(
                rentalRequest.RequesterId,
                "Your rental request has been accepted,see requests for more details.",
                NotificationType.RentalAccepted
            );

            return Ok();
        }

        [HttpPost("reject")]
        public async Task<IActionResult> RejectRentalRequest(int id)
        {
            var rentalRequest = await _context.RentalRequests.FindAsync(id);
            if (rentalRequest == null)
            {
                return NotFound();
            }

            rentalRequest.IsRejected = true;
            rentalRequest.Status = "0";
            await _context.SaveChangesAsync();

            // Notify the requester that the request has been rejected
            await _notificationService.CreateNotificationAsync(
                rentalRequest.RequesterId,
                "Your rental request has been rejected.",
                NotificationType.RentalRejected
            );

            return Ok();
        }
    }

}
