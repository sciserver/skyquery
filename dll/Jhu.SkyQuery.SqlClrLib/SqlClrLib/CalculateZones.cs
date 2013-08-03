using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    private struct ZoneDef
    {
        public int ZoneID;
        public double DecMin;
        public double DecMax;
        public double Alpha;
    }

    [Microsoft.SqlServer.Server.SqlFunction(
        DataAccess = DataAccessKind.None, IsDeterministic = true, IsPrecise = false,
        SystemDataAccess = SystemDataAccessKind.None,
        FillRowMethodName = "CalculateZones_Fill",
        TableDefinition = "ZoneID int, DecMin float, DecMax float, Alpha float")]
    public static IEnumerable CalculateZones(double zoneHeight, double theta, double partitionMin, double partitionMax)
    {
        int minzone = 0;
        int maxzone = (int)Math.Floor(180.0 / zoneHeight);

        while (minzone <= maxzone)
        {
            double zonedec = minzone * zoneHeight - 90;

            if (zonedec >= partitionMin && zonedec + zoneHeight <= partitionMax)
            {
                ZoneDef zd = new ZoneDef();
                zd.ZoneID = minzone;
                zd.DecMin = zonedec;
                zd.DecMax = zonedec + zoneHeight;
                zd.Alpha = CalculateAlpha(theta, zd.DecMin, zd.DecMax, zoneHeight);

                yield return zd;
            }

            minzone++;
        }
    }

    public static void CalculateZones_Fill(object o, out int zoneID, out double decMin, out double decMax, out double alpha)
    {
        ZoneDef zd = (ZoneDef)o;

        zoneID = zd.ZoneID;
        decMin = zd.DecMin;
        decMax = zd.DecMax;
        alpha = zd.Alpha;
    }
};

