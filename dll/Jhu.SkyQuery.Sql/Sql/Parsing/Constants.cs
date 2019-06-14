using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Sql.Parsing
{
    public class Constants
    {
        public const string AlgorithmBayesFactor = "BAYESFACTOR";
        public const string AlgorithmCone = "CONE";
        public const string AlgorithmChi2 = "CHI2";
        public const string InclusionMethodMust = "MUST";
        public const string InclusionMethodMay = "MAY";
        public const string InclusionMethodNot = "NOT";

        public const string PointHintName = "POINT";
        public const string HtmIdHintName = "HTMID";
        public const string ZoneIdHintName = "ZONEID";
        public const string ErrorHintName = "ERROR";
    }
}
