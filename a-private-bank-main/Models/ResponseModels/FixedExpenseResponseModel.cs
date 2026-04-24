using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a_private_bank_main.Models.ResponseModels
{
    public class FixedExpenseResponseModel
    {
        public string Name { get; set; }
        public string Category { get; set; } 
        public string Amount { get; set; }
        public int PeriodId { get; set; } = 0;
        public DateOnly PostingDate { get; set; }   
        public DateOnly DueDate { get; set; } 
        public bool IsPaid { get; set; }
    }
}
