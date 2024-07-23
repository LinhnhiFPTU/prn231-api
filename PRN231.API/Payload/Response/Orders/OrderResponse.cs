using PRN231.Repo.Models;

namespace PRN231.API.Payload.Response.POS
{
    public class OrderResponse
    {
        public List<InventoryItem> InventoryItems { get; set; }
        public double TotalAmountWithoutVAT { get; set; }
        public double TotalVATAmount { get; set; }
        public double TotalAmount {  get; set; }
    }
}
