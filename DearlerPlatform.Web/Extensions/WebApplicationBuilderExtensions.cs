using DearlerPlatform.Common.EventBusHelper;
using DearlerPlatform.Common.Models;
using DearlerPlatform.Common.RedisModule;
using DearlerPlatform.Core;
using DearlerPlatform.Extensions;
using DearlerPlatform.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace DearlerPlatform.Web.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static void ServiceEnter(this IServiceCollection services)
        {
            var configuration = services.GetConfiguration();
            var token = configuration.GetSection("Jwt").Get<JwtTokenModel>();

            #region jwt验证
            //设置使用的验证架构
            //这里设置的是JwtBearerDefaults.AuthenticationScheme
            //所以在具体使用的方法上也要设置使用的鉴权模块
            //jwt的授权也是在这里的
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    //不需要https(默认为true，开发环境才能为false)
                    opt.RequireHttpsMetadata = false;
                    //授权后将继承令牌存储
                    opt.SaveToken = true;
                    //将配置文件中的赋值给鉴权中心去判断
                    opt.TokenValidationParameters = new()
                    {
                        //这里就是把token那三段传递过来验证(签名验证)
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Security)),
                        ValidIssuer = token.Issuer,
                        ValidAudience = token.Audience
                    };
                    //触发方法
                    //验证失败
                    opt.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            //验证失败先终止代码
                            context.HandleResponse();
                            var res = "{\"code\":401,\"err\":\"无权限\"}";
                            //告诉前端返回的是json
                            context.Response.ContentType = "application/json";
                            //返回错误代码
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            //把错误内容返回给前端
                            context.Response.WriteAsync(res);

                            return Task.FromResult(0);
                        }
                    };
                });
            #endregion

            services.AddDbContext<DealerPlatformContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("Default"));
            });

            //builder.Services.AddTransient<ICustomerService, CustomerService>();
            //builder.Services.AddTransient(typeof(IRepository<>),typeof(Repository<>));

            services.AddScoped(typeof(LocalEventBus<>));
            services.AddSingleton<RedisCore>();
            //services.AddTransient<IRedisWorker, RedisWorker>();

            services.RepositoryRegister();
            services.ServiceRegister();

            services.AddControllers();

            services.AddCors(c => c.AddPolicy("any", p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

           
            //将autoMapping注册到容器中，并且添加实体映射类
            services.AddAutoMapper(typeof(DealerPlatformProfile));

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                //固定代码(抄抄抄!)
                var scheme = new OpenApiSecurityScheme()
                {
                    Description = "格式:Bearer token",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "Bearer",
                    Name = "Authorization", // 默认的参数名
                    In = ParameterLocation.Header,// 放于请求头中
                    Type = SecuritySchemeType.ApiKey

                };
                // 添加安全定义
                c.AddSecurityDefinition("Bearer", scheme);

                // 添加安全要求
                var requirement = new OpenApiSecurityRequirement();
                requirement[scheme] = new List<string>();
                c.AddSecurityRequirement(requirement);
            });
        }

        public static IConfiguration GetConfiguration(this IServiceCollection services)
        {
            //直接在容器里面取到注册在容器内的实例
            return services.BuildServiceProvider().GetService<IConfiguration>();
        }
    }
}
  