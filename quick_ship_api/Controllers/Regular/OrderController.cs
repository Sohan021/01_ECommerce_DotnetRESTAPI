using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using quick_ship_api.Models.Regular;
using quick_ship_api.Models.User;
using quick_ship_api.Presistence.Context;
using quick_ship_api.Service.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quick_ship_api.Controllers.Regular
{
    [Route("api/[controller]/[action]")]
    public class OrderController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly AppDbContext _context;

        public OrderController(SignInManager<ApplicationUser> signInManager,
                               UserManager<ApplicationUser> userManager,
                               RoleManager<ApplicationRole> roleManager,
                               AppDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }


        [HttpPost]
        public async Task<object> Checkout([FromBody]CheckOutResource checkOut)
        {
            var CurrentUserId = checkOut.CurrentUserId;

            var currentUserDetails = _userManager.Users.Where(_ => _.Id == CurrentUserId).FirstOrDefault();

            List<Product> products = checkOut.Products;

            Order corder = new Order();

            corder.RoleId = currentUserDetails.ApplicationRoleId;

            corder.UserId = CurrentUserId;

            corder.TotalAmount = checkOut.Amount;

            _context.Add(corder);
            _context.SaveChanges();

            var order = _context.Orders.Where(_ => _.Id == corder.Id).FirstOrDefault();

            if (products != null)
            {

                foreach (var product in products)
                {
                    OrderDetail orderDetails = new OrderDetail();
                    orderDetails.OrderId = order.Id;

                    orderDetails.ProductId = product.Id;

                    _context.Add(orderDetails);
                    await _context.SaveChangesAsync();

                }

            }

            var rowCount = _context.Orders.Count() + 1;

            order.OrderNo = rowCount;

            order.UserName = currentUserDetails.FirstName;

            order.PhoneNo = currentUserDetails.PhoneNumber;

            _context.Update(order);
            await _context.SaveChangesAsync();

            return order;
        }


        [HttpGet]
        public async Task<object> OrderList()
        {
            var ordrLst = await _context.Orders.ToListAsync();
            return ordrLst;
        }


        [HttpGet("{id}")]
        public async Task<object> OrderListCustomer([FromRoute] string id)
        {
            var ordrLst = await _context.Orders
                                .Where(_ => _.User.Id == id)
                                .ToListAsync();

            return ordrLst;
        }


        [HttpGet("{id}")]
        public async Task<object> OrderDetails([FromRoute] int id)
        {

            var products = await _context
                            .OrderDetails
                            .Where(_ => _.OrderId == id)
                            .Select(p => p.Product)
                            .Include(_ => _.Category)
                            .Include(_ => _.SubCategory)
                            .ToListAsync();

            return products;
        }

    }
}
