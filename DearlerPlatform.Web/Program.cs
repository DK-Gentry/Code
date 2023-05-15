using DearlerPlatform.Common.Models;
using DearlerPlatform.Core;
using DearlerPlatform.Core.Repository;
using DearlerPlatform.Extensions;
using DearlerPlatform.Service;
using DearlerPlatform.Service.CustomerApp;
using DearlerPlatform.Web.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ServiceEnter();

#region 
//var token = builder.Configuration.GetSection("Jwt").Get<JwtTokenModel>();

//#region jwt验证
////设置使用的验证架构
////这里设置的是JwtBearerDefaults.AuthenticationScheme
////所以在具体使用的方法上也要设置使用的鉴权模块
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(opt =>
//    {
//        //不需要https(默认为true，开发环境才能为false)
//        opt.RequireHttpsMetadata = false;
//        //授权后将继承令牌存储
//        opt.SaveToken = true;
//        //将配置文件中的赋值给鉴权中心去判断
//        opt.TokenValidationParameters = new()
//        {
//            //这里就是把token那三段传递过来验证(签名验证)
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Security)),
//            ValidIssuer = token.Issuer,
//            ValidAudience = token.Audience
//        };
//        //触发方法
//        //验证失败
//        opt.Events = new JwtBearerEvents
//        {
//            OnChallenge = context =>
//            {
//                //验证失败先终止代码
//                context.HandleResponse();
//                var res = "{\"code\":401,\"err\":\"无权限\"}";
//                //告诉前端返回的是json
//                context.Response.ContentType = "application/json";
//                //返回错误代码
//                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//                //把错误内容返回给前端
//                context.Response.WriteAsync(res);

//                return Task.FromResult(0);
//            }
//        };
//    });
//#endregion

//builder.Services.AddDbContext<DealerPlatformContext>(opt =>
//{
//    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
//});
#endregion

var app = builder.Build();

app.initEnter();

app.initMap();

app.Run();
