using AutoMapper;
using DearlerPlatform.Common.EventBusHelper;
using DearlerPlatform.Core.Repository;
using DearlerPlatform.Domain;
using DearlerPlatform.Service.CustomerApp.Dto;
using DearlerPlatform.Service.OrderApp.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DearlerPlatform.Service.CustomerApp
{
    public partial class CustomerService : ICustomerService
    {
        public IRepository<Customer> customerRepo { get; set; }
        public IRepository<CustomerInvoice> customerInvoiceRepo { get; set; }
        public IRepository<CustomerPwd> customerPwdRepo { get; set; }
        public IMapper Mapper { get; }
        //public LocalEventBus<SaleOrderDto> SaleOrderDtolocalEventBus { get; }

        public CustomerService(IRepository<Customer> customerRepo,
                               IRepository<CustomerInvoice> customerInvoiceRepo,
                               IRepository<CustomerPwd> customerPwdRepo,
                                IMapper mapper,
                                LocalEventBus<SaleOrderDto> saleOrderDtolocalEventBus)
        {
            this.customerRepo = customerRepo;
            this.customerInvoiceRepo = customerInvoiceRepo;
            this.customerPwdRepo = customerPwdRepo;
            this.Mapper = mapper;

            saleOrderDtolocalEventBus.localEventHandler += SaleOrderDtoLocalEventHandler;
        }

        public async Task<Customer> GetCustomerAsync(string customerNo)
        {
           return await customerRepo.GetAsync(m=>m.CustomerNo==customerNo);
        }
    }
}
