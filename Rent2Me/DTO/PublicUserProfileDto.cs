using Microsoft.AspNetCore.Http;
using Rent2Me.Models;
using System.Collections.Generic;

namespace Rent2Me.DTO
{
    public class PublicUserProfileDto
    {
        public string Name { get; set; } // Name is always public

        public string NationalID { get; set; } // Only if it's public
        public string Gender { get; set; } // Only if it's public
        public string Address { get; set; } // Only if it's public
        public string Phone { get; set; } // Only if it's public
        public string Email { get; set; } // Only if it's public
        public int Age { get; set; } // Only if it's public
       
    }

}
