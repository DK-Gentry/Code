using AutoMapper;
using DearlerPlatform.Domain;
using DearlerPlatform.Service.CustomerApp.Dto;
using DearlerPlatform.Service.OrderApp.Dto;
using DearlerPlatform.Service.ProductApp.Cto;
using DearlerPlatform.Service.ProductApp.Dto;
using DearlerPlatform.Service.ShappingCartApp.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DearlerPlatform.Service
{
    //先引入autoMapper的两个包文件 autoMapper和autoMapper.Extensions
    //使用autoMapping时候,需要先继承Profile类
    //然后再构造函数中写入映射规则
    //最后在Program中builder.Services.AddAutoMapper(typeof(DealerPlatformProfile));
    //去添加

    //autoMapping主要做数据映射
    //领域模型 转 视图模型
    public class DealerPlatformProfile:Profile
    {
        public DealerPlatformProfile()
        {
            //CreateMap表示数据映射关系，这里是从Product映射到ProductDto
            //.ForMember(dest=>dest.Id,m=>m.Ignore())表示映射的时候忽略Id
            //因为每个表都有一个Id这样会导致Id相互覆盖需要忽略只保留一个
            //但是ReverseMap表示相互映射关系
            
            //这里设置完具体的映射规则后,实际数据使用时候还是需要再Mapper.Map
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductSale, ProductDto>().ReverseMap();
            CreateMap<ProductPhoto, ProductDto>().ReverseMap();
            CreateMap<ProductSaleAreaDiff, ProductDto>().ReverseMap();
            CreateMap<ShoppingCart, ShoppingCartDto>().ReverseMap();
            CreateMap<ShoppingCart, ShappingCartInputDto>().ReverseMap();
            CreateMap<CustomerInvoice, InvoiceOfOrderConfirmDto>().ReverseMap();
            CreateMap<SaleOrderMaster, SaleOrderDto>().ReverseMap();

            //配置具体的元素类型
            CreateMap<ProductDto, ProductCto>()
                .ForMember(cto =>cto.ProductPhoto,m=>m.MapFrom(dto=>JsonConvert.SerializeObject(dto.ProductPhoto)))
                .ForMember(cto =>cto.ProductSale, m=>m.MapFrom(dto=>JsonConvert.SerializeObject(dto.ProductSale)));

            CreateMap<ProductCto, ProductDto>()
                .ForMember(dto => dto.ProductPhoto, m => m.MapFrom(cto => JsonConvert.DeserializeObject<ProductPhoto>(cto.ProductPhoto)))
                .ForMember(dto => dto.ProductSale, m => m.MapFrom(cto => JsonConvert.DeserializeObject<ProductSale>(cto.ProductSale)));

        }
    }
}
