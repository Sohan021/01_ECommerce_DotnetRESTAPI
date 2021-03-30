using Microsoft.AspNetCore.Identity;

namespace quick_ship_api.Models.User
{
    public class ApplicationRole : IdentityRole
    {
        public string RoleName { get; set; }

        public string Description { get; set; }
    }
}
