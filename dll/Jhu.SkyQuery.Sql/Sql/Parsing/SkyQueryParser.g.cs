using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Jhu.SkyQuery.Sql.Parsing
{
    public partial class SkyQueryParser : Jhu.Graywulf.Sql.Parsing.SqlParser
    {
        private static HashSet<string> keywords = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {
            "ABSOLUTE", "ALL", "AND", "ANY", "APPLY", "AS", "ASC", "BEGIN", "BETWEEN", 
            "BREAK", "BY", "CASE", "CATCH", "CLOSE", "CLUSTERED", "CONSTRAINT", "CONTINUE", "COUNT", "CREATE", 
            "CROSS", "CURSOR", "DEALLOCATE", "DECLARE", "DEFAULT", "DELETE", "DESC", "DISTINCT", "DROP", "ELSE", 
            "END", "ESCAPE", "EXCEPT", "EXIST", "EXISTS", "FETCH", "FIRST", "FOR", "FROM", "FULL", 
            "GOTO", "GROUP", "HASH", "HAVING", "IDENTITY", "IF", "IN", "INCLUDE", "INDEX", "INNER", 
            "INSERT", "INTERSECT", "INTO", "IS", "JOIN", "KEY", "LAST", "LEFT", "LIKE", "LOOP", 
            "MAY", "MERGE", "MUST", "NEXT", "NONCLUSTERED", "NOT", "NULL", "ON", "OPEN", "OPTION", 
            "OR", "ORDER", "OUTER", "OVER", "PARTITION", "PERCENT", "PRIMARY", "PRINT", "PRIOR", "RELATIVE", 
            "REMOTE", "REPEATABLE", "RETURN", "RIGHT", "ROWS", "SELECT", "SET", "SOME", "SYSTEM", "TABLE", 
            "TABLESAMPLE", "THEN", "THROW", "TIES", "TOP", "TRUNCATE", "TRY", "UNION", "UNIQUE", "UPDATE", 
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





    public partial class Statement : Jhu.Graywulf.Sql.Parsing.Statement, ICloneable
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
                        r5 = r5 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.BeginEndStatement());
                        CommitOrRollback(r5, parser);
                        a2 = r5;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r6

                        bool r6 = true;
                        r6 = r6 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.WhileStatement());
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
                        r10 = r10 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.IfStatement());
                        CommitOrRollback(r10, parser);
                        a2 = r10;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r11

                        bool r11 = true;
                        r11 = r11 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TryCatchStatement());
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
                        r25 = r25 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchSelectStatement());
                        CommitOrRollback(r25, parser);
                        a2 = r25;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r26

                        bool r26 = true;
                        r26 = r26 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionSelectStatement());
                        CommitOrRollback(r26, parser);
                        a2 = r26;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r27

                        bool r27 = true;
                        r27 = r27 && Match(parser, new Jhu.Graywulf.Sql.Parsing.SelectStatement());
                        CommitOrRollback(r27, parser);
                        a2 = r27;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r28

                        bool r28 = true;
                        r28 = r28 && Match(parser, new Jhu.Graywulf.Sql.Parsing.InsertStatement());
                        CommitOrRollback(r28, parser);
                        a2 = r28;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r29

                        bool r29 = true;
                        r29 = r29 && Match(parser, new Jhu.Graywulf.Sql.Parsing.UpdateStatement());
                        CommitOrRollback(r29, parser);
                        a2 = r29;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r30

                        bool r30 = true;
                        r30 = r30 && Match(parser, new Jhu.Graywulf.Sql.Parsing.DeleteStatement());
                        CommitOrRollback(r30, parser);
                        a2 = r30;
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
                Checkpoint(parser); // r31

                bool r31 = true;
                r31 = r31 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionQueryExpression());
                if (r31)
                { // may a32
                    bool a32 = false;
                    {
                        Checkpoint(parser); // r33

                        bool r33 = true;
                        if (r33)
                        { // may a34
                            bool a34 = false;
                            {
                                Checkpoint(parser); // r35

                                bool r35 = true;
                                r35 = r35 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r35, parser);
                                a34 = r35;
                            }

                            r33 |= a34;
                        } // end may a34

                        r33 = r33 && Match(parser, new Jhu.Graywulf.Sql.Parsing.OrderByClause());
                        CommitOrRollback(r33, parser);
                        a32 = r33;
                    }

                    r31 |= a32;
                } // end may a32

                if (r31)
                { // may a36
                    bool a36 = false;
                    {
                        Checkpoint(parser); // r37

                        bool r37 = true;
                        if (r37)
                        { // may a38
                            bool a38 = false;
                            {
                                Checkpoint(parser); // r39

                                bool r39 = true;
                                r39 = r39 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r39, parser);
                                a38 = r39;
                            }

                            r37 |= a38;
                        } // end may a38

                        r37 = r37 && Match(parser, new Jhu.Graywulf.Sql.Parsing.QueryHintClause());
                        CommitOrRollback(r37, parser);
                        a36 = r37;
                    }

                    r31 |= a36;
                } // end may a36

                CommitOrRollback(r31, parser);
                res = r31;
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
                Checkpoint(parser); // r40

                bool r40 = true;
                if (r40)
                { // alternatives a41 must
                    bool a41 = false;
                    if (!a41)
                    {
                        Checkpoint(parser); // r42

                        bool r42 = true;
                        r42 = r42 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionQueryExpressionBrackets());
                        CommitOrRollback(r42, parser);
                        a41 = r42;
                    }

                    if (!a41)
                    {
                        Checkpoint(parser); // r43

                        bool r43 = true;
                        r43 = r43 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionQuerySpecification());
                        CommitOrRollback(r43, parser);
                        a41 = r43;
                    }

                    r40 &= a41;

                } // end alternatives a41

                if (r40)
                { // may a44
                    bool a44 = false;
                    {
                        Checkpoint(parser); // r45

                        bool r45 = true;
                        if (r45)
                        { // may a46
                            bool a46 = false;
                            {
                                Checkpoint(parser); // r47

                                bool r47 = true;
                                r47 = r47 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r47, parser);
                                a46 = r47;
                            }

                            r45 |= a46;
                        } // end may a46

                        r45 = r45 && Match(parser, new Jhu.Graywulf.Sql.Parsing.QueryOperator());
                        if (r45)
                        { // may a48
                            bool a48 = false;
                            {
                                Checkpoint(parser); // r49

                                bool r49 = true;
                                r49 = r49 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r49, parser);
                                a48 = r49;
                            }

                            r45 |= a48;
                        } // end may a48

                        r45 = r45 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionQueryExpression());
                        CommitOrRollback(r45, parser);
                        a44 = r45;
                    }

                    r40 |= a44;
                } // end may a44

                CommitOrRollback(r40, parser);
                res = r40;
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
                Checkpoint(parser); // r50

                bool r50 = true;
                r50 = r50 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r50)
                { // may a51
                    bool a51 = false;
                    {
                        Checkpoint(parser); // r52

                        bool r52 = true;
                        r52 = r52 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r52, parser);
                        a51 = r52;
                    }

                    r50 |= a51;
                } // end may a51

                r50 = r50 && Match(parser, new Jhu.Graywulf.Sql.Parsing.QueryExpression());
                if (r50)
                { // may a53
                    bool a53 = false;
                    {
                        Checkpoint(parser); // r54

                        bool r54 = true;
                        r54 = r54 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r54, parser);
                        a53 = r54;
                    }

                    r50 |= a53;
                } // end may a53

                r50 = r50 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r50, parser);
                res = r50;
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
                Checkpoint(parser); // r55

                bool r55 = true;
                r55 = r55 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"SELECT"));
                if (r55)
                { // may a56
                    bool a56 = false;
                    {
                        Checkpoint(parser); // r57

                        bool r57 = true;
                        r57 = r57 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        if (r57)
                        { // alternatives a58 must
                            bool a58 = false;
                            if (!a58)
                            {
                                Checkpoint(parser); // r59

                                bool r59 = true;
                                r59 = r59 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ALL"));
                                CommitOrRollback(r59, parser);
                                a58 = r59;
                            }

                            if (!a58)
                            {
                                Checkpoint(parser); // r60

                                bool r60 = true;
                                r60 = r60 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DISTINCT"));
                                CommitOrRollback(r60, parser);
                                a58 = r60;
                            }

                            r57 &= a58;

                        } // end alternatives a58

                        CommitOrRollback(r57, parser);
                        a56 = r57;
                    }

                    r55 |= a56;
                } // end may a56

                if (r55)
                { // may a61
                    bool a61 = false;
                    {
                        Checkpoint(parser); // r62

                        bool r62 = true;
                        r62 = r62 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        r62 = r62 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TopExpression());
                        CommitOrRollback(r62, parser);
                        a61 = r62;
                    }

                    r55 |= a61;
                } // end may a61

                if (r55)
                { // may a63
                    bool a63 = false;
                    {
                        Checkpoint(parser); // r64

                        bool r64 = true;
                        r64 = r64 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r64, parser);
                        a63 = r64;
                    }

                    r55 |= a63;
                } // end may a63

                r55 = r55 && Match(parser, new Jhu.Graywulf.Sql.Parsing.SelectList());
                if (r55)
                { // may a65
                    bool a65 = false;
                    {
                        Checkpoint(parser); // r66

                        bool r66 = true;
                        if (r66)
                        { // may a67
                            bool a67 = false;
                            {
                                Checkpoint(parser); // r68

                                bool r68 = true;
                                r68 = r68 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r68, parser);
                                a67 = r68;
                            }

                            r66 |= a67;
                        } // end may a67

                        r66 = r66 && Match(parser, new Jhu.Graywulf.Sql.Parsing.IntoClause());
                        CommitOrRollback(r66, parser);
                        a65 = r66;
                    }

                    r55 |= a65;
                } // end may a65

                if (r55)
                { // may a69
                    bool a69 = false;
                    {
                        Checkpoint(parser); // r70

                        bool r70 = true;
                        if (r70)
                        { // may a71
                            bool a71 = false;
                            {
                                Checkpoint(parser); // r72

                                bool r72 = true;
                                r72 = r72 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r72, parser);
                                a71 = r72;
                            }

                            r70 |= a71;
                        } // end may a71

                        r70 = r70 && Match(parser, new Jhu.Graywulf.Sql.Parsing.FromClause());
                        CommitOrRollback(r70, parser);
                        a69 = r70;
                    }

                    r55 |= a69;
                } // end may a69

                if (r55)
                { // may a73
                    bool a73 = false;
                    {
                        Checkpoint(parser); // r74

                        bool r74 = true;
                        if (r74)
                        { // may a75
                            bool a75 = false;
                            {
                                Checkpoint(parser); // r76

                                bool r76 = true;
                                r76 = r76 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r76, parser);
                                a75 = r76;
                            }

                            r74 |= a75;
                        } // end may a75

                        r74 = r74 && Match(parser, new Jhu.Graywulf.Sql.Parsing.WhereClause());
                        CommitOrRollback(r74, parser);
                        a73 = r74;
                    }

                    r55 |= a73;
                } // end may a73

                if (r55)
                {
                    Checkpoint(parser); // r77

                    bool r77 = true;
                    if (r77)
                    { // may a78
                        bool a78 = false;
                        {
                            Checkpoint(parser); // r79

                            bool r79 = true;
                            r79 = r79 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                            CommitOrRollback(r79, parser);
                            a78 = r79;
                        }

                        r77 |= a78;
                    } // end may a78

                    r77 = r77 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionClause());
                    CommitOrRollback(r77, parser);
                    r55 = r77;
                }

                if (r55)
                { // may a80
                    bool a80 = false;
                    {
                        Checkpoint(parser); // r81

                        bool r81 = true;
                        if (r81)
                        { // may a82
                            bool a82 = false;
                            {
                                Checkpoint(parser); // r83

                                bool r83 = true;
                                r83 = r83 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r83, parser);
                                a82 = r83;
                            }

                            r81 |= a82;
                        } // end may a82

                        r81 = r81 && Match(parser, new Jhu.Graywulf.Sql.Parsing.GroupByClause());
                        CommitOrRollback(r81, parser);
                        a80 = r81;
                    }

                    r55 |= a80;
                } // end may a80

                if (r55)
                { // may a84
                    bool a84 = false;
                    {
                        Checkpoint(parser); // r85

                        bool r85 = true;
                        if (r85)
                        { // may a86
                            bool a86 = false;
                            {
                                Checkpoint(parser); // r87

                                bool r87 = true;
                                r87 = r87 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r87, parser);
                                a86 = r87;
                            }

                            r85 |= a86;
                        } // end may a86

                        r85 = r85 && Match(parser, new Jhu.Graywulf.Sql.Parsing.HavingClause());
                        CommitOrRollback(r85, parser);
                        a84 = r85;
                    }

                    r55 |= a84;
                } // end may a84

                CommitOrRollback(r55, parser);
                res = r55;
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
                Checkpoint(parser); // r88

                bool r88 = true;
                r88 = r88 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"REGION"));
                r88 = r88 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                if (r88)
                { // alternatives a89 must
                    bool a89 = false;
                    if (!a89)
                    {
                        Checkpoint(parser); // r90

                        bool r90 = true;
                        r90 = r90 && Match(parser, new Jhu.Graywulf.Sql.Parsing.StringConstant());
                        CommitOrRollback(r90, parser);
                        a89 = r90;
                    }

                    if (!a89)
                    {
                        Checkpoint(parser); // r91

                        bool r91 = true;
                        r91 = r91 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionExpression());
                        CommitOrRollback(r91, parser);
                        a89 = r91;
                    }

                    r88 &= a89;

                } // end alternatives a89

                CommitOrRollback(r88, parser);
                res = r88;
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
                Checkpoint(parser); // r92

                bool r92 = true;
                if (r92)
                { // alternatives a93 must
                    bool a93 = false;
                    if (!a93)
                    {
                        Checkpoint(parser); // r94

                        bool r94 = true;
                        r94 = r94 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionShape());
                        CommitOrRollback(r94, parser);
                        a93 = r94;
                    }

                    if (!a93)
                    {
                        Checkpoint(parser); // r95

                        bool r95 = true;
                        r95 = r95 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionExpressionBrackets());
                        CommitOrRollback(r95, parser);
                        a93 = r95;
                    }

                    r92 &= a93;

                } // end alternatives a93

                if (r92)
                { // may a96
                    bool a96 = false;
                    {
                        Checkpoint(parser); // r97

                        bool r97 = true;
                        if (r97)
                        { // may a98
                            bool a98 = false;
                            {
                                Checkpoint(parser); // r99

                                bool r99 = true;
                                r99 = r99 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r99, parser);
                                a98 = r99;
                            }

                            r97 |= a98;
                        } // end may a98

                        r97 = r97 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionOperator());
                        if (r97)
                        { // may a100
                            bool a100 = false;
                            {
                                Checkpoint(parser); // r101

                                bool r101 = true;
                                r101 = r101 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r101, parser);
                                a100 = r101;
                            }

                            r97 |= a100;
                        } // end may a100

                        r97 = r97 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionExpression());
                        CommitOrRollback(r97, parser);
                        a96 = r97;
                    }

                    r92 |= a96;
                } // end may a96

                CommitOrRollback(r92, parser);
                res = r92;
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
                Checkpoint(parser); // r102

                bool r102 = true;
                r102 = r102 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r102)
                { // may a103
                    bool a103 = false;
                    {
                        Checkpoint(parser); // r104

                        bool r104 = true;
                        r104 = r104 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r104, parser);
                        a103 = r104;
                    }

                    r102 |= a103;
                } // end may a103

                r102 = r102 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionExpression());
                if (r102)
                { // may a105
                    bool a105 = false;
                    {
                        Checkpoint(parser); // r106

                        bool r106 = true;
                        r106 = r106 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r106, parser);
                        a105 = r106;
                    }

                    r102 |= a105;
                } // end may a105

                r102 = r102 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r102, parser);
                res = r102;
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
                Checkpoint(parser); // r107

                bool r107 = true;
                if (r107)
                { // alternatives a108 must
                    bool a108 = false;
                    if (!a108)
                    {
                        Checkpoint(parser); // r109

                        bool r109 = true;
                        r109 = r109 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"UNION"));
                        CommitOrRollback(r109, parser);
                        a108 = r109;
                    }

                    if (!a108)
                    {
                        Checkpoint(parser); // r110

                        bool r110 = true;
                        r110 = r110 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"INTERSECT"));
                        CommitOrRollback(r110, parser);
                        a108 = r110;
                    }

                    if (!a108)
                    {
                        Checkpoint(parser); // r111

                        bool r111 = true;
                        r111 = r111 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"EXCEPT"));
                        CommitOrRollback(r111, parser);
                        a108 = r111;
                    }

                    r107 &= a108;

                } // end alternatives a108

                CommitOrRollback(r107, parser);
                res = r107;
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
                Checkpoint(parser); // r112

                bool r112 = true;
                r112 = r112 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionShapeType());
                r112 = r112 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                r112 = r112 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionArgumentList());
                if (r112)
                { // may a113
                    bool a113 = false;
                    {
                        Checkpoint(parser); // r114

                        bool r114 = true;
                        r114 = r114 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r114, parser);
                        a113 = r114;
                    }

                    r112 |= a113;
                } // end may a113

                r112 = r112 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r112, parser);
                res = r112;
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
                Checkpoint(parser); // r115

                bool r115 = true;
                if (r115)
                { // alternatives a116 must
                    bool a116 = false;
                    if (!a116)
                    {
                        Checkpoint(parser); // r117

                        bool r117 = true;
                        r117 = r117 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CIRCLE"));
                        CommitOrRollback(r117, parser);
                        a116 = r117;
                    }

                    if (!a116)
                    {
                        Checkpoint(parser); // r118

                        bool r118 = true;
                        r118 = r118 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CIRC"));
                        CommitOrRollback(r118, parser);
                        a116 = r118;
                    }

                    if (!a116)
                    {
                        Checkpoint(parser); // r119

                        bool r119 = true;
                        r119 = r119 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"RECTANGLE"));
                        CommitOrRollback(r119, parser);
                        a116 = r119;
                    }

                    if (!a116)
                    {
                        Checkpoint(parser); // r120

                        bool r120 = true;
                        r120 = r120 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"RECT"));
                        CommitOrRollback(r120, parser);
                        a116 = r120;
                    }

                    if (!a116)
                    {
                        Checkpoint(parser); // r121

                        bool r121 = true;
                        r121 = r121 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"POLYGON"));
                        CommitOrRollback(r121, parser);
                        a116 = r121;
                    }

                    if (!a116)
                    {
                        Checkpoint(parser); // r122

                        bool r122 = true;
                        r122 = r122 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"POLY"));
                        CommitOrRollback(r122, parser);
                        a116 = r122;
                    }

                    if (!a116)
                    {
                        Checkpoint(parser); // r123

                        bool r123 = true;
                        r123 = r123 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CONVEX_HULL"));
                        CommitOrRollback(r123, parser);
                        a116 = r123;
                    }

                    if (!a116)
                    {
                        Checkpoint(parser); // r124

                        bool r124 = true;
                        r124 = r124 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CHULL"));
                        CommitOrRollback(r124, parser);
                        a116 = r124;
                    }

                    r115 &= a116;

                } // end alternatives a116

                CommitOrRollback(r115, parser);
                res = r115;
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
                Checkpoint(parser); // r125

                bool r125 = true;
                r125 = r125 && Match(parser, new Jhu.Graywulf.Sql.Parsing.NumericConstant());
                CommitOrRollback(r125, parser);
                res = r125;
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
                Checkpoint(parser); // r126

                bool r126 = true;
                if (r126)
                { // may a127
                    bool a127 = false;
                    {
                        Checkpoint(parser); // r128

                        bool r128 = true;
                        r128 = r128 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r128, parser);
                        a127 = r128;
                    }

                    r126 |= a127;
                } // end may a127

                r126 = r126 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionArgument());
                if (r126)
                { // may a129
                    bool a129 = false;
                    {
                        Checkpoint(parser); // r130

                        bool r130 = true;
                        if (r130)
                        { // may a131
                            bool a131 = false;
                            {
                                Checkpoint(parser); // r132

                                bool r132 = true;
                                r132 = r132 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r132, parser);
                                a131 = r132;
                            }

                            r130 |= a131;
                        } // end may a131

                        r130 = r130 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        r130 = r130 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionArgumentList());
                        CommitOrRollback(r130, parser);
                        a129 = r130;
                    }

                    r126 |= a129;
                } // end may a129

                CommitOrRollback(r126, parser);
                res = r126;
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
                Checkpoint(parser); // r133

                bool r133 = true;
                r133 = r133 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchQueryExpression());
                if (r133)
                { // may a134
                    bool a134 = false;
                    {
                        Checkpoint(parser); // r135

                        bool r135 = true;
                        if (r135)
                        { // may a136
                            bool a136 = false;
                            {
                                Checkpoint(parser); // r137

                                bool r137 = true;
                                r137 = r137 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r137, parser);
                                a136 = r137;
                            }

                            r135 |= a136;
                        } // end may a136

                        r135 = r135 && Match(parser, new Jhu.Graywulf.Sql.Parsing.OrderByClause());
                        CommitOrRollback(r135, parser);
                        a134 = r135;
                    }

                    r133 |= a134;
                } // end may a134

                if (r133)
                { // may a138
                    bool a138 = false;
                    {
                        Checkpoint(parser); // r139

                        bool r139 = true;
                        if (r139)
                        { // may a140
                            bool a140 = false;
                            {
                                Checkpoint(parser); // r141

                                bool r141 = true;
                                r141 = r141 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r141, parser);
                                a140 = r141;
                            }

                            r139 |= a140;
                        } // end may a140

                        r139 = r139 && Match(parser, new Jhu.Graywulf.Sql.Parsing.QueryHintClause());
                        CommitOrRollback(r139, parser);
                        a138 = r139;
                    }

                    r133 |= a138;
                } // end may a138

                CommitOrRollback(r133, parser);
                res = r133;
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
                Checkpoint(parser); // r142

                bool r142 = true;
                r142 = r142 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchQuerySpecification());
                CommitOrRollback(r142, parser);
                res = r142;
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
                Checkpoint(parser); // r143

                bool r143 = true;
                r143 = r143 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"SELECT"));
                if (r143)
                { // may a144
                    bool a144 = false;
                    {
                        Checkpoint(parser); // r145

                        bool r145 = true;
                        r145 = r145 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r145, parser);
                        a144 = r145;
                    }

                    r143 |= a144;
                } // end may a144

                r143 = r143 && Match(parser, new Jhu.Graywulf.Sql.Parsing.SelectList());
                if (r143)
                { // may a146
                    bool a146 = false;
                    {
                        Checkpoint(parser); // r147

                        bool r147 = true;
                        if (r147)
                        { // may a148
                            bool a148 = false;
                            {
                                Checkpoint(parser); // r149

                                bool r149 = true;
                                r149 = r149 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r149, parser);
                                a148 = r149;
                            }

                            r147 |= a148;
                        } // end may a148

                        r147 = r147 && Match(parser, new Jhu.Graywulf.Sql.Parsing.IntoClause());
                        CommitOrRollback(r147, parser);
                        a146 = r147;
                    }

                    r143 |= a146;
                } // end may a146

                if (r143)
                { // may a150
                    bool a150 = false;
                    {
                        Checkpoint(parser); // r151

                        bool r151 = true;
                        r151 = r151 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r151, parser);
                        a150 = r151;
                    }

                    r143 |= a150;
                } // end may a150

                r143 = r143 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchFromClause());
                if (r143)
                { // may a152
                    bool a152 = false;
                    {
                        Checkpoint(parser); // r153

                        bool r153 = true;
                        if (r153)
                        { // may a154
                            bool a154 = false;
                            {
                                Checkpoint(parser); // r155

                                bool r155 = true;
                                r155 = r155 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r155, parser);
                                a154 = r155;
                            }

                            r153 |= a154;
                        } // end may a154

                        r153 = r153 && Match(parser, new Jhu.Graywulf.Sql.Parsing.WhereClause());
                        CommitOrRollback(r153, parser);
                        a152 = r153;
                    }

                    r143 |= a152;
                } // end may a152

                if (r143)
                { // may a156
                    bool a156 = false;
                    {
                        Checkpoint(parser); // r157

                        bool r157 = true;
                        if (r157)
                        { // may a158
                            bool a158 = false;
                            {
                                Checkpoint(parser); // r159

                                bool r159 = true;
                                r159 = r159 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r159, parser);
                                a158 = r159;
                            }

                            r157 |= a158;
                        } // end may a158

                        r157 = r157 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionClause());
                        CommitOrRollback(r157, parser);
                        a156 = r157;
                    }

                    r143 |= a156;
                } // end may a156

                CommitOrRollback(r143, parser);
                res = r143;
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
                Checkpoint(parser); // r160

                bool r160 = true;
                r160 = r160 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"FROM"));
                if (r160)
                { // may a161
                    bool a161 = false;
                    {
                        Checkpoint(parser); // r162

                        bool r162 = true;
                        r162 = r162 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r162, parser);
                        a161 = r162;
                    }

                    r160 |= a161;
                } // end may a161

                r160 = r160 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableSourceExpression());
                CommitOrRollback(r160, parser);
                res = r160;
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
                Checkpoint(parser); // r163

                bool r163 = true;
                r163 = r163 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableSourceSpecification());
                if (r163)
                { // may a164
                    bool a164 = false;
                    {
                        Checkpoint(parser); // r165

                        bool r165 = true;
                        if (r165)
                        { // may a166
                            bool a166 = false;
                            {
                                Checkpoint(parser); // r167

                                bool r167 = true;
                                r167 = r167 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r167, parser);
                                a166 = r167;
                            }

                            r165 |= a166;
                        } // end may a166

                        r165 = r165 && Match(parser, new Jhu.Graywulf.Sql.Parsing.JoinedTable());
                        CommitOrRollback(r165, parser);
                        a164 = r165;
                    }

                    r163 |= a164;
                } // end may a164

                CommitOrRollback(r163, parser);
                res = r163;
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
                Checkpoint(parser); // r168

                bool r168 = true;
                r168 = r168 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"XMATCH"));
                if (r168)
                { // may a169
                    bool a169 = false;
                    {
                        Checkpoint(parser); // r170

                        bool r170 = true;
                        r170 = r170 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r170, parser);
                        a169 = r170;
                    }

                    r168 |= a169;
                } // end may a169

                r168 = r168 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r168)
                { // may a171
                    bool a171 = false;
                    {
                        Checkpoint(parser); // r172

                        bool r172 = true;
                        r172 = r172 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r172, parser);
                        a171 = r172;
                    }

                    r168 |= a171;
                } // end may a171

                r168 = r168 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableList());
                if (r168)
                { // may a173
                    bool a173 = false;
                    {
                        Checkpoint(parser); // r174

                        bool r174 = true;
                        r174 = r174 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r174, parser);
                        a173 = r174;
                    }

                    r168 |= a173;
                } // end may a173

                r168 = r168 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                if (r168)
                { // may a175
                    bool a175 = false;
                    {
                        Checkpoint(parser); // r176

                        bool r176 = true;
                        r176 = r176 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r176, parser);
                        a175 = r176;
                    }

                    r168 |= a175;
                } // end may a175

                r168 = r168 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchConstraint());
                if (r168)
                { // may a177
                    bool a177 = false;
                    {
                        Checkpoint(parser); // r178

                        bool r178 = true;
                        r178 = r178 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r178, parser);
                        a177 = r178;
                    }

                    r168 |= a177;
                } // end may a177

                r168 = r168 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                if (r168)
                { // may a179
                    bool a179 = false;
                    {
                        Checkpoint(parser); // r180

                        bool r180 = true;
                        r180 = r180 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r180, parser);
                        a179 = r180;
                    }

                    r168 |= a179;
                } // end may a179

                if (r168)
                { // may a181
                    bool a181 = false;
                    {
                        Checkpoint(parser); // r182

                        bool r182 = true;
                        r182 = r182 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"AS"));
                        if (r182)
                        { // may a183
                            bool a183 = false;
                            {
                                Checkpoint(parser); // r184

                                bool r184 = true;
                                r184 = r184 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r184, parser);
                                a183 = r184;
                            }

                            r182 |= a183;
                        } // end may a183

                        CommitOrRollback(r182, parser);
                        a181 = r182;
                    }

                    r168 |= a181;
                } // end may a181

                r168 = r168 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableAlias());
                CommitOrRollback(r168, parser);
                res = r168;
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
                Checkpoint(parser); // r185

                bool r185 = true;
                r185 = r185 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableSpecification());
                if (r185)
                { // may a186
                    bool a186 = false;
                    {
                        Checkpoint(parser); // r187

                        bool r187 = true;
                        if (r187)
                        { // may a188
                            bool a188 = false;
                            {
                                Checkpoint(parser); // r189

                                bool r189 = true;
                                r189 = r189 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r189, parser);
                                a188 = r189;
                            }

                            r187 |= a188;
                        } // end may a188

                        r187 = r187 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r187)
                        { // may a190
                            bool a190 = false;
                            {
                                Checkpoint(parser); // r191

                                bool r191 = true;
                                r191 = r191 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r191, parser);
                                a190 = r191;
                            }

                            r187 |= a190;
                        } // end may a190

                        r187 = r187 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableList());
                        CommitOrRollback(r187, parser);
                        a186 = r187;
                    }

                    r185 |= a186;
                } // end may a186

                CommitOrRollback(r185, parser);
                res = r185;
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
                Checkpoint(parser); // r192

                bool r192 = true;
                r192 = r192 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableInclusion());
                r192 = r192 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r192 = r192 && Match(parser, new Jhu.Graywulf.Sql.Parsing.SimpleTableSource());
                CommitOrRollback(r192, parser);
                res = r192;
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
                Checkpoint(parser); // r193

                bool r193 = true;
                if (r193)
                { // may a194
                    bool a194 = false;
                    {
                        Checkpoint(parser); // r195

                        bool r195 = true;
                        if (r195)
                        { // alternatives a196 must
                            bool a196 = false;
                            if (!a196)
                            {
                                Checkpoint(parser); // r197

                                bool r197 = true;
                                r197 = r197 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"MUST"));
                                CommitOrRollback(r197, parser);
                                a196 = r197;
                            }

                            if (!a196)
                            {
                                Checkpoint(parser); // r198

                                bool r198 = true;
                                r198 = r198 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"MAY"));
                                CommitOrRollback(r198, parser);
                                a196 = r198;
                            }

                            if (!a196)
                            {
                                Checkpoint(parser); // r199

                                bool r199 = true;
                                r199 = r199 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"NOT"));
                                CommitOrRollback(r199, parser);
                                a196 = r199;
                            }

                            r195 &= a196;

                        } // end alternatives a196

                        r195 = r195 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r195, parser);
                        a194 = r195;
                    }

                    r193 |= a194;
                } // end may a194

                r193 = r193 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"EXIST"));
                r193 = r193 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r193 = r193 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"IN"));
                CommitOrRollback(r193, parser);
                res = r193;
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
                Checkpoint(parser); // r200

                bool r200 = true;
                r200 = r200 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"LIMIT"));
                r200 = r200 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r200 = r200 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchAlgorithm());
                r200 = r200 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r200 = r200 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TO"));
                r200 = r200 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                if (r200)
                { // alternatives a201 must
                    bool a201 = false;
                    if (!a201)
                    {
                        Checkpoint(parser); // r202

                        bool r202 = true;
                        r202 = r202 && Match(parser, new Jhu.Graywulf.Sql.Parsing.NumericConstant());
                        CommitOrRollback(r202, parser);
                        a201 = r202;
                    }

                    if (!a201)
                    {
                        Checkpoint(parser); // r203

                        bool r203 = true;
                        r203 = r203 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableHint());
                        CommitOrRollback(r203, parser);
                        a201 = r203;
                    }

                    r200 &= a201;

                } // end alternatives a201

                CommitOrRollback(r200, parser);
                res = r200;
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
                Checkpoint(parser); // r204

                bool r204 = true;
                if (r204)
                { // alternatives a205 must
                    bool a205 = false;
                    if (!a205)
                    {
                        Checkpoint(parser); // r206

                        bool r206 = true;
                        r206 = r206 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"BAYESFACTOR"));
                        CommitOrRollback(r206, parser);
                        a205 = r206;
                    }

                    if (!a205)
                    {
                        Checkpoint(parser); // r207

                        bool r207 = true;
                        r207 = r207 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CONE"));
                        CommitOrRollback(r207, parser);
                        a205 = r207;
                    }

                    r204 &= a205;

                } // end alternatives a205

                CommitOrRollback(r204, parser);
                res = r204;
            }



            return res;
        }
    }

    public partial class StatementBlock : Jhu.Graywulf.Sql.Parsing.StatementBlock, ICloneable
    {
        public StatementBlock()
            :base()
        {
        }

        public StatementBlock(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public StatementBlock(Jhu.SkyQuery.Sql.Parsing.StatementBlock old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.StatementBlock(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r208

                bool r208 = true;
                if (r208)
                { // may a209
                    bool a209 = false;
                    {
                        Checkpoint(parser); // r210

                        bool r210 = true;
                        r210 = r210 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r210, parser);
                        a209 = r210;
                    }

                    r208 |= a209;
                } // end may a209

                if (r208)
                { // may a211
                    bool a211 = false;
                    {
                        Checkpoint(parser); // r212

                        bool r212 = true;
                        r212 = r212 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Statement());
                        CommitOrRollback(r212, parser);
                        a211 = r212;
                    }

                    r208 |= a211;
                } // end may a211

                if (r208)
                { // may a213
                    bool a213 = false;
                    {
                        Checkpoint(parser); // r214

                        bool r214 = true;
                        r214 = r214 && Match(parser, new Jhu.Graywulf.Sql.Parsing.StatementSeparator());
                        if (r214)
                        { // may a215
                            bool a215 = false;
                            {
                                Checkpoint(parser); // r216

                                bool r216 = true;
                                r216 = r216 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.StatementBlock());
                                CommitOrRollback(r216, parser);
                                a215 = r216;
                            }

                            r214 |= a215;
                        } // end may a215

                        CommitOrRollback(r214, parser);
                        a213 = r214;
                    }

                    r208 |= a213;
                } // end may a213

                if (r208)
                { // may a217
                    bool a217 = false;
                    {
                        Checkpoint(parser); // r218

                        bool r218 = true;
                        r218 = r218 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r218, parser);
                        a217 = r218;
                    }

                    r208 |= a217;
                } // end may a217

                CommitOrRollback(r208, parser);
                res = r208;
            }



            return res;
        }
    }

    public partial class BeginEndStatement : Jhu.Graywulf.Sql.Parsing.BeginEndStatement, ICloneable
    {
        public BeginEndStatement()
            :base()
        {
        }

        public BeginEndStatement(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public BeginEndStatement(Jhu.SkyQuery.Sql.Parsing.BeginEndStatement old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.BeginEndStatement(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r219

                bool r219 = true;
                r219 = r219 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"BEGIN"));
                r219 = r219 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.StatementBlock());
                r219 = r219 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"END"));
                CommitOrRollback(r219, parser);
                res = r219;
            }



            return res;
        }
    }

    public partial class TryCatchStatement : Jhu.Graywulf.Sql.Parsing.TryCatchStatement, ICloneable
    {
        public TryCatchStatement()
            :base()
        {
        }

        public TryCatchStatement(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public TryCatchStatement(Jhu.SkyQuery.Sql.Parsing.TryCatchStatement old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.TryCatchStatement(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r220

                bool r220 = true;
                r220 = r220 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"BEGIN"));
                r220 = r220 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r220 = r220 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TRY"));
                r220 = r220 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.StatementBlock());
                r220 = r220 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"END"));
                r220 = r220 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r220 = r220 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TRY"));
                r220 = r220 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r220 = r220 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"BEGIN"));
                r220 = r220 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r220 = r220 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CATCH"));
                if (r220)
                { // alternatives a221 must
                    bool a221 = false;
                    if (!a221)
                    {
                        Checkpoint(parser); // r222

                        bool r222 = true;
                        r222 = r222 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.StatementBlock());
                        CommitOrRollback(r222, parser);
                        a221 = r222;
                    }

                    if (!a221)
                    {
                        Checkpoint(parser); // r223

                        bool r223 = true;
                        r223 = r223 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r223, parser);
                        a221 = r223;
                    }

                    r220 &= a221;

                } // end alternatives a221

                r220 = r220 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"END"));
                r220 = r220 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r220 = r220 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CATCH"));
                CommitOrRollback(r220, parser);
                res = r220;
            }



            return res;
        }
    }

    public partial class WhileStatement : Jhu.Graywulf.Sql.Parsing.WhileStatement, ICloneable
    {
        public WhileStatement()
            :base()
        {
        }

        public WhileStatement(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public WhileStatement(Jhu.SkyQuery.Sql.Parsing.WhileStatement old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.WhileStatement(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r224

                bool r224 = true;
                r224 = r224 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"WHILE"));
                if (r224)
                { // may a225
                    bool a225 = false;
                    {
                        Checkpoint(parser); // r226

                        bool r226 = true;
                        r226 = r226 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r226, parser);
                        a225 = r226;
                    }

                    r224 |= a225;
                } // end may a225

                r224 = r224 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BooleanExpression());
                if (r224)
                { // may a227
                    bool a227 = false;
                    {
                        Checkpoint(parser); // r228

                        bool r228 = true;
                        r228 = r228 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r228, parser);
                        a227 = r228;
                    }

                    r224 |= a227;
                } // end may a227

                r224 = r224 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Statement());
                CommitOrRollback(r224, parser);
                res = r224;
            }



            return res;
        }
    }

    public partial class IfStatement : Jhu.Graywulf.Sql.Parsing.IfStatement, ICloneable
    {
        public IfStatement()
            :base()
        {
        }

        public IfStatement(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public IfStatement(Jhu.SkyQuery.Sql.Parsing.IfStatement old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.IfStatement(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r229

                bool r229 = true;
                r229 = r229 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"IF"));
                if (r229)
                { // may a230
                    bool a230 = false;
                    {
                        Checkpoint(parser); // r231

                        bool r231 = true;
                        r231 = r231 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r231, parser);
                        a230 = r231;
                    }

                    r229 |= a230;
                } // end may a230

                r229 = r229 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BooleanExpression());
                if (r229)
                { // may a232
                    bool a232 = false;
                    {
                        Checkpoint(parser); // r233

                        bool r233 = true;
                        r233 = r233 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r233, parser);
                        a232 = r233;
                    }

                    r229 |= a232;
                } // end may a232

                r229 = r229 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Statement());
                if (r229)
                { // may a234
                    bool a234 = false;
                    {
                        Checkpoint(parser); // r235

                        bool r235 = true;
                        r235 = r235 && Match(parser, new Jhu.Graywulf.Sql.Parsing.StatementSeparator());
                        r235 = r235 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ELSE"));
                        r235 = r235 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        r235 = r235 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Statement());
                        CommitOrRollback(r235, parser);
                        a234 = r235;
                    }

                    r229 |= a234;
                } // end may a234

                CommitOrRollback(r229, parser);
                res = r229;
            }



            return res;
        }
    }


}