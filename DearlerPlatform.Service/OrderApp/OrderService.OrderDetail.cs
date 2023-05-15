using DearlerPlatform.Domain;
using DearlerPlatform.Service.ShappingCartApp.Dto;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DearlerPlatform.Service.OrderApp
{
    public partial class OrderService
    {
        private async Task AddOrderDetail(List<ShoppingCartDto> carts,
                                    string customerNo,
                                    string orderNo,
                                    DateTime inputDate)
        {
            foreach (var cart in carts)
            {
                SaleOrderDetail detail = new()
                {
                    SaleOrderGuid = Guid.NewGuid().ToString(),
                    SaleOrderNo = orderNo,
                    ProductNo = cart.ProductNo,
                    ProductName = cart.ProductDto.ProductName,
                    ProductPhotoUrl = cart.ProductDto.ProductPhoto?.ProductPhotoUrl,
                    CustomerNo = customerNo,
                    InputDate = inputDate,
                    OrderNum = cart.ProductNum,
                    BasePrice = cart.ProductDto.ProductSale?.SalePrice ?? 0,
                    DiffPrice = 0,
                    SalePrice = cart.ProductDto.ProductSale?.SalePrice ?? 0
                };
                await OrderDetailRepo.InsertAsync(detail);
            }
        }

        private async Task<List<SaleOrderDetail>> GetOrderDetailsByOrderNo(string orderNo)
        {
            return await OrderDetailRepo.GetListAsync(m => m.SaleOrderNo == orderNo);
        }
    }
}
