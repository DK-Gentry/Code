using System;
using System.Collections.Generic;

namespace DearlerPlatform.Domain;

public partial class ProductSale
{
    public int Id { get; set; }

    public string SysNo { get; set; }

    public string ProductNo { get; set; }

    public string StockNo { get; set; }

    public double SalePrice { get; set; }
}
