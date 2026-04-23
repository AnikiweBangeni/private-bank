using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a_private_bank_main.Models
{
    
 public class TransactionsStatementEntityModel
    {
        [Name("Id")]
        public int Id { get; set; }

        [Name("Nr")]

        public int Nr { get; set; }

        [Name("Account")]
        public long Account { get; set; }

        [Name("Posting Date")]
        public DateTime PostingDate { get; set; }

        [Name("Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [Name("Description")]
        public string Description { get; set; }

        [Name("Original Description")]
        public string OriginalDescription { get; set; }

        [Name("Parent Category")]
        public string ParentCategory { get; set; }

        [Name("Category")]
        public string Category { get; set; }

        [Name("Money In")]
        public decimal? MoneyIn { get; set; }

        [Name("Money Out")]
        public decimal? MoneyOut { get; set; }

        [Name("Fee")]
        public decimal? Fee { get; set; }

        [Name("Balance")]
        public decimal? Balance { get; set; }
    }
}
