using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PRN231.Repo.Models
{
    public partial class Invoice
    {
        public Invoice()
        {
            InvoiceDetails = new HashSet<InvoiceDetail>();
        }

        public int Id { get; set; }
        public string? InvoiceCode { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public byte? Type { get; set; }
        public byte? Status { get; set; }
        public byte? PaymentMethod { get; set; }
        public string? CurrencyCode { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? DiscountRate { get; set; }
        public decimal? Vatrate { get; set; }
        public int? StoreId { get; set; }
        public int? InvoiceTemplateId { get; set; }
        public decimal? TotalSaleAmount { get; set; }
        public decimal? TotalDiscountAmount { get; set; }
        public decimal? TotalAmountWithoutVat { get; set; }
        public decimal? TotalVatamount { get; set; }
        public decimal? TotalAmount { get; set; }

        [JsonIgnore] public virtual InvoiceTemplate? InvoiceTemplate { get; set; }
        [JsonIgnore] public virtual Store? Store { get; set; }
        [JsonIgnore] public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    }
}
