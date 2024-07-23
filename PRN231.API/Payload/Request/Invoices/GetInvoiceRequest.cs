using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using PRN231.API.Enums;

namespace PRN231.API.Payload.Request.Invoices;

public class GetInvoiceRequest
{
    [BindProperty(Name = "store-id")] public int? storeId { get; set; }

    [BindProperty(Name = "from-date")]
    [DataType(DataType.Date)]
    public DateTime? fromDate { get; set; }

    [BindProperty(Name = "to-date")]
    [DataType(DataType.Date)]
    public DateTime? toDate { get; set; }

    [BindProperty(Name = "invoice-status")]
    public byte? invoiceStatus { get; set; }

    [BindProperty(Name = "page-index")] public int? pageIndex { get; set; } = 1;

    [BindProperty(Name = "page-size")]
    [Range(1, 200, ErrorMessage = "Page size need to be in range 1-200")]
    public int? pageSize { get; set; } = 50;
}