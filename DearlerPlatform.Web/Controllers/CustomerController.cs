using DearlerPlatform.Core.Consts;
using DearlerPlatform.Service.CustomerApp;
using DearlerPlatform.Service.CustomerApp.Dto;
using DearlerPlatform.Web.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DearlerPlatform.Web.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CustomerController : BaseController
    {
        public ICustomerService CustomerService { get;}

        public CustomerController(ICustomerService customerService) 
        {
            CustomerService = customerService;
        }

        [CtmAuthorizationFilter]
        [HttpGet("Invoice")]
        public async Task<List<InvoiceOfOrderConfirmDto>> Get()
        {
            var cno = HttpContext.Items[HttpContextItemKeyName.CUSTOMER_NO].ToString();
           return await CustomerService.GetInvoicesByUser(cno);
        }
    }
}
