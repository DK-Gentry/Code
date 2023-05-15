using AutoMapper;
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
        public async Task<List<InvoiceOfOrderConfirmDto>> GetInvoicesByUser(string customerNo)
        {
            var invoices = await customerInvoiceRepo.GetListAsync(m=>m.CustomerNo == customerNo);
            return Mapper.Map<List<CustomerInvoice>,List<InvoiceOfOrderConfirmDto>>(invoices);
        }

        public async Task<CustomerInvoice> GetCustomerInvoiceAsync(string invoiceNo)
        {
            return await customerInvoiceRepo.GetAsync(m=>m.InvoiceNo==invoiceNo);
        }

        private async Task SaleOrderDtoLocalEventHandler(SaleOrderDto saleOrderDto)
        {
            saleOrderDto.CustomerInvoice = await GetCustomerInvoiceAsync(saleOrderDto.InvoiceNo);
        }
    }
}
