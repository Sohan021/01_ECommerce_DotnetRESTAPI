using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace quick_ship_api.Models.User
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MobileNumber { get; set; }

        public string ProfilePhoto { get; set; }

        public string Address { get; set; }

        [DataType(DataType.PostalCode)]
        public int PostalCode { get; set; }

        //public int ServiceTypeId { get; set; }

        //public ServiceType ServiceType { get; set; }

        public string RoleName { get; set; }

        public string ApplicationRoleId { get; set; }

        public ApplicationRole ApplicationRole { get; set; }
    }
}
