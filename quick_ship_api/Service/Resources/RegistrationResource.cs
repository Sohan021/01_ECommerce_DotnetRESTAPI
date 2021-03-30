using Microsoft.AspNetCore.Http;
using quick_ship_api.Models.User;
using System.ComponentModel.DataAnnotations;

namespace quick_ship_api.Service.Resources
{
    public class RegistrationResource
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MobileNumber { get; set; }

        public string Email { get; set; }

        public IFormFile ProfilePhotoFile { get; set; }

        public string ProfilePhoto { get; set; }

        [DataType(DataType.PostalCode)]
        public int PostalCode { get; set; }

        public string Address { get; set; }

        public ApplicationRole ApplicationRole { get; set; }


        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
