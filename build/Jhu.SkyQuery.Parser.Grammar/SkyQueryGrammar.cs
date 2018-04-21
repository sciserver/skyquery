using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Jhu.Graywulf.Parsing;
using Jhu.Graywulf.Parsing.Generator;

namespace Jhu.SkyQuery.Parser.Grammar
{
    [Grammar(Namespace = "Jhu.SkyQuery.Parser", ParserName = "SkyQueryParser",
        Comparer = "StringComparer.InvariantCultureIgnoreCase", RootToken = "Jhu.SkyQuery.Parser.StatementBlock")]
    public class SkyQueryGrammar : Jhu.Graywulf.Sql.Parser.Grammar.SqlGrammar
    {
        public static new Expression<Rule> Statement = () =>
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

                    XMatchSelectStatement,      //
                    RegionSelectStatement,      //

                    SelectStatement,
                    InsertStatement,
                    UpdateStatement,
                    DeleteStatement
                )
            );

        #region Region grammar

        public static Expression<Rule> RegionSelectStatement = () =>
            Inherit
            (
                SelectStatement,
                Sequence
                (
                    //May(Sequence(CommonTableExpression, May(CommentOrWhitespace))),
                    RegionQueryExpression,
                    May(Sequence(May(CommentOrWhitespace), OrderByClause)),
                    May(Sequence(May(CommentOrWhitespace), QueryHintClause))
                )
            );

        public static Expression<Rule> RegionQueryExpression = () =>
            Inherit
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
            Inherit
            (
                QueryExpressionBrackets,
                Sequence(BracketOpen, May(CommentOrWhitespace), QueryExpression, May(CommentOrWhitespace), BracketClose)
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
                    May(Sequence(May(CommentOrWhitespace), FromClause)),
                    May(Sequence(May(CommentOrWhitespace), WhereClause)),
                    Sequence(May(CommentOrWhitespace), RegionClause),
                    May(Sequence(May(CommentOrWhitespace), GroupByClause)),
                    May(Sequence(May(CommentOrWhitespace), HavingClause))
                )
            );

        public static Expression<Rule> RegionClause = () =>
            Sequence
            (
                Keyword("REGION"),
                CommentOrWhitespace,
                Must(StringConstant) //, RegionExpression)
            );

        /*
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
                Literal("CIRCLE"), Literal("CIRC"),
                Literal("RECTANGLE"), Literal("RECT"),
                Literal("POLYGON"), Literal("POLY"),
                Literal("CONVEX_HULL"), Literal("CHULL")
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
        */

        #endregion
        #region XMatch grammar

        public static Expression<Rule> XMatchSelectStatement = () =>
            Inherit
            (
                RegionSelectStatement,
                Sequence
                (
                    //May(Sequence(CommonTableExpression, May(CommentOrWhitespace))),
                    XMatchQueryExpression,
                    May(Sequence(May(CommentOrWhitespace), OrderByClause)),
                    May(Sequence(May(CommentOrWhitespace), QueryHintClause))
                )
            );

        public static Expression<Rule> XMatchQueryExpression = () =>
            Inherit
            (
                RegionQueryExpression,
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
                    XMatchTableSource,
                    May(Sequence(May(CommentOrWhitespace), JoinedTable))
                )
            );

        public static Expression<Rule> XMatchTableSource = () =>
            Inherit
            (
                TableSource,
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
                Literal("LIMIT"),
                CommentOrWhitespace, XMatchAlgorithm,
                CommentOrWhitespace, Literal("TO"),
                CommentOrWhitespace, 
                Must(Number, TableHint)
            );

        public static Expression<Rule> XMatchAlgorithm = () =>
            Must(Literal("BAYESFACTOR"), Literal("CONE"));

        #endregion
    }
}
