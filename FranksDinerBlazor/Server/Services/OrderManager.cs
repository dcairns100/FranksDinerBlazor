using FranksDinerBlazor.Server.Interfaces;
using FranksDinerBlazor.Server.Models;
using FranksDinerBlazor.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace FranksDinerBlazor.Server.Services
{
    public class OrderManager : IOrder
    {
        readonly DatabaseContext _dbContext = new();
        public OrderManager(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Order> GetOrderDetails()
        {
            try
            {
                return _dbContext.Orders.ToList();
            }
            catch
            {
                throw;
            }
        }

        public void AddOrder(Order Order)
        {
            try
            {
                _dbContext.Orders.Add(Order);
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void UpdateOrderDetails(Order Order)
        {
            try
            {
                _dbContext.Entry(Order).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public Order GetOrderData(int id)
        {
            try
            {
                Order? Order = _dbContext.Orders.Find(id);
                if (Order != null)
                {
                    return Order;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteOrder(int id)
        {
            try
            {
                Order? Order = _dbContext.Orders.Find(id);
                if (Order != null)
                {
                    _dbContext.Orders.Remove(Order);
                    _dbContext.SaveChanges();
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DetachEntityState(Order Order)
        {
            try
            {
                _dbContext.Entry(Order).State = EntityState.Detached;
            }
            catch
            {
                throw;
            }
        }
    }
}
