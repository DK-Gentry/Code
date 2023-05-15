using DearlerPlatform.Service.ProductApp;
using DearlerPlatform.Service.ProductApp.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JetBrains.Annotations;
using static DearlerPlatform.Core.GlobalDto.PageWithSortDto;
using DearlerPlatform.Core.GlobalDto;

namespace DearlerPlatform.Web.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductController : BaseController
    {
        IProductService ProductService { get; }

        public ProductController(IProductService productService)
        {
            this.ProductService = productService;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetProductDtosAsync(
            string? searchText,string? productType,
            string belongTypeName,string? productProps,
            string sort, int pageIndex = 1, int PageSize = 30, 
            OrderType orderType = OrderType.Asc)
        {
            //sort ??= "ProductName";
            //pageIndex为0才会为1 |短路运算符
            //pageIndex = pageIndex | 1;
            //return await ProductService.GetProductDto(sort,pageIndex,PageSize);

            Dictionary<string, string> strProductProps = new();
            if (productProps!=null)
            {
                var ProductProps = productProps.Split("^") ?? new string[0];
                foreach (var item in ProductProps)
                {
                    var key = item.Split("_")[0];
                    var value = item.Split("_")[1];
                    strProductProps.Add(key, value);
                }
            }
            
            return await ProductService.GetProductDto(searchText,productType, belongTypeName, strProductProps, new PageWithSortDto
            {
                Sort = sort,
                PageIndex = pageIndex,
                PageSize = PageSize,
                orderType = orderType
            });
        }

        [HttpGet("BlongType")]
        public async Task<List<BlongTypeDto>> GetBlongType()
        {
            return await ProductService.GetBlongTypeDtoAsync();
        }

        [HttpGet("type")]
        public async Task<IEnumerable<productTypeDto>> GetProductTypeDtosAsync(string belongTypeName)
        {
            return await ProductService.GetProductType(belongTypeName);
        }

        [HttpGet("props")]
        public async Task<Dictionary<string,IEnumerable<string>>> GetProductProps(string belongTypeName = "1",string? typeNo="1")
        {
            return  await ProductService.ListProductTypes(belongTypeName, typeNo);
        }
    }
}
