using DearlerPlatform.Domain;
using DearlerPlatform.Service.CustomerApp.Dto;

namespace DearlerPlatform.Service.CustomerApp
{
    public interface ICustomerService:IocTag
    {
        public Task<bool> CheckPassword(CustomerLoginDto dto);

        public Task<Customer> GetCustomerAsync(string customerNo);

        public Task<List<InvoiceOfOrderConfirmDto>> GetInvoicesByUser(string customerNo);
    }
}