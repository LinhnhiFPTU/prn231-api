using System;
using System.Collections.Generic;

namespace PRN231.Repo.Models
{
    public partial class BrandPartnerMapping
    {
        public int Id { get; set; }
        public int? BrandId { get; set; }
        public int? PartnerId { get; set; }
        public byte? Status { get; set; }
        public string? Config { get; set; }

        public virtual Brand? Brand { get; set; }
        public virtual Partner? Partner { get; set; }
    }
}
