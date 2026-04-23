using System;
using System.Collections.Generic;

namespace a_private_bank_main.Models;

public partial class Person
{
    public int PersonId { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public DateTime? CreatedAt { get; set; }
}
