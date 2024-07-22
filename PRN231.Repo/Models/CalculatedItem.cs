using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Repo.Models
{
    public class CalculatedItem
    {
        public int InventoryItemId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal VatRate { get; set; }
        public decimal Amount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
