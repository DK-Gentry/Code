﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DearlerPlatform.Service.OrderApp.Dto
{
    public class OrderMasterInputDto
    {
        public DateTime DeliverDate { get; set; }

        public string invoice { get; set; }

        public string Remark { get; set; }
    }
}
