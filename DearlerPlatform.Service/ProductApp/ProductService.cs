using AutoMapper;
using DearlerPlatform.Common.EventBusHelper;
using DearlerPlatform.Common.RedisModule;
using DearlerPlatform.Core;
using DearlerPlatform.Core.Consts;
using DearlerPlatform.Core.GlobalDto;
using DearlerPlatform.Core.Repository;
using DearlerPlatform.Domain;
using DearlerPlatform.Service.ProductApp.Cto;
using DearlerPlatform.Service.ProductApp.Dto;
using DearlerPlatform.Service.ShappingCartApp.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DearlerPlatform.Service.ProductApp
{
    public partial class ProductService : IProductService
    {
        public ProductService(IRepository<Product> productRepo,
                             IRepository<ProductSale> productSale,
                             IRepository<ProductSaleAreaDiff> productSaleAreaDiff,
                             IRepository<ProductPhoto> productPhoto,
                             IMapper mapper,
                             DealerPlatformContext context,
                             LocalEventBus<List<ShoppingCartDto>> LocalEevntBusShoppingCartDto,
                             IRedisWorker redisWorker
                             )
        {
            this.ProductRepo = productRepo;
            this.ProductSaleRepo = productSale;
            this.ProductSaleAreaDiffRepo = productSaleAreaDiff;
            this.ProductPhotoRepo = productPhoto;
            this.Mapper = mapper;
            this.Context = context;
            LocalEevntBusShoppingCartDto.localEventHandler += LocalEventHandler;
            this.RedisWorker= redisWorker;
        }

        IRepository<Product> ProductRepo { get; }
        IRepository<ProductSale> ProductSaleRepo { get; }
        IRepository<ProductSaleAreaDiff> ProductSaleAreaDiffRepo { get; }
        IRepository<ProductPhoto> ProductPhotoRepo { get; }
        IMapper Mapper { get; }
        DealerPlatformContext Context { get; }
        IRedisWorker RedisWorker { get; }

        //根据shoppingCart表中用户添加的的产品的no到products表中寻找对应的产品信息在存回去
        public async Task LocalEventHandler(List<ShoppingCartDto> dtos)
        {
            //这里d => d.ProductNo表示拿到ProductNo不为null的所有ShoppingCartDto类型的集合
            var nos = dtos.Select(d => d.ProductNo);
           var productDtos= await GetProductByProductNosInCache(nos.ToArray());
            dtos.ForEach(dto =>
            {
                var productDto = productDtos.FirstOrDefault(m=>m.ProductNo==dto.ProductNo);
                dto.ProductDto = productDto;
            });
        }

        /// <summary>
        /// 得到Product中的数据
        /// </summary>
        /// <param name="sort">需要排序是属性</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="PageSize">当前页显示页数</param>
        /// <returns></returns>
        public async Task<IEnumerable<ProductDto>> GetProductDto(
            string searchText,
            string productType, 
            string belongTypeName,
            Dictionary<string, string> productProps,
            PageWithSortDto pageWithSortDto)
        {
            ////为null赋值
            pageWithSortDto.Sort ??= "ProductName";

            #region linq
            //这里就是要跳过前一页的数据显示下一页的
            //比如这里是第二页,那么当前页pageIndex-1表示第一页然后乘上我们每页需要显示的数据量就得到
            //上一页需要跳过的数据

            //排序
            //GetType通过反射得到类GetProperty得到类里面的属性然后GetValue得到属性的值
            //var products = (from p in (await ProductRepo.GetListAsync())
            //                orderby p.GetType().GetProperty(sort).GetValue(p)
            //                select p).Skip(skipNum).Take(PageSize).ToList();
            #endregion

            var bzgg = productProps.ContainsKey("ProductBZGG") ? productProps["ProductBZGG"] : null;
            productProps.TryGetValue("ProductCd", out string cd);
            productProps.TryGetValue("ProductPp", out string pp);
            productProps.TryGetValue("ProductXh", out string xh);
            productProps.TryGetValue("ProductCz", out string cz);
            productProps.TryGetValue("ProductHb", out string hb);
            productProps.TryGetValue("ProductHd", out string hd);
            productProps.TryGetValue("ProductGy", out string gy);
            productProps.TryGetValue("ProductHs", out string hs);
            productProps.TryGetValue("ProductMc", out string mc);
            productProps.TryGetValue("ProductDj", out string dj);
            productProps.TryGetValue("ProductGg", out string gg);
            productProps.TryGetValue("ProductYs", out string ys);

            int skip = (pageWithSortDto.PageIndex - 1) * pageWithSortDto.PageSize;
            //Skip表示跳过多少数据
            //Take表示拿多少数据
            var products = ProductRepo.GetIQueryable()
                .Where(m => m.BelongTypeName.ToLower() == belongTypeName.ToLower() &&
                (m.TypeNo==productType|| string.IsNullOrWhiteSpace(productType)) &&
                (m.ProductName.Contains(searchText) || string.IsNullOrWhiteSpace(searchText))
                &&
                (bzgg == null || m.ProductBzgg == bzgg)
                    && (cd == null || m.ProductCd == cd)
                    && (pp == null || m.ProductPp == pp)
                    && (xh == null || m.ProductXh == xh)
                    && (cz == null || m.ProductCz == cz)
                    && (hb == null || m.ProductHb == hb)
                    && (hd == null || m.ProductHd == hd)
                    && (gy == null || m.ProductGy == gy)
                    && (hs == null || m.ProductHs == hs)
                    && (mc == null || m.ProductMc == mc)
                    && (dj == null || m.ProductDj == dj)
                    && (gg == null || m.ProductGg == gg)
                    && (ys == null || m.ProductYs == ys)
                )
                //.OrderBy(pageWithSortDto.Sort)
                .Skip(skip).Take(pageWithSortDto.PageSize);

            //使用autoMapper数据映射<数据要被映射的类>(数据源)
            var dtos = Mapper.Map<List<ProductDto>>(products);

            //拿到前面筛选出来的一页显示的数据所对应的图片和价格
            var productPhotos = await GetProductPhotoByProductNo(products.Select(m => m.ProductNo).ToArray());
            var productSales = await GetProductSaleByProductNo(products.Select(m => m.ProductNo).ToArray());

            //因为在数据映射类Dto中有Photo和Sale类是存储每页需要显示的数据的这个是我们分页
            //分出来的需要我们自己筛选赋值
            dtos.ForEach(p =>
            {
                 p.ProductPhoto = productPhotos.FirstOrDefault(m => m.ProductNo == p.ProductNo);
                 p.ProductSale = productSales.FirstOrDefault(m => m.ProductNo  == p.ProductNo);
            });

            return dtos;
        }

        public async Task<List<BlongTypeDto>> GetBlongTypeDtoAsync()
        {
            //使用多线程的方式来实现异步
            return await Task.Run(() =>
            {
                var res = ProductRepo.GetIQueryable().Select(m => new BlongTypeDto
                {
                    SysNo = m.SysNo,
                    BelongTypeNanme = m.BelongTypeName,
                }).Distinct().ToList();
                return res;
            });
        }

        public async Task<IEnumerable<productTypeDto>> GetProductType(string belongTypeName)
        {
            //Select表示输出值，这里是把product(m)中的TypeName和TypeNo传递到productTypeDto中在返回
            return await Task.Run(() =>
            {
                var ProductType = ProductRepo.GetIQueryable().Where(m=> m.BelongTypeName==belongTypeName &&!string.IsNullOrWhiteSpace(m.TypeNo) && !string.IsNullOrWhiteSpace(m.TypeName)).Select(m => new productTypeDto
                {
                    TypeName = m.TypeName,
                    TypeNo = m.TypeNo
                    //Distinct表示去重复
                }).Distinct().ToList();
                return ProductType;
            });
        }

        public async Task<Dictionary<string,IEnumerable<string>>> ListProductTypes(string belongTypeNo,string typeNo)
        {
            Dictionary<string, IEnumerable<string>> dicProductType = new();
            // var products = Context.Products.Select(m=>new {
            //     PriductBzgg=m.ProductBzgg,
            //     ProductCd=m.ProductCd,})
            var products = await ProductRepo.GetListAsync(m => m.BelongTypeName == belongTypeNo && (m.TypeNo == typeNo || string.IsNullOrWhiteSpace(typeNo)));
            dicProductType.Add("ProductBzgg|包装规格", products.Select(m => m.ProductBzgg).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            dicProductType.Add("ProductCd|产地", products.Select(m => m.ProductCd).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            dicProductType.Add("ProductCz|材质", products.Select(m => m.ProductCz).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            dicProductType.Add("ProductDj|等级", products.Select(m => m.ProductDj).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            dicProductType.Add("ProductGg|规格", products.Select(m => m.ProductGg).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            dicProductType.Add("ProductGy|工艺", products.Select(m => m.ProductGy).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            dicProductType.Add("ProductHb|环保", products.Select(m => m.ProductHb).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            dicProductType.Add("ProductHd|厚度", products.Select(m => m.ProductHd).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            dicProductType.Add("ProductHs|花色", products.Select(m => m.ProductHs).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            dicProductType.Add("ProductMc|面材", products.Select(m => m.ProductMc).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            dicProductType.Add("ProductPp|品牌", products.Select(m => m.ProductPp).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            dicProductType.Add("ProductXh|型号", products.Select(m => m.ProductXh).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            dicProductType.Add("ProductYs|颜色", products.Select(m => m.ProductYs).Distinct().Where(m => !string.IsNullOrWhiteSpace(m)).ToList());
            return dicProductType;
        }

        private static object lockObj = new object();

        /// <summary>
        /// 通过ProductNos获取到product值(从redis中获取lock锁)
        /// </summary>
        /// <param name="postProductNos"></param>
        /// <returns></returns>
        public async Task<List<ProductDto>> GetProductByProductNosInCache(params string[] postProductNos)
        {
            //这里我们需要存到Product具体数据然后存到redis中
            //但是redis不能存储复杂质类中不能有数据类型还是类
            //于是我们只能建立另一张数据表(cto)这张表中复杂的数据类型是string类型
            //我们通过序列化字符串的方式存储,拿出来的时候再解析这样就能把类的类型存入到Redis中
            List<ProductCto> ctos = new();
            foreach (var productNo in postProductNos)
            {
                var res = RedisWorker.GetHashMemory<ProductCto>($"{RedisKeyName.PRODUCT_KEY}:{productNo}").FirstOrDefault();
                if (res == null)
                {
                    lock (lockObj)
                    {
                        //DTO->CTO
                        res = RedisWorker.GetHashMemory<ProductCto>($"{RedisKeyName.PRODUCT_KEY}:{productNo}").FirstOrDefault();
                        if (res == null)
                        {
                            res = Mapper.Map<ProductDto, ProductCto>(GetProductByProductNos(productNo).Result.FirstOrDefault());
                            RedisWorker.SetHashMemory<ProductCto>($"{RedisKeyName.PRODUCT_KEY}:{productNo}", res);
                        }
                    }
                }
                ctos.Add(res);
            }
            return Mapper.Map<List<ProductCto>, List<ProductDto>>(ctos);
        }

        public async Task<List<ProductDto>> GetProductByProductNos(params string[] postProductNos)
        {
            var productNos = postProductNos.Distinct();
            var products = await ProductRepo.GetListAsync(m=> productNos.Contains(m.ProductNo));
            var productDtos = Mapper.Map<List<Product>, List<ProductDto>>(products);
             var productSales = await GetProductSaleByProductNo(productDtos.Select(m => m.ProductNo).ToArray());
            productDtos.ForEach(p =>
            {
                p.ProductSale = productSales.FirstOrDefault(m => m.ProductNo == p.ProductNo);
            });
            return productDtos;
        }
    }
}
