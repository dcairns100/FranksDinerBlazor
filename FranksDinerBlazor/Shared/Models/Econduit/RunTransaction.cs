
namespace FranksDinerBlazor.Shared.Models.Econduit
{
    public class RunTransaction
    {
        public string Command { get; set; }
        public string Key { get; set; }
        public string Password { get; set; }
        public decimal Amount { get; set; }
        public int TerminalId { get; set; }
        public string RefID { get; set; }
        public string InvoiceNumber { get; set; }
        public string MerchantId { get; set; }
        public string Token { get; set; }
        public string ExpDate { get; set; }
    }
}
