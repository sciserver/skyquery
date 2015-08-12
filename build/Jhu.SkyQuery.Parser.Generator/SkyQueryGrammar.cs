using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Jhu.Graywulf.ParserLib;

namespace Jhu.SkyQuery.Parser.Generator
{
    [Grammar(Namespace = "Jhu.SkyQuery.Parser", ParserName = "SkyQueryParser",
        Comparer = "StringComparer.InvariantCultureIgnoreCase", RootToken = "SelectStatement")]
    class SkyQueryGrammar : Jhu.Graywulf.SqlParser.Generator.SqlGrammar
    {
        public static new Expression<Rule> QuerySpecification = () =>
            Sequence
            (
                Keyword("SELECT"),
                May(Sequence(CommentOrWhitespace, Must(Keyword("ALL"), CommentOrWhitespace, Keyword("DISTINCT")))),
                May(Sequence(CommentOrWhitespace, TopExpression)),
                CommentOrWhitespace, SelectList,
                May(Sequence(CommentOrWhitespace, IntoClause)),
                May(Sequence(CommentOrWhitespace, FromClause)),
                May(Sequence(CommentOrWhitespace, RegionClause)),       //
                May(Sequence(CommentOrWhitespace, WhereClause)),
                May(Sequence(CommentOrWhitespace, GroupByClause)),
                May(Sequence(CommentOrWhitespace, HavingClause))
            );

        public static new Expression<Rule> TableSource = () =>
            Must
            (
                FunctionTableSource,
                XMatchTableSource,                                      //
                SimpleTableSource,
                VariableTableSource,
                SubqueryTableSource
            );

        // This doesn't change the original rule but creates an overload in the
        // SkyQuery parser that further can be extended
        public static Expression<Rule> SimpleTableSource = () =>
            Sequence
            (
                TableOrViewName,
                May(Sequence(CommentOrWhitespace, May(Sequence(Keyword("AS"), CommentOrWhitespace)), TableAlias)),   // Optional
                May(Sequence(CommentOrWhitespace, TableSampleClause)),
                May(Sequence(CommentOrWhitespace, TableHintClause)),
                May(Sequence(CommentOrWhitespace, TablePartitionClause))
            );

        #region XMatch clause

        public static Expression<Rule> XMatchTableSource = () =>
            Sequence
            (
                Keyword("XMATCH"),
                May(CommentOrWhitespace), BracketOpen,
                May(CommentOrWhitespace), XMatchTableList,
                May(CommentOrWhitespace), Comma,
                May(CommentOrWhitespace), XMatchConstraint,
                May(CommentOrWhitespace), BracketClose,
                May(CommentOrWhitespace),
                May(Sequence(Keyword("AS"), CommentOrWhitespace)),
                TableAlias
            );

        public static Expression<Rule> XMatchTableList = () =>
            Sequence
            (
                XMatchTableSpecification,
                May(Sequence
                    (
                        May(CommentOrWhitespace),
                        Comma,
                        May(CommentOrWhitespace),
                        XMatchTableList)
                    )
            );

        public static Expression<Rule> XMatchTableSpecification = () =>
            Sequence
            (
                XMatchTableInclusion,
                CommentOrWhitespace, 
                SimpleTableSource
            );

        public static Expression<Rule> XMatchTableInclusion = () =>
            Sequence
            (
                May(Sequence(Must(Keyword("MUST"), Keyword("MAY"), Keyword("NOT")), CommentOrWhitespace)),
                Keyword("EXIST"),
                CommentOrWhitespace, Keyword("IN")
            );

        public static Expression<Rule> XMatchConstraint = () =>
            Sequence
            (
                Keyword("LIMIT"),
                CommentOrWhitespace, XMatchAlgorithm,
                CommentOrWhitespace, Keyword("TO"),
                CommentOrWhitespace, Number
            );

        public static Expression<Rule> XMatchAlgorithm = () =>
            Must(Keyword("BAYESFACTOR"), Keyword("DISTANCE"));

        #endregion
        #region Region grammar

        public static Expression<Rule> RegionClause = () =>
            Sequence
            (
                Keyword("REGION"),
                CommentOrWhitespace,
                Must(StringConstant, RegionExpression)
            );

        public static Expression<Rule> RegionExpression = () =>
            Sequence
            (
                // May(RegionInvese) -- TODO: add this if inverse is ever implemented
                // May(CommentOrWhitespace),
                Must(RegionShape, RegionExpressionBrackets),
                May(Sequence(May(CommentOrWhitespace), RegionOperator, May(CommentOrWhitespace), RegionExpression))
            );

        public static Expression<Rule> RegionExpressionBrackets = () =>
            Sequence
            (
                BracketOpen,
                May(CommentOrWhitespace),
                RegionExpression,
                May(CommentOrWhitespace),
                BracketClose
            );

        public static Expression<Rule> RegionOperator = () =>
            Must(Keyword("UNION"), Keyword("INTERSECT"), Keyword("DIFFERENCE"));

        public static Expression<Rule> RegionShape = () =>
            Sequence
            (
                RegionShapeType,
                BracketOpen,
                RegionArgumentList,
                May(CommentOrWhitespace),
                BracketClose
            );

        public static Expression<Rule> RegionShapeType = () =>
            Must
            (
                Keyword("CIRCLE"), Keyword("CIRC"),
                Keyword("RECTANGLE"), Keyword("RECT"),
                Keyword("POLYGON"), Keyword("POLY"),
                Keyword("CONVEX_HULL"), Keyword("CHULL")
            );

        public static Expression<Rule> RegionArgument = () =>
            Sequence
            (
                Number
            );

        public static Expression<Rule> RegionArgumentList = () =>
            Sequence
            (
                May(CommentOrWhitespace),
                RegionArgument,
                May(Sequence(May(CommentOrWhitespace), Comma, RegionArgumentList))
            );

        #endregion
    }
}
