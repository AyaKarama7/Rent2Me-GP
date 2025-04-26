//using Rent2Me.Models;
//using System.Threading.Tasks;
//using System.Linq;
//using System;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Hosting;
//using System.IO;
//using Microsoft.AspNetCore.Http;

//namespace Rent2Me.DTO
//{
//    public class UserProfileService
//    {
//        private readonly AppDbContext _customerRepository;
//        private readonly IWebHostEnvironment _hostingEnvironment;
//        public UserProfileService(AppDbContext customerRepository, IWebHostEnvironment hostEnvironment)
//        {
//            _customerRepository = customerRepository;
//            _hostingEnvironment = hostEnvironment;
//        }

//        public async Task<PublicUserProfileDto> GetPublicCustomerByNationalID(string nationalID)
//        {
//            var customer = _customerRepository.Customers.FirstOrDefault(x => x.NationalID == nationalID);

//            if (customer == null)
//                return null;

//            var publicDto = new PublicUserProfileDto
//            {
//                Name = customer.Name,
//            };

//            if (customer.IsAddressPublic)
//                publicDto.Address = customer.Address;

//            if (customer.IsPhonePublic)
//                publicDto.Phone = customer.Phone;

//            if (customer.IsMailPublic)
//                publicDto.Email = customer.Mail;

//            if (customer.IsAgePublic)
//                publicDto.Age = customer.Age;

//            if (customer.IsGenderPublic)
//                publicDto.Gender = customer.Gender;
//            if (customer.ImageName != null)
//                publicDto.ImageName = customer.ImageName;
//            if (customer.ImagePath != null)
//                publicDto.ImagePath = customer.ImagePath;
//            if (customer.DrivingLicenseName != null)
//                publicDto.DrivingName = customer.DrivingLicenseName;
//            if (customer.DrivingLicensePath != null)
//                publicDto.DrivingPath = customer.DrivingLicensePath;
//            return publicDto;
//        }

//    }

//}
