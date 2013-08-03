using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Jhu.SkyQuery.Xmd
{
	class Program
	{
        public static void Main(string[] args)
        {
            Xmd xmd = new Xmd();

            xmd.Columns.Add(new Column("[SDSS]:[PhotoObjAll].[ObjID]", "s_objID"));
            xmd.Columns.Add(new Column("[SDSS]:[PhotoObjAll].[RA]", "s_RA"));
            xmd.Columns.Add(new Column("[SDSS]:[PhotoObjAll].[Dec]", "s_Dec"));

            Table t;

            t = new Table(JoinMethod.NA, "[SDSS]:[PhotoObjAll]");
            t.XMatchMethod = XMatchMethod.Must;
            t.PointExpression = "POINT([SDSS]:[PhotoObjAll].[RA], [SDSS]:[PhotoObjAll].[Dec])";
            t.ErrorExpression = "0.02";
            xmd.Tables.Add(t);

            t = new Table(JoinMethod.Inner, "[SDSS]:[SpecObjAll]");
            t.XMatchMethod = XMatchMethod.None;
            t.On.Add(new List<And>(new And[] { new And("[SDSS]:[SpecObjAll].[BestPhotoObjId] = [SDSS]:[PhotoObjAll].[ObjId]") }));
            xmd.Tables.Add(t);

            t = new Table(JoinMethod.Cross, "[2MASS]:[PhotoObjAll]");
            t.XMatchMethod = XMatchMethod.Must;
            t.PointExpression = "POINT([2MASS]:[PhotoObjAll].[RA], [2MASS]:[PhotoObjAll].[Dec])";
            t.ErrorExpression = "0.02";
            xmd.Tables.Add(t);

            xmd.Where.Add(new List<And>(new And[] { new And("[SDSS]:[SpecObjAll].[Type] = 3") }));


            XmlSerializer ser = new XmlSerializer(typeof(Xmd));
            ser.Serialize(Console.Out, xmd);

            Console.ReadLine();
        }
	}
}
