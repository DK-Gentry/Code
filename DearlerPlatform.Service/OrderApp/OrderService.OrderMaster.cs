using DearlerPlatform.Domain;
using DearlerPlatform.Service.OrderApp.Dto;
using DearlerPlatform.Service.ShappingCartApp.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DearlerPlatform.Service.OrderApp
{
    public partial class OrderService
    {
        public async Task<bool> AddOrder(
            string customerNo,
            OrderMasterInputDto input,
            List<ShoppingCartDto> carts
            )
        {
            //context没有办法在task中跨线程执行
            //TransactionScope是可以用来跨线程执行事物
            using TransactionScope ts = new(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                //添加主订单
                DateTime inputDate = DateTime.Now;
                string orderNo = Guid.NewGuid().ToString();
                SaleOrderMaster master = new SaleOrderMaster
                {
                    CustomerNo = customerNo,
                    DeliveryDate = input.DeliverDate,
                    EditUserNo = customerNo,
                    InputDate = inputDate,
                    Remark = input.Remark ?? "null",
                    InvoiceNo = input.invoice ?? "null",
                    SaleOrderNo = orderNo,
                    StockNo = ""
                };
                //await OrderMasterRepo.InsertAsync(master);
                //添加流程
                await AddProgress(orderNo, inputDate);
                //添加订单详情
                await AddOrderDetail(carts, customerNo, orderNo, inputDate);
                //提高事物
                ts.Complete();
                //删除Redis中的购物车数据
                foreach (var cart in carts)
                {
                    RedisWorker.RemoveKey($"cart:{cart.CartGuid}:{customerNo}");
                }

                return true;
            }
            catch (System.Exception)
            {
                //如果执行失败
                ts.Dispose();
                throw;
            }
        }

    }
}
