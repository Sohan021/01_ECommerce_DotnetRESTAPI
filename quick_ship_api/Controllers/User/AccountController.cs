using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using quick_ship_api.Models.User;
using quick_ship_api.Presistence.Context;
using quick_ship_api.Service.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace quick_ship_api.Controllers.User
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _configuration;

        private readonly AppDbContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IHostingEnvironment env,
            IConfiguration configuration,

            AppDbContext context
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _env = env;
            _configuration = configuration;
            _context = context;
        }


        [HttpPost]
        public async Task<object> Signin([FromBody] LoginResource model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.MobileNumber, model.Password, false, false);

            if (result.Succeeded)
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.UserName == model.MobileNumber);
                var user = _userManager.Users.Where(_ => _.Id == appUser.Id).Include(_ => _.ApplicationRole).FirstOrDefault();
                var role = _roleManager.Roles.Where(_ => _.Id == appUser.ApplicationRoleId).FirstOrDefault();
                return (appUser, role);
            }
            throw new ApplicationException();
        }



        [HttpGet("{currentUserId}")]
        public async Task<object> Profile(string currentUserId)
        {
            var appSharer = await _userManager.Users
                .Where(_ => _.Id == currentUserId)

                .FirstOrDefaultAsync();

            return appSharer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomer()
        {
            var appSharers = await _userManager.Users
                .Where(_ => _.ApplicationRole.Name == "Customer").ToListAsync();

            return Ok(appSharers);
        }

        [HttpPost, DisableRequestSizeLimit]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> SavePhoto()
        {
            var files = Request.Form.Files as List<IFormFile>;
            string imageUrl = ImageUrl(files[0]);
            return Ok(await Task.FromResult(imageUrl));
        }

        [HttpPost]
        public async Task<object> SignUp([FromBody] RegistrationResource registrationResource)
        {
            var webRoot = _env.WebRootPath;

            var PathWithFolderName = Path.Combine(webRoot, "Image");

            var role = _roleManager.Roles.Where(r => r.Name == "Customer").FirstOrDefault();

            var customer = new ApplicationUser
            {
                UserName = registrationResource.MobileNumber,
                NormalizedUserName = registrationResource.MobileNumber,
                FirstName = registrationResource.FirstName,
                Email = registrationResource.Email,
                PhoneNumber = registrationResource.MobileNumber,
                RoleName = role.Name,
                ApplicationRole = role
            };

            var result = await _userManager.CreateAsync(customer, registrationResource.Password);

            if (result.Succeeded)
            {
                return Ok(registrationResource.FirstName + " " + registrationResource.LastName + " SignUp Successfully");
            }

            throw new ApplicationException("UNKNOWN_ERROR");
        }

        [HttpPut]
        public async Task<object> ProfileUpdate([FromBody] ProfileResource profileResource)
        {
            var webRoot = _env.WebRootPath;
            var PathWithFolderName = Path.Combine(webRoot, "Image");

            var ckPhoneNumber = _userManager.Users.Where(_ => _.PhoneNumber == profileResource.MobileNumber && _.Id != profileResource.CurrentUser).AsNoTracking().FirstOrDefault();

            if (ckPhoneNumber != null)
            {
                return Ok("This Phone Number is Already used");
            }
            var currentuserDetails = _userManager.Users.Where(_ => _.Id == profileResource.CurrentUser).FirstOrDefault(_ => _.Id == profileResource.CurrentUser);

            if (currentuserDetails != null)
            {
                currentuserDetails.FirstName = profileResource.FirstName;
                currentuserDetails.LastName = profileResource.LastName;
                currentuserDetails.ProfilePhoto = profileResource.ProfilePhoto;
                currentuserDetails.Email = profileResource.Email;
                currentuserDetails.PhoneNumber = profileResource.MobileNumber;
                currentuserDetails.Address = profileResource.Address;
                currentuserDetails.PostalCode = profileResource.PostalCode;
            }
            try
            {
                var result = await _userManager.UpdateAsync(currentuserDetails);
                if (result.Succeeded)
                {
                    return Ok("You your Profile Successfully");
                }
                return Ok("Nothing to update");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        public async Task<object> ChangePassword([FromBody] PasswordChangeResource passwordChangeResource)
        {
            var currentUser = _userManager.Users.Where(_ => _.Id == passwordChangeResource.currentUserId).FirstOrDefault(_ => _.Id == passwordChangeResource.currentUserId);
            if (ModelState.IsValid)
            {
                var result = await _userManager.ChangePasswordAsync(currentUser, passwordChangeResource.CurrentPassword, passwordChangeResource.NewPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);

                    }
                    return View();
                }
                await _signInManager.RefreshSignInAsync(currentUser);
                return Ok("Password Change Successfully");
            }
            return Ok(passwordChangeResource);
        }
        public string ImageUrl(IFormFile file)
        {


            if (file == null || file.Length == 0) return null;
            string extension = Path.GetExtension(file.FileName);

            string path_Root = _env.WebRootPath;

            string path_to_Images = path_Root + "\\Image\\" + file.FileName;

            using (var stream = new FileStream(path_to_Images, FileMode.Create))
            {

                file.CopyTo(stream);
                string revUrl = Reverse.reverse(path_to_Images);
                int count = 0;
                int flag = 0;

                for (int i = 0; i < revUrl.Length; i++)
                {
                    if (revUrl[i] == '\\')
                    {
                        count++;

                    }
                    if (count == 2)
                    {
                        flag = i;
                        break;
                    }
                }

                string sub = revUrl.Substring(0, flag + 1);
                string finalString = Reverse.reverse(sub);

                string f = finalString.Replace("\\", "/");
                return f;

            }
        }
    }

    public static class Reverse
    {
        public static string reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

    }
}
