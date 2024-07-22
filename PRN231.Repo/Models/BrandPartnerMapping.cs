using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PRN231.Repo.Models
{
    public partial class BrandPartnerMapping
    {
        public int Id { get; set; }
        public int? BrandId { get; set; }
        public int? PartnerId { get; set; }
        public byte? Status { get; set; }
        public string? Config { get; set; }

        [JsonIgnore] public virtual Brand? Brand { get; set; }
        [JsonIgnore] public virtual Partner? Partner { get; set; }
    }
}
