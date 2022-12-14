using APPUI.Data;
using APPUI.DTO;
using APPUI.Entities;
using APPUI.Entities.OrderAggregate;
using APPUI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APPUI.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly StoreContext _context;

        public OrdersController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetOrders()
        {
            return await _context.Orders
               .ProjectOrderToOrderDto()
               .Where(x => x.BuyerId == User.Identity.Name)
               .ToListAsync();
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            return await _context.Orders
                .ProjectOrderToOrderDto()
                .Where(x => x.BuyerId == User.Identity.Name && x.Id == id)
                .FirstOrDefaultAsync();
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateOrder(CreateOrderDto orderDto)
        {
            var cart = await _context.Carts
                .RetrieveCartWithItems(User.Identity.Name)
                .FirstOrDefaultAsync();

            if (cart == null) return BadRequest(new ProblemDetails { Title = "Could not locate basket" });

            var items = new List<OrderItem>();

            foreach (var item in cart.Items)
            {
                var productItem = await _context.Medicines.FindAsync(item.MedicineId);
                var itemOrdered = new MedicineItemOrdered
                {
                    MedicineId = productItem.Id,
                    Name = productItem.Name,
                    PictureUrl = productItem.PictureUrl
                };

                var orderItem = new OrderItem
                {
                    ItemOrdered = itemOrdered,
                    Price = productItem.Price,
                    Quantity = item.Quantity
                };
                items.Add(orderItem);
                productItem.QuantityStock -= item.Quantity;
            }

            var subtotal = items.Sum(item => item.Price * item.Quantity);
            var deliveryFee = subtotal > 10000 ? 0 : 500;

            var order = new Order
            {
                OrderItems = items,
                BuyerId = User.Identity.Name,
                ShippingAddress = orderDto.ShippingAddress,
                Subtotal = subtotal,
                DeliveryFee = deliveryFee,

            };

            _context.Orders.Add(order);
            _context.Carts.Remove(cart);

            if (orderDto.SaveAddress)
            {
                var user = await _context.Users
                    //.Include(a => a.Address)
                    .FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);

                var address = new UserAddress
                {
                    FullName = orderDto.ShippingAddress.FullName,
                    Address1 = orderDto.ShippingAddress.Address1,
                    Address2 = orderDto.ShippingAddress.Address2,
                    City = orderDto.ShippingAddress.City,
                    State = orderDto.ShippingAddress.State,
                    Zip = orderDto.ShippingAddress.Zip,
                    Country = orderDto.ShippingAddress.Country
                };
                _context.Update(user);
            }
            var result = await _context.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetOrder", new { id = order.Id }, order.Id);

            return BadRequest( "Problem while creating order" );
        }
    }
}
