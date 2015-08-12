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

        #endregion
        #region Properties

        public SimpleTableSource Table
        {
            get { return table; }
        }

        /// <summary>
        /// Placeholder for opting out from region constraint
        /// </summary>
        public bool IsNoRegion
        {
            get { return false; }
        }

        public bool IsEqSpecified
        {
            get { return isEqSpecified; }
        }

        public bool IsCartesianSpecified
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

        private ColumnReference HtmIdColumnReference
        {
            get
            {
                return htmIdHintArguments[0].FindDescendant<Expression>().FindDescendant<AnyVariable>().FindDescendant<ColumnIdentifier>().ColumnReference;
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

        private ColumnReference ZoneIdColumnReference
        {
            get
            {
                return zoneIdHintArguments[0].FindDescendant<Expression>().FindDescendant<AnyVariable>().FindDescendant<ColumnIdentifier>().ColumnReference;
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
        }

        #endregion
        #region Hint interpretation functions

        private void InterpretTableHints()
        {
            var hints = table.FindDescendant<TableHintClause>();

            if (hints != null)
            {
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
                eqIndex = 0;
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

            if (!HtmIdExpression.IsSingleColumn)
            {
                throw CreateException(ExceptionMessages.HtmIdIsNotSingleColumn);
            }
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

            if (!ZoneIdExpression.IsSingleColumn)
            {
                throw CreateException(ExceptionMessages.ZoneIdIsNotSingleColumn);
            }
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

        #endregion
        #region IndexSelectorFunctions

        /// <summary>
        /// Attempt to find an index that has an HTM ID in is as the first key column
        /// </summary>
        /// <returns></returns>
        public Index FindHtmIndex()
        {
            return FindIndexWithFirstKey(HtmIdColumnReference.ColumnName);
        }

        public Index FindZoneIndex()
        {
            return FindIndexWithFirstKey(ZoneIdColumnReference.ColumnName);
        }

        private Index FindIndexWithFirstKey(string columnName)
        {
            if (table.TableReference.DatabaseObject == null)
            {
                throw new InvalidOperationException(ExceptionMessages.QueryNamesNotResolved);
            }

            var t = (TableOrView)table.TableReference.DatabaseObject;

            foreach (var idx in t.Indexes.Values)
            {
                // TODO: modify this once columns are also stored by ordinal index and not just by name
                var col = idx.Columns.Values.Where(c => !c.IsIncluded).OrderBy(c => c.KeyOrdinal).FirstOrDefault();

                if (SqlServerSchemaManager.Comparer.Compare(columnName, col.Name) == 0)
                {
                    return idx;
                }
            }

            return null;
        }

        #endregion
        #region Coordinate string accessor functions

        private string GetCodeDatasetPrefix(SqlServerDataset codeDataset)
        {
            return String.Format("[{0}].[{1}]", codeDataset.DatabaseName, codeDataset.DefaultSchemaName);
        }

        public string GetRAString(SqlServerDataset codeDataset)
        {
            if (isEqSpecified)
            {
                return SqlServerCodeGenerator.GetCode(RAExpression, true);
            }
            else if (isCartesianSpecified)
            {
                return String.Format(
                    "{0}.CartesianToEqRa(({1}),({2}),({3}))",
                    GetCodeDatasetPrefix(codeDataset),
                    GetXString(codeDataset),
                    GetYString(codeDataset),
                    GetZString(codeDataset));
            }
            else
            {
                // TODO: Figure out from metadata
                throw new NotImplementedException();
            }
        }

        public string GetDecString(SqlServerDataset codeDataset)
        {
            if (isEqSpecified)
            {
                return SqlServerCodeGenerator.GetCode(DecExpression, true);
            }
            else if (isCartesianSpecified)
            {
                return String.Format(
                    "{0}.CartesianToEqDec(({1}),({2}),({3}))",
                    GetCodeDatasetPrefix(codeDataset),
                    GetXString(codeDataset),
                    GetYString(codeDataset),
                    GetZString(codeDataset));
            }
            else
            {
                // TODO: Figure out from metadata
                throw new NotImplementedException();
            }
        }

        public string GetXString(SqlServerDataset codeDataset)
        {
            if (isCartesianSpecified)
            {
                return SqlServerCodeGenerator.GetCode(XExpression, true);
            }
            else if (isEqSpecified)
            {
                return String.Format("{0}.EqToCartesianX(({1}),({2}))",
                    GetCodeDatasetPrefix(codeDataset),
                    GetRAString(codeDataset),
                    GetDecString(codeDataset));
            }
            else
            {
                // TODO: Figure out from metadata
                throw new NotImplementedException();
            }
        }

        public string GetYString(SqlServerDataset codeDataset)
        {
            if (isCartesianSpecified)
            {
                return SqlServerCodeGenerator.GetCode(YExpression, true);
            }
            else if (isEqSpecified)
            {
                return String.Format("{0}.EqToCartesianY(({1}),({2}))",
                    GetCodeDatasetPrefix(codeDataset),
                    GetRAString(codeDataset),
                    GetDecString(codeDataset));
            }
            else
            {
                // TODO: Figure out from metadata
                throw new NotImplementedException();
            }
        }

        public string GetZString(SqlServerDataset codeDataset)
        {
            if (isCartesianSpecified)
            {
                return SqlServerCodeGenerator.GetCode(ZExpression, true);
            }
            else if (isEqSpecified)
            {
                return String.Format("{0}.EqToCartesianZ(({1}),({2}))",
                    GetCodeDatasetPrefix(codeDataset),
                    GetRAString(codeDataset),
                    GetDecString(codeDataset));
            }
            else
            {
                // TODO: Figure out from metadata
                throw new NotImplementedException();
            }
        }

        public string GetHtmIdString()
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

        public string GetZoneIdString(SqlServerDataset codeDataset)
        {
                if (isZoneIdSpecified)
                {
                    return SqlServerCodeGenerator.GetCode(ZoneIdExpression, true);
                }
                else if (isEqSpecified || isCartesianSpecified)
                {
                    return String.Format(System.Globalization.CultureInfo.InvariantCulture,
                        "CONVERT(INT,FLOOR(({0} + 90.0) / @H))",
                        GetDecString(codeDataset));
                }
                else
                {
                    // TODO: Figure out from metadata
                    throw new NotImplementedException();
                }
        }

        #endregion

        private ValidatorException CreateException(string message)
        {
            return new ValidatorException(message);
        }
    }
}
