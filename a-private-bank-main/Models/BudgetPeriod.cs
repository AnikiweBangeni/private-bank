using System;
using System.Collections.Generic;

namespace a_private_bank_main.Models;

public partial class BudgetPeriod
{
    public int Id { get; set; }

    public DateOnly PeriodStart { get; set; }

    public DateOnly PeriodEnd { get; set; }

    public decimal? OpeningBalance { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<FixedExpense> FixedExpenses { get; set; } = new List<FixedExpense>();

    public virtual ICollection<Income> Incomes { get; set; } = new List<Income>();

    public virtual ICollection<VariableExpense> VariableExpenses { get; set; } = new List<VariableExpense>();

    public virtual ICollection<WishList> WishListDeferToPeriodNavigations { get; set; } = new List<WishList>();

    public virtual ICollection<WishList> WishListPeriods { get; set; } = new List<WishList>();
}
