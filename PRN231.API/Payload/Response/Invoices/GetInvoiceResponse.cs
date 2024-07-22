namespace PRN231.API.Payload.Response.Invoices;

public class GetInvoiceResponse
{
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public object? data { get; set; }
}