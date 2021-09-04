using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll_Manager.Services
{
    public class Kiemtra
    {
        public DateTime GetFirstDayOfWeek(DateTime dayInWeek)
        {
            var dayofWeek = (int)dayInWeek.DayOfWeek;
            switch (dayofWeek)
            {
                case 0:
                    return dayInWeek.AddDays(-6);
                case 1:
                    return dayInWeek;
                case 2:
                    return dayInWeek.AddDays(-1);
                case 3:
                    return dayInWeek.AddDays(-2);
                case 4:
                    return dayInWeek.AddDays(-3);
                case 5:
                    return dayInWeek.AddDays(-4);
                case 6:
                    return dayInWeek.AddDays(-5);
            }
            return dayInWeek;
        }

        public DateTime GetQuarter(DateTime date)
        {
            if (date.Month >= 4 && date.Month <= 6)
                return new DateTime(date.Year, 4, 1);
            else if (date.Month >= 7 && date.Month <= 9)
                return new DateTime(date.Year, 7, 1);
            else if (date.Month >= 10 && date.Month <= 12)
                return new DateTime(date.Year, 10, 1);
            else
                return new DateTime(date.Year, 1, 1);

        }
    }
}
