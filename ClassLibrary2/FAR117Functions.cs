using System;
using Microsoft.SqlServer.Server;
using System.Data.SqlClient;



public class FAR117Functions
{
    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static double FAR117_Table_A_LocalTime(DateTime reportTime)
    {
        int hour = reportTime.Hour * 100 + reportTime.Minute;
        using (SqlConnection conn = new SqlConnection("context connection=true"))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(
                "select max_block from [Scheduling].[dbo].[FAR117_Table_A] where " + hour.ToString() + " between startTime and endTime", conn);
            return (double)cmd.ExecuteScalar();
        }
    }

    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static double FAR117_Table_A_HerbTime(DateTime date, int time, string stationIATA)
    {

        var localTime = TimeFunctions.HerbDateandIntegerTimeToStationTime(date, time, stationIATA);
        int hour = localTime.Hour * 100 + localTime.Minute;
        using (SqlConnection conn = new SqlConnection("context connection=true"))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(
                "select max_block from [Scheduling].[dbo].[FAR117_Table_A] where " + hour.ToString() + " between startTime and endTime", conn);
            return (double)cmd.ExecuteScalar();
        }
    }

    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static double FAR117_Table_B_LocalTime(DateTime reportTime, int numberOfSegments)
    {
        numberOfSegments = Math.Min(Math.Max(numberOfSegments, 1), 7);
        int hour = reportTime.Hour * 100 + reportTime.Minute;

        using (SqlConnection conn = new SqlConnection("context connection=true"))
        {
            conn.Open();

            SqlCommand cmd = new SqlCommand(
                "select [" + numberOfSegments.ToString() + "] from [Scheduling].[dbo].[FAR117_Table_B] where " + hour.ToString() + " between startTime and endTime", conn);

            return (double)cmd.ExecuteScalar();
        }
    }

    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static double FAR117_Table_B_HerbTime(DateTime date, int time, string stationIATA, int numberOfSegments)
    {
        numberOfSegments = Math.Min(Math.Max(numberOfSegments, 1), 7);
        var localTime = TimeFunctions.HerbDateandIntegerTimeToStationTime(date, time, stationIATA);
        int hour = localTime.Hour * 100 + localTime.Minute;

        using (SqlConnection conn = new SqlConnection("context connection=true"))
        {
            conn.Open();

            SqlCommand cmd = new SqlCommand(
                "select [" + numberOfSegments.ToString() + "] from [Scheduling].[dbo].[FAR117_Table_B] where " + hour.ToString() + " between startTime and endTime", conn);

            return (double)cmd.ExecuteScalar();
        }
    }

    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static int CBA_LocalTime(DateTime reportTime)
    {
        int hour = reportTime.Hour * 100 + reportTime.Minute;
        using (SqlConnection conn = new SqlConnection("context connection=true"))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(
                "select actual from [Scheduling].[dbo].[CBA_Duty_Limits] where " + hour.ToString() + " between startTime and endTime", conn);
            return (int)cmd.ExecuteScalar();
        }
    }

    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static int CBA_HerbTime(DateTime date, int time, string stationIATA)
    {

        var localTime = TimeFunctions.HerbDateandIntegerTimeToStationTime(date, time, stationIATA);
        int hour = localTime.Hour * 100 + localTime.Minute;
        using (SqlConnection conn = new SqlConnection("context connection=true"))
        {
           
            conn.Open();
            SqlCommand cmd = new SqlCommand(
                "select actual from [Scheduling].[dbo].[CBA_Duty_Limits] where " + hour.ToString() + " between startTime and endTime", conn);
            return (int)cmd.ExecuteScalar();
        }
    }
}

