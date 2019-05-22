using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // Uses TZConvert Nugat Library
            // https://github.com/mj1856/TimeZoneConverter
            // PM> Install-Package TimeZoneConverter

            var time = 400;
            DateTime date = DateTime.Now;

            // Station Time Zone
            string timeZoneIATA = "America/Denver";
            string tzString = TimeZoneConverter.TZConvert.IanaToWindows(timeZoneIATA);
            TimeZoneInfo tzStation = TimeZoneInfo.FindSystemTimeZoneById(tzString);

            // Local Time Zone
           string localtzString = TimeZoneConverter.TZConvert.IanaToWindows("America/Chicago");
           TimeZoneInfo tzLocal = TimeZoneInfo.FindSystemTimeZoneById(localtzString);

           var hour = time / 100;
           var minutes = time - (hour * 100);
           var newDate = new DateTime(date.Year, date.Month, date.Day, hour, minutes, 0);

           // Convert from Central Time to Local Time
           TimeZoneInfo.ConvertTime(newDate, tzLocal, tzStation);

           Console.WriteLine(TimeZoneInfo.ConvertTime(newDate, tzLocal, tzStation));

            Console.ReadLine();
          
        }
    }
}
