using Rent2Me;
using Microsoft.AspNetCore.Mvc;
using Rent2Me.Services;
using System;
using System.Threading.Tasks;

namespace Rent2Me.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly MailService _mailService;
        public MailController(MailService mailService)
        {
            _mailService = mailService;
        }


        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm] MailRequest request)
        {
            try
            {
                await _mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw;
            }

        }
    }
}