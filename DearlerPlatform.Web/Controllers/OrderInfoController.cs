using DearlerPlatform.Core.Consts;
using DearlerPlatform.Service.CustomerApp;
using DearlerPlatform.Service.OrderApp;
using DearlerPlatform.Service.OrderApp.Dto;
using DearlerPlatform.Web.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DearlerPlatform.Web.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [CtmAuthorizationFilter]
    public class OrderInfoController : BaseController
    {
        IOrderService OrderService { get;}

        public OrderInfoController(IOrderService orderService,ICustomerService customerService) 
        {
            OrderService = orderService;
        }

        [HttpGet]
        public async Task<SaleOrderDto> GetSaleOrderDto(string orderNo)
        {
            //var customerNo = HttpContext.Items[HttpContextItemKeyName.CUSTOMER_NO].ToString();
            return await  OrderService.GetOrderInfoByOrderNo(orderNo);
        }
    }
}
