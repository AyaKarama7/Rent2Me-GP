using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Rent2Me.DTO
{
    public class UserRegisterationDTO
    {
        [Required(ErrorMessage = "NationalID is required.")]
        public string NationalID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        [StringLength(11, MinimumLength = 11)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password confermation is required.")]
        public string ConfirmPassword { get; set; }
        public IFormFile Image { get; set; }
        public IFormFile DrivingLicense { get; set; }

    }
}
