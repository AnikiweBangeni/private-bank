using System;
using System.Collections.Generic;

namespace a_private_bank_main.Models;

public partial class FixedExpense
{
    public int Id { get; set; }

    public int PeriodId { get; set; }

    public string Name { get; set; } = null!;

    public string? Category { get; set; }

    public decimal Amount { get; set; }

    public DateOnly? DueDate { get; set; }

    public bool? IsPaid { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual BudgetPeriod Period { get; set; } = null!;
}
