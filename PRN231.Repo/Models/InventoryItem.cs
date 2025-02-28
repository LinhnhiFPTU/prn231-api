﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PRN231.Repo.Models
{
    public partial class InventoryItem
    {
        public InventoryItem()
        {
            InvoiceDetails = new HashSet<InvoiceDetail>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? UnitName { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Vatrate { get; set; }
        public byte? Status { get; set; }
        public int? BrandId { get; set; }

        [JsonIgnore] public virtual Brand? Brand { get; set; }
        [JsonIgnore] public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    }
}
