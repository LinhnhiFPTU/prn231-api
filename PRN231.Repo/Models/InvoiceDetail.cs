using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PRN231.Repo.Models
{
    public partial class InvoiceDetail
    {
        public int Id { get; set; }
        public int? InvoiceId { get; set; }
        public int? InventoryItemId { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Amount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? Vatamount { get; set; }
        public int? SortOrder { get; set; }
        public bool? IsPromotion { get; set; }
        public bool? IsMemo { get; set; }

        [JsonIgnore] public virtual InventoryItem? InventoryItem { get; set; }
        [JsonIgnore] public virtual Invoice? Invoice { get; set; }
    }
}
