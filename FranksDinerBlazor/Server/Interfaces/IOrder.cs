using FranksDinerBlazor.Shared.Models;

namespace FranksDinerBlazor.Server.Interfaces
{
    public interface IOrder
    {
        public List<Order> GetOrderDetails();
        public void AddOrder(Order Order);
        public void UpdateOrderDetails(Order Order);
        public Order GetOrderData(int id);
        public void DeleteOrder(int id);
        public void DetachEntityState(Order Order);
    }
}
