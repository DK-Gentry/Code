using System;
using System.Collections.Generic;

namespace DearlerPlatform.Domain;

public partial class SaleOrderProgress
{
    public int Id { get; set; }

    public string SaleOrderNo { get; set; }

    public string ProgressGuid { get; set; }

    public int StepSn { get; set; }

    public string StepName { get; set; }

    public DateTime StepTime { get; set; }
}
