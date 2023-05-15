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

//#region jwt��֤
////����ʹ�õ���֤�ܹ�
////�������õ���JwtBearerDefaults.AuthenticationScheme
////�����ھ���ʹ�õķ�����ҲҪ����ʹ�õļ�Ȩģ��
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(opt =>
//    {
//        //����Ҫhttps(Ĭ��Ϊtrue��������������Ϊfalse)
//        opt.RequireHttpsMetadata = false;
//        //��Ȩ�󽫼̳����ƴ洢
//        opt.SaveToken = true;
//        //�������ļ��еĸ�ֵ����Ȩ����ȥ�ж�
//        opt.TokenValidationParameters = new()
//        {
//            //������ǰ�token�����δ��ݹ�����֤(ǩ����֤)
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Security)),
//            ValidIssuer = token.Issuer,
//            ValidAudience = token.Audience
//        };
//        //��������
//        //��֤ʧ��
//        opt.Events = new JwtBearerEvents
//        {
//            OnChallenge = context =>
//            {
//                //��֤ʧ������ֹ����
//                context.HandleResponse();
//                var res = "{\"code\":401,\"err\":\"��Ȩ��\"}";
//                //����ǰ�˷��ص���json
//                context.Response.ContentType = "application/json";
//                //���ش������
//                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//                //�Ѵ������ݷ��ظ�ǰ��
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
