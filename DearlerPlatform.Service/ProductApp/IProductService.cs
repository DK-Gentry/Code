using DearlerPlatform.Core.GlobalDto;
using DearlerPlatform.Domain;
using DearlerPlatform.Service.ProductApp.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DearlerPlatform.Service.ProductApp
{
    public interface IProductService:IocTag
    {
        Task<IEnumerable<ProductDto>> GetProductDto(string searchText,
            string productType, 
            string belongTypeName, 
            Dictionary<string,string> dicProductProps,
            PageWithSortDto pageWithSortDto);

        Task<IEnumerable<productTypeDto>> GetProductType(string belongTypeName);

        Task<Dictionary<string, IEnumerable<string>>> ListProductTypes(string belongTypeNo, string typeNo);

        Task<List<BlongTypeDto>> GetBlongTypeDtoAsync();
    }
}
