using System;
using System.Collections.Generic;

namespace a_private_bank_main.Models;

public partial class VariableExpense
{
    public int Id { get; set; }

    public int PeriodId { get; set; }

    public string Name { get; set; } = null!;

    public string? Category { get; set; }

    public decimal? BudgetedAmount { get; set; }

    public decimal? ActualAmount { get; set; }

    public DateOnly? SpentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual BudgetPeriod Period { get; set; } = null!;
}
