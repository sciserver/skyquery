using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jhu.SkyQuery.Parser;

namespace Jhu.SkyQuery.Jobs.Query
{
    static class Error
    {
        public static XMatchException NoCoordinateColumnFound(TableCoordinates coords, string column)
        {
            return new XMatchException(String.Format(
                ExceptionMessages.NoCoordinateColumnFound,
                column,
                coords.Table.TableReference.UniqueName));
        }

        public static XMatchException NoCoordinateColumnsFound(TableCoordinates coords)
        {
            return new XMatchException(String.Format(
                ExceptionMessages.NoCoordinateColumnsFound,
                coords.Table.TableReference.UniqueName));
        }

        public static XMatchException XMatchMultipleStatementsNotSupported()
        {
            return new XMatchException(ExceptionMessages.XMatchMultipleStatementsNotSupported);
        }

        public static XMatchException SearchRadiusTooLarge(double radius)
        {
            return new XMatchException(String.Format(ExceptionMessages.SearchRadiusTooLarge, radius));
        }
    }
}
