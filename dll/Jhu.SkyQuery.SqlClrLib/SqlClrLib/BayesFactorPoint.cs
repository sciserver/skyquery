using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.Native)]
public struct BayesFactorPoint : INullable
{
    public override string ToString()
    {
        // Replace the following code with your code
        return "";
    }

    public bool IsNull
    {
        get
        {
            // Put your code here
            return m_Null;
        }
    }

    public static BayesFactorPoint Null
    {
        get
        {
            BayesFactorPoint h = new BayesFactorPoint();
            h.m_Null = true;
            return h;
        }
    }

    public static BayesFactorPoint Parse(SqlString s)
    {
        if (s.IsNull)
            return Null;
        BayesFactorPoint u = new BayesFactorPoint();
        // Put your code here
        return u;
    }

    // Private member
    private bool m_Null;

    public double Ra;
	public double Dec;
	public double Cx;
    public double Cy;
    public double Cz;
	public double A;
	public double L;
	public double LogBF;
    public double dQ;
}