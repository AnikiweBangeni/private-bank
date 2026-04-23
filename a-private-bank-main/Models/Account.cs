using System;
using System.Collections.Generic;

namespace a_private_bank_main.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public int AccountRefPerson { get; set; }

    public long AccountNumber { get; set; }

    public string? BankName { get; set; }

    public DateTime? CreatedAt { get; set; }
}
