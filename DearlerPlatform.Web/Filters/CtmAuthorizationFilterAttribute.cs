using DearlerPlatform.Core.Consts;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace DearlerPlatform.Web.Filters
{
    public class CtmAuthorizationFilterAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //User是CliamsPrincipal
            //CliamsPrincipal是cliams的总类所有的cliam数据都能通过CliamsPrincipal拿到
            //User是当传递的值中有Claim类型会自动解析
            //我们在CreateToken时候CustomerNo是已Cliam的形式存储的,所以在前端返回过来后User会自动解析其中的值
            var user = context.HttpContext.User;
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)user.Identity;
            foreach (var claim in claimsIdentity.Claims)
            {
                if (claim.Type == HttpContextItemKeyName.CUSTOMER_NO)
                {
                    context.HttpContext.Items.Add(HttpContextItemKeyName.CUSTOMER_NO,claim.Value);
                }
            }
        }
    }
}
