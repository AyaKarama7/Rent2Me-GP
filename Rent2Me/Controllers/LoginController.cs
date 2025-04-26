using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Rent2Me.Models;
using Rent2Me.DTO;
using System.Linq;
using Rent2Me.Admin.Models;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;

    public LoginController(IConfiguration configuration, AppDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [HttpPost("login")]
    public IActionResult Login(UserLoginDTO model)
    {
        var user = _context.Customers.FirstOrDefault(u => u.NationalID == model.NationalID && u.Password == model.Password);
        if (user == null)
        {
            return Unauthorized(new { Error = "Invalid NationalID or password." });
        }

        var token = GenerateJwtToken(user);

        return Ok(user);
    }
    [HttpPost("ForgetPassword")]
    public IActionResult ForgetPassword(string email)
    {
        var user = _context.Customers.FirstOrDefault(u => u.Mail == email);
        if (user == null) return BadRequest(new { Error = "Invalid Mail" });
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("rent2mecompany@gmail.com");
        mail.To.Add(email);
        mail.Subject = "Reset Password";
        mail.IsBodyHtml = true;
        var vcode = $"123#sX";
        mail.Body = vcode;
        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("rent2mecompany@gmail.com", "312Omnia#"),
            EnableSsl = true
        };
        smtpClient.Send(mail);
        return Ok(vcode);
    }
    [HttpPost("reset")]
    public IActionResult ResetPassword(string userId,string password,string cpassword)
    {
        var user = _context.Customers.FirstOrDefault(u => u.NationalID == userId);
        if (user == null) return BadRequest(new { Error = "Incorrect user." });
        if (!IsValidPassword(password))
        {
            return BadRequest(new { Error = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character." });
        }

        if (!IsValidConfPassword(password, cpassword))
        {
            return BadRequest(new { Error = "Confirmation Password and Password do not match." });
        }
        return Ok();
    }
    private bool IsValidPassword(string password)
    {
        const string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";

        return Regex.IsMatch(password, pattern);
    }
    private bool IsValidConfPassword(string password, string confpassword)
    {
        if (password == confpassword) return true;
        return false;
    }
    private string GenerateJwtToken(Customer user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.NationalID)
                // Add more claims if needed (e.g., roles,permissions..)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
