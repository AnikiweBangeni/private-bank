using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using a_private_bank_main.Enums;

namespace a_private_bank_main.Enums
{
    /// <summary>
    /// Maps each ParentCategory to its allowed child Categories.
    /// Use this to drive dropdowns, validation, or EF query filters.
    /// </summary>
    public static class TransactionCategoryMapp
    {
        public static readonly IReadOnlyDictionary<TransactionParentCategory, TransactionCategory[]> All =
            new Dictionary<TransactionParentCategory, TransactionCategory[]>
            {
                [TransactionParentCategory.Cash] = [TransactionCategory.CashWithdrawal],
                [TransactionParentCategory.CashDeposit] = [TransactionCategory.CashDeposit],
                [TransactionParentCategory.Communication] = [TransactionCategory.Cellphone, TransactionCategory.Internet, TransactionCategory.Telephone],
                [TransactionParentCategory.Education] = [TransactionCategory.Education],
                [TransactionParentCategory.Entertainment] = [TransactionCategory.Alcohol, TransactionCategory.DigitalSubscriptions, TransactionCategory.GoingOut],
                [TransactionParentCategory.Fees] = [TransactionCategory.Fees],
                [TransactionParentCategory.Food] = [TransactionCategory.Groceries, TransactionCategory.Restaurants, TransactionCategory.Takeaways],
                [TransactionParentCategory.Household] = [TransactionCategory.Electricity, TransactionCategory.FurnitureAndAppliances, TransactionCategory.HomeMaintenance, TransactionCategory.Rent],
                [TransactionParentCategory.Insurance] = [TransactionCategory.LifeInsurance, TransactionCategory.VehicleInsurance],
                [TransactionParentCategory.Interest] = [TransactionCategory.Interest],
                [TransactionParentCategory.InvestmentIncome] = [TransactionCategory.InvestmentIncome],
                [TransactionParentCategory.LoansAndAccounts] = [TransactionCategory.LoanPayments],
                [TransactionParentCategory.Medical] = [TransactionCategory.DoctorsAndTherapists],
                [TransactionParentCategory.OtherIncome] = [TransactionCategory.OtherIncome],
                [TransactionParentCategory.PaymentReceived] = [TransactionCategory.PaymentReceived],
                [TransactionParentCategory.PersonalAndFamily] = [TransactionCategory.ChildrenAndDependants, TransactionCategory.ClothingAndShoes, TransactionCategory.DigitalPayments, TransactionCategory.PersonalCare, TransactionCategory.SportAndHobbies],
                [TransactionParentCategory.Salary] = [TransactionCategory.Salary],
                [TransactionParentCategory.SavingsAndInvestments] = [TransactionCategory.Investments],
                [TransactionParentCategory.Transfer] = [TransactionCategory.Transfer],
                [TransactionParentCategory.Transport] = [TransactionCategory.Fuel, TransactionCategory.Licence, TransactionCategory.Parking, TransactionCategory.Tolls, TransactionCategory.VehicleMaintenance, TransactionCategory.VehicleTracking],
                [TransactionParentCategory.Uncategorised] = [TransactionCategory.Uncategorised],
            };

        /// <summary>
        /// Returns the child categories for a given parent.
        /// </summary>
        public static TransactionCategory[] GetCategories(TransactionParentCategory parent)
            => All.TryGetValue(parent, out var cats) ? cats : [];

        /// <summary>
        /// Looks up which parent a category belongs to.
        /// </summary>
        public static TransactionParentCategory? GetParent(TransactionCategory category)
        {
            foreach (var (parent, cats) in All)
                if (cats.Contains(category)) return parent;
            return null;
        }
    }
}
