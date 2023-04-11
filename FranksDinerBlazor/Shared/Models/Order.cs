
namespace FranksDinerBlazor.Shared.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int? TableNumber { get; set; } = null!;
        public int TerminalId { get; set; }
        public string? Items { get; set; } = null!;
        public bool IsPaid { get; set; } = false;
        public bool IsConfirmed { get; set; } = false;
        public string? Message { get; set; } = null!;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal Amount { get; set; } = 0;
    }
}
