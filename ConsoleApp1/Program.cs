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
            DateTime date = new DateTime(2019, 5, 01);

            // Station Time Zone
            string timeZoneIATA = "America/Phoenix";
            string tzString = TimeZoneConverter.TZConvert.IanaToWindows(timeZoneIATA);
            TimeZoneInfo tzStation = TimeZoneInfo.FindSystemTimeZoneById(tzString);

            // Local Time Zone
           string localtzString = TimeZoneConverter.TZConvert.IanaToWindows("America/Chicago");
           TimeZoneInfo tzCentral = TimeZoneInfo.FindSystemTimeZoneById(localtzString);

           var hour = time / 100;
           var minutes = time - (hour * 100);
           var newDate = new DateTime(date.Year, date.Month, date.Day, hour, minutes, 0);

           // Convert from Central Time to Local Time
           var convertedDate = TimeZoneInfo.ConvertTime(newDate, tzCentral, tzStation);

           
           Console.WriteLine("{0} {1} is {2} {3}.",
           convertedDate, tzStation.IsDaylightSavingTime(newDate) ? tzStation.DaylightName : tzStation.StandardName,
           newDate, tzCentral.IsDaylightSavingTime(newDate) ? tzCentral.DaylightName : tzCentral.StandardName);

            Console.ReadLine();
          
        }
    }
}
