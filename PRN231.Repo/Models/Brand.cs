using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PRN231.Repo.Models
{
    public partial class Brand
    {
        public Brand()
        {
            Accounts = new HashSet<Account>();
            BrandPartnerMappings = new HashSet<BrandPartnerMapping>();
            InventoryItems = new HashSet<InventoryItem>();
            InvoiceTemplates = new HashSet<InvoiceTemplate>();
            Stores = new HashSet<Store>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public byte? Status { get; set; }
        public string? TaxCode { get; set; }
        public string? Description { get; set; }

        [JsonIgnore] public virtual ICollection<Account> Accounts { get; set; }
        [JsonIgnore] public virtual ICollection<BrandPartnerMapping> BrandPartnerMappings { get; set; }
        [JsonIgnore] public virtual ICollection<InventoryItem> InventoryItems { get; set; }
        [JsonIgnore] public virtual ICollection<InvoiceTemplate> InvoiceTemplates { get; set; }
        [JsonIgnore] public virtual ICollection<Store> Stores { get; set; }
    }
}
