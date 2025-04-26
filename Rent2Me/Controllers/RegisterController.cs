using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Rent2Me.DTO;
using Rent2Me.Models;
using Rent2Me.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class RegisterController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _hostingEnvironment;
    public RegisterController(AppDbContext context,IWebHostEnvironment hostEnvironment)
    {
        _context = context;
        _hostingEnvironment = hostEnvironment;
    }
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromForm]UserRegisterationDTO model)
    {

        if (!IsValidNationalID(model.NationalID))
        {
            return BadRequest(new { Error = "Invalid National ID." });
        }

        if (!IsValidName(model.Name))
        {
            return BadRequest(new { Error = "Invalid Name." });
        }

        if (!IsValidPhone(model.Phone))
        {
            return BadRequest(new { Error = "Invalid Phone Number." });
        }

        if (!IsValidGmail(model.Mail))
        {
            return BadRequest(new { Error = "Invalid Gmail address." });
        }

        if (!IsValidPassword(model.Password))
        {
            return BadRequest(new { Error = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character." });
        }

        if(!IsValidConfPassword(model.Password,model.ConfirmPassword))
        {
            return BadRequest(new { Error = "Confirmation Password and Password do not match." });
        }

        if(!IsValidAge(model.NationalID))
        {
            return BadRequest(new { Error = "You are under 18 years old" });
        }
        if(IsExistCustomer(model.NationalID))
        {
            return BadRequest(new { Error = "You already have acount, please Login instead" });
        }
        var user = new Customer
        {
            NationalID = model.NationalID,
            Name = model.Name,
            Phone = model.Phone,
            Address = model.Address,
            Mail = model.Mail,
            Password = model.Password,

        };
        {
            user.Birthdate = GetBirthdate(model.NationalID);
            user.Age = GetAge(model.NationalID);
            user.Gender = GetGender(model.NationalID);
        }
        if (model.Image != null && model.Image.Length > 0)
        {
            var wwwrootPath = Path.Combine(_hostingEnvironment.WebRootPath, "Files");
            if (!Directory.Exists(wwwrootPath))
            {
                Directory.CreateDirectory(wwwrootPath);
            }
            var imageName = $"{user.NationalID}_Image.jpg";
            var imagePath = Path.Combine("Files", imageName); // Use relative path
            using (var stream = new FileStream(Path.Combine(_hostingEnvironment.WebRootPath, imagePath), FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }
            user.ImagePath = imagePath; // Save relative path or just the filename

        }
        if (model.DrivingLicense != null && model.DrivingLicense.Length > 0)
        {

            var imageName = $"{user.NationalID}_DrivingLicense.jpg";
            var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "Files", imageName);
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await model.DrivingLicense.CopyToAsync(stream);
            }
            user.DrivingLicensePath = imagePath;
        }

        _context.Customers.Add(user);
        _context.SaveChanges();
        var token = Guid.NewGuid().ToString();
        return Ok(new { Message = "User registered successfully"});

    }
    private bool IsValidNationalID(string nationalID)
    {
        return !string.IsNullOrEmpty(nationalID) && nationalID.Length == 14 && nationalID.All(char.IsDigit);
    }

    private bool IsValidName(string name)
    {
        return !string.IsNullOrEmpty(name) && Regex.IsMatch(name, @"^[A-Za-z\s]+$");
    }

    private bool IsValidPhone(string phone)
    {
        return (!string.IsNullOrEmpty(phone) && phone.Length == 11 && phone.All(char.IsDigit)&& ! _context.Customers.Any(u => u.Phone == phone));
    }

    private bool IsValidGmail(string mail)
    {
        return (!string.IsNullOrEmpty(mail) && Regex.IsMatch(mail, @"^[a-zA-Z0-9._%+-]+@gmail.com$")&& ! _context.Customers.Any(u => u.Mail == mail));
    }

    private bool IsValidPassword(string password)
    {
        const string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";

        return Regex.IsMatch(password, pattern);
    }
    private bool IsValidConfPassword(string password,string confpassword)
    {
        if(password==confpassword)return true;
        return false;
    }
    private bool IsValidAge(string nationalID)
    {
        int age = GetAge(nationalID);
        if (age >= 18) return true;
        return false;
    }
    private bool IsExistCustomer(string nationalID)
    {
        return (_context.Customers.Any(u => u.NationalID == nationalID));
    }
    private DateTime GetBirthdate(string nationalId)
    {
        try
        {
            int century = int.Parse(nationalId.Substring(0, 1));
            int year = int.Parse(nationalId.Substring(1, 2));
            int month = int.Parse(nationalId.Substring(3, 2));
            int day = int.Parse(nationalId.Substring(5, 2));
            //30303172402205
            int birthYear = (century - 1) * 1000 + year;
            DateTime birthdate = new DateTime(birthYear, month, day);

            return birthdate;
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Invalid National ID format", ex);
        }
    }
    private int GetAge(string nationalId)
    {
        try
        {
            DateTime birthdate = GetBirthdate(nationalId);
            DateTime currentDate = DateTime.Today; 
            TimeSpan age = currentDate - birthdate; 
            int years = (int)(age.TotalDays / 365.25);
            return years;
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Invalid National ID format", ex);
        }
    }
    private string GetGender(string nationalId)
    {
        string genderDigit = nationalId.Substring(12, 1);
        try
        {
            string gender = (int.Parse(genderDigit) % 2 == 0) ? "Female" : "Male";
            return gender;
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Invalid National ID format", ex);
        }
    }
}