using System;
using System.Collections.Generic;

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

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<BrandPartnerMapping> BrandPartnerMappings { get; set; }
        public virtual ICollection<InventoryItem> InventoryItems { get; set; }
        public virtual ICollection<InvoiceTemplate> InvoiceTemplates { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
    }
}
