using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PRN231.Repo.Models
{
    public partial class Partner
    {
        public Partner()
        {
            BrandPartnerMappings = new HashSet<BrandPartnerMapping>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ApiUrl { get; set; }
        public byte? Type { get; set; }
        public byte? Environment { get; set; }
        public byte? Status { get; set; }

        [JsonIgnore] public virtual ICollection<BrandPartnerMapping> BrandPartnerMappings { get; set; }
    }
}
