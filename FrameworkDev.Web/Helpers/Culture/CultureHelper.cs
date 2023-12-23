using System;
using System.Globalization;
using System.Threading;

namespace FrameworkDev.Web.Helpers.Culture
{
    public static class CultureHelper
    {
        public static void SetGregorianCulture()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
        }

        public static void SetPersianCulture()
        {
            Thread.CurrentThread.CurrentCulture = PersianCulture.Instance;
            Thread.CurrentThread.CurrentUICulture = PersianCulture.Instance;
        }

        public static DateTime GetCurrentDate()
        {
            SetPersianCulture();
            return DateTime.Now;
        }

        public static DateTime GetCurrentWeekStartDate()
        {
            SetPersianCulture();
            DateTime d = DateTime.Now;
            DayOfWeek dow = d.DayOfWeek;
            return d.AddDays((-1 * (int)dow) - 1);
        }

        public static DateTime GetCurrentWeekEndDate()
        {
            SetPersianCulture();
            DateTime d = DateTime.Now;
            DayOfWeek dow = d.DayOfWeek;
            return d.AddDays((-1 * (int)dow) - 1 + 7);
        }

        public static DateTime GetCurrentMonthStartDate()
        {
            SetPersianCulture();

            DateTime d = DateTime.Now;

            string dStr = d.ToShortDateString();

            int yy = int.Parse(dStr.Split('/')[0]);
            int mm = int.Parse(dStr.Split('/')[1]);

            return DateTime.Parse(string.Format("{0}/{1}/{2}", yy, mm, 1));
        }

        public static DateTime GetCurrentMonthEndDate()
        {
            SetPersianCulture();

            DateTime d = DateTime.Now;

            string dStr = d.ToShortDateString();

            int yy = int.Parse(dStr.Split('/')[0]);
            int mm = int.Parse(dStr.Split('/')[1]);
            int dd = 31;

            if (mm == 12)
            {
                if (yy % 4 == 3) dd = 30;
                else dd = 29;
            }
            else if (mm > 6) { dd = 30; }
            else { dd = 31; }

            return DateTime.Parse(string.Format("{0}/{1}/{2}", yy, mm, dd));
        }

        public static DateTime GetCurrentYearStartDate()
        {
            SetPersianCulture();

            DateTime d = DateTime.Now;

            string dStr = d.ToShortDateString();

            int yy = int.Parse(dStr.Split('/')[0]);

            return DateTime.Parse(string.Format("{0}/{1}/{2}", yy, 1, 1));
        }

        public static DateTime GetCurrentYearEndDate()
        {
            SetPersianCulture();
            DateTime d = DateTime.Now;

            string dStr = d.ToShortDateString();

            int yy = int.Parse(dStr.Split('/')[0]);
            int mm = 12;
            int dd = 31;

            if (yy % 4 == 3) dd = 30;
            else dd = 29;

            return DateTime.Parse(string.Format("{0}/{1}/{2}", yy, mm, dd));
        }

        public static string GetPersianNumber(string englishNumber)
        {
            string persianNumber = "";
            foreach (char ch in englishNumber)
            {
                persianNumber += (char)(1776 + char.GetNumericValue(ch));
            }
            return persianNumber;
        }

        public static string GetEnglishNumber(string persianNumber)
        {
            string englishNumber = "";
            foreach (char ch in persianNumber)
            {
                englishNumber += char.GetNumericValue(ch);
            }
            return englishNumber;
        }

    }
}
