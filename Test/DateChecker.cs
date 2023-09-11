using System;

public class DateTimeHelper
{
    public static string GetCurrentGMTDate()
    {
        // Get the current UTC (Coordinated Universal Time) date and time
        DateTime utcNow = DateTime.UtcNow;

        // Convert the UTC date to a GMT date string in the format "day month year"
        string gmtDate = utcNow.ToString("dd/MM/yyyy");

        return gmtDate;
    }
}
