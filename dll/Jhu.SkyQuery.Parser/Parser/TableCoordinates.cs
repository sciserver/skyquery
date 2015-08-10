using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Schema.SqlServer;
using Jhu.Graywulf.SqlCodeGen.SqlServer;

namespace Jhu.SkyQuery.Parser
{
    // Implements function to get coordinates and coordinate
    // errors applying to a table from the query or from the metadata
    public class TableCoordinates
    {
        #region Private member varibles

        private SimpleTableSource table;

        private TableHint pointHint;
        private Argument[] pointHintArguments;
        private TableHint htmIdHint;
        private Argument[] htmIdHintArguments;
        private TableHint zoneIdHint;
        private Argument[] zoneIdHintArguments;
        private TableHint errorHint;
        private Argument[] errorHintArguments;

        private bool isEqSpecified;
        private bool isCartesianSpecified;
        private bool isHtmIdSpecified;
        private bool isZoneIdSpecified;
        private bool isErrorSpecified;
        private bool isErrorLimitsSpecified;

        private int eqIndex;
        private int cartesianIndex;

        private string codeDatasetFunctionPrefix;

        #endregion
        #region Properties

        public SimpleTableSource Table
        {
            get { return table; }
        }

        bool IsEqSpecified
        {
            get { return isEqSpecified; }
        }

        bool IsCartesianSpecified
        {
            get { return isEqSpecified; }
        }

        private Expression RAExpression
        {
            get
            {
                return pointHintArguments[eqIndex].FindDescendant<Expression>();
            }
        }

        private Expression DecExpression
        {
            get
            {
                return pointHintArguments[eqIndex + 1].FindDescendant<Expression>();
            }
        }

        private Expression XExpression
        {
            get
            {
                return pointHintArguments[cartesianIndex].FindDescendant<Expression>();
            }
        }

        private Expression YExpression
        {
            get
            {
                return pointHintArguments[cartesianIndex + 1].FindDescendant<Expression>();
            }
        }

        private Expression ZExpression
        {
            get
            {
                return pointHintArguments[cartesianIndex + 2].FindDescendant<Expression>();
            }
        }

        public string RA
        {
            get
            {
                if (isEqSpecified)
                {
                    return SqlServerCodeGenerator.GetCode(RAExpression, true);
                }
                else if (isCartesianSpecified)
                {
                    return String.Format(
                        "{0}.CartesianToEqRa(({1}),({2}),({3}))",
                        codeDatasetFunctionPrefix,
                        X, Y, Z);
                }
                else
                {
                    // TODO: Figure out from metadata
                    throw new NotImplementedException();
                }
            }
        }

        public string Dec
        {
            get
            {
                if (isEqSpecified)
                {
                    return SqlServerCodeGenerator.GetCode(DecExpression, true);
                }
                else if (isCartesianSpecified)
                {
                    return String.Format(
                        "{0}.CartesianToEqDec(({1}),({2}),({3}))",
                        codeDatasetFunctionPrefix,
                        X, Y, Z);
                }
                else
                {
                    // TODO: Figure out from metadata
                    throw new NotImplementedException();
                }
            }
        }

        public string X
        {
            get
            {
                if (isCartesianSpecified)
                {
                    return SqlServerCodeGenerator.GetCode(XExpression, true);
                }
                else if (isEqSpecified)
                {
                    return String.Format("{0}.EqToCartesianX(({1}),({2}))",
                        codeDatasetFunctionPrefix,
                        RA, Dec);
                }
                else
                {
                    // TODO: Figure out from metadata
                    throw new NotImplementedException();
                }
            }
        }

        public string Y
        {
            get
            {
                if (isCartesianSpecified)
                {
                    return SqlServerCodeGenerator.GetCode(YExpression, true);
                }
                else if (isEqSpecified)
                {
                    return String.Format("{0}.EqToCartesianY(({1}),({2}))",
                        codeDatasetFunctionPrefix,
                        RA, Dec);
                }
                else
                {
                    // TODO: Figure out from metadata
                    throw new NotImplementedException();
                }
            }
        }

