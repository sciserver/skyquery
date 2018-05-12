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
            "BREAK", "BY", "CASE", "CATCH", "CLOSE", "CLUSTERED", "CONSTRAINT", "CONTINUE", "CREATE", "CROSS", 
            "CURSOR", "DEALLOCATE", "DECLARE", "DEFAULT", "DELETE", "DESC", "DISTINCT", "DROP", "ELSE", "END", 
            "ESCAPE", "EXCEPT", "EXIST", "EXISTS", "FETCH", "FIRST", "FOR", "FROM", "FULL", "GOTO", 
            "GROUP", "HASH", "HAVING", "IDENTITY", "IF", "IN", "INCLUDE", "INDEX", "INNER", "INSERT", 
            "INTERSECT", "INTO", "IS", "JOIN", "KEY", "LAST", "LEFT", "LIKE", "LOOP", "MAY", 
            "MERGE", "MUST", "NEXT", "NONCLUSTERED", "NOT", "NULL", "ON", "OPEN", "OPTION", "OR", 
            "ORDER", "OUTER", "OVER", "PARTITION", "PERCENT", "PRIMARY", "PRINT", "PRIOR", "REGION", "RELATIVE", 
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

                    r88 &= a89;

                } // end alternatives a89

                CommitOrRollback(r88, parser);
                res = r88;
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
                Checkpoint(parser); // r91

                bool r91 = true;
                r91 = r91 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchQueryExpression());
                if (r91)
                { // may a92
                    bool a92 = false;
                    {
                        Checkpoint(parser); // r93

                        bool r93 = true;
                        if (r93)
                        { // may a94
                            bool a94 = false;
                            {
                                Checkpoint(parser); // r95

                                bool r95 = true;
                                r95 = r95 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r95, parser);
                                a94 = r95;
                            }

                            r93 |= a94;
                        } // end may a94

                        r93 = r93 && Match(parser, new Jhu.Graywulf.Sql.Parsing.OrderByClause());
                        CommitOrRollback(r93, parser);
                        a92 = r93;
                    }

                    r91 |= a92;
                } // end may a92

                if (r91)
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

                        r97 = r97 && Match(parser, new Jhu.Graywulf.Sql.Parsing.QueryHintClause());
                        CommitOrRollback(r97, parser);
                        a96 = r97;
                    }

                    r91 |= a96;
                } // end may a96

                CommitOrRollback(r91, parser);
                res = r91;
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
                Checkpoint(parser); // r100

                bool r100 = true;
                r100 = r100 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchQuerySpecification());
                CommitOrRollback(r100, parser);
                res = r100;
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
                Checkpoint(parser); // r101

                bool r101 = true;
                r101 = r101 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"SELECT"));
                if (r101)
                { // may a102
                    bool a102 = false;
                    {
                        Checkpoint(parser); // r103

                        bool r103 = true;
                        r103 = r103 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r103, parser);
                        a102 = r103;
                    }

                    r101 |= a102;
                } // end may a102

                r101 = r101 && Match(parser, new Jhu.Graywulf.Sql.Parsing.SelectList());
                if (r101)
                { // may a104
                    bool a104 = false;
                    {
                        Checkpoint(parser); // r105

                        bool r105 = true;
                        if (r105)
                        { // may a106
                            bool a106 = false;
                            {
                                Checkpoint(parser); // r107

                                bool r107 = true;
                                r107 = r107 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r107, parser);
                                a106 = r107;
                            }

                            r105 |= a106;
                        } // end may a106

                        r105 = r105 && Match(parser, new Jhu.Graywulf.Sql.Parsing.IntoClause());
                        CommitOrRollback(r105, parser);
                        a104 = r105;
                    }

                    r101 |= a104;
                } // end may a104

                if (r101)
                { // may a108
                    bool a108 = false;
                    {
                        Checkpoint(parser); // r109

                        bool r109 = true;
                        r109 = r109 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r109, parser);
                        a108 = r109;
                    }

                    r101 |= a108;
                } // end may a108

                r101 = r101 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchFromClause());
                if (r101)
                { // may a110
                    bool a110 = false;
                    {
                        Checkpoint(parser); // r111

                        bool r111 = true;
                        if (r111)
                        { // may a112
                            bool a112 = false;
                            {
                                Checkpoint(parser); // r113

                                bool r113 = true;
                                r113 = r113 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r113, parser);
                                a112 = r113;
                            }

                            r111 |= a112;
                        } // end may a112

                        r111 = r111 && Match(parser, new Jhu.Graywulf.Sql.Parsing.WhereClause());
                        CommitOrRollback(r111, parser);
                        a110 = r111;
                    }

                    r101 |= a110;
                } // end may a110

                if (r101)
                { // may a114
                    bool a114 = false;
                    {
                        Checkpoint(parser); // r115

                        bool r115 = true;
                        if (r115)
                        { // may a116
                            bool a116 = false;
                            {
                                Checkpoint(parser); // r117

                                bool r117 = true;
                                r117 = r117 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r117, parser);
                                a116 = r117;
                            }

                            r115 |= a116;
                        } // end may a116

                        r115 = r115 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionClause());
                        CommitOrRollback(r115, parser);
                        a114 = r115;
                    }

                    r101 |= a114;
                } // end may a114

                CommitOrRollback(r101, parser);
                res = r101;
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
                Checkpoint(parser); // r118

                bool r118 = true;
                r118 = r118 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"FROM"));
                if (r118)
                { // may a119
                    bool a119 = false;
                    {
                        Checkpoint(parser); // r120

                        bool r120 = true;
                        r120 = r120 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r120, parser);
                        a119 = r120;
                    }

                    r118 |= a119;
                } // end may a119

                r118 = r118 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableSourceExpression());
                CommitOrRollback(r118, parser);
                res = r118;
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
                Checkpoint(parser); // r121

                bool r121 = true;
                r121 = r121 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableSource());
                if (r121)
                { // may a122
                    bool a122 = false;
                    {
                        Checkpoint(parser); // r123

                        bool r123 = true;
                        if (r123)
                        { // may a124
                            bool a124 = false;
                            {
                                Checkpoint(parser); // r125

                                bool r125 = true;
                                r125 = r125 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r125, parser);
                                a124 = r125;
                            }

                            r123 |= a124;
                        } // end may a124

                        r123 = r123 && Match(parser, new Jhu.Graywulf.Sql.Parsing.JoinedTable());
                        CommitOrRollback(r123, parser);
                        a122 = r123;
                    }

                    r121 |= a122;
                } // end may a122

                CommitOrRollback(r121, parser);
                res = r121;
            }



            return res;
        }
    }

    public partial class XMatchTableSource : Jhu.Graywulf.Sql.Parsing.TableSource, ICloneable
    {
        public XMatchTableSource()
            :base()
        {
        }

        public XMatchTableSource(Jhu.SkyQuery.Sql.Parsing.XMatchTableSource old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.XMatchTableSource(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r126

                bool r126 = true;
                r126 = r126 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"XMATCH"));
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

                r126 = r126 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r126)
                { // may a129
                    bool a129 = false;
                    {
                        Checkpoint(parser); // r130

                        bool r130 = true;
                        r130 = r130 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r130, parser);
                        a129 = r130;
                    }

                    r126 |= a129;
                } // end may a129

                r126 = r126 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableList());
                if (r126)
                { // may a131
                    bool a131 = false;
                    {
                        Checkpoint(parser); // r132

                        bool r132 = true;
                        r132 = r132 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r132, parser);
                        a131 = r132;
                    }

                    r126 |= a131;
                } // end may a131

                r126 = r126 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                if (r126)
                { // may a133
                    bool a133 = false;
                    {
                        Checkpoint(parser); // r134

                        bool r134 = true;
                        r134 = r134 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r134, parser);
                        a133 = r134;
                    }

                    r126 |= a133;
                } // end may a133

                r126 = r126 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchConstraint());
                if (r126)
                { // may a135
                    bool a135 = false;
                    {
                        Checkpoint(parser); // r136

                        bool r136 = true;
                        r136 = r136 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r136, parser);
                        a135 = r136;
                    }

                    r126 |= a135;
                } // end may a135

                r126 = r126 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                if (r126)
                { // may a137
                    bool a137 = false;
                    {
                        Checkpoint(parser); // r138

                        bool r138 = true;
                        r138 = r138 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r138, parser);
                        a137 = r138;
                    }

                    r126 |= a137;
                } // end may a137

                if (r126)
                { // may a139
                    bool a139 = false;
                    {
                        Checkpoint(parser); // r140

                        bool r140 = true;
                        r140 = r140 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"AS"));
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

                        CommitOrRollback(r140, parser);
                        a139 = r140;
                    }

                    r126 |= a139;
                } // end may a139

                r126 = r126 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableAlias());
                CommitOrRollback(r126, parser);
                res = r126;
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
                Checkpoint(parser); // r143

                bool r143 = true;
                r143 = r143 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableSpecification());
                if (r143)
                { // may a144
                    bool a144 = false;
                    {
                        Checkpoint(parser); // r145

                        bool r145 = true;
                        if (r145)
                        { // may a146
                            bool a146 = false;
                            {
                                Checkpoint(parser); // r147

                                bool r147 = true;
                                r147 = r147 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r147, parser);
                                a146 = r147;
                            }

                            r145 |= a146;
                        } // end may a146

                        r145 = r145 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r145)
                        { // may a148
                            bool a148 = false;
                            {
                                Checkpoint(parser); // r149

                                bool r149 = true;
                                r149 = r149 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r149, parser);
                                a148 = r149;
                            }

                            r145 |= a148;
                        } // end may a148

                        r145 = r145 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableList());
                        CommitOrRollback(r145, parser);
                        a144 = r145;
                    }

                    r143 |= a144;
                } // end may a144

                CommitOrRollback(r143, parser);
                res = r143;
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
                Checkpoint(parser); // r150

                bool r150 = true;
                r150 = r150 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableInclusion());
                r150 = r150 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r150 = r150 && Match(parser, new Jhu.Graywulf.Sql.Parsing.SimpleTableSource());
                CommitOrRollback(r150, parser);
                res = r150;
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
                Checkpoint(parser); // r151

                bool r151 = true;
                if (r151)
                { // may a152
                    bool a152 = false;
                    {
                        Checkpoint(parser); // r153

                        bool r153 = true;
                        if (r153)
                        { // alternatives a154 must
                            bool a154 = false;
                            if (!a154)
                            {
                                Checkpoint(parser); // r155

                                bool r155 = true;
                                r155 = r155 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"MUST"));
                                CommitOrRollback(r155, parser);
                                a154 = r155;
                            }

                            if (!a154)
                            {
                                Checkpoint(parser); // r156

                                bool r156 = true;
                                r156 = r156 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"MAY"));
                                CommitOrRollback(r156, parser);
                                a154 = r156;
                            }

                            if (!a154)
                            {
                                Checkpoint(parser); // r157

                                bool r157 = true;
                                r157 = r157 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"NOT"));
                                CommitOrRollback(r157, parser);
                                a154 = r157;
                            }

                            r153 &= a154;

                        } // end alternatives a154

                        r153 = r153 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r153, parser);
                        a152 = r153;
                    }

                    r151 |= a152;
                } // end may a152

                r151 = r151 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"EXIST"));
                r151 = r151 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r151 = r151 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"IN"));
                CommitOrRollback(r151, parser);
                res = r151;
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
                Checkpoint(parser); // r158

                bool r158 = true;
                r158 = r158 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"LIMIT"));
                r158 = r158 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r158 = r158 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchAlgorithm());
                r158 = r158 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r158 = r158 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TO"));
                r158 = r158 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                if (r158)
                { // alternatives a159 must
                    bool a159 = false;
                    if (!a159)
                    {
                        Checkpoint(parser); // r160

                        bool r160 = true;
                        r160 = r160 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Number());
                        CommitOrRollback(r160, parser);
                        a159 = r160;
                    }

                    if (!a159)
                    {
                        Checkpoint(parser); // r161

                        bool r161 = true;
                        r161 = r161 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableHint());
                        CommitOrRollback(r161, parser);
                        a159 = r161;
                    }

                    r158 &= a159;

                } // end alternatives a159

                CommitOrRollback(r158, parser);
                res = r158;
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
                Checkpoint(parser); // r162

                bool r162 = true;
                if (r162)
                { // alternatives a163 must
                    bool a163 = false;
                    if (!a163)
                    {
                        Checkpoint(parser); // r164

                        bool r164 = true;
                        r164 = r164 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"BAYESFACTOR"));
                        CommitOrRollback(r164, parser);
                        a163 = r164;
                    }

                    if (!a163)
                    {
                        Checkpoint(parser); // r165

                        bool r165 = true;
                        r165 = r165 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CONE"));
                        CommitOrRollback(r165, parser);
                        a163 = r165;
                    }

                    r162 &= a163;

                } // end alternatives a163

                CommitOrRollback(r162, parser);
                res = r162;
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

                if (r166)
                { // may a169
                    bool a169 = false;
                    {
                        Checkpoint(parser); // r170

                        bool r170 = true;
                        r170 = r170 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Statement());
                        CommitOrRollback(r170, parser);
                        a169 = r170;
                    }

                    r166 |= a169;
                } // end may a169

                if (r166)
                { // may a171
                    bool a171 = false;
                    {
                        Checkpoint(parser); // r172

                        bool r172 = true;
                        r172 = r172 && Match(parser, new Jhu.Graywulf.Sql.Parsing.StatementSeparator());
                        if (r172)
                        { // may a173
                            bool a173 = false;
                            {
                                Checkpoint(parser); // r174

                                bool r174 = true;
                                r174 = r174 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.StatementBlock());
                                CommitOrRollback(r174, parser);
                                a173 = r174;
                            }

                            r172 |= a173;
                        } // end may a173

                        CommitOrRollback(r172, parser);
                        a171 = r172;
                    }

                    r166 |= a171;
                } // end may a171

                if (r166)
                { // may a175
                    bool a175 = false;
                    {
                        Checkpoint(parser); // r176

                        bool r176 = true;
                        r176 = r176 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r176, parser);
                        a175 = r176;
                    }

                    r166 |= a175;
                } // end may a175

                CommitOrRollback(r166, parser);
                res = r166;
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
                Checkpoint(parser); // r177

                bool r177 = true;
                r177 = r177 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"BEGIN"));
                r177 = r177 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.StatementBlock());
                r177 = r177 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"END"));
                CommitOrRollback(r177, parser);
                res = r177;
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
                Checkpoint(parser); // r178

                bool r178 = true;
                r178 = r178 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"BEGIN"));
                r178 = r178 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r178 = r178 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TRY"));
                r178 = r178 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.StatementBlock());
                r178 = r178 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"END"));
                r178 = r178 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r178 = r178 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TRY"));
                r178 = r178 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r178 = r178 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"BEGIN"));
                r178 = r178 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r178 = r178 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CATCH"));
                if (r178)
                { // alternatives a179 must
                    bool a179 = false;
                    if (!a179)
                    {
                        Checkpoint(parser); // r180

                        bool r180 = true;
                        r180 = r180 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.StatementBlock());
                        CommitOrRollback(r180, parser);
                        a179 = r180;
                    }

                    if (!a179)
                    {
                        Checkpoint(parser); // r181

                        bool r181 = true;
                        r181 = r181 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r181, parser);
                        a179 = r181;
                    }

                    r178 &= a179;

                } // end alternatives a179

                r178 = r178 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"END"));
                r178 = r178 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r178 = r178 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CATCH"));
                CommitOrRollback(r178, parser);
                res = r178;
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
                Checkpoint(parser); // r182

                bool r182 = true;
                r182 = r182 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"WHILE"));
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

                r182 = r182 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BooleanExpression());
                if (r182)
                { // may a185
                    bool a185 = false;
                    {
                        Checkpoint(parser); // r186

                        bool r186 = true;
                        r186 = r186 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r186, parser);
                        a185 = r186;
                    }

                    r182 |= a185;
                } // end may a185

                r182 = r182 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Statement());
                CommitOrRollback(r182, parser);
                res = r182;
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
                Checkpoint(parser); // r187

                bool r187 = true;
                r187 = r187 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"IF"));
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

                r187 = r187 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BooleanExpression());
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

                r187 = r187 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Statement());
                if (r187)
                { // may a192
                    bool a192 = false;
                    {
                        Checkpoint(parser); // r193

                        bool r193 = true;
                        r193 = r193 && Match(parser, new Jhu.Graywulf.Sql.Parsing.StatementSeparator());
                        r193 = r193 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ELSE"));
                        r193 = r193 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        r193 = r193 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Statement());
                        CommitOrRollback(r193, parser);
                        a192 = r193;
                    }

                    r187 |= a192;
                } // end may a192

                CommitOrRollback(r187, parser);
                res = r187;
            }



            return res;
        }
    }


}