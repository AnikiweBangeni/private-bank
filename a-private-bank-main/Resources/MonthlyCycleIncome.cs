using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a_private_bank_main.Resources
{
    public static class MonthlyCycleIncome
    {
        public static DateTime GetCycleStart(DateTime today)
        {
            var thisMonthPayday = GetAdjustedPayday(today);

            if (today >= thisMonthPayday)
            {
                return thisMonthPayday;
            }

            // otherwise use previous month’s payday
            var lastMonth = today.AddMonths(-1);
            return GetAdjustedPayday(lastMonth);
        }
        public static DateTime GetAdjustedPayday(DateTime date)
        {
            var payday = new DateTime(date.Year, date.Month, 24);

            // If 24th falls on Saturday or Sunday, move to Friday
            if (payday.DayOfWeek == DayOfWeek.Saturday)
                payday = payday.AddDays(-1);

            if (payday.DayOfWeek == DayOfWeek.Sunday)
                payday = payday.AddDays(-2);

            return payday;
        }
    }
}
