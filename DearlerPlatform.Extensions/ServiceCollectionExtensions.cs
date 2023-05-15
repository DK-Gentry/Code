using DearlerPlatform.Core.Repository;
using DearlerPlatform.Domain;
using DearlerPlatform.Service.CustomerApp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DearlerPlatform.Extensions
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// 依赖注册扩展Repository
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection RepositoryRegister(this IServiceCollection services)
        {
            //反射获得Core层的程序集dll
            var asmCore = Assembly.Load("DearlerPlatform.Core");
            //通过dll拿到core层中的所有类，然后查找到其中Repository类
            // `1表示:Repository是泛型类，代表这个泛型类有一个参数，有两个就是`2
            var implementationType = asmCore.GetTypes().FirstOrDefault(m=>m.Name=="Repository`1");
            var interfaceType = implementationType.GetInterface("IRepository`1");
          
            services.AddTransient(typeof(IRepository<>), implementationType);
            return services;
        }

        /// <summary>
        /// 依赖注册扩展Service
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ServiceRegister(this IServiceCollection services)
        {
            List<Assembly> assemblys = new();

            //因为配置文件获取类是默认添加到容器中的
            //这里是从容器中将他取出来
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetService<IConfiguration>();

            //从appsetting中读取字符串然后再用反射得到具体数据添加到List中
            List<string> classes = configuration["IocClasses"].Split(",").ToList();

            classes.ForEach(c =>
            {
                var assembly = Assembly.Load(c);
                assemblys.Add(assembly);
            });

            //反射获得Core层的程序集dll
            //var asmService = Assembly.Load("DearlerPlatform.Service");
            //var asmRedisWorker = Assembly.Load("DearlerPlatform.Common");
            //assemblys.Add(asmService);
            //assemblys.Add(asmRedisWorker);

            foreach (var assembly in assemblys)
            {
                var implementationTypes = assembly.GetTypes().Where(
               //确定当前类型是否可分配给指定 targetType 的变量 return bool。
               m => m.IsAssignableTo(typeof(IocTag)) &&
               !m.IsAbstract && !m.IsInterface);
                foreach (var implementationType in implementationTypes)
                {
                    //var interfaceType = implementationType.GetInterfaces()
                    //    .Where(m=>m!=typeof(IocTag)).FirstOrDefault();
                    //services.AddTransient(typeof(ICustomerService), implementationType);

                    var interfaceType = implementationType.GetInterfaces()
                     .Where(m => m != typeof(IocTag)).FirstOrDefault();
                    services.AddTransient(interfaceType, implementationType);
                }
            }
           
            return services;
        }
    }
}
