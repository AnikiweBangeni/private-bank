using a_private_bank_main.Contracts;
using a_private_bank_main.Models;
using a_private_bank_main.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                var folderPath = @"C:\Users\0209255666080\source\Projects 2026\Documents";

                var dataDocFile = new DirectoryInfo(folderPath)
                    .GetFiles("*.csv")
                    .OrderByDescending(x => x.LastWriteTime)
                    .FirstOrDefault();

                if (dataDocFile == null)
                    throw new FileNotFoundException("No CSV files found in the folder.");

                // 🔥 PROPER CONFIG (THIS FIXES CS0106 / CONFIG ERRORS)
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HeaderValidated = null,
                    MissingFieldFound = null,
                    TrimOptions = TrimOptions.Trim,
                    PrepareHeaderForMatch = args => args.Header?.Trim()
                };

                using var readDoc = new StreamReader(dataDocFile.FullName);
                using var csv = new CsvReader(readDoc, config);

                var records = csv
                    .GetRecords<TransactionsStatement>()
                    .Where(x => x.Balance >= 0)
                    .ToList();

                if (!records.Any())
                    throw new InvalidOperationException("No valid records found in the CSV file.");

                return records;
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
