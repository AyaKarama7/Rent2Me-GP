using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Rent2Me.Services
{
    public interface IMailService
    {
        public interface IMailService
        {
            public Task SendEmailAsync(MailRequest mailRequest);
        }
    }
}
