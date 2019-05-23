using System;
using System.Data;
using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;
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
    private static string TimeZoneForIATA(string stationIATA)
    {
        using (SqlConnection conn = new SqlConnection("context connection=true"))
        {
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
    public static DateTime HerbToLocal(DateTime date, int time, string stationIATA)
    {
        // Uses TZConvert Nugat Library
        // https://github.com/mj1856/TimeZoneConverter
        // PM> Install-Package TimeZoneConverter

        // Station Time Zone
        string timeZoneStationIATA = TimeZoneForIATA(stationIATA);
        string tzStationString = TimeZoneConverter.TZConvert.IanaToWindows(timeZoneStationIATA);
        TimeZoneInfo tzStation = TimeZoneInfo.FindSystemTimeZoneById(tzStationString);

        // Central Time Zone
        string timeZoneHerbIATA = TimeZoneForIATA("DAL");
        string tzHerbString = TimeZoneConverter.TZConvert.IanaToWindows(timeZoneHerbIATA);
        TimeZoneInfo tzHerb = TimeZoneInfo.FindSystemTimeZoneById(tzHerbString);

        var hour = time / 100;
        var minutes = time - (hour * 100);
        var herbDateTime = new DateTime(date.Year, date.Month, date.Day, hour, minutes, 0);

        var stationDateTime = TimeZoneInfo.ConvertTime(herbDateTime, tzHerb, tzStation);
        // Convert from Central Time to Local Time
        return stationDateTime;
    }

}
