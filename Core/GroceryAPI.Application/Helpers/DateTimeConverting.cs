using System.Globalization;

namespace GroceryAPI.Application.Helpers
{
    public class DateTimeConverting
    {
        public static DateTime ConvertStringToDateTime(string dateTimeString)
        {
            string format = "dddd, MMMM d, yyyy | HH:mm - HH:mm";
            DateTime dateTime = DateTime.ParseExact(dateTimeString, format, CultureInfo.InvariantCulture);
            return dateTime;
        }
    }
}
