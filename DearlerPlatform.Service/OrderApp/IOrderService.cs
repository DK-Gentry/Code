using DearlerPlatform.Domain;
using DearlerPlatform.Service.OrderApp.Dto;
using DearlerPlatform.Service.ShappingCartApp.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DearlerPlatform.Service.OrderApp
{
    public interface IOrderService : IocTag
    {
        Task<bool> AddOrder(string customerNo, OrderMasterInputDto input, List<ShoppingCartDto> carts);

        Task<SaleOrderDto> GetOrderInfoByOrderNo(string orderNo);

        Task<bool> BuyAgain(string SaleOrderNo);
    }
}
