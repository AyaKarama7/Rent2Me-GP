using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent2Me.DTO;
using Rent2Me.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rent2Me.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        //private readonly UserProfileService _userProfileService;
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UserProfileController( AppDbContext _context,IWebHostEnvironment hostEnvironment)
        {
            //_userProfileService = userProfileService;
            context = _context;
            _hostingEnvironment = hostEnvironment;
        }


        [HttpGet("{nationalID}")]
        public async Task<IActionResult> GetPublicCustomerByNationalID(string nationalID)
        {
            var customer = context.Customers.FirstOrDefault(x => x.NationalID == nationalID);

            if (customer == null)
                return null;

            var publicDto = new PublicUserProfileDto
            {
                Name = customer.Name,
            };

            if (customer.IsAddressPublic)
                publicDto.Address = customer.Address;

            if (customer.IsPhonePublic)
                publicDto.Phone = customer.Phone;

            if (customer.IsMailPublic)
                publicDto.Email = customer.Mail;

            if (customer.IsAgePublic)
                publicDto.Age = customer.Age;

            if (customer.IsGenderPublic)
                publicDto.Gender = customer.Gender;
            return Ok(customer);
        }
        
        [HttpGet("{customerId}/image")]
        public async Task<IActionResult> GetCustomerImage(string customerId)
        {
            var customer = await context.Customers.FindAsync(customerId);

            if (customer == null || string.IsNullOrEmpty(customer.ImagePath))
            {
                return NotFound();
            }

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", customer.ImagePath);

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }

            var imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
            return File(imageBytes, "image/jpeg");
        }

        [HttpGet("{customerId}/drivingLicense")]
        public async Task<IActionResult> GetCustomerDrivingLicense(string customerId)
        {
            var customer = await context.Customers.FindAsync(customerId);

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

        [HttpPatch("update")]
        public IActionResult UpdateProfile(string nationalId, ProfileUpdateParameters request)
        {
            var user = context.Customers.FirstOrDefault(c => c.NationalID == nationalId);
            if (user == null)
            {
                return NotFound();
            }
            var propertyInfo = typeof(Customer).GetProperty(request.PropertyName);
            var value = request.NewValue;
            if (propertyInfo == null)
            {
                return BadRequest(new { Error = "Invalid property name." });
            }
            propertyInfo.SetValue(user, Convert.ChangeType(value, propertyInfo.PropertyType));
            context.SaveChanges();
            return Ok(new { Message = "Profile updated successfully" });
        }

        [HttpPatch("{customerId}/updateProfileImage")]
        public async Task<IActionResult> UpdateProfileImage(string customerId, [FromForm] IFormParameter image)
        {
            var customer = context.Customers.FirstOrDefault(c => c.NationalID == customerId);
            if (customer == null) return NotFound();
            if (image.Image == null)
            {
                customer.ImagePath = null;
            }
            else
            {
                var imageName = $"{customer.NationalID}_Image.jpg";
                var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "Files", imageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.Image.CopyToAsync(stream);
                }
                customer.ImagePath = imagePath;
            }
            context.SaveChanges();
            return Ok();
        }
        [HttpPatch("{customerId}/updateDrivingLicense")]
        public async Task<IActionResult> UpdateDrivingLicense(string customerId, [FromForm] IFormParameter image)
        {
            var customer = context.Customers.FirstOrDefault(c => c.NationalID == customerId);
            if (customer == null) return NotFound();
            if (image.Image == null)
            {
                customer.DrivingLicensePath = null;
            }
            else
            {
                var imageName = $"{customer.NationalID}_DrivingLicense.jpg";
                var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "Files", imageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.Image.CopyToAsync(stream);
                }
                customer.DrivingLicensePath= imagePath;
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpGet("DisplayUserRequests")]
        public async Task<IActionResult> DisplayUserRequests(string nationalId, string status)
        {
            var user = await context.Customers.Include(u => u.Requests).FirstOrDefaultAsync(u => u.NationalID == nationalId);
            if (user == null)
            {
                return BadRequest(new { Error = "User doesn't exist." });
            }

            var UserRequests = user.Requests
                              .Where(request=>request.Status==status)
                              .Select(request => new RequestOutpuParameters
                              {
                                  RequestId = request.RentalRequestId,
                                  RequesterId = request.RequesterId,
                                  CarOwnerId = request.CarOwnerId,
                                  PlaceOfDelivery = request.PlaceOfDelivery,
                                  TimeOfDelivery = request.TimeOfDelivery,
                                  LicensePlate = request.LicensePlate,
                              })
                              .ToList();
            return Ok(UserRequests);
        }
    }

}
