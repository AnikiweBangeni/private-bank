using System;
using System.Collections.Generic;

namespace a_private_bank_main.Models;

public partial class TransactionsStatement
{
    public int Id { get; set; }

    public int Nr { get; set; }

    public long Account { get; set; }

    public DateTime PostingDate { get; set; }

    public DateTime TransactionDate { get; set; }

    public string Description { get; set; } = null!;

    public string OriginalDescription { get; set; } = null!;

    public string ParentCategory { get; set; } = null!;

    public string Category { get; set; } = null!;

    public decimal? MoneyIn { get; set; }

    public decimal? MoneyOut { get; set; }

    public decimal? Fee { get; set; }

    public decimal? Balance { get; set; }
}
