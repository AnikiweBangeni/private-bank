using a_private_bank_main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using a_private_bank_main.Resources;
using a_private_bank_main.Contracts;
using a_private_bank_main.Models.ResponseModels;
using Microsoft.EntityFrameworkCore;

namespace a_private_bank_main.Resources
{
    
    public class MonthlyCycle : IIncomeResourceContract
    {
        private readonly AprivateBankContext _dbContext;

        public MonthlyCycle(AprivateBankContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IncomeValueResponseModel> GetCycleIncomeAsync( )
        {
            var today = DateTime.Today;
            var cycleStart = MonthlyCycleIncome.GetCycleStart(today);

            var incomes =  _dbContext.TransactionsStatements
                .Where(a =>
                    a.ParentCategory == "Salary" && a.OriginalDescription.Contains("CASHFOCUS SALARY") &&
                    a.TransactionDate >= cycleStart &&
                    a.TransactionDate <= today).FirstOrDefaultAsync().Result;

            return new IncomeValueResponseModel { Date = incomes.TransactionDate, Amount = incomes.MoneyIn , Description = incomes.Description};

        }


    }
}
