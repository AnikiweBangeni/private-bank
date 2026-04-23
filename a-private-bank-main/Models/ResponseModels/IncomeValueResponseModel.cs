using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a_private_bank_main.Models.ResponseModels
{
    public class IncomeValueResponseModel
    {
       public DateTime Date { get; set; }
        public decimal? Amount { get; set; }

        public string Description { get; set; } 
    }
}
