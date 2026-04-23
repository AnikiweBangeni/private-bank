using System;
using System.Collections.Generic;

namespace a_private_bank_main.Models;

public partial class Income
{
    public int Id { get; set; }

    public int PeriodId { get; set; }

    public string Source { get; set; } = null!;

    public string? Category { get; set; }

    public decimal ExpectedAmount { get; set; }

    public decimal? ActualAmount { get; set; }

    public DateOnly? ReceivedDate { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual BudgetPeriod Period { get; set; } = null!;
}
