using DearlerPlatform.Domain;
using DearlerPlatform.Service.ShappingCartApp.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DearlerPlatform.Service.ShappingCartApp
{
    public interface IShoppingCartAppService:IocTag
    {
        Task<ShoppingCart> SetShoppingCart(ShappingCartInputDto input);
        Task<List<ShoppingCartDto>> GetShoppingCartDtos(string customerNo);
        Task<string> UpdateCartSelect(ShoppingCartSelectedEditDto edit,string customerNum);
        Task<int> GetShoppingCartNum(string customerNo); 
    }
}
