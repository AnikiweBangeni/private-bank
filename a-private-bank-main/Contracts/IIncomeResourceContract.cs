using a_private_bank_main.Models;
using a_private_bank_main.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a_private_bank_main.Contracts
{
    public interface IIncomeResourceContract
    {
        Task<IncomeValueResponseModel> GetCycleIncomeAsync( );
    }
}
