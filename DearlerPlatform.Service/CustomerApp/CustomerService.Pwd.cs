using DearlerPlatform.Common.Md5Module;
using DearlerPlatform.Domain;
using DearlerPlatform.Service.CustomerApp.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DearlerPlatform.Service.CustomerApp
{
    public partial class CustomerService: ICustomerService
    {
        public async Task<bool> CheckPassword(CustomerLoginDto dto)
        {
            var res = await customerPwdRepo.GetAsync(m=>m.CustomerNo==dto.CustomerNo && m.CustomerPwd1==dto.Password.ToMd5());
            if (res!=null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
