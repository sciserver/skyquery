using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Jhu.Graywulf.ParserLib;

namespace Jhu.SkyQuery.Parser.Generator
{
    [Grammar(Namespace = "Jhu.SkyQuery.Parser", ParserName = "SkyQueryParser",
        Comparer = "StringComparer.InvariantCultureIgnoreCase", RootToken = "Jhu.Graywulf.SqlParser.StatementBlock")]
    class SkyQueryGrammar : Jhu.Graywulf.SqlParser.Generator.SqlGrammar
    {
        // This doesn't change the original rule but creates an overload in the
        // SkyQuery parser that further can be extended
        public static new Expression<Rule> SimpleTableSource = () => Inherit();

        #region Region grammar

        public static Expression<Rule> RegionSelectStatement = () => 
            Override
            (
                SelectStatement,
                Sequence
                (
                    May(CommonTableExpression),
                    May(CommentOrWhitespace),
                    RegionQueryExpression,
                    May(Sequence(May(CommentOrWhitespace), OrderByClause)),
                    May(Sequence(May(CommentOrWhitespace), QueryHintClause))
                )
            );

        public static Expression<Rule> RegionQueryExpression = () =>
            Override
            (
                QueryExpression,
                Sequence
                (
                    Must
                    (
                        RegionQueryExpressionBrackets,
                        RegionQuerySpecification
                    ),
                    May(Sequence(May(CommentOrWhitespace), QueryOperator, May(CommentOrWhitespace), RegionQueryExpression))
                )
            );

        public static Expression<Rule> RegionQueryExpressionBrackets = () =>
            Override
            (
                QueryExpressionBrackets,
                QuerySpecification
            );

        public static new Expression<Rule> RegionQuerySpecification = () =>
            Override
            (
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
                )
            );

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
            Must(Keyword("UNION"), Keyword("INTERSECT"), Keyword("EXCEPT"));

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
        #region XMatch grammar

        public static Expression<Rule> XMatchSelectStatement = () =>
            Override
            (
                RegionSelectStatement,
                Sequence
                (
                    XMatchQueryExpression,
                    May(Sequence(May(CommentOrWhitespace), OrderByClause)),
                    May(Sequence(May(CommentOrWhitespace), QueryHintClause))
                )
            );

        public static Expression<Rule> XMatchQueryExpression = () =>
            Override
            (
                QueryExpression,
                XMatchQuerySpecification
            );

        public static Expression<Rule> XMatchQuerySpecification = () =>
            Override
            (
                QuerySpecification,
                Sequence
                (
                    Keyword("SELECT"),
                    May(CommentOrWhitespace),
                    SelectList,
                    May(Sequence(May(CommentOrWhitespace), IntoClause)),
                    May(Sequence(May(CommentOrWhitespace), XMatchFromClause)),
                    May(Sequence(May(CommentOrWhitespace), RegionClause)),
                    May(Sequence(May(CommentOrWhitespace), WhereClause))
                )
            );

        public static Expression<Rule> XMatchFromClause = () =>
            Override
            (
                FromClause,
                Sequence
                (
                    May(CommentOrWhitespace),
                    XMatchTableSourceExpression
                )
            );

        public static Expression<Rule> XMatchTableSourceExpression = () =>
            Override
            (
                TableSourceExpression,
                Sequence
                (
                    XMatchTableSource,
                    May(Sequence(May(CommentOrWhitespace), JoinedTable))
                )
            );

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
    }
}
