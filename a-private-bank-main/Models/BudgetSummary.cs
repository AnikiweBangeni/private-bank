using System;
using System.Collections.Generic;

namespace a_private_bank_main.Models;

public partial class BudgetSummary
{
    public int PeriodId { get; set; }

    public DateOnly PeriodStart { get; set; }

    public DateOnly PeriodEnd { get; set; }

    public decimal? OpeningBalance { get; set; }

    public decimal TotalIncomeExpected { get; set; }

    public decimal TotalIncomeReceived { get; set; }

    public decimal TotalFixed { get; set; }

    public decimal TotalVariableBudgeted { get; set; }

    public decimal TotalVariableActual { get; set; }

    public decimal WishListApproved { get; set; }

    public decimal WishListTotalPending { get; set; }

    public decimal? BalanceAfterApprovedWishes { get; set; }

    public decimal? BalanceIfAllWishesBought { get; set; }
}
