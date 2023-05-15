using DearlerPlatform.Common.Models;
using DearlerPlatform.Common.RedisModule;
using DearlerPlatform.Common.TokenModule;
using DearlerPlatform.Domain;
using DearlerPlatform.Service.CustomerApp;
using DearlerPlatform.Service.CustomerApp.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace DearlerPlatform.Web.Controllers
{
    public class LoginController : BaseController
    {
        public ICustomerService CustomerService { get; }
        public IConfiguration Configuration { get; }
        public LoginController(ICustomerService customerService, IConfiguration configuration)
        {
            this.CustomerService = customerService;
            this.Configuration = configuration;
        }

        [HttpPost]
        public async Task<string> CheckLogin(CustomerLoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CustomerNo) || string.IsNullOrWhiteSpace(dto.Password))
            {
                HttpContext.Response.StatusCode = 400;
                return "error";
            }
            var isSuccess = await CustomerService.CheckPassword(dto);
            if (isSuccess)
            {
                //得到Customer的值然后生成jwt的token
                var customer = await CustomerService.GetCustomerAsync(dto.CustomerNo);
                return GetToken(customer.Id,customer.CustomerNo,customer.CustomerName);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
                return "NonUser";
            }
        }

        private string GetToken(int customerId, string CustomerNo, string CustomerName)
        {
            //将配置文件中的值映射到JwrTokenModel中
            var token = Configuration.GetSection("Jwt").Get<JwtTokenModel>();
            token.Id = customerId;
            token.CustomerNo = CustomerNo;
            token.CustomerName = CustomerName;
            return TokenHelper.CreateToken(token);
        }
    }
}
