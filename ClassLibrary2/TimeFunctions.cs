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

 
    public static string TimeZoneForIATA(string iata)
    {
        using (SqlConnection conn
            = new SqlConnection("context connection=true"))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(
                "select TimeZone from [dbo].[IATA_TimeZone] where iata = '" + iata + "'", conn);
            return (string)cmd.ExecuteScalar();
        }
    }

    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static DateTime HerbToLocal(DateTime date, int time, string iata)
    {
        // Uses TZConvert Nugat Library
        // https://github.com/mj1856/TimeZoneConverter
        // PM> Install-Package TimeZoneConverter

        // Station Time Zone
        string timeZoneIATA = TimeZoneForIATA(iata);
        string tzString = TimeZoneConverter.TZConvert.IanaToWindows(timeZoneIATA);
        TimeZoneInfo tzStation = TimeZoneInfo.FindSystemTimeZoneById(tzString);

        // Local Time Zone
        string localtzString = TimeZoneConverter.TZConvert.IanaToWindows("America/Chicago");
        TimeZoneInfo tzLocal = TimeZoneInfo.FindSystemTimeZoneById(localtzString);

        var hour = time / 100;
        var minutes = time - (hour * 100);
        var newDate = new DateTime(date.Year, date.Month, date.Day, hour, minutes, 0);

        var convertedDate = TimeZoneInfo.ConvertTime(newDate, tzLocal, tzStation);
        // Convert from Central Time to Local Time
        return convertedDate;
    }

}
