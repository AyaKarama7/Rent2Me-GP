using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rent2Me.Models;
using System.IO;
using System.Threading.Tasks;

namespace Rent2Me.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetDrivingLicenseController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GetDrivingLicenseController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("{customerId}/driving_license")]
        public async Task<IActionResult> GetDrivingLicense(string customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);

            if (customer == null || string.IsNullOrEmpty(customer.DrivingLicensePath))
            {
                return NotFound();
            }

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", customer.DrivingLicensePath);

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }

            var imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
            return File(imageBytes, "image/jpeg");
        }
    }
}
