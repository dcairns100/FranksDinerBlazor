using FranksDinerBlazor.Server.Interfaces;
using FranksDinerBlazor.Shared.Constants;
using FranksDinerBlazor.Shared.Models;
using FranksDinerBlazor.Shared.Models.Econduit;
using Microsoft.AspNetCore.Mvc;

namespace FranksDinerBlazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrder _IOrder;
        private readonly IConfiguration _config;
        private readonly IEconduitService _econduitService;
        public OrderController(IOrder iOrder, IConfiguration config, IEconduitService econduitService)
        {
            _IOrder = iOrder;
            _config = config;
            _econduitService = econduitService;
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
                break;              
            }

            return Ok(order);

            //var parameters = new RunTransaction
            //{
            //    Command = "Sale",
            //    Key = _config.GetValue<string>("Settings:Econduit:ApiKey"),
            //    Password = _config.GetValue<string>("Settings:Econduit:ApiPassword"),
            //    Amount = order.Amount,
            //    TerminalId = order.TerminalId,
            //    RefID = string.Format(order.OrderDate.ToString("yyyyMMdd"), "-", order.TableNumber),
            //    InvoiceNumber = string.Empty,
            //    MerchantId = string.Empty,
            //    Token = string.Empty,
            //    ExpDate = string.Empty
            //};

            //if(await _econduitService.RunTransaction(parameters)){
            //    order.IsPaid = true;
            //    _IOrder.UpdateOrderDetails(order);

            //    return Ok("Payment Successful");
            //}
            //else
            //{
            //    return BadRequest("Payment Unsuccessful");
            //}            
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
    }
}
