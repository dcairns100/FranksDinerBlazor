
using FranksDinerBlazor.Shared.Constants;

namespace FranksDinerBlazor.Shared.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int? TableNumber { get; set; } = null!;
        public string? Items { get; set; } = null!;
        public string? Message { get; set; } = null!;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = OrderStatus.Pending;
    }
}
