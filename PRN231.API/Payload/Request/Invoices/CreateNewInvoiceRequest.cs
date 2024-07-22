using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace PRN231.API.Payload.Request.Invoices;

public class CreateNewInvoiceRequest
{
    [Required(ErrorMessage = "Store Id is required")]
    [FromForm(Name = "store-id")]
    public int StoreId { get; set; }

    [Required(ErrorMessage = "Payment method is required")]
    [FromForm(Name = "payment-method")]
    public byte? PaymentMethod { get; set; }

    [Required(ErrorMessage = "Invoice Template Id is required")]
    [FromForm(Name = "invoice-template-id")]
    public int? InvoiceTemplateId { get; set; }

    [Required(ErrorMessage = "Inventory items is required")]
    [FromForm(Name = "list-inventory-items")]
    public string InventoryItems { get; set; }
}