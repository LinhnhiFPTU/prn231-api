using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRN231.API.Payload.Response;
using PRN231.API.Payload.Response.POS;
using PRN231.Repo.Implements;
using PRN231.Repo.Interfaces;
using PRN231.Repo.Models;

namespace PRN231.API.Controllers
{
    [ApiController]
    [Route("pos")]
    public class PosController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PosController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost("orders")]
        public IActionResult MakeOrderFromPOS(List<int> inventoryItemId)
        {
            var inventoryItems = _unitOfWork.InventoryItemRepository.Get(filter: i => inventoryItemId.Contains(i.Id)).ToList();
            double totalWithoutVat = 0;
            double totalVatAmount = 0;
            double totalAmount = 0;

            foreach (var item in inventoryItems)
            {
                double itemTotal = (double)item.UnitPrice.Value;
                double itemVat = 0;

                if (item.Vatrate.HasValue)
                {
                    itemVat =  itemTotal * (double)(item.Vatrate / 100);
                }

                totalWithoutVat += itemTotal;
                totalVatAmount += itemVat;
                totalAmount += itemTotal + itemVat;
            }
            var result = new BasicResponse
            {
                IsSuccess = true,
                Message = "",
                StatusCode = 200,
                Data = new OrderResponse
                {
                    InventoryItems = inventoryItems,
                    TotalAmountWithoutVAT = totalWithoutVat,
                    TotalVATAmount = totalVatAmount,
                    TotalAmount = totalAmount
                }
            };

            return Ok(result);

        }
    }
}
