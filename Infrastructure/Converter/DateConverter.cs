using System.Globalization;

namespace Infrastructure.Converter;

public static class DateConverter
{
    public static string ToShamsi(this DateTime value)
    {
        PersianCalendar pc = new PersianCalendar();
        string monthName = string.Empty;
        switch (pc.GetMonth(value))
        {
            case 1: { monthName = "فروردین"; break; }
            case 2: { monthName = "اردیبهشت"; break; }
            case 3: { monthName = "خرداد"; break; }
            case 4: { monthName = "تیر"; break; }
            case 5: { monthName = "مرداد"; break; }
            case 6: { monthName = "شهریور"; break; }
            case 7: { monthName = "مهر"; break; }
            case 8: { monthName = "آبان"; break; }
            case 9: { monthName = "آذر"; break; }
            case 10: { monthName = "دی"; break; }
            case 11: { monthName = "بهمن"; break; }
            case 12: { monthName = "اسفند"; break; }
        }
        return pc.GetDayOfMonth(value).ToString("00") + " " + monthName + " " + pc.GetYear(value).ToString();
    }
}

