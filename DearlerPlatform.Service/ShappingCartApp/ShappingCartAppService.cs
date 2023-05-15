using AutoMapper;
using DearlerPlatform.Common.EventBusHelper;
using DearlerPlatform.Common.RedisModule;
using DearlerPlatform.Core;
using DearlerPlatform.Core.Repository;
using DearlerPlatform.Domain;
using DearlerPlatform.Service.ShappingCartApp.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DearlerPlatform.Service.ShappingCartApp
{
    public partial class ShappingCartAppService:IShoppingCartAppService
    {
        public IRepository<ShoppingCart> CartRepo { get; }
        public IMapper Mapper { get; }
        LocalEventBus<List<ShoppingCartDto>> LocalEventBusShoppingCartDto { get; }
        public DealerPlatformContext Context { get; }
        public IRedisWorker RedisWorker { get; }
        RedisCore RedisCore { get; }

        public ShappingCartAppService(
            IRepository<ShoppingCart> cartRepo,
            IMapper mapper,
            LocalEventBus<List<ShoppingCartDto>> localEventBusShoppingCartDto,
            DealerPlatformContext context,
            IRedisWorker redisWorker,
            RedisCore redisCore
            )
        {
            CartRepo = cartRepo;
            Mapper = mapper;
            LocalEventBusShoppingCartDto = localEventBusShoppingCartDto;
            Context = context;
            RedisWorker = redisWorker;
            RedisCore = redisCore;

        }

        public async Task<ShoppingCart> SetShoppingCart(ShappingCartInputDto input)
        {
            ShoppingCart shoppingCartRes = null;
            //from to 被映射类 源数据类 
            //这里是将ShappingCartInputDto中的数据映射到ShoppingCart,然后存入数据库中

            //从数据库拿数据
            //var shoppingCart = await CartRepo.GetAsync(m=>m.ProductNo==input.ProductNo);
            //从redis中拿
            var shoppingCart = RedisWorker.GetHashMemory<ShoppingCart>($"cart:*:{input.CustomerNo}").FirstOrDefault(m=>m.ProductNo==input.ProductNo);
            if (shoppingCart!=null)
            {
                shoppingCart.ProductNum++;
                RedisWorker.SetHashMemory($"cart:{shoppingCart.CartGuid}:{shoppingCart.CustomerNo}", shoppingCart);
                //shoppingCartRes = await CartRepo.UpdateAsync(shoppingCart);
            }
            else
            {
                shoppingCart = await CartRepo.GetAsync(m => m.ProductNo == input.ProductNo);
                if (shoppingCart != null)
                {
                    shoppingCart.ProductNum++;
                    //shoppingCartRes = await CartRepo.UpdateAsync(shoppingCart);
                    RedisWorker.SetHashMemory($"cart:{shoppingCart.CartGuid}:{shoppingCart.CustomerNo}", shoppingCart);
                    //return shoppingCartRes;
                }
                else
                {
                    shoppingCart = Mapper.Map<ShappingCartInputDto, ShoppingCart>(input);
                    shoppingCart.CartGuid = Guid.NewGuid().ToString();
                    shoppingCart.CartSelected = true;
                    //向redis中写入数据
                    RedisWorker.SetHashMemory($"cart:{shoppingCart.CartGuid}:{shoppingCart.CustomerNo}", shoppingCart);
                }
            }
            //shoppingCartRes = await CartRepo.UpdateAsync(shoppingCart);
            //向数据库中写入数据
            shoppingCartRes = await CartRepo.InsertAsync(shoppingCart);
            return shoppingCartRes;     
        }

        public async Task<List<ShoppingCartDto>> GetShoppingCartDtos(string customerNo) 
        {
            //var carts = await CartRepo.GetListAsync(m=>m.CustomerNo==customerNo); 
            var carts = RedisWorker.GetHashMemory<ShoppingCart>($"cart:*:{customerNo}");
            var dtos= Mapper.Map<List<ShoppingCart>,List<ShoppingCartDto>>(carts);
            await LocalEventBusShoppingCartDto.Publish(dtos); 
            return dtos;
        }

        /// <summary>
        /// 更新数据库的值
        /// </summary>
        /// <param name="CartGuid"></param>
        /// <param name="cartSelected"></param>
        /// <returns></returns>
        public async Task<string> UpdateCartSelect(ShoppingCartSelectedEditDto edit,string customerNum)
        {
            if (edit.ProductNum <= 0)
            {
                RedisCore.RemoveKey($"cart:{edit.CartGuids[0]}:{customerNum}");
                return "Remove";
            }
            var shoppingCart = RedisWorker.GetHashMemory<ShoppingCart>($"cart:{edit.CartGuids[0]}:*").FirstOrDefault();
            shoppingCart.CartSelected = edit.CartSelected;
            shoppingCart.ProductNum = edit.ProductNum;

            RedisWorker.SetHashMemory($"cart:{edit.CartGuids[0]}:{customerNum}", shoppingCart);
            return "Update";

            //下面是从数据库中拿到数据
            //try
            //{
            //    foreach (var cartGuid in edit.CartGuids)
            //    {
            //        ShoppingCart cart = new()
            //        {
 
            //        };
            //        //把cart加入监听
            //        Context.Attach(cart);
            //        //具体需要监听哪个字段
            //        Context.Entry(cart).Property(m => m.CartSelected).IsModified = true;
            //        Context.Entry(cart).Property(m => m.ProductNum).IsModified= true;
            //    }
            //    return await Context.SaveChangesAsync() > 0;
            //}
            //catch (Exception)
            //{
            //    return false;
            //    throw;
            //}
        }

        /// <summary>
        /// 获取购物车数量
        /// </summary>
        /// <param name="customerNo"></param>
        /// <returns></returns>
        public async Task<int> GetShoppingCartNum(string customerNo)
        {
            //向redis中写入数据
            var carts = RedisWorker.GetHashMemory<ShoppingCart>($"cart:*:{customerNo}");
            //向数据库中写入数据
            //var carts = await CartRepo.GetListAsync(m => m.CustomerNo == customerNo && m.CartSelected);
            var currentCarNum = 0;
            carts.ForEach(m =>
            {
                currentCarNum += m.ProductNum;
            });
            return currentCarNum;
        }
    }
}
