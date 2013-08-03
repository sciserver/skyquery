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
                May(Sequence(CommentOrWhitespace, HavingClause))
            );

        

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
                CommentOrWhitespace, TableOrViewName,
                CommentOrWhitespace, XMatchArgumentList
            );

        public static Expression<Rule> XMatchTableInclusion = () =>
            Sequence
            (
                May(Sequence(Must(Keyword("Must"), Keyword("MAY"), Keyword("NOT")), CommentOrWhitespace)),
                Keyword("EXIST")
            );

        public static Expression<Rule> XMatchArgumentList = () =>
            Sequence
            (
                Keyword("ON"),
                CommentOrWhitespace, AdqlPoint,
                May(Sequence(May(CommentOrWhitespace), Comma, May(CommentOrWhitespace), ArgumentList))
            );

        public static Expression<Rule> XMatchHavingClause = () =>
            Sequence
            (
                Keyword("HAVING"), CommentOrWhitespace, Keyword("LIMIT"), CommentOrWhitespace, Number
            );

        public static Expression<Rule> AdqlPoint = () => FunctionCall;
    }
}
