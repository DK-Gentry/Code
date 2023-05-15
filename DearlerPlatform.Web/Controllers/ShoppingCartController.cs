using DearlerPlatform.Domain;
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
    public class ShoppingCartController : BaseController
    {
        public ShoppingCartController(
            IShoppingCartAppService shoppingCartAppService,
            IProductService productService 
            ) 
        {
            ShoppingCartService = shoppingCartAppService;
        }

        public IShoppingCartAppService ShoppingCartService { get;}

        [CtmAuthorizationFilter]
        [HttpPost]
        public async Task<ShoppingCart> SetShoppingCart(ShappingCartInputDto input)
        {
            var customerNo = HttpContext.Items["CustomerNo"].ToString();
            input.CustomerNo = customerNo;
            return await ShoppingCartService.SetShoppingCart(input);
        }

        [CtmAuthorizationFilter]
        [HttpGet("num")]
        public async Task<int> GetShoppingcartNum()
        {
            var customerNo = HttpContext.Items["CustomerNo"].ToString();
           return await ShoppingCartService.GetShoppingCartNum(customerNo);
        }


        [HttpGet]
        [CtmAuthorizationFilter]
        public async Task<dynamic> GetShoppingCartDtosAsync()
        {
            var customerNo = HttpContext.Items["CustomerNo"].ToString();
            var carts = await ShoppingCartService.GetShoppingCartDtos(customerNo);
            var productDtos = carts.Select(m=>m.ProductDto);
            var types = productDtos?.Select(m=>new
            {
                TypeNo = m?.TypeNo,
                TypeName = m?.TypeName,
                TypeSelected = false
            }).Distinct();
            return new {carts,types};
        }

        [HttpPost("CartSelected")]
        [CtmAuthorizationFilter]
        public async Task<string> UpdateSelected(ShoppingCartSelectedEditDto edit)
        {
            var customer = HttpContext.Items["CustomerNo"]?.ToString();
            return await ShoppingCartService.UpdateCartSelect(edit,customer);
        }
    }
}
