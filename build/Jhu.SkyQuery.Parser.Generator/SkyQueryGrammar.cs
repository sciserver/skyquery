using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Jhu.Graywulf.ParserLib;

namespace Jhu.SkyQuery.Parser.Generator
{
    [Grammar(Namespace = "Jhu.SkyQuery.Parser", ParserName = "SkyQueryParser",
        Comparer="StringComparer.InvariantCultureIgnoreCase", RootToken = "SelectStatement")]
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
                May(Sequence(CommentOrWhitespace, XMatchClause)),
                May(Sequence(CommentOrWhitespace, WhereClause)),
                May(Sequence(CommentOrWhitespace, GroupByClause)),
                May(Sequence(CommentOrWhitespace, HavingClause)),
                May(Sequence(CommentOrWhitespace, RegionClause))
            );

        #region Table hint extensions to support POINT and ERROR

        public static new Expression<Rule> TableSource = () =>
            Must
            (
                FunctionTableSource,
                XMatchTableSource,
                SimpleTableSource,
                VariableTableSource,
                SubqueryTableSource
            );

        public static Expression<Rule> XMatchTableSource = () =>
            Sequence
            (
                TableOrViewName,
                May(Sequence(CommentOrWhitespace, May(Sequence(Keyword("AS"), CommentOrWhitespace)), TableAlias)),   // Optional
                Sequence(CommentOrWhitespace, XMatchHintClause)     // Required
            );

        public static Expression<Rule> XMatchHintClause = () =>
            Sequence
            (
                Keyword("WITH"),
                May(CommentOrWhitespace), BracketOpen, May(CommentOrWhitespace),
                XMatchTableHintList,
                May(CommentOrWhitespace), BracketClose
            );

        public static new Expression<Rule> XMatchTableHintList = () =>
            Sequence
            (
                Must(
                    XMatchPoint,
                    XMatchError
                ),
                May(Sequence(May(CommentOrWhitespace), Comma, May(CommentOrWhitespace), XMatchTableHintList))
            );

        public static Expression<Rule> XMatchPoint = () =>
            Sequence
            (
                Keyword("POINT"),
                FunctionArguments
            );

        public static Expression<Rule> XMatchError = () =>
            Sequence
            (
                Keyword("ERROR"),
                FunctionArguments
            );

        #endregion
        #region XMatch clause

        public static Expression<Rule> XMatchClause = () =>
            Sequence
            (
                Keyword("XMATCH"),
                CommentOrWhitespace, XMatchAlgorithm,
                May(Sequence(CommentOrWhitespace, Keyword("AS"))),
                CommentOrWhitespace, TableAlias,
                CommentOrWhitespace, XMatchTableList,
                CommentOrWhitespace, XMatchHavingClause
            );

        public static Expression<Rule> XMatchAlgorithm = () =>
            Must(Keyword("BAYESFACTOR"), Keyword("DISTANCE"));

        public static Expression<Rule> XMatchTableList = () =>
            Sequence
            (
                XMatchTableSpecification,
                May(Sequence(CommentOrWhitespace, XMatchTableList))
            );

        public static Expression<Rule> XMatchTableSpecification = () =>
            Sequence
            (
                XMatchTableInclusion,
                CommentOrWhitespace, TableOrViewName
            );

        public static Expression<Rule> XMatchTableInclusion = () =>
            Sequence
            (
                May(Sequence(Must(Keyword("MUST"), Keyword("MAY"), Keyword("NOT")), CommentOrWhitespace)),
                Keyword("EXIST")
            );

        public static Expression<Rule> XMatchHavingClause = () =>
            Sequence
            (
                Keyword("HAVING"), CommentOrWhitespace, Keyword("LIMIT"), CommentOrWhitespace, Number
            );

        #endregion
        #region Region clause

        public static Expression<Rule> RegionClause = () =>
            Sequence
            (
                Keyword("REGION"),
                FunctionArguments
            );

        #endregion
    }
}
