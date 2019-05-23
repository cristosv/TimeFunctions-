using System;
using Microsoft.SqlServer.Server;
using System.Data.SqlClient;

public class TimeFunctions
{
    //[Microsoft.SqlServer.Server.SqlProcedure]
    //public static void HelloWorld(out string text)
    //{
    //    SqlContext.Pipe.Send("sqlContext.Pipe.Send!" + Environment.NewLine);
    //    text = "Hello world!";
    //}

    //[SqlFunction()]
    //public static int AddOne(int i)
    //{
    //    return i + 1;
    //}

    //"Server=tcp:az-sqlpsrc01.int.swapa.org ,1433;Database=Scheduling_Dev;uid=MobileApp;pwd=fzmnfAmAox61;"
    public static string TimeZoneForIATA(string stationIATA)
    {
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString =
                "Data Source=tcp:az-sqlpsrc01.int.swapa.org;" +
                "Initial Catalog=Scheduling_Dev;" +
                "User id=MobileApp;" +
                "Password=fzmnfAmAox61;";
            conn.Open();
            SqlCommand cmd = new SqlCommand(
                "select TimeZone from [dbo].[IATA_TimeZone] where iata = '" + stationIATA + "'", conn);
            return (string)cmd.ExecuteScalar();
        }
    }

    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static int IATADatabaseCount()
    {
        using (SqlConnection conn
            = new SqlConnection("context connection=true"))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(
                "select count (*) from [dbo].[IATA_TimeZone]", conn);
            return (int)cmd.ExecuteScalar();
        }
    }

 

        /* Take a date, time (integer) and 3 letter station identifier and returns
         * a local/station dateTime.  
        
         
         */
    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static DateTime HerbDateandTimeToStationTime(DateTime date, int time, string stationIATA)
    {
        // Uses TZConvert Nugat Library
        // https://github.com/mj1856/TimeZoneConverter
        // PM> Install-Package TimeZoneConverter

        // Convert integer time to create a dateTime
        var hour = time / 100;
        var minutes = time - (hour * 100);
        var herbDateTime = new DateTime(date.Year, date.Month, date.Day, hour, minutes, 0);

        // Station Time Zone
        string timeZoneStationIATA = TimeZoneForIATA(stationIATA);
        string tzStationString = TimeZoneConverter.TZConvert.IanaToWindows(timeZoneStationIATA);
        TimeZoneInfo tzStation = TimeZoneInfo.FindSystemTimeZoneById(tzStationString);

        // Central Time Zone
        string timeZoneHerbIATA = TimeZoneForIATA("DAL");
        string tzHerbString = TimeZoneConverter.TZConvert.IanaToWindows(timeZoneHerbIATA);
        TimeZoneInfo tzHerb = TimeZoneInfo.FindSystemTimeZoneById(tzHerbString);

        // Convert from Central Time to Local/station Time
        var stationDateTime = TimeZoneInfo.ConvertTime(herbDateTime, tzHerb, tzStation);
        
        return stationDateTime;
    }

    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static DateTime StationTimeToStationTime(DateTime dateTime, string fromStationIATA, string toStationIATA)
    {
        // Uses TZConvert Nugat Library
        // https://github.com/mj1856/TimeZoneConverter
        // PM> Install-Package TimeZoneConverter

        var fromStationDateTime = dateTime;

        // Station 1 Time Zone
        string timeZoneFromStationIATA = TimeZoneForIATA(fromStationIATA);
        string tzFromStationString = TimeZoneConverter.TZConvert.IanaToWindows(timeZoneFromStationIATA);
        TimeZoneInfo tzFromStation = TimeZoneInfo.FindSystemTimeZoneById(tzFromStationString);

        // Station 2 Time Zone
        string timeZoneToStationIATA = TimeZoneForIATA(toStationIATA);
        string tzToStationString = TimeZoneConverter.TZConvert.IanaToWindows(timeZoneToStationIATA);
        TimeZoneInfo tzToStation = TimeZoneInfo.FindSystemTimeZoneById(tzToStationString);

        // Convert from Central Time to Local/station Time
        var stationDateTime = TimeZoneInfo.ConvertTime(fromStationDateTime, tzFromStation, tzToStation);

        return stationDateTime;
    }

    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static DateTime HerbDateandTimeToUTCTime(DateTime date, int time)
    {
        // Uses TZConvert Nugat Library
        // https://github.com/mj1856/TimeZoneConverter
        // PM> Install-Package TimeZoneConverter

        // Convert integer time to create a dateTime
        var hour = time / 100;
        var minutes = time - (hour * 100);
        var herbDateTime = new DateTime(date.Year, date.Month, date.Day, hour, minutes, 0);

        // UTC Time Zone
        TimeZoneInfo tzUTC = TimeZoneInfo.Utc;

        // Central Time Zone
        string timeZoneHerbIATA = TimeZoneForIATA("DAL");
        string tzHerbString = TimeZoneConverter.TZConvert.IanaToWindows(timeZoneHerbIATA);
        TimeZoneInfo tzHerb = TimeZoneInfo.FindSystemTimeZoneById(tzHerbString);

        // Convert from Central Time to Local/station Time
        var UTCDateTime = TimeZoneInfo.ConvertTime(herbDateTime, tzHerb, tzUTC);

        return UTCDateTime;
    }

    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static DateTime StationTimeToUTC(DateTime dateTime, string stationIATA)
    {
        // Uses TZConvert Nugat Library
        // https://github.com/mj1856/TimeZoneConverter
        // PM> Install-Package TimeZoneConverter

        var stationDateTime = dateTime;

        // UTC Time Zone
        TimeZoneInfo tzUTC = TimeZoneInfo.Utc;

        // Station Time Zone
        string timeZoneStationIATA = TimeZoneForIATA(stationIATA);
        string tzStationString = TimeZoneConverter.TZConvert.IanaToWindows(timeZoneStationIATA);
        TimeZoneInfo tzStation = TimeZoneInfo.FindSystemTimeZoneById(tzStationString);

        // Convert from Station Time to UTC Time
        var UTCDateTime = TimeZoneInfo.ConvertTime(stationDateTime, tzStation, tzUTC);

        return UTCDateTime;
    }

    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static DateTime UTCtoStationTime(DateTime dateTime, string stationIATA)
    {
        // Uses TZConvert Nugat Library
        // https://github.com/mj1856/TimeZoneConverter
        // PM> Install-Package TimeZoneConverter

        var utcDateTime = dateTime;

        // UTC Time Zone
        TimeZoneInfo tzUTC = TimeZoneInfo.Utc;

        // Station Time Zone
        string timeZoneStationIATA = TimeZoneForIATA(stationIATA);
        string tzStationString = TimeZoneConverter.TZConvert.IanaToWindows(timeZoneStationIATA);
        TimeZoneInfo tzStation = TimeZoneInfo.FindSystemTimeZoneById(tzStationString);

        // Convert from Station Time to UTC Time
        var stationDateTime = TimeZoneInfo.ConvertTime(utcDateTime, tzUTC, tzStation);

        return stationDateTime;
    }

}
