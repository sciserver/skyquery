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
        #region Constants

        public const string HtmIdColumnName = "htmid";
        public const string ZoneIdColumnName = "zoneid";
        public const string RaColumnName = "ra";
        public const string DecColumnName = "dec";
        public const string CxColumnName = "cx";
        public const string CyColumnName = "cy";
        public const string CzColumnName = "cz";

        #endregion
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
            get
            {
                return !isCartesianSpecified && !isEqSpecified && !isHtmIdSpecified &&
                  !IsEqColumnsAvailable && !IsCartesianColumnsAvailable;
            }
        }

        #endregion
        #region Coordinate column accessors

        public bool IsEqColumnsAvailable
        {
            get
            {
                return
                    table.TableReference != null &&
                    table.TableReference.TableOrView != null &&
                    table.TableReference.TableOrView.Columns.ContainsKey(RaColumnName) &&
                    table.TableReference.TableOrView.Columns.ContainsKey(DecColumnName);
            }
        }

        public bool IsCartesianColumnsAvailable
        {
            get
            {
                return
                    table.TableReference != null &&
                    table.TableReference.TableOrView != null &&
                    table.TableReference.TableOrView.Columns.ContainsKey(CxColumnName) &&
                    table.TableReference.TableOrView.Columns.ContainsKey(CyColumnName) &&
                    table.TableReference.TableOrView.Columns.ContainsKey(CzColumnName);
            }
        }

        public Expression RAColumnExpression
        {
            get
            {
                return CreateColumnExpression(RaColumnName);
            }
        }

        public Expression DecColumnExpression
        {
            get
            {
                return CreateColumnExpression(DecColumnName);
            }
        }

        public Expression XColumnExpression
        {
            get
            {
                return CreateColumnExpression(CxColumnName);
            }
        }

        public Expression YColumnExpression
        {
            get
            {
                return CreateColumnExpression(CyColumnName);
            }
        }

        public Expression ZColumnExpression
        {
            get
            {
                return CreateColumnExpression(CzColumnName);
            }
        }

        #endregion
        #region Coordinate hint accessors

        public bool IsEqHintSpecified
        {
            get { return isEqSpecified; }
        }

        public Expression RAHintExpression
        {
            get
            {
                return pointHintArguments[eqIndex];
            }
        }

        public Expression DecHintExpression
        {
            get
            {
                return pointHintArguments[eqIndex + 1];
            }
        }

        public bool IsCartesianHintSpecified
        {
            get { return isCartesianSpecified; }
        }

        public Expression XHintExpression
        {
            get
            {
                return pointHintArguments[cartesianIndex];
            }
        }

        public Expression YHintExpression
        {
            get
            {
                return pointHintArguments[cartesianIndex + 1];
            }
        }

        public Expression ZHintExpression
        {
            get
            {
                return pointHintArguments[cartesianIndex + 2];
            }
        }

        #endregion
        #region HtmID column and hint accessors

        public bool IsHtmIdColumnAvailable
        {
            get
            {
                return
                    table.TableReference != null &&
                    table.TableReference.TableOrView != null &&
                    table.TableReference.TableOrView.Columns.ContainsKey(HtmIdColumnName);
            }
        }

        public Expression HtmIdColumnExpression
        {
            get
            {
                return CreateColumnExpression(HtmIdColumnName);
            }
        }

        public bool IsHtmIdHintSpecified
        {
            get { return isHtmIdSpecified; }
        }

        public Expression HtmIdHintExpression
        {
            get
            {
                return htmIdHintArguments[0];
            }
        }
        
        #endregion
        #region ZoneID column and hint accessors

        public bool IsZoneIdColumnAvailable
        {
            get
            {
                return
                    table.TableReference != null &&
                    table.TableReference.TableOrView != null &&
                    table.TableReference.TableOrView.Columns.ContainsKey(ZoneIdColumnName);
            }
        }

        public Expression ZoneIdColumnExpression
        {
            get
            {
                return CreateColumnExpression(ZoneIdColumnName);
            }
        }

        public bool IsZoneIdHintSpecified
        {
            get { return isZoneIdSpecified; }
        }

        public Expression ZoneIdHintExpression
        {
            get
            {
                return zoneIdHintArguments[0];
            }
        }
        
        #endregion
        #region Coordinate error hint accessors

        public bool IsErrorHintSpecified
        {
            get { return isErrorSpecified; }
        }

        public bool IsConstantError
        {
            get
            {
                return !IsErrorHintSpecified || errorHintArguments.Length == 1 && errorHintArguments[0].IsConstantNumber;
            }
        }


        public bool IsErrorLimitsHintSpecified
        {
            get { return isErrorLimitsSpecified; }
        }

        public Expression ErrorHintExpression
        {
            get
            {
                return errorHintArguments[0];
            }
        }

        public Expression ErrorHintMinExpression
        {
            get
            {
                return errorHintArguments[1];
            }
        }

        public Expression ErrorHintMaxExpression
        {
            get
            {
                return errorHintArguments[2];
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

            if (!ZoneIdHintExpression.IsSingleColumn)
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
        
        private Expression CreateColumnExpression(string column)
        {
            var cr = new ColumnReference(table.TableReference, table.TableReference.TableOrView.Columns[column]);
            return Expression.Create(cr);
        }

        private ValidatorException CreateException(string message)
        {
            return new ValidatorException(message);
        }
    }
}
