using System;
using Microsoft.SqlServer.Server;
using System.Data.SqlClient;

/* Take a date, time (integer) and 3 letter station identifier and returns
 * a local/station dateTime.  

    // Uses TZConvert Nugat Library
    // https://github.com/mj1856/TimeZoneConverter
    // PM> Install-Package TimeZoneConverter

    Written by: Cristos Vasilas, 23 May 2019


    To test locally using the console app, use the following sql connection string:'
    conn.ConnectionString =
                "Data Source=tcp:az-sqlpsrc01.int.swapa.org;" +
                "Initial Catalog=Scheduling_Dev;" +
                "User id=MobileApp;" +
                "Password=fzmnfAmAox61;";

    else to use in the libary use:
              using (SqlConnection conn = new SqlConnection("context connection=true"))

    //using (SqlConnection conn = new SqlConnection())
                conn.ConnectionString =
                "Data Source=tcp:az-sqlpsrc01.int.swapa.org;" +
                "Initial Catalog=Scheduling_Dev;" +
                "User id=MobileApp;" +
                "Password=fzmnfAmAox61;";
 */
public class TimeFunctions
{
    public static string TimeZoneForIATA(string stationIATA)
    {
        using (SqlConnection conn = new SqlConnection("context connection=true"))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(
                "select TimeZone from [Scheduling].[dbo].[IATA_TimeZone] where iata = '" + stationIATA + "'", conn);
            return (string)cmd.ExecuteScalar();
        }
    }

    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static int IATADatabaseCount()
    {
        using (SqlConnection conn = new SqlConnection("context connection=true"))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(
                "select count (*) from [Scheduling].[dbo].[IATA_TimeZone]", conn);
            return (int)cmd.ExecuteScalar();
        }
    }


    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static DateTime HerbDateandIntegerTimeToStationTime(DateTime date, int time, string stationIATA)
    {
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
    public static DateTime HerbDateandIntegerTimeToUTCTime(DateTime date, int time)
    {
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
