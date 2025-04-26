using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using Rent2Me.DTO;
using Rent2Me.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rent2Me.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public CarController(AppDbContext _context,IWebHostEnvironment hostEnvironment)
        {
            context = _context;
            _hostingEnvironment = hostEnvironment;

        }
        [HttpGet("DisplayAllCars")]
        public IActionResult DisplayAllCars()
        {
            var cars = context.CarDetails
                              .Select(car => new CarInputParameters
                              {
                                  LicencePlate = car.LicencePlate,
                                  CarType = car.CarType,
                                  Brand = car.Brand,
                                  Color = car.Color,
                                  CurrentMileage = car.CurrentMileage,
                                  Year = car.Year,
                                  Model = car.Model,
                                  SeatingCapacity = car.SeatingCapacity,
                                  RentingPrice = car.RentingPrice,
                                  UserrID = car.CustomerId,
                              })
                              .ToList();

            return Ok(cars);
        }

        [HttpGet("DisplayByUser")]
        public async Task<IActionResult> DisplayUserCars(string nationalId)
        {
            var user = await context.Customers.Include(u => u.Cars).FirstOrDefaultAsync(u => u.NationalID == nationalId);
            if (user == null)
            {
                return BadRequest(new { Error = "User doesn't exist." });
            }
            var car = user.Cars.ToList();

            var UserCars = user.Cars
                              .Select(car => new CarInputParameters
                              {
                                  LicencePlate = car.LicencePlate,
                                  CarType = car.CarType,
                                  Brand = car.Brand,
                                  Color = car.Color,
                                  CurrentMileage = car.CurrentMileage,
                                  Year = car.Year,
                                  Model = car.Model,
                                  SeatingCapacity = car.SeatingCapacity,
                                  RentingPrice = car.RentingPrice,
                                  UserrID=car.CustomerId,
                              })
                              .ToList();
            return Ok(UserCars);
        }
        [HttpGet("DisplayCar")]
        public async Task<IActionResult> DisplayCar(string license)
        {
            var car = await context.CarDetails.FirstOrDefaultAsync(c => c.LicencePlate==license);
            if (car == null)
            {
                return BadRequest(new { Error = "car doesn't exist." });
            }
            return Ok(car);
        }
        [HttpPost("AddCar")]
        public async Task<IActionResult> AddCar([FromForm] CarInputParameters input)
        {
            var customer = await context.Set<Customer>()
           .FirstOrDefaultAsync(c => c.NationalID == input.UserrID);
            
            if (context.CarDetails.Any(u => u.LicencePlate == input.LicencePlate))
            {
                return BadRequest(new { Error = "Car already exist" });
            }
            var car = new CarDetails
            {
                LicencePlate = input.LicencePlate,
                CarType = input.CarType,
                Brand = input.Brand,
                Color = input.Color,
                CurrentMileage = input.CurrentMileage,
                Year = input.Year,
                Model = input.Model,
                SeatingCapacity = input.SeatingCapacity,
                RentingPrice = input.RentingPrice,
                Avilability = true

            };
            if (input.Image != null && input.Image.Length > 0)
            {

                var imageName = $"{car.LicencePlate}_Image.jpg";
                var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "Files", imageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await input.Image.CopyToAsync(stream);
                }
                car.ImagePath = imagePath;
            }
            if (input.PropertyDeed != null && input.PropertyDeed.Length > 0)
            {

                var imageName = $"{car.LicencePlate}_PropertyDeed.jpg";
                var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "Files", imageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await input.PropertyDeed.CopyToAsync(stream);
                }
                car.PropertyDeedPath = imagePath;
            }
            context.Add(car);
            context.SaveChanges();
            if (customer != null)
            {
                customer.Cars.Add(car);
                context.SaveChanges();
            }
            return Ok(new { Message = "Car was added successfully." });
        }


        [HttpDelete("RemoveCar")]
        public IActionResult DeleteCar(string license)
        {
            var car = context.CarDetails.FirstOrDefault(c => c.LicencePlate == license);
            if (car == null)
            {
                return NotFound(new { Error = "Error occured" });
            }
            context.Remove(car);
            context.SaveChanges();
            return Ok(new { Message = "Car deleted successfully" });
        }
        [HttpPost("search")]
        public IActionResult SearchCars([FromBody] CarSearchParameters searchParameters)
        {
            var query = context.CarDetails.Where(car => car.Avilability == true);

            if (searchParameters.CarType != null)
            {
                query = context.CarDetails.Where(car => car.CarType == searchParameters.CarType);
            }

            if (searchParameters.Brand != null)
            {
                query = context.CarDetails.Where(car => car.Brand == searchParameters.Brand);
            }

            if (searchParameters.Color != null)
            {
                query = context.CarDetails.Where(car => car.Color == searchParameters.Color);
            }

            if (searchParameters.Year!=null)
            {
                query = context.CarDetails.Where(car => car.Year == searchParameters.Year);
            }

            if (searchParameters.Model != null)
            {
                query = context.CarDetails.Where(car => car.Model == searchParameters.Model);
            }

            if (searchParameters.SeatingCapacity!=null)
            {
                query = context.CarDetails.Where(car => car.SeatingCapacity == searchParameters.SeatingCapacity);
            }

            var searchResults = query.Select(car => new CarSearchResult
            {
                Car = car,
                SimilarityScore = searchParameters.CalculateScore(car, searchParameters)
            }).ToList();

            searchResults.Sort((x, y) => y.SimilarityScore.CompareTo(x.SimilarityScore));
            var filteredCars = searchResults.Select(result => result.Car).ToList();

            return Ok(filteredCars);
        }

        [HttpPatch("update")]
        public IActionResult UpdateCar(string license, CarUpdateParameters request)
        {
            var car = context.CarDetails.FirstOrDefault(c => c.LicencePlate == license);
            if (car == null)
            {
                return NotFound();
            }
            var propertyInfo = typeof(CarDetails).GetProperty(request.PropertyName);
            var value = request.NewValue;
            if (propertyInfo == null)
            {
                return BadRequest(new { Error = "Invalid property name." });
            }
            propertyInfo.SetValue(car, Convert.ChangeType(value, propertyInfo.PropertyType));
            context.SaveChanges();
            return Ok(car);
        }

        [HttpPut("{licensePlate}/updateCarImage")]
        public async Task<IActionResult> UpdateCarImage(string licensePlate, [FromForm] IFormParameter image)
        {
            var car = context.CarDetails.FirstOrDefault(c => c.LicencePlate == licensePlate);
            if (car == null) return NotFound();
            if (image.Image == null)
            {
                car.ImagePath = null;
            }
            else
            {
                var imageName = $"{car.LicencePlate}_Image.jpg";
                var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "Files", imageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.Image.CopyToAsync(stream);
                }
                car.ImagePath = imagePath;
            }
            context.SaveChanges();
            return Ok(car);
        }

        [HttpPut("{licensePlate}/updateCarPropertyDeed")]
        public async Task<IActionResult> UpdateCarPropertyDeed(string licensePlate, [FromForm] IFormParameter image)
        {
            var car = context.CarDetails.FirstOrDefault(c => c.LicencePlate == licensePlate);
            if (car == null) return NotFound();
            if (image == null)
            {
                car.PropertyDeedPath = null;
            }
            else
            {
                var imageName = $"{car.LicencePlate}_PropertyDeed.jpg";
                var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "Files", imageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.Image.CopyToAsync(stream);
                }
                car.PropertyDeedPath = imagePath;
            }
            context.SaveChanges();
            return Ok(car);
        }

        [HttpGet("{licensePlate}/image")]
        public async Task<IActionResult> GetCarImage(string licensePlate)
        {
            var car = context.CarDetails.FirstOrDefault(c => c.LicencePlate == licensePlate);

            if (car == null || string.IsNullOrEmpty(car.ImagePath))
            {
                return NotFound();
            }

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", car.ImagePath);

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }

            var imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
            return File(imageBytes, "image/jpeg");
        }
        [HttpGet("{licensePlate}/propertyDeed")]
        public async Task<IActionResult> GetcarPropertyDeed(string licensePlate)
        {
            var car = context.CarDetails.FirstOrDefault(c => c.LicencePlate == licensePlate);

            if (car == null || string.IsNullOrEmpty(car.PropertyDeedPath))
            {
                return NotFound();
            }

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", car.PropertyDeedPath);

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }

            var imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
            return File(imageBytes, "image/jpeg");
        }
    }
}
