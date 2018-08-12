using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Sql.Parsing;
using Jhu.Graywulf.Sql.NameResolution;
using Jhu.Graywulf.Sql.Validation;

namespace Jhu.SkyQuery.Sql.Parsing
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

        private Jhu.Graywulf.Sql.Parsing.TableHint pointHint;
        private Jhu.Graywulf.Sql.Parsing.Expression[] pointHintArguments;
        private Jhu.Graywulf.Sql.Parsing.TableHint htmIdHint;
        private Jhu.Graywulf.Sql.Parsing.Expression[] htmIdHintArguments;
        private Jhu.Graywulf.Sql.Parsing.TableHint zoneIdHint;
        private Jhu.Graywulf.Sql.Parsing.Expression[] zoneIdHintArguments;
        private Jhu.Graywulf.Sql.Parsing.TableHint errorHint;
        private Jhu.Graywulf.Sql.Parsing.Expression[] errorHintArguments;

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

        public Jhu.Graywulf.Sql.Parsing.Expression RAColumnExpression
        {
            get
            {
                return CreateColumnExpression(RaColumnName);
            }
        }

        public Jhu.Graywulf.Sql.Parsing.Expression DecColumnExpression
        {
            get
            {
                return CreateColumnExpression(DecColumnName);
            }
        }

        public Jhu.Graywulf.Sql.Parsing.Expression XColumnExpression
        {
            get
            {
                return CreateColumnExpression(CxColumnName);
            }
        }

        public Jhu.Graywulf.Sql.Parsing.Expression YColumnExpression
        {
            get
            {
                return CreateColumnExpression(CyColumnName);
            }
        }

        public Jhu.Graywulf.Sql.Parsing.Expression ZColumnExpression
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

        public Jhu.Graywulf.Sql.Parsing.Expression RAHintExpression
        {
            get
            {
                return pointHintArguments[eqIndex];
            }
        }

        public Jhu.Graywulf.Sql.Parsing.Expression DecHintExpression
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

        public Jhu.Graywulf.Sql.Parsing.Expression XHintExpression
        {
            get
            {
                return pointHintArguments[cartesianIndex];
            }
        }

        public Jhu.Graywulf.Sql.Parsing.Expression YHintExpression
        {
            get
            {
                return pointHintArguments[cartesianIndex + 1];
            }
        }

        public Jhu.Graywulf.Sql.Parsing.Expression ZHintExpression
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

        public Jhu.Graywulf.Sql.Parsing.Expression HtmIdColumnExpression
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

        public Jhu.Graywulf.Sql.Parsing.Expression HtmIdHintExpression
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

        public Jhu.Graywulf.Sql.Parsing.Expression ZoneIdColumnExpression
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

        public Jhu.Graywulf.Sql.Parsing.Expression ZoneIdHintExpression
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

        public Jhu.Graywulf.Sql.Parsing.Expression ErrorHintExpression
        {
            get
            {
                return errorHintArguments[0];
            }
        }

        public Jhu.Graywulf.Sql.Parsing.Expression ErrorHintMinExpression
        {
            get
            {
                return errorHintArguments[1];
            }
        }

        public Jhu.Graywulf.Sql.Parsing.Expression ErrorHintMaxExpression
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
                    switch (hint.HintName.Value.ToUpperInvariant())
                    {
                        case Constants.PointHintName:
                            InterpretPointHint(hint);
                            break;
                        case Constants.HtmIdHintName:
                            InterpretHtmIdHint(hint);
                            break;
                        case Constants.ZoneIdHintName:
                            InterpretZoneIdHint(hint);
                            break;
                        case Constants.ErrorHintName:
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
            pointHintArguments = hint.GetArguments();

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
            htmIdHintArguments = hint.GetArguments();

            if (htmIdHintArguments.Length != 1)
            {
                throw CreateException(ExceptionMessages.InvalidHtmIdFormat);
            }

            isHtmIdSpecified = true;
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
        }

        private void InterpretErrorHint(TableHint hint)
        {
            errorHint = hint;
            errorHintArguments = hint.GetArguments();

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

        #endregion

        private Jhu.Graywulf.Sql.Parsing.Expression CreateColumnExpression(string column)
        {
            throw new NotImplementedException();

            var c = table.TableReference.TableOrView.Columns[column];
            var tr = table.TableReference;
            var cr = new ColumnReference(c, tr, new DataTypeReference(c.DataType));

            // Review how many identifier parts are generated here
            return Expression.Create(cr, 3);
        }

        private ValidatorException CreateException(string message)
        {
            return new ValidatorException(message);
        }
    }
}
