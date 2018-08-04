using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Jhu.SkyQuery.Sql.Parsing
{
    public partial class SkyQueryParser : Jhu.Graywulf.Sql.Extensions.Parsing.GraywulfSqlParser
    {
        private static HashSet<string> keywords = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {
            "ABSOLUTE", "ALL", "AND", "ANY", "APPLY", "AS", "ASC", "BEGIN", "BETWEEN", 
            "BREAK", "BY", "CASE", "CATCH", "CLOSE", "CLUSTERED", "CONSTRAINT", "CONTINUE", "CREATE", "CROSS", 
            "CURSOR", "DEALLOCATE", "DECLARE", "DEFAULT", "DELETE", "DESC", "DISTINCT", "DROP", "ELSE", "END", 
            "ESCAPE", "EXCEPT", "EXIST", "EXISTS", "FETCH", "FIRST", "FOR", "FROM", "FULL", "GOTO", 
            "GROUP", "HASH", "HAVING", "IDENTITY", "IF", "IN", "INCLUDE", "INDEX", "INNER", "INSERT", 
            "INTERSECT", "INTO", "IS", "JOIN", "KEY", "LAST", "LEFT", "LIKE", "LOOP", "MAY", 
            "MERGE", "MUST", "NEXT", "NONCLUSTERED", "NOT", "NULL", "ON", "OPEN", "OPTION", "OR", 
            "ORDER", "OUTER", "OVER", "PARTITION", "PERCENT", "PRIMARY", "PRINT", "PRIOR", "RELATIVE", "REMOTE", 
            "REPEATABLE", "RETURN", "RIGHT", "ROWS", "SELECT", "SET", "SOME", "SYSTEM", "TABLE", "TABLESAMPLE", 
            "THEN", "THROW", "TIES", "TOP", "TRUNCATE", "TRY", "UNION", "UNIQUE", "UPDATE", "USING", 
            "VALUES", "WHEN", "WHERE", "WHILE", "WITH", "XMATCH", 

        };

        public override HashSet<string> Keywords
        {
            get { return keywords; }
        }   

        public static StringComparer ComparerInstance
        {
            get { return StringComparer.InvariantCultureIgnoreCase; }
        }

        public override StringComparer Comparer
        {
            get { return StringComparer.InvariantCultureIgnoreCase; }
        }

        public override Jhu.Graywulf.Parsing.Token Execute(string code)
        {
            return Execute(new Jhu.SkyQuery.Sql.Parsing.StatementBlock(), code);
        }
    }





    public partial class Statement : Jhu.Graywulf.Sql.Extensions.Parsing.Statement, ICloneable
    {
        public Statement()
            :base()
        {
        }

        public Statement(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public Statement(Jhu.SkyQuery.Sql.Parsing.Statement old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.Statement(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r1

                bool r1 = true;
                if (r1)
                { // alternatives a2 must
                    bool a2 = false;
                    if (!a2)
                    {
                        Checkpoint(parser); // r3

                        bool r3 = true;
                        r3 = r3 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Label());
                        CommitOrRollback(r3, parser);
                        a2 = r3;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r4

                        bool r4 = true;
                        r4 = r4 && Match(parser, new Jhu.Graywulf.Sql.Parsing.GotoStatement());
                        CommitOrRollback(r4, parser);
                        a2 = r4;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r5

                        bool r5 = true;
                        r5 = r5 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BeginEndStatement());
                        CommitOrRollback(r5, parser);
                        a2 = r5;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r6

                        bool r6 = true;
                        r6 = r6 && Match(parser, new Jhu.Graywulf.Sql.Parsing.WhileStatement());
                        CommitOrRollback(r6, parser);
                        a2 = r6;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r7

                        bool r7 = true;
                        r7 = r7 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BreakStatement());
                        CommitOrRollback(r7, parser);
                        a2 = r7;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r8

                        bool r8 = true;
                        r8 = r8 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ContinueStatement());
                        CommitOrRollback(r8, parser);
                        a2 = r8;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r9

                        bool r9 = true;
                        r9 = r9 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ReturnStatement());
                        CommitOrRollback(r9, parser);
                        a2 = r9;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r10

                        bool r10 = true;
                        r10 = r10 && Match(parser, new Jhu.Graywulf.Sql.Parsing.IfStatement());
                        CommitOrRollback(r10, parser);
                        a2 = r10;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r11

                        bool r11 = true;
                        r11 = r11 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TryCatchStatement());
                        CommitOrRollback(r11, parser);
                        a2 = r11;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r12

                        bool r12 = true;
                        r12 = r12 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ThrowStatement());
                        CommitOrRollback(r12, parser);
                        a2 = r12;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r13

                        bool r13 = true;
                        r13 = r13 && Match(parser, new Jhu.Graywulf.Sql.Parsing.DeclareCursorStatement());
                        CommitOrRollback(r13, parser);
                        a2 = r13;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r14

                        bool r14 = true;
                        r14 = r14 && Match(parser, new Jhu.Graywulf.Sql.Parsing.SetCursorStatement());
                        CommitOrRollback(r14, parser);
                        a2 = r14;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r15

                        bool r15 = true;
                        r15 = r15 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CursorOperationStatement());
                        CommitOrRollback(r15, parser);
                        a2 = r15;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r16

                        bool r16 = true;
                        r16 = r16 && Match(parser, new Jhu.Graywulf.Sql.Parsing.FetchStatement());
                        CommitOrRollback(r16, parser);
                        a2 = r16;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r17

                        bool r17 = true;
                        r17 = r17 && Match(parser, new Jhu.Graywulf.Sql.Parsing.DeclareVariableStatement());
                        CommitOrRollback(r17, parser);
                        a2 = r17;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r18

                        bool r18 = true;
                        r18 = r18 && Match(parser, new Jhu.Graywulf.Sql.Parsing.SetVariableStatement());
                        CommitOrRollback(r18, parser);
                        a2 = r18;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r19

                        bool r19 = true;
                        r19 = r19 && Match(parser, new Jhu.Graywulf.Sql.Parsing.DeclareTableStatement());
                        CommitOrRollback(r19, parser);
                        a2 = r19;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r20

                        bool r20 = true;
                        r20 = r20 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CreateTableStatement());
                        CommitOrRollback(r20, parser);
                        a2 = r20;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r21

                        bool r21 = true;
                        r21 = r21 && Match(parser, new Jhu.Graywulf.Sql.Parsing.DropTableStatement());
                        CommitOrRollback(r21, parser);
                        a2 = r21;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r22

                        bool r22 = true;
                        r22 = r22 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TruncateTableStatement());
                        CommitOrRollback(r22, parser);
                        a2 = r22;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r23

                        bool r23 = true;
                        r23 = r23 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CreateIndexStatement());
                        CommitOrRollback(r23, parser);
                        a2 = r23;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r24

                        bool r24 = true;
                        r24 = r24 && Match(parser, new Jhu.Graywulf.Sql.Parsing.DropIndexStatement());
                        CommitOrRollback(r24, parser);
                        a2 = r24;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r25

                        bool r25 = true;
                        r25 = r25 && Match(parser, new Jhu.Graywulf.Sql.Extensions.Parsing.PartitionedSelectStatement());
                        CommitOrRollback(r25, parser);
                        a2 = r25;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r26

                        bool r26 = true;
                        r26 = r26 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchSelectStatement());
                        CommitOrRollback(r26, parser);
                        a2 = r26;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r27

                        bool r27 = true;
                        r27 = r27 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionSelectStatement());
                        CommitOrRollback(r27, parser);
                        a2 = r27;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r28

                        bool r28 = true;
                        r28 = r28 && Match(parser, new Jhu.Graywulf.Sql.Parsing.SelectStatement());
                        CommitOrRollback(r28, parser);
                        a2 = r28;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r29

                        bool r29 = true;
                        r29 = r29 && Match(parser, new Jhu.Graywulf.Sql.Parsing.InsertStatement());
                        CommitOrRollback(r29, parser);
                        a2 = r29;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r30

                        bool r30 = true;
                        r30 = r30 && Match(parser, new Jhu.Graywulf.Sql.Parsing.UpdateStatement());
                        CommitOrRollback(r30, parser);
                        a2 = r30;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r31

                        bool r31 = true;
                        r31 = r31 && Match(parser, new Jhu.Graywulf.Sql.Parsing.DeleteStatement());
                        CommitOrRollback(r31, parser);
                        a2 = r31;
                    }

                    r1 &= a2;

                } // end alternatives a2

                CommitOrRollback(r1, parser);
                res = r1;
            }



            return res;
        }
    }

    public partial class RegionSelectStatement : Jhu.Graywulf.Sql.Parsing.SelectStatement, ICloneable
    {
        public RegionSelectStatement()
            :base()
        {
        }

        public RegionSelectStatement(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public RegionSelectStatement(Jhu.SkyQuery.Sql.Parsing.RegionSelectStatement old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.RegionSelectStatement(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r32

                bool r32 = true;
                r32 = r32 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionQueryExpression());
                if (r32)
                { // may a33
                    bool a33 = false;
                    {
                        Checkpoint(parser); // r34

                        bool r34 = true;
                        if (r34)
                        { // may a35
                            bool a35 = false;
                            {
                                Checkpoint(parser); // r36

                                bool r36 = true;
                                r36 = r36 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r36, parser);
                                a35 = r36;
                            }

                            r34 |= a35;
                        } // end may a35

                        r34 = r34 && Match(parser, new Jhu.Graywulf.Sql.Parsing.OrderByClause());
                        CommitOrRollback(r34, parser);
                        a33 = r34;
                    }

                    r32 |= a33;
                } // end may a33

                if (r32)
                { // may a37
                    bool a37 = false;
                    {
                        Checkpoint(parser); // r38

                        bool r38 = true;
                        if (r38)
                        { // may a39
                            bool a39 = false;
                            {
                                Checkpoint(parser); // r40

                                bool r40 = true;
                                r40 = r40 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r40, parser);
                                a39 = r40;
                            }

                            r38 |= a39;
                        } // end may a39

                        r38 = r38 && Match(parser, new Jhu.Graywulf.Sql.Parsing.QueryHintClause());
                        CommitOrRollback(r38, parser);
                        a37 = r38;
                    }

                    r32 |= a37;
                } // end may a37

                CommitOrRollback(r32, parser);
                res = r32;
            }



            return res;
        }
    }

    public partial class RegionQueryExpression : Jhu.Graywulf.Sql.Parsing.QueryExpression, ICloneable
    {
        public RegionQueryExpression()
            :base()
        {
        }

        public RegionQueryExpression(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public RegionQueryExpression(Jhu.SkyQuery.Sql.Parsing.RegionQueryExpression old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.RegionQueryExpression(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r41

                bool r41 = true;
                if (r41)
                { // alternatives a42 must
                    bool a42 = false;
                    if (!a42)
                    {
                        Checkpoint(parser); // r43

                        bool r43 = true;
                        r43 = r43 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionQueryExpressionBrackets());
                        CommitOrRollback(r43, parser);
                        a42 = r43;
                    }

                    if (!a42)
                    {
                        Checkpoint(parser); // r44

                        bool r44 = true;
                        r44 = r44 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionQuerySpecification());
                        CommitOrRollback(r44, parser);
                        a42 = r44;
                    }

                    r41 &= a42;

                } // end alternatives a42

                if (r41)
                { // may a45
                    bool a45 = false;
                    {
                        Checkpoint(parser); // r46

                        bool r46 = true;
                        if (r46)
                        { // may a47
                            bool a47 = false;
                            {
                                Checkpoint(parser); // r48

                                bool r48 = true;
                                r48 = r48 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r48, parser);
                                a47 = r48;
                            }

                            r46 |= a47;
                        } // end may a47

                        r46 = r46 && Match(parser, new Jhu.Graywulf.Sql.Parsing.QueryOperator());
                        if (r46)
                        { // may a49
                            bool a49 = false;
                            {
                                Checkpoint(parser); // r50

                                bool r50 = true;
                                r50 = r50 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r50, parser);
                                a49 = r50;
                            }

                            r46 |= a49;
                        } // end may a49

                        r46 = r46 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionQueryExpression());
                        CommitOrRollback(r46, parser);
                        a45 = r46;
                    }

                    r41 |= a45;
                } // end may a45

                CommitOrRollback(r41, parser);
                res = r41;
            }



            return res;
        }
    }

    public partial class RegionQueryExpressionBrackets : Jhu.Graywulf.Sql.Parsing.QueryExpressionBrackets, ICloneable
    {
        public RegionQueryExpressionBrackets()
            :base()
        {
        }

        public RegionQueryExpressionBrackets(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public RegionQueryExpressionBrackets(Jhu.SkyQuery.Sql.Parsing.RegionQueryExpressionBrackets old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.RegionQueryExpressionBrackets(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r51

                bool r51 = true;
                r51 = r51 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r51)
                { // may a52
                    bool a52 = false;
                    {
                        Checkpoint(parser); // r53

                        bool r53 = true;
                        r53 = r53 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r53, parser);
                        a52 = r53;
                    }

                    r51 |= a52;
                } // end may a52

                r51 = r51 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionQueryExpression());
                if (r51)
                { // may a54
                    bool a54 = false;
                    {
                        Checkpoint(parser); // r55

                        bool r55 = true;
                        r55 = r55 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r55, parser);
                        a54 = r55;
                    }

                    r51 |= a54;
                } // end may a54

                r51 = r51 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r51, parser);
                res = r51;
            }



            return res;
        }
    }

    public partial class RegionQuerySpecification : Jhu.Graywulf.Sql.Parsing.QuerySpecification, ICloneable
    {
        public RegionQuerySpecification()
            :base()
        {
        }

        public RegionQuerySpecification(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public RegionQuerySpecification(Jhu.SkyQuery.Sql.Parsing.RegionQuerySpecification old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.RegionQuerySpecification(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r56

                bool r56 = true;
                r56 = r56 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"SELECT"));
                if (r56)
                { // may a57
                    bool a57 = false;
                    {
                        Checkpoint(parser); // r58

                        bool r58 = true;
                        r58 = r58 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        if (r58)
                        { // alternatives a59 must
                            bool a59 = false;
                            if (!a59)
                            {
                                Checkpoint(parser); // r60

                                bool r60 = true;
                                r60 = r60 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ALL"));
                                CommitOrRollback(r60, parser);
                                a59 = r60;
                            }

                            if (!a59)
                            {
                                Checkpoint(parser); // r61

                                bool r61 = true;
                                r61 = r61 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DISTINCT"));
                                CommitOrRollback(r61, parser);
                                a59 = r61;
                            }

                            r58 &= a59;

                        } // end alternatives a59

                        CommitOrRollback(r58, parser);
                        a57 = r58;
                    }

                    r56 |= a57;
                } // end may a57

                if (r56)
                { // may a62
                    bool a62 = false;
                    {
                        Checkpoint(parser); // r63

                        bool r63 = true;
                        r63 = r63 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        r63 = r63 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TopExpression());
                        CommitOrRollback(r63, parser);
                        a62 = r63;
                    }

                    r56 |= a62;
                } // end may a62

                if (r56)
                { // may a64
                    bool a64 = false;
                    {
                        Checkpoint(parser); // r65

                        bool r65 = true;
                        r65 = r65 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r65, parser);
                        a64 = r65;
                    }

                    r56 |= a64;
                } // end may a64

                r56 = r56 && Match(parser, new Jhu.Graywulf.Sql.Parsing.SelectList());
                if (r56)
                { // may a66
                    bool a66 = false;
                    {
                        Checkpoint(parser); // r67

                        bool r67 = true;
                        if (r67)
                        { // may a68
                            bool a68 = false;
                            {
                                Checkpoint(parser); // r69

                                bool r69 = true;
                                r69 = r69 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r69, parser);
                                a68 = r69;
                            }

                            r67 |= a68;
                        } // end may a68

                        r67 = r67 && Match(parser, new Jhu.Graywulf.Sql.Parsing.IntoClause());
                        CommitOrRollback(r67, parser);
                        a66 = r67;
                    }

                    r56 |= a66;
                } // end may a66

                if (r56)
                { // may a70
                    bool a70 = false;
                    {
                        Checkpoint(parser); // r71

                        bool r71 = true;
                        if (r71)
                        { // may a72
                            bool a72 = false;
                            {
                                Checkpoint(parser); // r73

                                bool r73 = true;
                                r73 = r73 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r73, parser);
                                a72 = r73;
                            }

                            r71 |= a72;
                        } // end may a72

                        r71 = r71 && Match(parser, new Jhu.Graywulf.Sql.Parsing.FromClause());
                        CommitOrRollback(r71, parser);
                        a70 = r71;
                    }

                    r56 |= a70;
                } // end may a70

                if (r56)
                { // may a74
                    bool a74 = false;
                    {
                        Checkpoint(parser); // r75

                        bool r75 = true;
                        if (r75)
                        { // may a76
                            bool a76 = false;
                            {
                                Checkpoint(parser); // r77

                                bool r77 = true;
                                r77 = r77 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r77, parser);
                                a76 = r77;
                            }

                            r75 |= a76;
                        } // end may a76

                        r75 = r75 && Match(parser, new Jhu.Graywulf.Sql.Parsing.WhereClause());
                        CommitOrRollback(r75, parser);
                        a74 = r75;
                    }

                    r56 |= a74;
                } // end may a74

                if (r56)
                {
                    Checkpoint(parser); // r78

                    bool r78 = true;
                    if (r78)
                    { // may a79
                        bool a79 = false;
                        {
                            Checkpoint(parser); // r80

                            bool r80 = true;
                            r80 = r80 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                            CommitOrRollback(r80, parser);
                            a79 = r80;
                        }

                        r78 |= a79;
                    } // end may a79

                    r78 = r78 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionClause());
                    CommitOrRollback(r78, parser);
                    r56 = r78;
                }

                if (r56)
                { // may a81
                    bool a81 = false;
                    {
                        Checkpoint(parser); // r82

                        bool r82 = true;
                        if (r82)
                        { // may a83
                            bool a83 = false;
                            {
                                Checkpoint(parser); // r84

                                bool r84 = true;
                                r84 = r84 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r84, parser);
                                a83 = r84;
                            }

                            r82 |= a83;
                        } // end may a83

                        r82 = r82 && Match(parser, new Jhu.Graywulf.Sql.Parsing.GroupByClause());
                        CommitOrRollback(r82, parser);
                        a81 = r82;
                    }

                    r56 |= a81;
                } // end may a81

                if (r56)
                { // may a85
                    bool a85 = false;
                    {
                        Checkpoint(parser); // r86

                        bool r86 = true;
                        if (r86)
                        { // may a87
                            bool a87 = false;
                            {
                                Checkpoint(parser); // r88

                                bool r88 = true;
                                r88 = r88 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r88, parser);
                                a87 = r88;
                            }

                            r86 |= a87;
                        } // end may a87

                        r86 = r86 && Match(parser, new Jhu.Graywulf.Sql.Parsing.HavingClause());
                        CommitOrRollback(r86, parser);
                        a85 = r86;
                    }

                    r56 |= a85;
                } // end may a85

                CommitOrRollback(r56, parser);
                res = r56;
            }



            return res;
        }
    }

    public partial class RegionClause : Jhu.Graywulf.Parsing.Node, ICloneable
    {
        public RegionClause()
            :base()
        {
        }

        public RegionClause(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public RegionClause(Jhu.SkyQuery.Sql.Parsing.RegionClause old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.RegionClause(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r89

                bool r89 = true;
                r89 = r89 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"REGION"));
                r89 = r89 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                if (r89)
                { // alternatives a90 must
                    bool a90 = false;
                    if (!a90)
                    {
                        Checkpoint(parser); // r91

                        bool r91 = true;
                        r91 = r91 && Match(parser, new Jhu.Graywulf.Sql.Parsing.StringConstant());
                        CommitOrRollback(r91, parser);
                        a90 = r91;
                    }

                    if (!a90)
                    {
                        Checkpoint(parser); // r92

                        bool r92 = true;
                        r92 = r92 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionExpression());
                        CommitOrRollback(r92, parser);
                        a90 = r92;
                    }

                    r89 &= a90;

                } // end alternatives a90

                CommitOrRollback(r89, parser);
                res = r89;
            }



            return res;
        }
    }

    public partial class RegionExpression : Jhu.Graywulf.Parsing.Node, ICloneable
    {
        public RegionExpression()
            :base()
        {
        }

        public RegionExpression(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public RegionExpression(Jhu.SkyQuery.Sql.Parsing.RegionExpression old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.RegionExpression(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r93

                bool r93 = true;
                if (r93)
                { // alternatives a94 must
                    bool a94 = false;
                    if (!a94)
                    {
                        Checkpoint(parser); // r95

                        bool r95 = true;
                        r95 = r95 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionShape());
                        CommitOrRollback(r95, parser);
                        a94 = r95;
                    }

                    if (!a94)
                    {
                        Checkpoint(parser); // r96

                        bool r96 = true;
                        r96 = r96 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionExpressionBrackets());
                        CommitOrRollback(r96, parser);
                        a94 = r96;
                    }

                    r93 &= a94;

                } // end alternatives a94

                if (r93)
                { // may a97
                    bool a97 = false;
                    {
                        Checkpoint(parser); // r98

                        bool r98 = true;
                        if (r98)
                        { // may a99
                            bool a99 = false;
                            {
                                Checkpoint(parser); // r100

                                bool r100 = true;
                                r100 = r100 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r100, parser);
                                a99 = r100;
                            }

                            r98 |= a99;
                        } // end may a99

                        r98 = r98 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionOperator());
                        if (r98)
                        { // may a101
                            bool a101 = false;
                            {
                                Checkpoint(parser); // r102

                                bool r102 = true;
                                r102 = r102 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r102, parser);
                                a101 = r102;
                            }

                            r98 |= a101;
                        } // end may a101

                        r98 = r98 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionExpression());
                        CommitOrRollback(r98, parser);
                        a97 = r98;
                    }

                    r93 |= a97;
                } // end may a97

                CommitOrRollback(r93, parser);
                res = r93;
            }



            return res;
        }
    }

    public partial class RegionExpressionBrackets : Jhu.Graywulf.Parsing.Node, ICloneable
    {
        public RegionExpressionBrackets()
            :base()
        {
        }

        public RegionExpressionBrackets(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public RegionExpressionBrackets(Jhu.SkyQuery.Sql.Parsing.RegionExpressionBrackets old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.RegionExpressionBrackets(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r103

                bool r103 = true;
                r103 = r103 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r103)
                { // may a104
                    bool a104 = false;
                    {
                        Checkpoint(parser); // r105

                        bool r105 = true;
                        r105 = r105 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r105, parser);
                        a104 = r105;
                    }

                    r103 |= a104;
                } // end may a104

                r103 = r103 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionExpression());
                if (r103)
                { // may a106
                    bool a106 = false;
                    {
                        Checkpoint(parser); // r107

                        bool r107 = true;
                        r107 = r107 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r107, parser);
                        a106 = r107;
                    }

                    r103 |= a106;
                } // end may a106

                r103 = r103 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r103, parser);
                res = r103;
            }



            return res;
        }
    }

    public partial class RegionOperator : Jhu.Graywulf.Parsing.Node, ICloneable
    {
        public RegionOperator()
            :base()
        {
        }

        public RegionOperator(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public RegionOperator(Jhu.SkyQuery.Sql.Parsing.RegionOperator old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.RegionOperator(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r108

                bool r108 = true;
                if (r108)
                { // alternatives a109 must
                    bool a109 = false;
                    if (!a109)
                    {
                        Checkpoint(parser); // r110

                        bool r110 = true;
                        r110 = r110 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"UNION"));
                        CommitOrRollback(r110, parser);
                        a109 = r110;
                    }

                    if (!a109)
                    {
                        Checkpoint(parser); // r111

                        bool r111 = true;
                        r111 = r111 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"INTERSECT"));
                        CommitOrRollback(r111, parser);
                        a109 = r111;
                    }

                    if (!a109)
                    {
                        Checkpoint(parser); // r112

                        bool r112 = true;
                        r112 = r112 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"EXCEPT"));
                        CommitOrRollback(r112, parser);
                        a109 = r112;
                    }

                    r108 &= a109;

                } // end alternatives a109

                CommitOrRollback(r108, parser);
                res = r108;
            }



            return res;
        }
    }

    public partial class RegionShape : Jhu.Graywulf.Parsing.Node, ICloneable
    {
        public RegionShape()
            :base()
        {
        }

        public RegionShape(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public RegionShape(Jhu.SkyQuery.Sql.Parsing.RegionShape old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.RegionShape(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r113

                bool r113 = true;
                r113 = r113 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionShapeType());
                r113 = r113 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                r113 = r113 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionArgumentList());
                if (r113)
                { // may a114
                    bool a114 = false;
                    {
                        Checkpoint(parser); // r115

                        bool r115 = true;
                        r115 = r115 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r115, parser);
                        a114 = r115;
                    }

                    r113 |= a114;
                } // end may a114

                r113 = r113 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r113, parser);
                res = r113;
            }



            return res;
        }
    }

    public partial class RegionShapeType : Jhu.Graywulf.Parsing.Node, ICloneable
    {
        public RegionShapeType()
            :base()
        {
        }

        public RegionShapeType(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public RegionShapeType(Jhu.SkyQuery.Sql.Parsing.RegionShapeType old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.RegionShapeType(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r116

                bool r116 = true;
                if (r116)
                { // alternatives a117 must
                    bool a117 = false;
                    if (!a117)
                    {
                        Checkpoint(parser); // r118

                        bool r118 = true;
                        r118 = r118 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CIRCLE"));
                        CommitOrRollback(r118, parser);
                        a117 = r118;
                    }

                    if (!a117)
                    {
                        Checkpoint(parser); // r119

                        bool r119 = true;
                        r119 = r119 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CIRC"));
                        CommitOrRollback(r119, parser);
                        a117 = r119;
                    }

                    if (!a117)
                    {
                        Checkpoint(parser); // r120

                        bool r120 = true;
                        r120 = r120 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"RECTANGLE"));
                        CommitOrRollback(r120, parser);
                        a117 = r120;
                    }

                    if (!a117)
                    {
                        Checkpoint(parser); // r121

                        bool r121 = true;
                        r121 = r121 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"RECT"));
                        CommitOrRollback(r121, parser);
                        a117 = r121;
                    }

                    if (!a117)
                    {
                        Checkpoint(parser); // r122

                        bool r122 = true;
                        r122 = r122 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"POLYGON"));
                        CommitOrRollback(r122, parser);
                        a117 = r122;
                    }

                    if (!a117)
                    {
                        Checkpoint(parser); // r123

                        bool r123 = true;
                        r123 = r123 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"POLY"));
                        CommitOrRollback(r123, parser);
                        a117 = r123;
                    }

                    if (!a117)
                    {
                        Checkpoint(parser); // r124

                        bool r124 = true;
                        r124 = r124 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CONVEX_HULL"));
                        CommitOrRollback(r124, parser);
                        a117 = r124;
                    }

                    if (!a117)
                    {
                        Checkpoint(parser); // r125

                        bool r125 = true;
                        r125 = r125 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CHULL"));
                        CommitOrRollback(r125, parser);
                        a117 = r125;
                    }

                    r116 &= a117;

                } // end alternatives a117

                CommitOrRollback(r116, parser);
                res = r116;
            }



            return res;
        }
    }

    public partial class RegionArgument : Jhu.Graywulf.Parsing.Node, ICloneable
    {
        public RegionArgument()
            :base()
        {
        }

        public RegionArgument(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public RegionArgument(Jhu.SkyQuery.Sql.Parsing.RegionArgument old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.RegionArgument(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r126

                bool r126 = true;
                r126 = r126 && Match(parser, new Jhu.Graywulf.Sql.Parsing.NumericConstant());
                CommitOrRollback(r126, parser);
                res = r126;
            }



            return res;
        }
    }

    public partial class RegionArgumentList : Jhu.Graywulf.Parsing.Node, ICloneable
    {
        public RegionArgumentList()
            :base()
        {
        }

        public RegionArgumentList(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public RegionArgumentList(Jhu.SkyQuery.Sql.Parsing.RegionArgumentList old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.RegionArgumentList(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r127

                bool r127 = true;
                if (r127)
                { // may a128
                    bool a128 = false;
                    {
                        Checkpoint(parser); // r129

                        bool r129 = true;
                        r129 = r129 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r129, parser);
                        a128 = r129;
                    }

                    r127 |= a128;
                } // end may a128

                r127 = r127 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionArgument());
                if (r127)
                { // may a130
                    bool a130 = false;
                    {
                        Checkpoint(parser); // r131

                        bool r131 = true;
                        if (r131)
                        { // may a132
                            bool a132 = false;
                            {
                                Checkpoint(parser); // r133

                                bool r133 = true;
                                r133 = r133 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r133, parser);
                                a132 = r133;
                            }

                            r131 |= a132;
                        } // end may a132

                        r131 = r131 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        r131 = r131 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionArgumentList());
                        CommitOrRollback(r131, parser);
                        a130 = r131;
                    }

                    r127 |= a130;
                } // end may a130

                CommitOrRollback(r127, parser);
                res = r127;
            }



            return res;
        }
    }

    public partial class XMatchSelectStatement : Jhu.SkyQuery.Sql.Parsing.RegionSelectStatement, ICloneable
    {
        public XMatchSelectStatement()
            :base()
        {
        }

        public XMatchSelectStatement(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public XMatchSelectStatement(Jhu.SkyQuery.Sql.Parsing.XMatchSelectStatement old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.XMatchSelectStatement(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r134

                bool r134 = true;
                r134 = r134 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchQueryExpression());
                if (r134)
                { // may a135
                    bool a135 = false;
                    {
                        Checkpoint(parser); // r136

                        bool r136 = true;
                        if (r136)
                        { // may a137
                            bool a137 = false;
                            {
                                Checkpoint(parser); // r138

                                bool r138 = true;
                                r138 = r138 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r138, parser);
                                a137 = r138;
                            }

                            r136 |= a137;
                        } // end may a137

                        r136 = r136 && Match(parser, new Jhu.Graywulf.Sql.Parsing.OrderByClause());
                        CommitOrRollback(r136, parser);
                        a135 = r136;
                    }

                    r134 |= a135;
                } // end may a135

                if (r134)
                { // may a139
                    bool a139 = false;
                    {
                        Checkpoint(parser); // r140

                        bool r140 = true;
                        if (r140)
                        { // may a141
                            bool a141 = false;
                            {
                                Checkpoint(parser); // r142

                                bool r142 = true;
                                r142 = r142 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r142, parser);
                                a141 = r142;
                            }

                            r140 |= a141;
                        } // end may a141

                        r140 = r140 && Match(parser, new Jhu.Graywulf.Sql.Parsing.QueryHintClause());
                        CommitOrRollback(r140, parser);
                        a139 = r140;
                    }

                    r134 |= a139;
                } // end may a139

                CommitOrRollback(r134, parser);
                res = r134;
            }



            return res;
        }
    }

    public partial class XMatchQueryExpression : Jhu.SkyQuery.Sql.Parsing.RegionQueryExpression, ICloneable
    {
        public XMatchQueryExpression()
            :base()
        {
        }

        public XMatchQueryExpression(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public XMatchQueryExpression(Jhu.SkyQuery.Sql.Parsing.XMatchQueryExpression old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.XMatchQueryExpression(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r143

                bool r143 = true;
                r143 = r143 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchQuerySpecification());
                CommitOrRollback(r143, parser);
                res = r143;
            }



            return res;
        }
    }

    public partial class XMatchQuerySpecification : Jhu.SkyQuery.Sql.Parsing.RegionQuerySpecification, ICloneable
    {
        public XMatchQuerySpecification()
            :base()
        {
        }

        public XMatchQuerySpecification(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public XMatchQuerySpecification(Jhu.SkyQuery.Sql.Parsing.XMatchQuerySpecification old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.XMatchQuerySpecification(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r144

                bool r144 = true;
                r144 = r144 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"SELECT"));
                if (r144)
                { // may a145
                    bool a145 = false;
                    {
                        Checkpoint(parser); // r146

                        bool r146 = true;
                        r146 = r146 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r146, parser);
                        a145 = r146;
                    }

                    r144 |= a145;
                } // end may a145

                r144 = r144 && Match(parser, new Jhu.Graywulf.Sql.Parsing.SelectList());
                if (r144)
                { // may a147
                    bool a147 = false;
                    {
                        Checkpoint(parser); // r148

                        bool r148 = true;
                        if (r148)
                        { // may a149
                            bool a149 = false;
                            {
                                Checkpoint(parser); // r150

                                bool r150 = true;
                                r150 = r150 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r150, parser);
                                a149 = r150;
                            }

                            r148 |= a149;
                        } // end may a149

                        r148 = r148 && Match(parser, new Jhu.Graywulf.Sql.Parsing.IntoClause());
                        CommitOrRollback(r148, parser);
                        a147 = r148;
                    }

                    r144 |= a147;
                } // end may a147

                if (r144)
                { // may a151
                    bool a151 = false;
                    {
                        Checkpoint(parser); // r152

                        bool r152 = true;
                        r152 = r152 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r152, parser);
                        a151 = r152;
                    }

                    r144 |= a151;
                } // end may a151

                r144 = r144 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchFromClause());
                if (r144)
                { // may a153
                    bool a153 = false;
                    {
                        Checkpoint(parser); // r154

                        bool r154 = true;
                        if (r154)
                        { // may a155
                            bool a155 = false;
                            {
                                Checkpoint(parser); // r156

                                bool r156 = true;
                                r156 = r156 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r156, parser);
                                a155 = r156;
                            }

                            r154 |= a155;
                        } // end may a155

                        r154 = r154 && Match(parser, new Jhu.Graywulf.Sql.Parsing.WhereClause());
                        CommitOrRollback(r154, parser);
                        a153 = r154;
                    }

                    r144 |= a153;
                } // end may a153

                if (r144)
                { // may a157
                    bool a157 = false;
                    {
                        Checkpoint(parser); // r158

                        bool r158 = true;
                        if (r158)
                        { // may a159
                            bool a159 = false;
                            {
                                Checkpoint(parser); // r160

                                bool r160 = true;
                                r160 = r160 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r160, parser);
                                a159 = r160;
                            }

                            r158 |= a159;
                        } // end may a159

                        r158 = r158 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionClause());
                        CommitOrRollback(r158, parser);
                        a157 = r158;
                    }

                    r144 |= a157;
                } // end may a157

                CommitOrRollback(r144, parser);
                res = r144;
            }



            return res;
        }
    }

    public partial class XMatchFromClause : Jhu.Graywulf.Sql.Parsing.FromClause, ICloneable
    {
        public XMatchFromClause()
            :base()
        {
        }

        public XMatchFromClause(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public XMatchFromClause(Jhu.SkyQuery.Sql.Parsing.XMatchFromClause old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.XMatchFromClause(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r161

                bool r161 = true;
                r161 = r161 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"FROM"));
                if (r161)
                { // may a162
                    bool a162 = false;
                    {
                        Checkpoint(parser); // r163

                        bool r163 = true;
                        r163 = r163 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r163, parser);
                        a162 = r163;
                    }

                    r161 |= a162;
                } // end may a162

                r161 = r161 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableSourceExpression());
                CommitOrRollback(r161, parser);
                res = r161;
            }



            return res;
        }
    }

    public partial class XMatchTableSourceExpression : Jhu.Graywulf.Sql.Parsing.TableSourceExpression, ICloneable
    {
        public XMatchTableSourceExpression()
            :base()
        {
        }

        public XMatchTableSourceExpression(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public XMatchTableSourceExpression(Jhu.SkyQuery.Sql.Parsing.XMatchTableSourceExpression old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.XMatchTableSourceExpression(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r164

                bool r164 = true;
                r164 = r164 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableSourceSpecification());
                if (r164)
                { // may a165
                    bool a165 = false;
                    {
                        Checkpoint(parser); // r166

                        bool r166 = true;
                        if (r166)
                        { // may a167
                            bool a167 = false;
                            {
                                Checkpoint(parser); // r168

                                bool r168 = true;
                                r168 = r168 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r168, parser);
                                a167 = r168;
                            }

                            r166 |= a167;
                        } // end may a167

                        r166 = r166 && Match(parser, new Jhu.Graywulf.Sql.Parsing.JoinedTable());
                        CommitOrRollback(r166, parser);
                        a165 = r166;
                    }

                    r164 |= a165;
                } // end may a165

                CommitOrRollback(r164, parser);
                res = r164;
            }



            return res;
        }
    }

    public partial class XMatchTableSourceSpecification : Jhu.Graywulf.Sql.Parsing.TableSourceSpecification, ICloneable
    {
        public XMatchTableSourceSpecification()
            :base()
        {
        }

        public XMatchTableSourceSpecification(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public XMatchTableSourceSpecification(Jhu.SkyQuery.Sql.Parsing.XMatchTableSourceSpecification old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.XMatchTableSourceSpecification(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r169

                bool r169 = true;
                r169 = r169 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"XMATCH"));
                if (r169)
                { // may a170
                    bool a170 = false;
                    {
                        Checkpoint(parser); // r171

                        bool r171 = true;
                        r171 = r171 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r171, parser);
                        a170 = r171;
                    }

                    r169 |= a170;
                } // end may a170

                r169 = r169 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r169)
                { // may a172
                    bool a172 = false;
                    {
                        Checkpoint(parser); // r173

                        bool r173 = true;
                        r173 = r173 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r173, parser);
                        a172 = r173;
                    }

                    r169 |= a172;
                } // end may a172

                r169 = r169 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableList());
                if (r169)
                { // may a174
                    bool a174 = false;
                    {
                        Checkpoint(parser); // r175

                        bool r175 = true;
                        r175 = r175 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r175, parser);
                        a174 = r175;
                    }

                    r169 |= a174;
                } // end may a174

                r169 = r169 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                if (r169)
                { // may a176
                    bool a176 = false;
                    {
                        Checkpoint(parser); // r177

                        bool r177 = true;
                        r177 = r177 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r177, parser);
                        a176 = r177;
                    }

                    r169 |= a176;
                } // end may a176

                r169 = r169 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchConstraint());
                if (r169)
                { // may a178
                    bool a178 = false;
                    {
                        Checkpoint(parser); // r179

                        bool r179 = true;
                        r179 = r179 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r179, parser);
                        a178 = r179;
                    }

                    r169 |= a178;
                } // end may a178

                r169 = r169 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                if (r169)
                { // may a180
                    bool a180 = false;
                    {
                        Checkpoint(parser); // r181

                        bool r181 = true;
                        r181 = r181 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r181, parser);
                        a180 = r181;
                    }

                    r169 |= a180;
                } // end may a180

                if (r169)
                { // may a182
                    bool a182 = false;
                    {
                        Checkpoint(parser); // r183

                        bool r183 = true;
                        r183 = r183 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"AS"));
                        if (r183)
                        { // may a184
                            bool a184 = false;
                            {
                                Checkpoint(parser); // r185

                                bool r185 = true;
                                r185 = r185 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r185, parser);
                                a184 = r185;
                            }

                            r183 |= a184;
                        } // end may a184

                        CommitOrRollback(r183, parser);
                        a182 = r183;
                    }

                    r169 |= a182;
                } // end may a182

                r169 = r169 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableAlias());
                CommitOrRollback(r169, parser);
                res = r169;
            }



            return res;
        }
    }

    public partial class XMatchTableList : Jhu.Graywulf.Parsing.Node, ICloneable
    {
        public XMatchTableList()
            :base()
        {
        }

        public XMatchTableList(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public XMatchTableList(Jhu.SkyQuery.Sql.Parsing.XMatchTableList old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.XMatchTableList(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r186

                bool r186 = true;
                r186 = r186 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableSpecification());
                if (r186)
                { // may a187
                    bool a187 = false;
                    {
                        Checkpoint(parser); // r188

                        bool r188 = true;
                        if (r188)
                        { // may a189
                            bool a189 = false;
                            {
                                Checkpoint(parser); // r190

                                bool r190 = true;
                                r190 = r190 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r190, parser);
                                a189 = r190;
                            }

                            r188 |= a189;
                        } // end may a189

                        r188 = r188 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r188)
                        { // may a191
                            bool a191 = false;
                            {
                                Checkpoint(parser); // r192

                                bool r192 = true;
                                r192 = r192 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r192, parser);
                                a191 = r192;
                            }

                            r188 |= a191;
                        } // end may a191

                        r188 = r188 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableList());
                        CommitOrRollback(r188, parser);
                        a187 = r188;
                    }

                    r186 |= a187;
                } // end may a187

                CommitOrRollback(r186, parser);
                res = r186;
            }



            return res;
        }
    }

    public partial class XMatchTableSpecification : Jhu.Graywulf.Parsing.Node, ICloneable
    {
        public XMatchTableSpecification()
            :base()
        {
        }

        public XMatchTableSpecification(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public XMatchTableSpecification(Jhu.SkyQuery.Sql.Parsing.XMatchTableSpecification old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.XMatchTableSpecification(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r193

                bool r193 = true;
                r193 = r193 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableInclusion());
                r193 = r193 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r193 = r193 && Match(parser, new Jhu.Graywulf.Sql.Parsing.SimpleTableSource());
                CommitOrRollback(r193, parser);
                res = r193;
            }



            return res;
        }
    }

    public partial class XMatchTableInclusion : Jhu.Graywulf.Parsing.Node, ICloneable
    {
        public XMatchTableInclusion()
            :base()
        {
        }

        public XMatchTableInclusion(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public XMatchTableInclusion(Jhu.SkyQuery.Sql.Parsing.XMatchTableInclusion old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.XMatchTableInclusion(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r194

                bool r194 = true;
                if (r194)
                { // may a195
                    bool a195 = false;
                    {
                        Checkpoint(parser); // r196

                        bool r196 = true;
                        if (r196)
                        { // alternatives a197 must
                            bool a197 = false;
                            if (!a197)
                            {
                                Checkpoint(parser); // r198

                                bool r198 = true;
                                r198 = r198 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"MUST"));
                                CommitOrRollback(r198, parser);
                                a197 = r198;
                            }

                            if (!a197)
                            {
                                Checkpoint(parser); // r199

                                bool r199 = true;
                                r199 = r199 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"MAY"));
                                CommitOrRollback(r199, parser);
                                a197 = r199;
                            }

                            if (!a197)
                            {
                                Checkpoint(parser); // r200

                                bool r200 = true;
                                r200 = r200 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"NOT"));
                                CommitOrRollback(r200, parser);
                                a197 = r200;
                            }

                            r196 &= a197;

                        } // end alternatives a197

                        r196 = r196 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r196, parser);
                        a195 = r196;
                    }

                    r194 |= a195;
                } // end may a195

                r194 = r194 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"EXIST"));
                r194 = r194 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r194 = r194 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"IN"));
                CommitOrRollback(r194, parser);
                res = r194;
            }



            return res;
        }
    }

    public partial class XMatchConstraint : Jhu.Graywulf.Parsing.Node, ICloneable
    {
        public XMatchConstraint()
            :base()
        {
        }

        public XMatchConstraint(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public XMatchConstraint(Jhu.SkyQuery.Sql.Parsing.XMatchConstraint old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.XMatchConstraint(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r201

                bool r201 = true;
                r201 = r201 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"LIMIT"));
                r201 = r201 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r201 = r201 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchAlgorithm());
                r201 = r201 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r201 = r201 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TO"));
                r201 = r201 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                if (r201)
                { // alternatives a202 must
                    bool a202 = false;
                    if (!a202)
                    {
                        Checkpoint(parser); // r203

                        bool r203 = true;
                        r203 = r203 && Match(parser, new Jhu.Graywulf.Sql.Parsing.NumericConstant());
                        CommitOrRollback(r203, parser);
                        a202 = r203;
                    }

                    if (!a202)
                    {
                        Checkpoint(parser); // r204

                        bool r204 = true;
                        r204 = r204 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableHint());
                        CommitOrRollback(r204, parser);
                        a202 = r204;
                    }

                    r201 &= a202;

                } // end alternatives a202

                CommitOrRollback(r201, parser);
                res = r201;
            }



            return res;
        }
    }

    public partial class XMatchAlgorithm : Jhu.Graywulf.Parsing.Node, ICloneable
    {
        public XMatchAlgorithm()
            :base()
        {
        }

        public XMatchAlgorithm(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public XMatchAlgorithm(Jhu.SkyQuery.Sql.Parsing.XMatchAlgorithm old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.XMatchAlgorithm(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r205

                bool r205 = true;
                if (r205)
                { // alternatives a206 must
                    bool a206 = false;
                    if (!a206)
                    {
                        Checkpoint(parser); // r207

                        bool r207 = true;
                        r207 = r207 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"BAYESFACTOR"));
                        CommitOrRollback(r207, parser);
                        a206 = r207;
                    }

                    if (!a206)
                    {
                        Checkpoint(parser); // r208

                        bool r208 = true;
                        r208 = r208 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CONE"));
                        CommitOrRollback(r208, parser);
                        a206 = r208;
                    }

                    r205 &= a206;

                } // end alternatives a206

                CommitOrRollback(r205, parser);
                res = r205;
            }



            return res;
        }
    }


}