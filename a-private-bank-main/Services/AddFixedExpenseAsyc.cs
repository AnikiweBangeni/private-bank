using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using a_private_bank_main.Models.ResponseModels;

namespace a_private_bank_main.Services
{
    public class AddFixedExpenseAsyc
    {
        public async Task<FixedExpenseResponseModel> AddFixedExpenseAsync(string OrigionalDescription, DateOnly PostingDate, string Category, decimal Amount, DateOnly dueDate, bool? isPaid, string? notes)
        {
           var fixedExpense = FixedExpenseResponseModel { Name = OrigionalDescription, Category = Category};

            return fixedExpense;    
        }

    }
}
