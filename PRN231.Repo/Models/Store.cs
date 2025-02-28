﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PRN231.Repo.Models
{
    public partial class Store
    {
        public Store()
        {
            Invoices = new HashSet<Invoice>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Code { get; set; }
        public byte? Status { get; set; }
        public int? BrandId { get; set; }
        public string? Address { get; set; }

        [JsonIgnore] public virtual Brand? Brand { get; set; }
        [JsonIgnore] public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
