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

            var time = 500;
            DateTime date = new DateTime(2019, 6, 18);
            var hour = time / 100;
            var minutes = time - (hour * 100);
            var originalTime = new DateTime(date.Year, date.Month, date.Day, hour, minutes, 0);

            var stationTime = TimeFunctions.HerbDateandIntegerTimeToStationTime(date, time, "DEN");
            var utcTime = TimeFunctions.StationTimeToUTC(stationTime, "LAX");
            var bwiTime = TimeFunctions.UTCtoStationTime(utcTime, "BWI");
            var laxTime = TimeFunctions.StationTimeToStationTime(bwiTime, "BWI", "LAX");
            var herbTime = TimeFunctions.StationTimeToStationTime(laxTime, "LAX", "DAL");
            var phxTime = TimeFunctions.StationTimeToStationTime(herbTime, "DAL", "PHX");

            var max_block_LocalTime = FAR117Functions.FAR117_Table_A_LocalTime(originalTime);
            var max_fdp_LocalTime = FAR117Functions.FAR117_Table_B_LocalTime(originalTime, 5);
            var max_cba_LocalTime = FAR117Functions.CBA_LocalTime(originalTime);

            var max_block_HerbTime = FAR117Functions.FAR117_Table_A_HerbTime(originalTime, time, "DEN");
            var max_fdp_HerbTime = FAR117Functions.FAR117_Table_B_HerbTime(originalTime, time, "DEN", 5);
            var max_cba_HerbTime = FAR117Functions.CBA_HerbTime(originalTime, time, "DEN");

            Console.WriteLine( "originalTime HerbTime: {0}\nLAX_Time: {1}\nutcTime: {2}\nBWI_Time: {3}\nLAX_Time:{4}\nHerbTime:{5}\nPHX_Time:{6}", originalTime, stationTime, utcTime, bwiTime, laxTime, herbTime, phxTime);


            Console.WriteLine("Local Time {0} Max_FDP: {1}, Max_Block: {2}, CBA_Actual: {3}", originalTime, max_fdp_LocalTime, max_block_LocalTime, max_cba_LocalTime);
            Console.WriteLine("Herb Time {0} Max_FDP: {1}, Max_Block: {2}, CBA_Actual: {3}", stationTime, max_fdp_HerbTime, max_block_HerbTime, max_cba_HerbTime);
            //Console.WriteLine("{0} {1} is {2} {3}.",
            //convertedDate, tzStation.IsDaylightSavingTime(newDate) ? tzStation.DaylightName : tzStation.StandardName,
            //newDate, tzCentral.IsDaylightSavingTime(newDate) ? tzCentral.DaylightName : tzCentral.StandardName);

            Console.ReadLine();
          
        }
    }

 
}

