using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rent2Me.DTO;
using Rent2Me.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rent2Me.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ContractController(AppDbContext _context, IWebHostEnvironment hostEnvironment)
        {
            context = _context;
            _hostingEnvironment = hostEnvironment;
        }
        [HttpPost("Admin")]
        public async Task<IActionResult> AddContract([FromForm] IFormParameter image)
        {
            var contract = new Contract();
            context.Contracts.Add(contract);
            context.SaveChanges();
            var imageName = $"{contract.Id}_Contract.jpg";
            var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "Files", imageName);
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await image.Image.CopyToAsync(stream);
            }
            contract.ContractPath = imagePath;
            context.SaveChanges();
            return Ok();
        }

        [HttpGet("SystemContract")]
        public async Task<IActionResult> GetContract(int contractId=1)
        {
            var contract = context.Contracts.FirstOrDefault(c => c.Id == contractId);
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", contract.ContractPath);

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }

            var imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
            return File(imageBytes, "image/jpeg");
        }

        [HttpGet("renter_contract")]
        public async Task<IActionResult> GetRenterContract(int requestId)
        {
            var request = context.RentalRequests.FirstOrDefault(c => c.RentalRequestId == requestId);
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", request.RenterContractPath);

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }

            var imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
            return File(imageBytes, "image/jpeg");
        }

        [HttpGet("{requestId}/contract")]
        public async Task<IActionResult> GetCarImage(int requestId)
        {
            var request = context.RentalRequests.FirstOrDefault(c => c.RentalRequestId == requestId);

            if (request == null || string.IsNullOrEmpty(request.RenterContractPath))
            {
                return NotFound();
            }

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", request.RenterContractPath);

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }

            var imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
            return File(imageBytes, "image/jpeg");
        }
    }
}
