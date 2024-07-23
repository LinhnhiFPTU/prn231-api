using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRN231.API.Enums;
using PRN231.API.Middlewares;
using PRN231.API.Payload.Request.Invoices;
using PRN231.API.Payload.Response;
using PRN231.API.Payload.Response.Invoices;
using PRN231.Repo.Interfaces;
using PRN231.Repo.Models;

namespace SE160377.Lab03ProductManagement.API.Controllers;

[ApiController]
[Route("invoices")]
public class InvoicesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public InvoicesController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    [AuthorizePolicy(RoleEnum.ADMIN, RoleEnum.STORE_MANAGER)]
    public IActionResult GetInvoices([FromQuery] GetInvoiceRequest getInvoiceRequest)
    {
        if (getInvoiceRequest.storeId is not null)
        {
            var store = _unitOfWork.StoreRepository.GetById(getInvoiceRequest.storeId);
            if (store == null)
                throw new KeyNotFoundException("Store ID " + getInvoiceRequest.storeId + " does not exist");
        }

        var invoices = _unitOfWork.InvoiceRepository.Get(
            invoice =>
                (getInvoiceRequest.storeId == null || invoice.StoreId == getInvoiceRequest.storeId) &&
                (getInvoiceRequest.fromDate == null || invoice.CreateDate >= getInvoiceRequest.fromDate) &&
                (getInvoiceRequest.toDate == null || invoice.CreateDate <= getInvoiceRequest.toDate) &&
                (getInvoiceRequest.invoiceStatus == null ||
                 invoice.Status == getInvoiceRequest.invoiceStatus),
            invoice => invoice.OrderBy(invoice => invoice.CreateDate),
            "Store,InvoiceDetails");

        var pageIndex = getInvoiceRequest.pageIndex ?? 1;
        var pageSize = getInvoiceRequest.pageSize ?? 50;

        var enumerable = invoices.ToList();
        var totalItems = enumerable.Count;
        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
        var response = new BasicResponse
        {
            IsSuccess = true,
            Message = "Get all invoices successfully",
            StatusCode = HttpStatusCode.OK.GetHashCode(),
            Data = new GetInvoiceResponse()
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = pageIndex,
                PageSize = pageSize,
                data = enumerable.Skip((pageIndex - 1) * pageSize).Take(pageSize)
            }
        };
        return Ok(response);
    }

    [HttpGet("{id}")]
    [AuthorizePolicy(RoleEnum.ADMIN, RoleEnum.STORE_MANAGER)]
    public IActionResult GetInvoice(int id)
    {
        var invoice = _unitOfWork.InvoiceRepository.GetById(id);
        if (invoice == null) throw new KeyNotFoundException("Invoice ID " + id + " does not exist");

        var storeOfInvoice = _unitOfWork.StoreRepository.GetById(invoice.StoreId);

        var response = new BasicResponse
        {
            IsSuccess = true,
            Message = "Get invoice by id successfully",
            StatusCode = HttpStatusCode.OK.GetHashCode(),
            Data = new
            {
                invoice.Id,
                invoice.InvoiceCode,
                invoice.CreateDate,
                invoice.UpdateDate,
                invoice.Type,
                invoice.Status,
                invoice.PaymentMethod,
                invoice.CurrencyCode,
                invoice.ExchangeRate,
                invoice.DiscountRate,
                invoice.Vatrate,
                invoice.TotalSaleAmount,
                invoice.TotalDiscountAmount,
                invoice.TotalAmountWithoutVat,
                invoice.TotalVatamount,
                invoice.TotalAmount,
                invoice.InvoiceDetails,
                StoreName = storeOfInvoice.Name
            }
        };
        return Ok(response);
    }

    [HttpPost]
    [AuthorizePolicy(RoleEnum.ADMIN, RoleEnum.STORE_MANAGER)]
    public async Task<IActionResult> CreateInvoice([FromForm] CreateNewInvoiceRequest createNewInvoiceRequest)
    {
        var store = _unitOfWork.StoreRepository.GetById(createNewInvoiceRequest.StoreId);
        if (store == null)
            throw new KeyNotFoundException("Store ID " + createNewInvoiceRequest.StoreId + " does not exist");

        var invoiceTemplate = _unitOfWork.InvoiceTemplateRepository.GetById(createNewInvoiceRequest.InvoiceTemplateId);
        if (invoiceTemplate == null)
            throw new KeyNotFoundException("Invoice Template ID " + createNewInvoiceRequest.InvoiceTemplateId +
                                           " does not exist");

        var inventoryItems =
            JsonConvert.DeserializeObject<List<InventoryItemRequest>>(createNewInvoiceRequest.InventoryItems);

        var invoice = new Invoice
        {
            StoreId = createNewInvoiceRequest.StoreId,
            PaymentMethod = createNewInvoiceRequest.PaymentMethod,
            InvoiceTemplateId = createNewInvoiceRequest.InvoiceTemplateId,
            CreateDate = DateTime.Now,
            UpdateDate = DateTime.Now,
            Status = (byte)Status.SUCCESS,
            CurrencyCode = "VND",
            ExchangeRate = 1,
            DiscountRate = 0,
            Vatrate = 0,
            TotalSaleAmount = 0,
            TotalDiscountAmount = 0,
            TotalAmountWithoutVat = 0,
            TotalVatamount = 0,
            TotalAmount = 0
        };

        _unitOfWork.InvoiceRepository.Insert(invoice);

        foreach (var inventoryItem in inventoryItems)
        {
            var item = _unitOfWork.InventoryItemRepository.GetById(inventoryItem.Id);
            if (item == null)
                throw new KeyNotFoundException("Inventory Item ID " + inventoryItem.Id + " does not exist");

            var invoiceDetail = new InvoiceDetail
            {
                Invoice = invoice,
                InventoryItem = item,
                Quantity = inventoryItem.Quantity,
                UnitPrice = item.UnitPrice,
                Amount = item.UnitPrice * inventoryItem.Quantity,
                DiscountAmount = 0,
                Vatamount = item.UnitPrice * item.Vatrate * inventoryItem.Quantity / 100,
                SortOrder = 1,
                IsPromotion = false,
                IsMemo = false
            };

            _unitOfWork.InvoiceDetailRepository.Insert(invoiceDetail);
        }

        var avgVatRate = invoice.InvoiceDetails.Average(invoiceDetail => invoiceDetail.InventoryItem.Vatrate);
        var totalVatAmount = invoice.InvoiceDetails.Sum(invoiceDetail => invoiceDetail.Vatamount);
        var totalAmount = invoice.InvoiceDetails.Sum(invoiceDetail => invoiceDetail.Amount);
        var totalAmountWithoutVat = totalAmount - totalVatAmount;

        invoice.Vatrate = avgVatRate;
        invoice.TotalVatamount = totalVatAmount;
        invoice.TotalAmountWithoutVat = totalAmountWithoutVat;
        invoice.TotalAmount = totalAmount;

        _unitOfWork.InvoiceRepository.Update(invoice);

        await _unitOfWork.SaveAsync();

        var response = new BasicResponse
        {
            IsSuccess = true,
            Message = "Create invoice successfully",
            StatusCode = HttpStatusCode.OK.GetHashCode()
        };
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [AuthorizePolicy(RoleEnum.ADMIN, RoleEnum.STORE_MANAGER)]
    public async Task<IActionResult> DeleteInvoice(int id)
    {
        var invoice = _unitOfWork.InvoiceRepository.GetById(id);
        if (invoice == null) throw new KeyNotFoundException("Invoice ID " + id + " does not exist");

        _unitOfWork.InvoiceRepository.Delete(invoice);
        await _unitOfWork.SaveAsync();

        var response = new BasicResponse
        {
            IsSuccess = true,
            Message = "Delete invoice successfully",
            StatusCode = HttpStatusCode.OK.GetHashCode()
        };
        return Ok(response);
    }
}