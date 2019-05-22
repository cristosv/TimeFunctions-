﻿using System;
using System.Data;
using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;
using System.Data.SqlClient;

public class HelloWorldProc
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void HelloWorld(out string text)
    {
        SqlContext.Pipe.Send("sqlContext.Pipe.Send!" + Environment.NewLine);
        text = "Hello world!";
    }

    [SqlFunction()]
    public static int AddOne(int i)
    {
        return i + 1;
    }

    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static int IATADatabaseCount()
    {
        using (SqlConnection conn
            = new SqlConnection("context connection=true"))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(
                "select count (*) from IATA_TimeZone", conn);
            return (int)cmd.ExecuteScalar();
        }
    }

    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static string TimeZoneFor(string iata)
    {
        using (SqlConnection conn
            = new SqlConnection("context connection=true"))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(
                "select TimeZone from IATA_TimeZone where iata = '" + iata + "'", conn);
            return (string)cmd.ExecuteScalar();
        }
    }

}
