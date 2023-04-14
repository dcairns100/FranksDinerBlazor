using FranksDinerBlazor.Server.Interfaces;
using FranksDinerBlazor.Shared.Constants;
using FranksDinerBlazor.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace FranksDinerBlazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrder _IOrder;
        private readonly IConfiguration _config;
        public OrderController(IOrder iOrder, IConfiguration config)
        {
            _IOrder = iOrder;
            _config = config;
        }

        [HttpGet]
        public async Task<List<Order>> Get()
        {
            return await Task.FromResult(_IOrder.GetOrderDetails());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Order Order = _IOrder.GetOrderData(id);
            if (Order != null)
            {
                return Ok(Order);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Post(Order order)
        {
            _IOrder.AddOrder(order);

            var timeout = _config.GetValue<int>("Settings:OrderTimeout");

            while (order.Status == OrderStatus.Pending)
            {
                if (DateTime.Now <= order.OrderDate.AddSeconds(timeout))
                {
                    _IOrder.DetachEntityState(order);
                    order = _IOrder.GetOrderData(order.Id);
                    continue;
                }
                order.Message = "pending order has timed out, please call for assistance";
                break;              
            }

            return Ok(order);     
        }

        [HttpPut]
        public void Put(Order order)
        {
            _IOrder.UpdateOrderDetails(order);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _IOrder.DeleteOrder(id);
            return Ok();
        }

        [HttpPut("status")]
        public void Status(Order order)
        {
            var origOrder = _IOrder.GetOrderData(order.Id);
            origOrder.Status = order.Status;
            _IOrder.UpdateOrderDetails(origOrder);
        }
    }
}
