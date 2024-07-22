using System;
using System.Collections.Generic;

namespace PRN231.Repo.Models
{
    public partial class InvoiceTemplate
    {
        public InvoiceTemplate()
        {
            Invoices = new HashSet<Invoice>();
        }

        public int Id { get; set; }
        public int? BrandId { get; set; }
        public string? TemplateName { get; set; }
        public byte? TemplateType { get; set; }
        public byte? InvoiceType { get; set; }

        public virtual Brand? Brand { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
