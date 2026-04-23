using a_private_bank_main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using a_private_bank_main.Resources;

namespace a_private_bank_main.Contracts
{
    public interface IDataResourceContracts
    {
        Task<List<TransactionsStatement>> ImportDataDocAsync( );
        Task<bool> PostImportedDataToDbAsync( );
    }
}
