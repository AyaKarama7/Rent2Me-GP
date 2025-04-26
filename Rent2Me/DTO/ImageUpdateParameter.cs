using Microsoft.AspNetCore.Http;

namespace Rent2Me.DTO
{
    public class ImageUpdateParameter
    {
        public string PropertyName { get; set; }
        public IFormFile Image { get; set; }
    }
}
