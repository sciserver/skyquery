using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.SkyQuery.Sql.QueryValidation
{
    class SkyQueryValidator
    {
#if false
        private void InterpretHtmIdHint(TableHint hint)
        {
            htmIdHint = hint;
            htmIdHintArguments = hint.GetArguments();

            if (htmIdHintArguments.Length != 1)
            {
                throw CreateException(ExceptionMessages.InvalidHtmIdFormat);
            }

            isHtmIdSpecified = true;

            if (!htmIdHintArguments[0].IsSingleColumn)
            {
                throw CreateException(ExceptionMessages.HtmIdIsNotSingleColumn);
            }
        }

        private void InterpretZoneIdHint(TableHint hint)
        {
            zoneIdHint = hint;
            zoneIdHintArguments = hint.GetArguments();

            if (zoneIdHintArguments.Length != 1)
            {
                throw CreateException(ExceptionMessages.InvalidZoneIdFormat);
            }

            isZoneIdSpecified = true;

            if (!ZoneIdHintExpression.IsSingleColumn)
            {
                throw CreateException(ExceptionMessages.ZoneIdIsNotSingleColumn);
            }
        }
#endif
    }
}
