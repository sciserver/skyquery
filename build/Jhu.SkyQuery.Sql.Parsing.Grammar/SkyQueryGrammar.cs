﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Jhu.Graywulf.Parsing;
using Jhu.Graywulf.Parsing.Generator;

namespace Jhu.SkyQuery.Sql.Parsing.Grammar
{
    [Grammar(
        Namespace = "Jhu.SkyQuery.Sql.Parsing",
        ParserName = "SkyQueryParser",
        Comparer = "StringComparer.InvariantCultureIgnoreCase",
        RootToken = "Jhu.SkyQuery.Sql.Parsing.StatementBlock")]
    public class SkyQueryGrammar : Jhu.Graywulf.Sql.Extensions.Grammar.GraywulfSqlGrammar
    {
        public static new Expression<Rule> AnyStatement = () =>
            Override
            (
                Must
                (
                    Label,
                    GotoStatement,
                    BeginEndStatement,
                    WhileStatement,
                    BreakStatement,
                    ContinueStatement,
                    ReturnStatement,
                    IfStatement,
                    TryCatchStatement,
                    ThrowStatement,

                    DeclareCursorStatement,
                    SetCursorStatement,
                    CursorOperationStatement,
                    FetchStatement,

                    DeclareVariableStatement,
                    SetVariableStatement,

                    DeclareTableStatement,

                    CreateTableStatement,
                    DropTableStatement,
                    TruncateTableStatement,

                    CreateIndexStatement,
                    DropIndexStatement,

                    XMatchSelectStatement,       //

                    SelectStatement,
                    InsertStatement,
                    UpdateStatement,
                    DeleteStatement
                )
            );

        public static Expression<Rule> CoordinatesTableSource = () => Inherit(SimpleTableSource);

        #region Region grammar

        // Any query construct can be extended with the REGION clause because we know
        // how to rewrite just any select into an HTM-based query

        public static new Expression<Rule> QueryExpression = () =>
            Override
            (
                Sequence
                (
                    Must
                    (
                        QueryExpressionBrackets,
                        Must
                        (
                            RegionQuerySpecification,
                            QuerySpecification
                        )
                    ),
                    May(Sequence(May(CommentOrWhitespace), QueryOperator, May(CommentOrWhitespace), QueryExpression))
                )
            );

        public static Expression<Rule> RegionQuerySpecification = () =>
            Inherit
            (
                QuerySpecification,
                Sequence
                (
                    Keyword("SELECT"),
                    May(Sequence(CommentOrWhitespace, Must(Keyword("ALL"), Keyword("DISTINCT")))),
                    May(Sequence(CommentOrWhitespace, TopExpression)),
                    May(CommentOrWhitespace),
                    SelectList,
                    May(Sequence(May(CommentOrWhitespace), IntoClause)),
                    May(Sequence(May(CommentOrWhitespace), Must(PartitionedFromClause, FromClause))),
                    May(Sequence(May(CommentOrWhitespace), WhereClause)),
                    May(Sequence(May(CommentOrWhitespace), RegionClause)),       //
                    May(Sequence(May(CommentOrWhitespace), GroupByClause)),
                    May(Sequence(May(CommentOrWhitespace), HavingClause))
                )
            );

        public static Expression<Rule> RegionClause = () =>
            Sequence
            (
                Keyword("REGION", true),
                May(CommentOrWhitespace),
                Must(StringConstant, RegionExpression)
            );

