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
        private Expression[] pointHintArguments;
        private TableHint htmIdHint;
        private Expression[] htmIdHintArguments;
        private TableHint zoneIdHint;
        private Expression[] zoneIdHintArguments;
        private TableHint errorHint;
        private Expression[] errorHintArguments;

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
            get { return !isCartesianSpecified && !isEqSpecified && !isHtmIdSpecified; }
        }

        public bool IsEqSpecified
        {
            get { return isEqSpecified; }
        }

        public bool IsCartesianSpecified
        {
            get { return isCartesianSpecified; }
        }

        public Expression RAExpression
        {
            get
            {
                return pointHintArguments[eqIndex];
            }
        }

        public Expression DecExpression
        {
            get
            {
                return pointHintArguments[eqIndex + 1];
            }
        }

        public Expression XExpression
        {
            get
            {
                return pointHintArguments[cartesianIndex];
            }
        }

        public Expression YExpression
        {
            get
            {
                return pointHintArguments[cartesianIndex + 1];
            }
        }

        public Expression ZExpression
        {
            get
            {
                return pointHintArguments[cartesianIndex + 2];
            }
        }

        public bool IsHtmIdSpecified
        {
            get { return isHtmIdSpecified; }
        }

        public Expression HtmIdExpression
        {
            get
            {
                return htmIdHintArguments[0];
            }
        }

        public ColumnReference HtmIdColumnReference
        {
            get
            {
                return HtmIdExpression.FindDescendant<AnyVariable>().FindDescendant<ColumnIdentifier>().ColumnReference;
            }
        }

        public bool IsZoneIdSpecified
        {
            get { return isZoneIdSpecified; }
        }

        public Expression ZoneIdExpression
        {
            get
            {
                return zoneIdHintArguments[0];
            }
        }

        public ColumnReference ZoneIdColumnReference
        {
            get
            {
                return zoneIdHintArguments[0].FindDescendant<AnyVariable>().FindDescendant<ColumnIdentifier>().ColumnReference;
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
                return errorHintArguments[0];
            }
        }

        public Expression ErrorMinExpression
        {
            get
            {
                return errorHintArguments[1];
            }
        }

        public Expression ErrorMaxExpression
        {
            get
            {
                return errorHintArguments[2];
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
                            // Nothing to do with unknown hints
                            break;
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

            if (!htmIdHintArguments[0].IsSingleColumn)
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

        private Expression[] GetHintArguments(TableHint hint)
        {
            return hint.FindDescendant<FunctionArguments>()
                       .FindDescendant<ArgumentList>()
                       .EnumerateDescendants<Argument>()
                       .Select(a => a.Expression)
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

        private ValidatorException CreateException(string message)
        {
            return new ValidatorException(message);
        }
    }
}
