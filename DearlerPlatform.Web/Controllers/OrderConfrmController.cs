using DearlerPlatform.Core.Consts;
using DearlerPlatform.Domain;
using DearlerPlatform.Service.OrderApp;
using DearlerPlatform.Service.OrderApp.Dto;
using DearlerPlatform.Service.ProductApp;
using DearlerPlatform.Service.ShappingCartApp;
using DearlerPlatform.Service.ShappingCartApp.Dto;
using DearlerPlatform.Web.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DearlerPlatform.Web.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [CtmAuthorizationFilter]
    public class OrderConfrmController:BaseController
    {
        IShoppingCartAppService ShoppingCartAppService { get; }
        IOrderService OrderService { get; }

        public OrderConfrmController(
            IShoppingCartAppService shoppingCartAppService,
            IProductService productService,
            IOrderService orderService
            ) 
        {
            ShoppingCartAppService = shoppingCartAppService;
            OrderService = orderService;
        }

        [HttpGet]
        public async Task<IEnumerable<ShoppingCartDto>> Get()
        {
            var customerNo = HttpContext.Items[HttpContextItemKeyName.CUSTOMER_NO].ToString();
            var carts =(await ShoppingCartAppService.GetShoppingCartDtos(customerNo)).Where(m=>m.CartSelected);
            return carts;
        }

        [HttpPost]
        public async Task<bool> Add(OrderMasterInputDto input)
        {
            var customerNo = HttpContext.Items[HttpContextItemKeyName.CUSTOMER_NO].ToString();
            var carts = (await ShoppingCartAppService.GetShoppingCartDtos(customerNo)).Where(m => m.CartSelected).ToList();
            return await OrderService.AddOrder(customerNo,input, carts);
        }
    }
}