        public string Z
        {
            get
            {
                if (isCartesianSpecified)
                {
                    return SqlServerCodeGenerator.GetCode(ZExpression, true);
                }
                else if (isEqSpecified)
                {
                    return String.Format("{0}.EqToCartesianZ(({1}),({2}))",
                        codeDatasetFunctionPrefix,
                        RA, Dec);
                }
                else
                {
                    // TODO: Figure out from metadata
                    throw new NotImplementedException();
                }
            }
        }

        public bool IsHtmIdSpecified
        {
            get { return isHtmIdSpecified; }
        }

        private Expression HtmIdExpression
        {
            get
            {
                return htmIdHintArguments[0].FindDescendant<Expression>();
            }
        }

        public string HtmId
        {
            get
            {
                if (isHtmIdSpecified)
                {
                    return SqlServerCodeGenerator.GetCode(HtmIdExpression, true);
                }
                else
                {
                    // TODO: Figure out from metadata
                    throw new NotImplementedException();
                }
            }
        }

        public bool IsZoneIdSpecified
        {
            get { return isZoneIdSpecified; }
        }

        private Expression ZoneIdExpression
        {
            get
            {
                return zoneIdHintArguments[0].FindDescendant<Expression>();
            }
        }

        public string ZoneId
        {
            get
            {
                if (isZoneIdSpecified)
                {
                    return SqlServerCodeGenerator.GetCode(ZoneIdExpression, true);
                }
                else
                {
                    // TODO: Figure out from metadata
                    throw new NotImplementedException();
                }
            }
        }

        public bool IsErrorSpecified
        {
            get { return isErrorSpecified; }
        }

        public bool IsConstantError
        {
            get
            {
                return errorHintArguments.Length == 1 && errorHintArguments[0].FindDescendant<Expression>().IsConstantNumber;
            }
        }


        public bool IsErrorLimitsSpecified
        {
            get { return isErrorLimitsSpecified; }
        }

        public Expression ErrorExpression
        {
            get
            {
                return errorHintArguments[0].FindDescendant<Expression>();
            }
        }

        public Expression ErrorMinExpression
        {
            get
            {
                return errorHintArguments[1].FindDescendant<Expression>();
            }
        }

        public Expression ErrorMaxExpression
        {
            get
            {
                return errorHintArguments[2].FindDescendant<Expression>();
            }
        }

        public string Error
        {
            get
            {
                if (isErrorSpecified)
                {
                    return SqlServerCodeGenerator.GetCode(ErrorExpression, true);
                }
                else
                {
                    // TODO: Figure out from metadata
                    throw new NotImplementedException();
                }
            }
        }

        public string ErrorMin
        {
            get
            {
                if (isErrorLimitsSpecified)
                {
                    return SqlServerCodeGenerator.GetCode(ErrorMinExpression, true);
                }
                else
                {
                    // TODO: Figure out from metadata
                    throw new NotImplementedException();
                }
            }
        }

        public string ErrorMax
        {
            get
            {
                if (isErrorLimitsSpecified)
                {
                    return SqlServerCodeGenerator.GetCode(ErrorMaxExpression, true);
                }
                else
                {
                    // TODO: Figure out from metadata
                    throw new NotImplementedException();
                }
            }
        }

        #endregion
        #region Constructors and initializers

        public TableCoordinates(SimpleTableSource table)
        {
            InitializeMembers();

            this.codeDatasetFunctionPrefix = String.Empty;

            this.table = table;
            InterpretTableHints();
        }

        public TableCoordinates(SimpleTableSource table, SqlServerDataset codeDataset)
        {
            InitializeMembers();

            // TODO: move skyquery code to its own schema + add versioning!
            this.codeDatasetFunctionPrefix = String.Format("[{0}].[{1}]", codeDataset.DatabaseName, codeDataset.DefaultSchemaName);

            this.table = table;
            InterpretTableHints();
        }