        public static Expression<Rule> RegionExpression = () =>
            Sequence
            (
                May(Sequence(RegionNotOperator, May(CommentOrWhitespace))),
                Must
                (
                    RegionShape,
                    RegionExpressionBrackets
                ),
                May
                (
                    Sequence
                    (
                        May(CommentOrWhitespace),
                        RegionOperator,
                        May(CommentOrWhitespace),
                        RegionExpression
                    )
                )
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

        public static Expression<Rule> RegionNotOperator = () =>
            Inherit
            (
                Operator,
                Must
                (
                    Keyword("NOT")
                )
            );

        public static Expression<Rule> RegionOperator = () =>
            Inherit
            (
                Operator,
                Must
                (
                    Keyword("UNION"),
                    Keyword("INTERSECT"),
                    Keyword("EXCEPT")
                )
            );

        public static Expression<Rule> RegionShape = () =>
            Sequence
            (
                Must
                (
                    Literal("CIRCLE"), Literal("CIRC"),
                    Literal("RECTANGLE"), Literal("RECT"),
                    Literal("POLYGON"), Literal("POLY"),
                    Literal("CONVEX_HULL"), Literal("CHULL")
                ),
                May(CommentOrWhitespace),
                BracketOpen,
                May(CommentOrWhitespace),
                RegionArgumentList,
                May(CommentOrWhitespace),
                BracketClose
            );

        public static Expression<Rule> RegionArgument = () =>
            Sequence
            (
                NumericConstant
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
            Inherit
            (
                SelectStatement,
                Sequence
                (
                    //May(Sequence(CommonTableExpression, May(CommentOrWhitespace))),
                    XMatchQueryExpression,
                    May(Sequence(May(CommentOrWhitespace), OrderByClause)),
                    May(Sequence(May(CommentOrWhitespace), OptionClause))
                )
            );

        public static Expression<Rule> XMatchQueryExpression = () =>
            Inherit
            (
                QueryExpression,
                XMatchQuerySpecification
            );

        public static Expression<Rule> XMatchQuerySpecification = () =>
            Inherit
            (
                RegionQuerySpecification,
                Sequence
                (
                    Keyword("SELECT"),
                    May(CommentOrWhitespace),
                    SelectList,
                    May(Sequence(May(CommentOrWhitespace), IntoClause)),
                    May(CommentOrWhitespace), XMatchFromClause,
                    May(Sequence(May(CommentOrWhitespace), WhereClause)),
                    May(Sequence(May(CommentOrWhitespace), RegionClause))
                )
            );

        public static Expression<Rule> XMatchFromClause = () =>
            Inherit
            (
                FromClause,
                Sequence
                (
                    Keyword("FROM"),
                    May(CommentOrWhitespace),
                    XMatchTableSourceExpression
                )
            );

        public static Expression<Rule> XMatchTableSourceExpression = () =>
            Inherit
            (
                TableSourceExpression,
                Sequence
                (
                    XMatchTableSourceSpecification,
                    May(Sequence(May(CommentOrWhitespace), JoinedTable))
                )
            );

        public static Expression<Rule> XMatchTableSourceSpecification = () =>
            Inherit
            (
                TableSourceSpecification,
                XMatchTableSource
            );


        public static Expression<Rule> XMatchTableSource = () =>
            Inherit
            (
                TableSource,
                Must
                (
                    Sequence
                    (
                        Keyword("XMATCH"),
                        May(CommentOrWhitespace), BracketOpen,
                        May(CommentOrWhitespace), XMatchTableList,
                        May(CommentOrWhitespace), Comma,
                        May(CommentOrWhitespace), XMatchConstraint,
                        May(CommentOrWhitespace), BracketClose,
                        May(CommentOrWhitespace),
                        May(Sequence(Keyword("AS"), May(CommentOrWhitespace))),
                        TableAlias
                    ),
                    Sequence
                    (
                        TableAlias,
                        May(CommentOrWhitespace),
                        Keyword("AS"),
                        May(CommentOrWhitespace),
                        Keyword("XMATCH"),
                        May(CommentOrWhitespace),
                        May(CommentOrWhitespace), BracketOpen,
                        May(CommentOrWhitespace), XMatchTableList,
                        May(CommentOrWhitespace), Comma,
                        May(CommentOrWhitespace), XMatchConstraint,
                        May(CommentOrWhitespace), BracketClose                        
                    )
                )
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
            Inherit
            (
                TableSourceSpecification,
                Sequence
                (
                    XMatchTableInclusion,
                    May(CommentOrWhitespace),
                    CoordinatesTableSource
                )
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
                Literal("LIMIT"),
                CommentOrWhitespace, XMatchAlgorithm,
                CommentOrWhitespace, Literal("TO"),
                CommentOrWhitespace,
                Must(NumericConstant, TableHint)
            );

        public static Expression<Rule> XMatchAlgorithm = () =>
            Must(Literal("BAYESFACTOR"), Literal("CONE"));

        #endregion
    }
}
