using AutoMapper;
using DearlerPlatform.Common.EventBusHelper;
using DearlerPlatform.Common.RedisModule;
using DearlerPlatform.Core.Repository;
using DearlerPlatform.Domain;
using DearlerPlatform.Service.OrderApp.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DearlerPlatform.Service.OrderApp
{
    public partial class OrderService:IOrderService
    {
        public IRepository<SaleOrderMaster> OrderMasterRepo { get;}
        public IRepository<SaleOrderDetail> OrderDetailRepo { get; }
        public IRepository<SaleOrderProgress> OrderProgressRepo { get; }
        public IRedisWorker RedisWorker { get; }
        public IMapper Mapper { get; }
        public LocalEventBus<SaleOrderDto> SaleOrderDtoLocalEventBus { get; }

        public OrderService(
            IRepository<SaleOrderMaster> orderMasterRepo,
              IRepository<SaleOrderDetail> orderDeetailRepo,
                IRepository<SaleOrderProgress> orderProgressRepo,
            IRedisWorker redisWorker,
            IMapper mapper,
             LocalEventBus<SaleOrderDto> saleOrderDtoLocalEventBus
            ) 
        {
            this.OrderMasterRepo = orderMasterRepo;
            this.OrderDetailRepo = orderDeetailRepo;
            this.OrderProgressRepo = orderProgressRepo;
            this.RedisWorker = redisWorker;
            this.Mapper = mapper;
            this.SaleOrderDtoLocalEventBus = saleOrderDtoLocalEventBus;
        }

        /// <summary>
        /// 获得订单详情
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public async Task<SaleOrderDto> GetOrderInfoByOrderNo(string orderNo)
        {
            //获取主订单
            var orderMaster = (await OrderMasterRepo.GetListAsync(m => m.SaleOrderNo == orderNo)).FirstOrDefault();
            //转成DTO
            var saleOrderDto = Mapper.Map<SaleOrderDto>(orderMaster);
            //获取订单流程
            saleOrderDto.OrderProgress = (await GetProgressByOrderNos(saleOrderDto.SaleOrderNo)).FirstOrDefault();
            //根据主订单获取订单详情
            saleOrderDto.OrderDetails = await GetOrderDetailsByOrderNo(saleOrderDto.SaleOrderNo);
            //触发总线，获取开票人信息
            await SaleOrderDtoLocalEventBus.Publish(saleOrderDto);
            return saleOrderDto;
        }

        public async Task<bool> BuyAgain(string SaleOrderNo)
        {
            using TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var newOrdeTime = DateTime.Now;
                var master = await OrderMasterRepo.GetAsync(m => m.SaleOrderNo == SaleOrderNo);
                var details = await OrderDetailRepo.GetListAsync(m => m.SaleOrderNo == SaleOrderNo);
                var newSaleOrderNo = Guid.NewGuid().ToString();
                master.SaleOrderNo = newSaleOrderNo;
                master.InputDate = newOrdeTime;
                master.DeliveryDate = newOrdeTime.AddDays(1);
                await OrderMasterRepo.InsertAsync(master);

                details.ForEach(async d =>
                {
                    d.SaleOrderNo = newSaleOrderNo;
                    d.SaleOrderGuid = Guid.NewGuid().ToString();
                    d.InputDate = newOrdeTime;
                    await OrderDetailRepo.InsertAsync(d);
                });

                var progress = new SaleOrderProgress
                {
                    ProgressGuid = Guid.NewGuid().ToString(),
                    SaleOrderNo = newSaleOrderNo,
                    StepName = "下单",
                    StepSn = 1,
                    StepTime = newOrdeTime
                };

                await OrderProgressRepo.InsertAsync(progress);
                ts.Complete();
                return false;
            }
            catch (Exception)
            {
                ts.Dispose();
                throw;
            }
        }

    }
}