        private void InitializeMembers()
        {
            this.table = null;

            this.pointHint = null;
            this.pointHintArguments = null;
            this.htmIdHint = null;
            this.htmIdHintArguments = null;
            this.zoneIdHint = null;
            this.zoneIdHintArguments = null;
            this.errorHint = null;
            this.errorHintArguments = null;

            this.isEqSpecified = false;
            this.isCartesianSpecified = false;
            this.isHtmIdSpecified = false;
            this.isZoneIdSpecified = false;
            this.isErrorSpecified = false;
            this.isErrorLimitsSpecified = false;

            this.eqIndex = -1;
            this.cartesianIndex = -1;

            this.codeDatasetFunctionPrefix = Constants.DefaultCodeDatasetFunctionPrefix;
        }

        #endregion

        private void InterpretTableHints()
        {
            var hints = table.FindDescendant<TableHintClause>();
            var hintlist = hints.FindDescendant<TableHintList>();

            foreach (var hint in hintlist.EnumerateDescendantsRecursive<TableHint>())
            {
                switch (hint.Identifier.Value.ToUpperInvariant())
                {
                    case Constants.PointHintIdentifier:
                        InterpretPointHint(hint);
                        break;
                    case Constants.HtmIdHintIdentifier:
                        InterpretHtmIdHint(hint);
                        break;
                    case Constants.ZoneIdHintIdentifier:
                        InterpretZoneIdHint(hint);
                        break;
                    case Constants.ErrorHintIdentifier:
                        InterpretErrorHint(hint);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private void InterpretPointHint(TableHint hint)
        {
            pointHint = hint;
            pointHintArguments = GetHintArguments(hint);

            if (pointHintArguments.Length == 2)
            {
                isEqSpecified = true;
                isCartesianSpecified = false;
                eqIndex = 0;
            }
            else if (pointHintArguments.Length == 3)
            {
                isEqSpecified = false;
                isCartesianSpecified = true;
                cartesianIndex = 0;
            }
            else if (pointHintArguments.Length == 5)
            {
                isEqSpecified = true;
                isCartesianSpecified = true;
                cartesianIndex = 2;
            }
            else
            {
                throw CreateException(ExceptionMessages.InvalidPointFormat);
            }
        }

        private void InterpretHtmIdHint(TableHint hint)
        {
            htmIdHint = hint;
            htmIdHintArguments = GetHintArguments(hint);

            if (htmIdHintArguments.Length != 1)
            {
                throw CreateException(ExceptionMessages.InvalidHtmIdFormat);
            }

            isHtmIdSpecified = true;
        }

        private void InterpretZoneIdHint(TableHint hint)
        {
            zoneIdHint = hint;
            zoneIdHintArguments = GetHintArguments(hint);

            if (zoneIdHintArguments.Length != 1)
            {
                throw CreateException(ExceptionMessages.InvalidZoneIdFormat);
            }

            isZoneIdSpecified = true;
        }

        private void InterpretErrorHint(TableHint hint)
        {
            errorHint = hint;
            errorHintArguments = GetHintArguments(hint);

            if (errorHintArguments.Length == 1)
            {
                isErrorSpecified = true;
                isErrorLimitsSpecified = false;
            }
            else if (errorHintArguments.Length == 3)
            {
                isErrorSpecified = true;
                isErrorLimitsSpecified = true;
            }
            else
            {
                throw CreateException(ExceptionMessages.InvalidErrorFormat);
            }
        }

        private Argument[] GetHintArguments(TableHint hint)
        {
            return hint.FindDescendant<FunctionArguments>()
                       .FindDescendant<ArgumentList>()
                       .EnumerateDescendants<Argument>()
                       .ToArray();
        }

        private ValidatorException CreateException(string message)
        {
            return new ValidatorException(message);
        }
    }
}
