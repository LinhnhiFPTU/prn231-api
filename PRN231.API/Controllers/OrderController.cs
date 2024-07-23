using Microsoft.AspNetCore.Mvc;
using PRN231.API.Enums;
using PRN231.API.Middlewares;
using PRN231.API.Payload.Request.Orders;
using PRN231.API.Payload.Response;
using PRN231.API.Payload.Response.POS;
using PRN231.Repo.Interfaces;

namespace PRN231.API.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        [HttpPost]
        [AuthorizePolicy(RoleEnum.ADMIN, RoleEnum.STORE_MANAGER, RoleEnum.CASHIER)]
        public IActionResult MakeOrderFromPOS(List<MakeOrderRequest> orderRequestItems)
        {
            var inventoryItems = _unitOfWork.InventoryItemRepository.Get(filter: i => orderRequestItems.Select(item => item.InventoryItemId).Contains(i.Id)).ToList();
            double totalWithoutVat = 0;
            double totalVatAmount = 0;
            double totalAmount = 0;

            foreach (var item in inventoryItems)
            {
                if (item.UnitPrice == null) continue;
                
                var itemIndex = inventoryItems.FindIndex(i => i.Id == item.Id);
                if (itemIndex == -1) continue;
                
                var quantity = orderRequestItems[itemIndex].Quantity;

                var itemTotal = (double) item.UnitPrice.Value * quantity;
                double itemVatAmount = 0;

                if (item.Vatrate.HasValue)
                {
                    itemVatAmount =  itemTotal * (double)(item.Vatrate / 100);
                }

                totalWithoutVat += itemTotal;
                totalVatAmount += itemVatAmount;
                totalAmount += itemTotal + itemVatAmount;
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
