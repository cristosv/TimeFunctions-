using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            

            // Uses TZConvert Nugat Library
            // https://github.com/mj1856/TimeZoneConverter
            // PM> Install-Package TimeZoneConverter

            var time = 400;
            DateTime date = new DateTime(2019, 5, 01);
            var hour = time / 100;
            var minutes = time - (hour * 100);
            var originalTime = new DateTime(date.Year, date.Month, date.Day, hour, minutes, 0);

            var stationTime = TimeFunctions.HerbDateandIntegerTimeToStationTime(date, time, "LAX");
            var utcTime = TimeFunctions.StationTimeToUTC(stationTime, "LAX");
            var bwiTime = TimeFunctions.UTCtoStationTime(utcTime, "BWI");
            var laxTime = TimeFunctions.StationTimeToStationTime(bwiTime, "BWI", "LAX");
            var herbTime = TimeFunctions.StationTimeToStationTime(laxTime, "LAX", "DAL");
            var phxTime = TimeFunctions.StationTimeToStationTime(herbTime, "DAL", "PHX");

            Console.WriteLine(
                "originalTime HerbTime: {0}\nLAX_Time: {1}\nutcTime: {2}\nBWI_Time: {3}\nLAX_Time:{4}\nHerbTime:{5}\nPHX_Time:{6}", originalTime, stationTime, utcTime, bwiTime, laxTime, herbTime, phxTime
                );
           
           //Console.WriteLine("{0} {1} is {2} {3}.",
           //convertedDate, tzStation.IsDaylightSavingTime(newDate) ? tzStation.DaylightName : tzStation.StandardName,
           //newDate, tzCentral.IsDaylightSavingTime(newDate) ? tzCentral.DaylightName : tzCentral.StandardName);

            Console.ReadLine();
          
        }
    }

 
}

