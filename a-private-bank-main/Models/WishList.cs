using System;
using System.Collections.Generic;

namespace a_private_bank_main.Models;

public partial class WishList
{
    public int Id { get; set; }

    public int PeriodId { get; set; }

    public string ItemName { get; set; } = null!;

    public string? Category { get; set; }

    public decimal EstimatedCost { get; set; }

    public short? Priority { get; set; }

    public string? Status { get; set; }

    public bool? IsNecessity { get; set; }

    public int? DeferToPeriod { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual BudgetPeriod? DeferToPeriodNavigation { get; set; }

    public virtual BudgetPeriod Period { get; set; } = null!;
}
