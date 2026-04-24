using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a_private_bank_main.Enums
{
    public enum TransactionParentCategory
    {
        [Description("Cash")] Cash,
        [Description("Cash Deposit")] CashDeposit,
        [Description("Communication")] Communication,
        [Description("Education")] Education,
        [Description("Entertainment")] Entertainment,
        [Description("Fees")] Fees,
        [Description("Food")] Food,
        [Description("Household")] Household,
        [Description("Insurance")] Insurance,
        [Description("Interest")] Interest,
        [Description("Investment Income")] InvestmentIncome,
        [Description("Loans & Accounts")] LoansAndAccounts,
        [Description("Medical")] Medical,
        [Description("Other Income")] OtherIncome,
        [Description("Payment Received")] PaymentReceived,
        [Description("Personal & Family")] PersonalAndFamily,
        [Description("Salary")] Salary,
        [Description("Savings & Investments")] SavingsAndInvestments,
        [Description("Transfer")] Transfer,
        [Description("Transport")] Transport,
        [Description("Uncategorised")] Uncategorised,
    }
}
