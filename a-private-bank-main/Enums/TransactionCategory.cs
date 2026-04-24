using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a_private_bank_main.Enums
{
    public enum TransactionCategory
    {
        // Cash
        [Description("Cash Withdrawal")] CashWithdrawal,

        // Cash Deposit
        [Description("Cash Deposit")] CashDeposit,

        // Communication
        [Description("Cellphone")] Cellphone,
        [Description("Internet")] Internet,
        [Description("Telephone")] Telephone,

        // Education
        [Description("Education")] Education,

        // Entertainment
        [Description("Alcohol")] Alcohol,
        [Description("Digital Subscriptions")] DigitalSubscriptions,
        [Description("Going Out")] GoingOut,

        // Fees
        [Description("Fees")] Fees,

        // Food
        [Description("Groceries")] Groceries,
        [Description("Restaurants")] Restaurants,
        [Description("Takeaways")] Takeaways,

        // Household
        [Description("Electricity")] Electricity,
        [Description("Furniture & Appliances")] FurnitureAndAppliances,
        [Description("Home Maintenance")] HomeMaintenance,
        [Description("Rent")] Rent,

        // Insurance
        [Description("Life Insurance")] LifeInsurance,
        [Description("Vehicle Insurance")] VehicleInsurance,

        // Interest
        [Description("Interest")] Interest,

        // Investment Income
        [Description("Investment Income")] InvestmentIncome,

        // Loans & Accounts
        [Description("Loan Payments")] LoanPayments,

        // Medical
        [Description("Doctors & Therapists")] DoctorsAndTherapists,

        // Other Income
        [Description("Other Income")] OtherIncome,

        // Payment Received
        [Description("Payment Received")] PaymentReceived,

        // Personal & Family
        [Description("Children & Dependants")] ChildrenAndDependants,
        [Description("Clothing & Shoes")] ClothingAndShoes,
        [Description("Digital Payments")] DigitalPayments,
        [Description("Personal Care")] PersonalCare,
        [Description("Sport & Hobbies")] SportAndHobbies,

        // Salary
        [Description("Salary")] Salary,

        // Savings & Investments
        [Description("Investments")] Investments,

        // Transfer
        [Description("Transfer")] Transfer,

        // Transport
        [Description("Fuel")] Fuel,
        [Description("Licence")] Licence,
        [Description("Parking")] Parking,
        [Description("Tolls")] Tolls,
        [Description("Vehicle Maintenance")] VehicleMaintenance,
        [Description("Vehicle Tracking")] VehicleTracking,

        // Uncategorised
        [Description("Uncategorised")] Uncategorised,
    }
}
