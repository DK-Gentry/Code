using DearlerPlatform.Core.Repository;
using DearlerPlatform.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DearlerPlatform.Service.ProductApp
{
    public partial class ProductService
    {
        public async Task<List<ProductSale>> GetProductSaleByProductNo(params string[] productNos)
        {
            return await ProductSaleRepo.GetListAsync(m => productNos.Contains(m.ProductNo));
        }
    }
}
