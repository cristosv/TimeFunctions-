using System;
using Microsoft.SqlServer.Server;
using System.Data.SqlClient;



public class FAR117Functions
{
    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static double FAR117_Table_A_LocalTime(DateTime reportTime)
    {
        int hour = reportTime.Hour * 100 + reportTime.Minute;
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString =
                "Data Source=tcp:az-sqlpsrc01.int.swapa.org;" +
                "Initial Catalog=Scheduling_Dev;" +
                "User id=MobileApp;" +
                "Password=fzmnfAmAox61;";
            conn.Open();
            SqlCommand cmd = new SqlCommand(
                "select max_block from [dbo].[FAR117_Table_A] where " + hour.ToString() + " between startTime and endTime", conn);
            return (double)cmd.ExecuteScalar();
        }
    }

    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static double FAR117_Table_A_HerbTime(DateTime date, int time, string stationIATA)
    {

        var localTime = TimeFunctions.HerbDateandIntegerTimeToStationTime(date, time, stationIATA);
        int hour = localTime.Hour * 100 + localTime.Minute;
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString =
                "Data Source=tcp:az-sqlpsrc01.int.swapa.org;" +
                "Initial Catalog=Scheduling_Dev;" +
                "User id=MobileApp;" +
                "Password=fzmnfAmAox61;";
            conn.Open();
            SqlCommand cmd = new SqlCommand(
                "select max_block from [dbo].[FAR117_Table_A] where " + hour.ToString() + " between startTime and endTime", conn);
            return (double)cmd.ExecuteScalar();
        }
    }

    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static double FAR117_Table_B_LocalTime(DateTime reportTime, int numberOfSegments)
    {
        numberOfSegments = Math.Min(Math.Max(numberOfSegments, 1), 7);
        int hour = reportTime.Hour * 100 + reportTime.Minute;

        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString =
                "Data Source=tcp:az-sqlpsrc01.int.swapa.org;" +
                "Initial Catalog=Scheduling_Dev;" +
                "User id=MobileApp;" +
                "Password=fzmnfAmAox61;";
            conn.Open();

            SqlCommand cmd = new SqlCommand(
                "select [" + numberOfSegments.ToString() + "] from [dbo].[FAR117_Table_B] where " + hour.ToString() + " between startTime and endTime", conn);

            return (double)cmd.ExecuteScalar();
        }
    }

    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static double FAR117_Table_B_HerbTime(DateTime date, int time, string stationIATA, int numberOfSegments)
    {
        numberOfSegments = Math.Min(Math.Max(numberOfSegments, 1), 7);
        var localTime = TimeFunctions.HerbDateandIntegerTimeToStationTime(date, time, stationIATA);
        int hour = localTime.Hour * 100 + localTime.Minute;

        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString =
                "Data Source=tcp:az-sqlpsrc01.int.swapa.org;" +
                "Initial Catalog=Scheduling_Dev;" +
                "User id=MobileApp;" +
                "Password=fzmnfAmAox61;";
            conn.Open();

            SqlCommand cmd = new SqlCommand(
                "select [" + numberOfSegments.ToString() + "] from [dbo].[FAR117_Table_B] where " + hour.ToString() + " between startTime and endTime", conn);

            return (double)cmd.ExecuteScalar();
        }
    }
}

