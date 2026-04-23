using a_private_bank_main.Contracts;
using a_private_bank_main.Models;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using a_private_bank_main.Models;
using Microsoft.EntityFrameworkCore;

namespace a_private_bank_main.Resources
{
    public class ImportData : IDataResourceContracts
    {
        private readonly AprivateBankContext _dbContext;

        public ImportData(AprivateBankContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TransactionsStatement>> ImportDataDocAsync( )
        {
            try
            {
                var dataDocPath = @"C:\Users\0209255666080\source\Projects 2026\Downloads\account_statement_1-Dec-2025_to_14-Mar-2026.csv";

                using (var readDoc = new StreamReader(dataDocPath))

                using (var docContent = new CsvReader(readDoc, CultureInfo.InvariantCulture))
                {
                    var docRecords = docContent.GetRecords<TransactionsStatement>().Where(x => x.Balance >= 0).ToList();


                    if(docRecords == null || !docRecords.Any())
                    {
                        throw new InvalidOperationException("No valid records found in the CSV file.");
                    }

                    return docRecords;
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }



        public async  Task<bool> PostImportedDataToDbAsync( )
        {
            var data = this.ImportDataDocAsync().Result;

            if (data == null)
            {
                throw new InvalidOperationException("No data to import. ImportDataDocAsync returned null.");
            }

            var existing = await _dbContext.TransactionsStatements.Select(x => x.Nr).ToListAsync();

            await _dbContext.TransactionsStatements.AddRangeAsync(data.Where(c => !existing.Contains(c.Nr)).Select(x => new TransactionsStatementEntityModel
            {
                Account = x.Account,
                Balance = x.Balance,
                Category = x.Category,
                Description = x.Description,
                Fee = x.Fee,
                MoneyIn = x.MoneyIn,
                MoneyOut = x.MoneyOut,
                Nr = x.Nr,
                OriginalDescription = x.OriginalDescription,
                ParentCategory = x.ParentCategory,
                PostingDate = x.PostingDate,
                TransactionDate = x.TransactionDate,

            }));

            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
    
}
