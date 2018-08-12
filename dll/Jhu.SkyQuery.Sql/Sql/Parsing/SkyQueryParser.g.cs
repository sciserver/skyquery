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





    public partial class AnyStatement : Jhu.Graywulf.Sql.Parsing.AnyStatement, ICloneable
    {
        public AnyStatement()
            :base()
        {
        }

        public AnyStatement(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public AnyStatement(Jhu.SkyQuery.Sql.Parsing.AnyStatement old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.AnyStatement(this);
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
                        r13 = r13 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.DeclareCursorStatement());
                        CommitOrRollback(r13, parser);
                        a2 = r13;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r14

                        bool r14 = true;
                        r14 = r14 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SetCursorStatement());
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
                        r17 = r17 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.DeclareVariableStatement());
                        CommitOrRollback(r17, parser);
                        a2 = r17;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r18

                        bool r18 = true;
                        r18 = r18 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SetVariableStatement());
                        CommitOrRollback(r18, parser);
                        a2 = r18;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r19

                        bool r19 = true;
                        r19 = r19 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.DeclareTableStatement());
                        CommitOrRollback(r19, parser);
                        a2 = r19;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r20

                        bool r20 = true;
                        r20 = r20 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.CreateTableStatement());
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
                        r26 = r26 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SelectStatement());
                        CommitOrRollback(r26, parser);
                        a2 = r26;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r27

                        bool r27 = true;
                        r27 = r27 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.InsertStatement());
                        CommitOrRollback(r27, parser);
                        a2 = r27;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r28

                        bool r28 = true;
                        r28 = r28 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.UpdateStatement());
                        CommitOrRollback(r28, parser);
                        a2 = r28;
                    }

                    if (!a2)
                    {
                        Checkpoint(parser); // r29

                        bool r29 = true;
                        r29 = r29 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.DeleteStatement());
                        CommitOrRollback(r29, parser);
                        a2 = r29;
                    }

                    r1 &= a2;

                } // end alternatives a2

                CommitOrRollback(r1, parser);
                res = r1;
            }



            return res;
        }
    }

    public partial class QueryExpression : Jhu.Graywulf.Sql.Extensions.Parsing.QueryExpression, ICloneable
    {
        public QueryExpression()
            :base()
        {
        }

        public QueryExpression(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public QueryExpression(Jhu.SkyQuery.Sql.Parsing.QueryExpression old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.QueryExpression(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r30

                bool r30 = true;
                if (r30)
                { // alternatives a31 must
                    bool a31 = false;
                    if (!a31)
                    {
                        Checkpoint(parser); // r32

                        bool r32 = true;
                        r32 = r32 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.QueryExpressionBrackets());
                        CommitOrRollback(r32, parser);
                        a31 = r32;
                    }

                    if (!a31)
                    {
                        Checkpoint(parser); // r33

                        bool r33 = true;
                        if (r33)
                        { // alternatives a34 must
                            bool a34 = false;
                            if (!a34)
                            {
                                Checkpoint(parser); // r35

                                bool r35 = true;
                                r35 = r35 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionQuerySpecification());
                                CommitOrRollback(r35, parser);
                                a34 = r35;
                            }

                            if (!a34)
                            {
                                Checkpoint(parser); // r36

                                bool r36 = true;
                                r36 = r36 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.QuerySpecification());
                                CommitOrRollback(r36, parser);
                                a34 = r36;
                            }

                            r33 &= a34;

                        } // end alternatives a34

                        CommitOrRollback(r33, parser);
                        a31 = r33;
                    }

                    r30 &= a31;

                } // end alternatives a31

                if (r30)
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

                        r38 = r38 && Match(parser, new Jhu.Graywulf.Sql.Parsing.QueryOperator());
                        if (r38)
                        { // may a41
                            bool a41 = false;
                            {
                                Checkpoint(parser); // r42

                                bool r42 = true;
                                r42 = r42 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r42, parser);
                                a41 = r42;
                            }

                            r38 |= a41;
                        } // end may a41

                        r38 = r38 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.QueryExpression());
                        CommitOrRollback(r38, parser);
                        a37 = r38;
                    }

                    r30 |= a37;
                } // end may a37

                CommitOrRollback(r30, parser);
                res = r30;
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
                Checkpoint(parser); // r43

                bool r43 = true;
                r43 = r43 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"SELECT"));
                if (r43)
                { // may a44
                    bool a44 = false;
                    {
                        Checkpoint(parser); // r45

                        bool r45 = true;
                        r45 = r45 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        if (r45)
                        { // alternatives a46 must
                            bool a46 = false;
                            if (!a46)
                            {
                                Checkpoint(parser); // r47

                                bool r47 = true;
                                r47 = r47 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ALL"));
                                CommitOrRollback(r47, parser);
                                a46 = r47;
                            }

                            if (!a46)
                            {
                                Checkpoint(parser); // r48

                                bool r48 = true;
                                r48 = r48 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DISTINCT"));
                                CommitOrRollback(r48, parser);
                                a46 = r48;
                            }

                            r45 &= a46;

                        } // end alternatives a46

                        CommitOrRollback(r45, parser);
                        a44 = r45;
                    }

                    r43 |= a44;
                } // end may a44

                if (r43)
                { // may a49
                    bool a49 = false;
                    {
                        Checkpoint(parser); // r50

                        bool r50 = true;
                        r50 = r50 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        r50 = r50 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TopExpression());
                        CommitOrRollback(r50, parser);
                        a49 = r50;
                    }

                    r43 |= a49;
                } // end may a49

                if (r43)
                { // may a51
                    bool a51 = false;
                    {
                        Checkpoint(parser); // r52

                        bool r52 = true;
                        r52 = r52 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r52, parser);
                        a51 = r52;
                    }

                    r43 |= a51;
                } // end may a51

                r43 = r43 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SelectList());
                if (r43)
                { // may a53
                    bool a53 = false;
                    {
                        Checkpoint(parser); // r54

                        bool r54 = true;
                        if (r54)
                        { // may a55
                            bool a55 = false;
                            {
                                Checkpoint(parser); // r56

                                bool r56 = true;
                                r56 = r56 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r56, parser);
                                a55 = r56;
                            }

                            r54 |= a55;
                        } // end may a55

                        r54 = r54 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.IntoClause());
                        CommitOrRollback(r54, parser);
                        a53 = r54;
                    }

                    r43 |= a53;
                } // end may a53

                if (r43)
                { // may a57
                    bool a57 = false;
                    {
                        Checkpoint(parser); // r58

                        bool r58 = true;
                        if (r58)
                        { // may a59
                            bool a59 = false;
                            {
                                Checkpoint(parser); // r60

                                bool r60 = true;
                                r60 = r60 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r60, parser);
                                a59 = r60;
                            }

                            r58 |= a59;
                        } // end may a59

                        if (r58)
                        { // alternatives a61 must
                            bool a61 = false;
                            if (!a61)
                            {
                                Checkpoint(parser); // r62

                                bool r62 = true;
                                r62 = r62 && Match(parser, new Jhu.Graywulf.Sql.Extensions.Parsing.PartitionedFromClause());
                                CommitOrRollback(r62, parser);
                                a61 = r62;
                            }

                            if (!a61)
                            {
                                Checkpoint(parser); // r63

                                bool r63 = true;
                                r63 = r63 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.FromClause());
                                CommitOrRollback(r63, parser);
                                a61 = r63;
                            }

                            r58 &= a61;

                        } // end alternatives a61

                        CommitOrRollback(r58, parser);
                        a57 = r58;
                    }

                    r43 |= a57;
                } // end may a57

                if (r43)
                { // may a64
                    bool a64 = false;
                    {
                        Checkpoint(parser); // r65

                        bool r65 = true;
                        if (r65)
                        { // may a66
                            bool a66 = false;
                            {
                                Checkpoint(parser); // r67

                                bool r67 = true;
                                r67 = r67 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r67, parser);
                                a66 = r67;
                            }

                            r65 |= a66;
                        } // end may a66

                        r65 = r65 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.WhereClause());
                        CommitOrRollback(r65, parser);
                        a64 = r65;
                    }

                    r43 |= a64;
                } // end may a64

                if (r43)
                { // may a68
                    bool a68 = false;
                    {
                        Checkpoint(parser); // r69

                        bool r69 = true;
                        if (r69)
                        { // may a70
                            bool a70 = false;
                            {
                                Checkpoint(parser); // r71

                                bool r71 = true;
                                r71 = r71 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r71, parser);
                                a70 = r71;
                            }

                            r69 |= a70;
                        } // end may a70

                        r69 = r69 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionClause());
                        CommitOrRollback(r69, parser);
                        a68 = r69;
                    }

                    r43 |= a68;
                } // end may a68

                if (r43)
                { // may a72
                    bool a72 = false;
                    {
                        Checkpoint(parser); // r73

                        bool r73 = true;
                        if (r73)
                        { // may a74
                            bool a74 = false;
                            {
                                Checkpoint(parser); // r75

                                bool r75 = true;
                                r75 = r75 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r75, parser);
                                a74 = r75;
                            }

                            r73 |= a74;
                        } // end may a74

                        r73 = r73 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.GroupByClause());
                        CommitOrRollback(r73, parser);
                        a72 = r73;
                    }

                    r43 |= a72;
                } // end may a72

                if (r43)
                { // may a76
                    bool a76 = false;
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

                        r77 = r77 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.HavingClause());
                        CommitOrRollback(r77, parser);
                        a76 = r77;
                    }

                    r43 |= a76;
                } // end may a76

                CommitOrRollback(r43, parser);
                res = r43;
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
                Checkpoint(parser); // r80

                bool r80 = true;
                r80 = r80 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"REGION"));
                if (r80)
                { // may a81
                    bool a81 = false;
                    {
                        Checkpoint(parser); // r82

                        bool r82 = true;
                        r82 = r82 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r82, parser);
                        a81 = r82;
                    }

                    r80 |= a81;
                } // end may a81

                if (r80)
                { // alternatives a83 must
                    bool a83 = false;
                    if (!a83)
                    {
                        Checkpoint(parser); // r84

                        bool r84 = true;
                        r84 = r84 && Match(parser, new Jhu.Graywulf.Sql.Parsing.StringConstant());
                        CommitOrRollback(r84, parser);
                        a83 = r84;
                    }

                    if (!a83)
                    {
                        Checkpoint(parser); // r85

                        bool r85 = true;
                        r85 = r85 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionExpression());
                        CommitOrRollback(r85, parser);
                        a83 = r85;
                    }

                    r80 &= a83;

                } // end alternatives a83

                CommitOrRollback(r80, parser);
                res = r80;
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
                Checkpoint(parser); // r86

                bool r86 = true;
                if (r86)
                { // may a87
                    bool a87 = false;
                    {
                        Checkpoint(parser); // r88

                        bool r88 = true;
                        r88 = r88 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionNotOperator());
                        if (r88)
                        { // may a89
                            bool a89 = false;
                            {
                                Checkpoint(parser); // r90

                                bool r90 = true;
                                r90 = r90 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r90, parser);
                                a89 = r90;
                            }

                            r88 |= a89;
                        } // end may a89

                        CommitOrRollback(r88, parser);
                        a87 = r88;
                    }

                    r86 |= a87;
                } // end may a87

                if (r86)
                { // alternatives a91 must
                    bool a91 = false;
                    if (!a91)
                    {
                        Checkpoint(parser); // r92

                        bool r92 = true;
                        r92 = r92 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionShape());
                        CommitOrRollback(r92, parser);
                        a91 = r92;
                    }

                    if (!a91)
                    {
                        Checkpoint(parser); // r93

                        bool r93 = true;
                        r93 = r93 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionExpressionBrackets());
                        CommitOrRollback(r93, parser);
                        a91 = r93;
                    }

                    r86 &= a91;

                } // end alternatives a91

                if (r86)
                { // may a94
                    bool a94 = false;
                    {
                        Checkpoint(parser); // r95

                        bool r95 = true;
                        if (r95)
                        { // may a96
                            bool a96 = false;
                            {
                                Checkpoint(parser); // r97

                                bool r97 = true;
                                r97 = r97 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r97, parser);
                                a96 = r97;
                            }

                            r95 |= a96;
                        } // end may a96

                        r95 = r95 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionOperator());
                        if (r95)
                        { // may a98
                            bool a98 = false;
                            {
                                Checkpoint(parser); // r99

                                bool r99 = true;
                                r99 = r99 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r99, parser);
                                a98 = r99;
                            }

                            r95 |= a98;
                        } // end may a98

                        r95 = r95 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionExpression());
                        CommitOrRollback(r95, parser);
                        a94 = r95;
                    }

                    r86 |= a94;
                } // end may a94

                CommitOrRollback(r86, parser);
                res = r86;
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
                Checkpoint(parser); // r100

                bool r100 = true;
                r100 = r100 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r100)
                { // may a101
                    bool a101 = false;
                    {
                        Checkpoint(parser); // r102

                        bool r102 = true;
                        r102 = r102 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r102, parser);
                        a101 = r102;
                    }

                    r100 |= a101;
                } // end may a101

                r100 = r100 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionExpression());
                if (r100)
                { // may a103
                    bool a103 = false;
                    {
                        Checkpoint(parser); // r104

                        bool r104 = true;
                        r104 = r104 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r104, parser);
                        a103 = r104;
                    }

                    r100 |= a103;
                } // end may a103

                r100 = r100 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r100, parser);
                res = r100;
            }



            return res;
        }
    }

    public partial class RegionNotOperator : Jhu.Graywulf.Sql.Parsing.Operator, ICloneable
    {
        public RegionNotOperator()
            :base()
        {
        }

        public RegionNotOperator(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public RegionNotOperator(Jhu.SkyQuery.Sql.Parsing.RegionNotOperator old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.RegionNotOperator(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r105

                bool r105 = true;
                if (r105)
                { // alternatives a106 must
                    bool a106 = false;
                    if (!a106)
                    {
                        Checkpoint(parser); // r107

                        bool r107 = true;
                        r107 = r107 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"NOT"));
                        CommitOrRollback(r107, parser);
                        a106 = r107;
                    }

                    r105 &= a106;

                } // end alternatives a106

                CommitOrRollback(r105, parser);
                res = r105;
            }



            return res;
        }
    }

    public partial class RegionOperator : Jhu.Graywulf.Sql.Parsing.Operator, ICloneable
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
                if (r113)
                { // alternatives a114 must
                    bool a114 = false;
                    if (!a114)
                    {
                        Checkpoint(parser); // r115

                        bool r115 = true;
                        r115 = r115 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CIRCLE"));
                        CommitOrRollback(r115, parser);
                        a114 = r115;
                    }

                    if (!a114)
                    {
                        Checkpoint(parser); // r116

                        bool r116 = true;
                        r116 = r116 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CIRC"));
                        CommitOrRollback(r116, parser);
                        a114 = r116;
                    }

                    if (!a114)
                    {
                        Checkpoint(parser); // r117

                        bool r117 = true;
                        r117 = r117 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"RECTANGLE"));
                        CommitOrRollback(r117, parser);
                        a114 = r117;
                    }

                    if (!a114)
                    {
                        Checkpoint(parser); // r118

                        bool r118 = true;
                        r118 = r118 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"RECT"));
                        CommitOrRollback(r118, parser);
                        a114 = r118;
                    }

                    if (!a114)
                    {
                        Checkpoint(parser); // r119

                        bool r119 = true;
                        r119 = r119 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"POLYGON"));
                        CommitOrRollback(r119, parser);
                        a114 = r119;
                    }

                    if (!a114)
                    {
                        Checkpoint(parser); // r120

                        bool r120 = true;
                        r120 = r120 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"POLY"));
                        CommitOrRollback(r120, parser);
                        a114 = r120;
                    }

                    if (!a114)
                    {
                        Checkpoint(parser); // r121

                        bool r121 = true;
                        r121 = r121 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CONVEX_HULL"));
                        CommitOrRollback(r121, parser);
                        a114 = r121;
                    }

                    if (!a114)
                    {
                        Checkpoint(parser); // r122

                        bool r122 = true;
                        r122 = r122 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CHULL"));
                        CommitOrRollback(r122, parser);
                        a114 = r122;
                    }

                    r113 &= a114;

                } // end alternatives a114

                if (r113)
                { // may a123
                    bool a123 = false;
                    {
                        Checkpoint(parser); // r124

                        bool r124 = true;
                        r124 = r124 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r124, parser);
                        a123 = r124;
                    }

                    r113 |= a123;
                } // end may a123

                r113 = r113 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r113)
                { // may a125
                    bool a125 = false;
                    {
                        Checkpoint(parser); // r126

                        bool r126 = true;
                        r126 = r126 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r126, parser);
                        a125 = r126;
                    }

                    r113 |= a125;
                } // end may a125

                r113 = r113 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionArgumentList());
                if (r113)
                { // may a127
                    bool a127 = false;
                    {
                        Checkpoint(parser); // r128

                        bool r128 = true;
                        r128 = r128 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r128, parser);
                        a127 = r128;
                    }

                    r113 |= a127;
                } // end may a127

                r113 = r113 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r113, parser);
                res = r113;
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
                Checkpoint(parser); // r129

                bool r129 = true;
                r129 = r129 && Match(parser, new Jhu.Graywulf.Sql.Parsing.NumericConstant());
                CommitOrRollback(r129, parser);
                res = r129;
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

                r130 = r130 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionArgument());
                if (r130)
                { // may a133
                    bool a133 = false;
                    {
                        Checkpoint(parser); // r134

                        bool r134 = true;
                        if (r134)
                        { // may a135
                            bool a135 = false;
                            {
                                Checkpoint(parser); // r136

                                bool r136 = true;
                                r136 = r136 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r136, parser);
                                a135 = r136;
                            }

                            r134 |= a135;
                        } // end may a135

                        r134 = r134 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        r134 = r134 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionArgumentList());
                        CommitOrRollback(r134, parser);
                        a133 = r134;
                    }

                    r130 |= a133;
                } // end may a133

                CommitOrRollback(r130, parser);
                res = r130;
            }



            return res;
        }
    }

    public partial class XMatchSelectStatement : Jhu.Graywulf.Sql.Parsing.SelectStatement, ICloneable
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
                Checkpoint(parser); // r137

                bool r137 = true;
                r137 = r137 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchQueryExpression());
                if (r137)
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

                        r139 = r139 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OrderByClause());
                        CommitOrRollback(r139, parser);
                        a138 = r139;
                    }

                    r137 |= a138;
                } // end may a138

                if (r137)
                { // may a142
                    bool a142 = false;
                    {
                        Checkpoint(parser); // r143

                        bool r143 = true;
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

                        r143 = r143 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OptionClause());
                        CommitOrRollback(r143, parser);
                        a142 = r143;
                    }

                    r137 |= a142;
                } // end may a142

                CommitOrRollback(r137, parser);
                res = r137;
            }



            return res;
        }
    }

    public partial class XMatchQueryExpression : Jhu.SkyQuery.Sql.Parsing.QueryExpression, ICloneable
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
                Checkpoint(parser); // r146

                bool r146 = true;
                r146 = r146 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchQuerySpecification());
                CommitOrRollback(r146, parser);
                res = r146;
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
                Checkpoint(parser); // r147

                bool r147 = true;
                r147 = r147 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"SELECT"));
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

                r147 = r147 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SelectList());
                if (r147)
                { // may a150
                    bool a150 = false;
                    {
                        Checkpoint(parser); // r151

                        bool r151 = true;
                        if (r151)
                        { // may a152
                            bool a152 = false;
                            {
                                Checkpoint(parser); // r153

                                bool r153 = true;
                                r153 = r153 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r153, parser);
                                a152 = r153;
                            }

                            r151 |= a152;
                        } // end may a152

                        r151 = r151 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.IntoClause());
                        CommitOrRollback(r151, parser);
                        a150 = r151;
                    }

                    r147 |= a150;
                } // end may a150

                if (r147)
                { // may a154
                    bool a154 = false;
                    {
                        Checkpoint(parser); // r155

                        bool r155 = true;
                        r155 = r155 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r155, parser);
                        a154 = r155;
                    }

                    r147 |= a154;
                } // end may a154

                r147 = r147 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchFromClause());
                if (r147)
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

                        r157 = r157 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.WhereClause());
                        CommitOrRollback(r157, parser);
                        a156 = r157;
                    }

                    r147 |= a156;
                } // end may a156

                if (r147)
                { // may a160
                    bool a160 = false;
                    {
                        Checkpoint(parser); // r161

                        bool r161 = true;
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

                        r161 = r161 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.RegionClause());
                        CommitOrRollback(r161, parser);
                        a160 = r161;
                    }

                    r147 |= a160;
                } // end may a160

                CommitOrRollback(r147, parser);
                res = r147;
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
                Checkpoint(parser); // r164

                bool r164 = true;
                r164 = r164 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"FROM"));
                if (r164)
                { // may a165
                    bool a165 = false;
                    {
                        Checkpoint(parser); // r166

                        bool r166 = true;
                        r166 = r166 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r166, parser);
                        a165 = r166;
                    }

                    r164 |= a165;
                } // end may a165

                r164 = r164 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableSourceExpression());
                CommitOrRollback(r164, parser);
                res = r164;
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
                Checkpoint(parser); // r167

                bool r167 = true;
                r167 = r167 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableSourceSpecification());
                if (r167)
                { // may a168
                    bool a168 = false;
                    {
                        Checkpoint(parser); // r169

                        bool r169 = true;
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

                        r169 = r169 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.JoinedTable());
                        CommitOrRollback(r169, parser);
                        a168 = r169;
                    }

                    r167 |= a168;
                } // end may a168

                CommitOrRollback(r167, parser);
                res = r167;
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
                Checkpoint(parser); // r172

                bool r172 = true;
                r172 = r172 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableSource());
                CommitOrRollback(r172, parser);
                res = r172;
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

        public XMatchTableSource(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
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
                Checkpoint(parser); // r173

                bool r173 = true;
                r173 = r173 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"XMATCH"));
                if (r173)
                { // may a174
                    bool a174 = false;
                    {
                        Checkpoint(parser); // r175

                        bool r175 = true;
                        r175 = r175 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r175, parser);
                        a174 = r175;
                    }

                    r173 |= a174;
                } // end may a174

                r173 = r173 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r173)
                { // may a176
                    bool a176 = false;
                    {
                        Checkpoint(parser); // r177

                        bool r177 = true;
                        r177 = r177 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r177, parser);
                        a176 = r177;
                    }

                    r173 |= a176;
                } // end may a176

                r173 = r173 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableList());
                if (r173)
                { // may a178
                    bool a178 = false;
                    {
                        Checkpoint(parser); // r179

                        bool r179 = true;
                        r179 = r179 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r179, parser);
                        a178 = r179;
                    }

                    r173 |= a178;
                } // end may a178

                r173 = r173 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                if (r173)
                { // may a180
                    bool a180 = false;
                    {
                        Checkpoint(parser); // r181

                        bool r181 = true;
                        r181 = r181 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r181, parser);
                        a180 = r181;
                    }

                    r173 |= a180;
                } // end may a180

                r173 = r173 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchConstraint());
                if (r173)
                { // may a182
                    bool a182 = false;
                    {
                        Checkpoint(parser); // r183

                        bool r183 = true;
                        r183 = r183 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r183, parser);
                        a182 = r183;
                    }

                    r173 |= a182;
                } // end may a182

                r173 = r173 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                if (r173)
                { // may a184
                    bool a184 = false;
                    {
                        Checkpoint(parser); // r185

                        bool r185 = true;
                        r185 = r185 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r185, parser);
                        a184 = r185;
                    }

                    r173 |= a184;
                } // end may a184

                if (r173)
                { // may a186
                    bool a186 = false;
                    {
                        Checkpoint(parser); // r187

                        bool r187 = true;
                        r187 = r187 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"AS"));
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

                        CommitOrRollback(r187, parser);
                        a186 = r187;
                    }

                    r173 |= a186;
                } // end may a186

                r173 = r173 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableAlias());
                CommitOrRollback(r173, parser);
                res = r173;
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
                Checkpoint(parser); // r190

                bool r190 = true;
                r190 = r190 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableSpecification());
                if (r190)
                { // may a191
                    bool a191 = false;
                    {
                        Checkpoint(parser); // r192

                        bool r192 = true;
                        if (r192)
                        { // may a193
                            bool a193 = false;
                            {
                                Checkpoint(parser); // r194

                                bool r194 = true;
                                r194 = r194 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r194, parser);
                                a193 = r194;
                            }

                            r192 |= a193;
                        } // end may a193

                        r192 = r192 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r192)
                        { // may a195
                            bool a195 = false;
                            {
                                Checkpoint(parser); // r196

                                bool r196 = true;
                                r196 = r196 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r196, parser);
                                a195 = r196;
                            }

                            r192 |= a195;
                        } // end may a195

                        r192 = r192 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableList());
                        CommitOrRollback(r192, parser);
                        a191 = r192;
                    }

                    r190 |= a191;
                } // end may a191

                CommitOrRollback(r190, parser);
                res = r190;
            }



            return res;
        }
    }

    public partial class XMatchTableSpecification : Jhu.Graywulf.Sql.Parsing.TableSourceSpecification, ICloneable
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
                Checkpoint(parser); // r197

                bool r197 = true;
                r197 = r197 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableInclusion());
                if (r197)
                { // may a198
                    bool a198 = false;
                    {
                        Checkpoint(parser); // r199

                        bool r199 = true;
                        r199 = r199 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r199, parser);
                        a198 = r199;
                    }

                    r197 |= a198;
                } // end may a198

                r197 = r197 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SimpleTableSource());
                CommitOrRollback(r197, parser);
                res = r197;
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
                Checkpoint(parser); // r200

                bool r200 = true;
                if (r200)
                { // may a201
                    bool a201 = false;
                    {
                        Checkpoint(parser); // r202

                        bool r202 = true;
                        if (r202)
                        { // alternatives a203 must
                            bool a203 = false;
                            if (!a203)
                            {
                                Checkpoint(parser); // r204

                                bool r204 = true;
                                r204 = r204 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"MUST"));
                                CommitOrRollback(r204, parser);
                                a203 = r204;
                            }

                            if (!a203)
                            {
                                Checkpoint(parser); // r205

                                bool r205 = true;
                                r205 = r205 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"MAY"));
                                CommitOrRollback(r205, parser);
                                a203 = r205;
                            }

                            if (!a203)
                            {
                                Checkpoint(parser); // r206

                                bool r206 = true;
                                r206 = r206 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"NOT"));
                                CommitOrRollback(r206, parser);
                                a203 = r206;
                            }

                            r202 &= a203;

                        } // end alternatives a203

                        r202 = r202 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r202, parser);
                        a201 = r202;
                    }

                    r200 |= a201;
                } // end may a201

                r200 = r200 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"EXIST"));
                r200 = r200 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r200 = r200 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"IN"));
                CommitOrRollback(r200, parser);
                res = r200;
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
                Checkpoint(parser); // r207

                bool r207 = true;
                r207 = r207 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"LIMIT"));
                r207 = r207 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r207 = r207 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchAlgorithm());
                r207 = r207 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r207 = r207 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TO"));
                r207 = r207 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                if (r207)
                { // alternatives a208 must
                    bool a208 = false;
                    if (!a208)
                    {
                        Checkpoint(parser); // r209

                        bool r209 = true;
                        r209 = r209 && Match(parser, new Jhu.Graywulf.Sql.Parsing.NumericConstant());
                        CommitOrRollback(r209, parser);
                        a208 = r209;
                    }

                    if (!a208)
                    {
                        Checkpoint(parser); // r210

                        bool r210 = true;
                        r210 = r210 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableHint());
                        CommitOrRollback(r210, parser);
                        a208 = r210;
                    }

                    r207 &= a208;

                } // end alternatives a208

                CommitOrRollback(r207, parser);
                res = r207;
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
                Checkpoint(parser); // r211

                bool r211 = true;
                if (r211)
                { // alternatives a212 must
                    bool a212 = false;
                    if (!a212)
                    {
                        Checkpoint(parser); // r213

                        bool r213 = true;
                        r213 = r213 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"BAYESFACTOR"));
                        CommitOrRollback(r213, parser);
                        a212 = r213;
                    }

                    if (!a212)
                    {
                        Checkpoint(parser); // r214

                        bool r214 = true;
                        r214 = r214 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CONE"));
                        CommitOrRollback(r214, parser);
                        a212 = r214;
                    }

                    r211 &= a212;

                } // end alternatives a212

                CommitOrRollback(r211, parser);
                res = r211;
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
                Checkpoint(parser); // r215

                bool r215 = true;
                if (r215)
                { // may a216
                    bool a216 = false;
                    {
                        Checkpoint(parser); // r217

                        bool r217 = true;
                        r217 = r217 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r217, parser);
                        a216 = r217;
                    }

                    r215 |= a216;
                } // end may a216

                if (r215)
                { // may a218
                    bool a218 = false;
                    {
                        Checkpoint(parser); // r219

                        bool r219 = true;
                        r219 = r219 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.AnyStatement());
                        CommitOrRollback(r219, parser);
                        a218 = r219;
                    }

                    r215 |= a218;
                } // end may a218

                if (r215)
                { // may a220
                    bool a220 = false;
                    {
                        Checkpoint(parser); // r221

                        bool r221 = true;
                        r221 = r221 && Match(parser, new Jhu.Graywulf.Sql.Parsing.StatementSeparator());
                        if (r221)
                        { // may a222
                            bool a222 = false;
                            {
                                Checkpoint(parser); // r223

                                bool r223 = true;
                                r223 = r223 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.StatementBlock());
                                CommitOrRollback(r223, parser);
                                a222 = r223;
                            }

                            r221 |= a222;
                        } // end may a222

                        CommitOrRollback(r221, parser);
                        a220 = r221;
                    }

                    r215 |= a220;
                } // end may a220

                if (r215)
                { // may a224
                    bool a224 = false;
                    {
                        Checkpoint(parser); // r225

                        bool r225 = true;
                        r225 = r225 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r225, parser);
                        a224 = r225;
                    }

                    r215 |= a224;
                } // end may a224

                CommitOrRollback(r215, parser);
                res = r215;
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
                Checkpoint(parser); // r226

                bool r226 = true;
                r226 = r226 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"BEGIN"));
                r226 = r226 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.StatementBlock());
                r226 = r226 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"END"));
                CommitOrRollback(r226, parser);
                res = r226;
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
                Checkpoint(parser); // r227

                bool r227 = true;
                r227 = r227 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"BEGIN"));
                r227 = r227 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r227 = r227 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TRY"));
                r227 = r227 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.StatementBlock());
                r227 = r227 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"END"));
                r227 = r227 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r227 = r227 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TRY"));
                r227 = r227 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r227 = r227 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"BEGIN"));
                r227 = r227 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r227 = r227 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CATCH"));
                if (r227)
                { // alternatives a228 must
                    bool a228 = false;
                    if (!a228)
                    {
                        Checkpoint(parser); // r229

                        bool r229 = true;
                        r229 = r229 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.StatementBlock());
                        CommitOrRollback(r229, parser);
                        a228 = r229;
                    }

                    if (!a228)
                    {
                        Checkpoint(parser); // r230

                        bool r230 = true;
                        r230 = r230 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r230, parser);
                        a228 = r230;
                    }

                    r227 &= a228;

                } // end alternatives a228

                r227 = r227 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"END"));
                r227 = r227 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r227 = r227 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CATCH"));
                CommitOrRollback(r227, parser);
                res = r227;
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
                Checkpoint(parser); // r231

                bool r231 = true;
                r231 = r231 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"WHILE"));
                if (r231)
                { // may a232
                    bool a232 = false;
                    {
                        Checkpoint(parser); // r233

                        bool r233 = true;
                        r233 = r233 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r233, parser);
                        a232 = r233;
                    }

                    r231 |= a232;
                } // end may a232

                r231 = r231 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.LogicalExpression());
                if (r231)
                { // may a234
                    bool a234 = false;
                    {
                        Checkpoint(parser); // r235

                        bool r235 = true;
                        r235 = r235 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r235, parser);
                        a234 = r235;
                    }

                    r231 |= a234;
                } // end may a234

                r231 = r231 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.AnyStatement());
                CommitOrRollback(r231, parser);
                res = r231;
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
                Checkpoint(parser); // r236

                bool r236 = true;
                r236 = r236 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"IF"));
                if (r236)
                { // may a237
                    bool a237 = false;
                    {
                        Checkpoint(parser); // r238

                        bool r238 = true;
                        r238 = r238 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r238, parser);
                        a237 = r238;
                    }

                    r236 |= a237;
                } // end may a237

                r236 = r236 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.LogicalExpression());
                if (r236)
                { // may a239
                    bool a239 = false;
                    {
                        Checkpoint(parser); // r240

                        bool r240 = true;
                        r240 = r240 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r240, parser);
                        a239 = r240;
                    }

                    r236 |= a239;
                } // end may a239

                r236 = r236 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.AnyStatement());
                if (r236)
                { // may a241
                    bool a241 = false;
                    {
                        Checkpoint(parser); // r242

                        bool r242 = true;
                        r242 = r242 && Match(parser, new Jhu.Graywulf.Sql.Parsing.StatementSeparator());
                        r242 = r242 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ELSE"));
                        r242 = r242 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        r242 = r242 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.AnyStatement());
                        CommitOrRollback(r242, parser);
                        a241 = r242;
                    }

                    r236 |= a241;
                } // end may a241

                CommitOrRollback(r236, parser);
                res = r236;
            }



            return res;
        }
    }

    public partial class SelectStatement : Jhu.Graywulf.Sql.Parsing.SelectStatement, ICloneable
    {
        public SelectStatement()
            :base()
        {
        }

        public SelectStatement(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public SelectStatement(Jhu.SkyQuery.Sql.Parsing.SelectStatement old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.SelectStatement(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r243

                bool r243 = true;
                if (r243)
                { // may a244
                    bool a244 = false;
                    {
                        Checkpoint(parser); // r245

                        bool r245 = true;
                        r245 = r245 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommonTableExpression());
                        if (r245)
                        { // may a246
                            bool a246 = false;
                            {
                                Checkpoint(parser); // r247

                                bool r247 = true;
                                r247 = r247 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r247, parser);
                                a246 = r247;
                            }

                            r245 |= a246;
                        } // end may a246

                        CommitOrRollback(r245, parser);
                        a244 = r245;
                    }

                    r243 |= a244;
                } // end may a244

                r243 = r243 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.QueryExpression());
                if (r243)
                { // may a248
                    bool a248 = false;
                    {
                        Checkpoint(parser); // r249

                        bool r249 = true;
                        if (r249)
                        { // may a250
                            bool a250 = false;
                            {
                                Checkpoint(parser); // r251

                                bool r251 = true;
                                r251 = r251 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r251, parser);
                                a250 = r251;
                            }

                            r249 |= a250;
                        } // end may a250

                        r249 = r249 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OrderByClause());
                        CommitOrRollback(r249, parser);
                        a248 = r249;
                    }

                    r243 |= a248;
                } // end may a248

                if (r243)
                { // may a252
                    bool a252 = false;
                    {
                        Checkpoint(parser); // r253

                        bool r253 = true;
                        if (r253)
                        { // may a254
                            bool a254 = false;
                            {
                                Checkpoint(parser); // r255

                                bool r255 = true;
                                r255 = r255 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r255, parser);
                                a254 = r255;
                            }

                            r253 |= a254;
                        } // end may a254

                        r253 = r253 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OptionClause());
                        CommitOrRollback(r253, parser);
                        a252 = r253;
                    }

                    r243 |= a252;
                } // end may a252

                CommitOrRollback(r243, parser);
                res = r243;
            }



            return res;
        }
    }

    public partial class CursorDefinition : Jhu.Graywulf.Sql.Parsing.CursorDefinition, ICloneable
    {
        public CursorDefinition()
            :base()
        {
        }

        public CursorDefinition(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public CursorDefinition(Jhu.SkyQuery.Sql.Parsing.CursorDefinition old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.CursorDefinition(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r256

                bool r256 = true;
                r256 = r256 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CURSOR"));
                r256 = r256 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r256 = r256 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"FOR"));
                r256 = r256 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r256 = r256 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SelectStatement());
                CommitOrRollback(r256, parser);
                res = r256;
            }



            return res;
        }
    }

    public partial class DeclareCursorStatement : Jhu.Graywulf.Sql.Parsing.DeclareCursorStatement, ICloneable
    {
        public DeclareCursorStatement()
            :base()
        {
        }

        public DeclareCursorStatement(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public DeclareCursorStatement(Jhu.SkyQuery.Sql.Parsing.DeclareCursorStatement old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.DeclareCursorStatement(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r257

                bool r257 = true;
                r257 = r257 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DECLARE"));
                if (r257)
                { // may a258
                    bool a258 = false;
                    {
                        Checkpoint(parser); // r259

                        bool r259 = true;
                        r259 = r259 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r259, parser);
                        a258 = r259;
                    }

                    r257 |= a258;
                } // end may a258

                if (r257)
                { // alternatives a260 must
                    bool a260 = false;
                    if (!a260)
                    {
                        Checkpoint(parser); // r261

                        bool r261 = true;
                        r261 = r261 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CursorName());
                        if (r261)
                        { // may a262
                            bool a262 = false;
                            {
                                Checkpoint(parser); // r263

                                bool r263 = true;
                                if (r263)
                                { // may a264
                                    bool a264 = false;
                                    {
                                        Checkpoint(parser); // r265

                                        bool r265 = true;
                                        r265 = r265 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                        CommitOrRollback(r265, parser);
                                        a264 = r265;
                                    }

                                    r263 |= a264;
                                } // end may a264

                                r263 = r263 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.CursorDefinition());
                                CommitOrRollback(r263, parser);
                                a262 = r263;
                            }

                            r261 |= a262;
                        } // end may a262

                        CommitOrRollback(r261, parser);
                        a260 = r261;
                    }

                    if (!a260)
                    {
                        Checkpoint(parser); // r266

                        bool r266 = true;
                        r266 = r266 && Match(parser, new Jhu.Graywulf.Sql.Parsing.UserVariable());
                        if (r266)
                        { // may a267
                            bool a267 = false;
                            {
                                Checkpoint(parser); // r268

                                bool r268 = true;
                                r268 = r268 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r268, parser);
                                a267 = r268;
                            }

                            r266 |= a267;
                        } // end may a267

                        r266 = r266 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CURSOR"));
                        CommitOrRollback(r266, parser);
                        a260 = r266;
                    }

                    r257 &= a260;

                } // end alternatives a260

                CommitOrRollback(r257, parser);
                res = r257;
            }



            return res;
        }
    }

    public partial class SetCursorStatement : Jhu.Graywulf.Sql.Parsing.SetCursorStatement, ICloneable
    {
        public SetCursorStatement()
            :base()
        {
        }

        public SetCursorStatement(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public SetCursorStatement(Jhu.SkyQuery.Sql.Parsing.SetCursorStatement old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.SetCursorStatement(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r269

                bool r269 = true;
                r269 = r269 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"SET"));
                if (r269)
                { // may a270
                    bool a270 = false;
                    {
                        Checkpoint(parser); // r271

                        bool r271 = true;
                        r271 = r271 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r271, parser);
                        a270 = r271;
                    }

                    r269 |= a270;
                } // end may a270

                r269 = r269 && Match(parser, new Jhu.Graywulf.Sql.Parsing.UserVariable());
                if (r269)
                { // may a272
                    bool a272 = false;
                    {
                        Checkpoint(parser); // r273

                        bool r273 = true;
                        r273 = r273 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r273, parser);
                        a272 = r273;
                    }

                    r269 |= a272;
                } // end may a272

                r269 = r269 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Equals1());
                if (r269)
                { // may a274
                    bool a274 = false;
                    {
                        Checkpoint(parser); // r275

                        bool r275 = true;
                        r275 = r275 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r275, parser);
                        a274 = r275;
                    }

                    r269 |= a274;
                } // end may a274

                r269 = r269 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.CursorDefinition());
                CommitOrRollback(r269, parser);
                res = r269;
            }



            return res;
        }
    }

    public partial class Subquery : Jhu.Graywulf.Sql.Parsing.Subquery, ICloneable
    {
        public Subquery()
            :base()
        {
        }

        public Subquery(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public Subquery(Jhu.SkyQuery.Sql.Parsing.Subquery old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.Subquery(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r276

                bool r276 = true;
                r276 = r276 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r276)
                { // may a277
                    bool a277 = false;
                    {
                        Checkpoint(parser); // r278

                        bool r278 = true;
                        r278 = r278 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r278, parser);
                        a277 = r278;
                    }

                    r276 |= a277;
                } // end may a277

                r276 = r276 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.QueryExpression());
                if (r276)
                { // may a279
                    bool a279 = false;
                    {
                        Checkpoint(parser); // r280

                        bool r280 = true;
                        if (r280)
                        { // may a281
                            bool a281 = false;
                            {
                                Checkpoint(parser); // r282

                                bool r282 = true;
                                r282 = r282 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r282, parser);
                                a281 = r282;
                            }

                            r280 |= a281;
                        } // end may a281

                        r280 = r280 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OrderByClause());
                        CommitOrRollback(r280, parser);
                        a279 = r280;
                    }

                    r276 |= a279;
                } // end may a279

                if (r276)
                { // may a283
                    bool a283 = false;
                    {
                        Checkpoint(parser); // r284

                        bool r284 = true;
                        r284 = r284 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r284, parser);
                        a283 = r284;
                    }

                    r276 |= a283;
                } // end may a283

                r276 = r276 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r276, parser);
                res = r276;
            }



            return res;
        }
    }

    public partial class ExpressionSubquery : Jhu.Graywulf.Sql.Parsing.ExpressionSubquery, ICloneable
    {
        public ExpressionSubquery()
            :base()
        {
        }

        public ExpressionSubquery(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public ExpressionSubquery(Jhu.SkyQuery.Sql.Parsing.ExpressionSubquery old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.ExpressionSubquery(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r285

                bool r285 = true;
                r285 = r285 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Subquery());
                CommitOrRollback(r285, parser);
                res = r285;
            }



            return res;
        }
    }

    public partial class Operand : Jhu.Graywulf.Sql.Parsing.Operand, ICloneable
    {
        public Operand()
            :base()
        {
        }

        public Operand(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public Operand(Jhu.SkyQuery.Sql.Parsing.Operand old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.Operand(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r286

                bool r286 = true;
                if (r286)
                { // alternatives a287 must
                    bool a287 = false;
                    if (!a287)
                    {
                        Checkpoint(parser); // r288

                        bool r288 = true;
                        r288 = r288 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Constant());
                        CommitOrRollback(r288, parser);
                        a287 = r288;
                    }

                    if (!a287)
                    {
                        Checkpoint(parser); // r289

                        bool r289 = true;
                        r289 = r289 && Match(parser, new Jhu.Graywulf.Sql.Parsing.SystemVariable());
                        CommitOrRollback(r289, parser);
                        a287 = r289;
                    }

                    if (!a287)
                    {
                        Checkpoint(parser); // r290

                        bool r290 = true;
                        r290 = r290 && Match(parser, new Jhu.Graywulf.Sql.Parsing.UserVariable());
                        CommitOrRollback(r290, parser);
                        a287 = r290;
                    }

                    if (!a287)
                    {
                        Checkpoint(parser); // r291

                        bool r291 = true;
                        r291 = r291 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ExpressionSubquery());
                        CommitOrRollback(r291, parser);
                        a287 = r291;
                    }

                    if (!a287)
                    {
                        Checkpoint(parser); // r292

                        bool r292 = true;
                        r292 = r292 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ExpressionBrackets());
                        CommitOrRollback(r292, parser);
                        a287 = r292;
                    }

                    if (!a287)
                    {
                        Checkpoint(parser); // r293

                        bool r293 = true;
                        r293 = r293 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SimpleCaseExpression());
                        CommitOrRollback(r293, parser);
                        a287 = r293;
                    }

                    if (!a287)
                    {
                        Checkpoint(parser); // r294

                        bool r294 = true;
                        r294 = r294 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SearchedCaseExpression());
                        CommitOrRollback(r294, parser);
                        a287 = r294;
                    }

                    if (!a287)
                    {
                        Checkpoint(parser); // r295

                        bool r295 = true;
                        r295 = r295 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.UdtStaticMemberAccessList());
                        CommitOrRollback(r295, parser);
                        a287 = r295;
                    }

                    if (!a287)
                    {
                        Checkpoint(parser); // r296

                        bool r296 = true;
                        r296 = r296 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.CastAndParseFunctionCall());
                        CommitOrRollback(r296, parser);
                        a287 = r296;
                    }

                    if (!a287)
                    {
                        Checkpoint(parser); // r297

                        bool r297 = true;
                        r297 = r297 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ConvertFunctionCall());
                        CommitOrRollback(r297, parser);
                        a287 = r297;
                    }

                    if (!a287)
                    {
                        Checkpoint(parser); // r298

                        bool r298 = true;
                        r298 = r298 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.DateFunctionCall());
                        CommitOrRollback(r298, parser);
                        a287 = r298;
                    }

                    if (!a287)
                    {
                        Checkpoint(parser); // r299

                        bool r299 = true;
                        r299 = r299 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.IifFunctionCall());
                        CommitOrRollback(r299, parser);
                        a287 = r299;
                    }

                    if (!a287)
                    {
                        Checkpoint(parser); // r300

                        bool r300 = true;
                        r300 = r300 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.StarFunctionCall());
                        CommitOrRollback(r300, parser);
                        a287 = r300;
                    }

                    if (!a287)
                    {
                        Checkpoint(parser); // r301

                        bool r301 = true;
                        r301 = r301 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.AggregateFunctionCall());
                        CommitOrRollback(r301, parser);
                        a287 = r301;
                    }

                    if (!a287)
                    {
                        Checkpoint(parser); // r302

                        bool r302 = true;
                        r302 = r302 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.WindowedFunctionCall());
                        CommitOrRollback(r302, parser);
                        a287 = r302;
                    }

                    if (!a287)
                    {
                        Checkpoint(parser); // r303

                        bool r303 = true;
                        r303 = r303 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SystemFunctionCall());
                        CommitOrRollback(r303, parser);
                        a287 = r303;
                    }

                    if (!a287)
                    {
                        Checkpoint(parser); // r304

                        bool r304 = true;
                        r304 = r304 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ObjectName());
                        CommitOrRollback(r304, parser);
                        a287 = r304;
                    }

                    r286 &= a287;

                } // end alternatives a287

                if (r286)
                { // may a305
                    bool a305 = false;
                    {
                        Checkpoint(parser); // r306

                        bool r306 = true;
                        if (r306)
                        { // may a307
                            bool a307 = false;
                            {
                                Checkpoint(parser); // r308

                                bool r308 = true;
                                r308 = r308 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r308, parser);
                                a307 = r308;
                            }

                            r306 |= a307;
                        } // end may a307

                        r306 = r306 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.MemberAccessList());
                        CommitOrRollback(r306, parser);
                        a305 = r306;
                    }

                    r286 |= a305;
                } // end may a305

                CommitOrRollback(r286, parser);
                res = r286;
            }



            return res;
        }
    }

    public partial class Expression : Jhu.Graywulf.Sql.Parsing.Expression, ICloneable
    {
        public Expression()
            :base()
        {
        }

        public Expression(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public Expression(Jhu.SkyQuery.Sql.Parsing.Expression old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.Expression(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r309

                bool r309 = true;
                if (r309)
                { // alternatives a310 must
                    bool a310 = false;
                    if (!a310)
                    {
                        Checkpoint(parser); // r311

                        bool r311 = true;
                        r311 = r311 && Match(parser, new Jhu.Graywulf.Sql.Parsing.UnaryOperator());
                        if (r311)
                        { // may a312
                            bool a312 = false;
                            {
                                Checkpoint(parser); // r313

                                bool r313 = true;
                                r313 = r313 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r313, parser);
                                a312 = r313;
                            }

                            r311 |= a312;
                        } // end may a312

                        r311 = r311 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        CommitOrRollback(r311, parser);
                        a310 = r311;
                    }

                    if (!a310)
                    {
                        Checkpoint(parser); // r314

                        bool r314 = true;
                        r314 = r314 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Operand());
                        CommitOrRollback(r314, parser);
                        a310 = r314;
                    }

                    r309 &= a310;

                } // end alternatives a310

                if (r309)
                { // may a315
                    bool a315 = false;
                    {
                        Checkpoint(parser); // r316

                        bool r316 = true;
                        if (r316)
                        { // may a317
                            bool a317 = false;
                            {
                                Checkpoint(parser); // r318

                                bool r318 = true;
                                r318 = r318 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r318, parser);
                                a317 = r318;
                            }

                            r316 |= a317;
                        } // end may a317

                        r316 = r316 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BinaryOperator());
                        if (r316)
                        { // may a319
                            bool a319 = false;
                            {
                                Checkpoint(parser); // r320

                                bool r320 = true;
                                r320 = r320 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r320, parser);
                                a319 = r320;
                            }

                            r316 |= a319;
                        } // end may a319

                        r316 = r316 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        CommitOrRollback(r316, parser);
                        a315 = r316;
                    }

                    r309 |= a315;
                } // end may a315

                CommitOrRollback(r309, parser);
                res = r309;
            }



            return res;
        }
    }

    public partial class ExpressionBrackets : Jhu.Graywulf.Sql.Parsing.ExpressionBrackets, ICloneable
    {
        public ExpressionBrackets()
            :base()
        {
        }

        public ExpressionBrackets(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public ExpressionBrackets(Jhu.SkyQuery.Sql.Parsing.ExpressionBrackets old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.ExpressionBrackets(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r321

                bool r321 = true;
                r321 = r321 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r321)
                { // may a322
                    bool a322 = false;
                    {
                        Checkpoint(parser); // r323

                        bool r323 = true;
                        r323 = r323 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r323, parser);
                        a322 = r323;
                    }

                    r321 |= a322;
                } // end may a322

                r321 = r321 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r321)
                { // may a324
                    bool a324 = false;
                    {
                        Checkpoint(parser); // r325

                        bool r325 = true;
                        r325 = r325 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r325, parser);
                        a324 = r325;
                    }

                    r321 |= a324;
                } // end may a324

                r321 = r321 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r321, parser);
                res = r321;
            }



            return res;
        }
    }

    public partial class ComparisonPredicate : Jhu.Graywulf.Sql.Parsing.ComparisonPredicate, ICloneable
    {
        public ComparisonPredicate()
            :base()
        {
        }

        public ComparisonPredicate(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public ComparisonPredicate(Jhu.SkyQuery.Sql.Parsing.ComparisonPredicate old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.ComparisonPredicate(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r326

                bool r326 = true;
                r326 = r326 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r326)
                { // may a327
                    bool a327 = false;
                    {
                        Checkpoint(parser); // r328

                        bool r328 = true;
                        r328 = r328 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r328, parser);
                        a327 = r328;
                    }

                    r326 |= a327;
                } // end may a327

                r326 = r326 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ComparisonOperator());
                if (r326)
                { // may a329
                    bool a329 = false;
                    {
                        Checkpoint(parser); // r330

                        bool r330 = true;
                        r330 = r330 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r330, parser);
                        a329 = r330;
                    }

                    r326 |= a329;
                } // end may a329

                r326 = r326 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                CommitOrRollback(r326, parser);
                res = r326;
            }



            return res;
        }
    }

    public partial class Predicate : Jhu.Graywulf.Sql.Parsing.Predicate, ICloneable
    {
        public Predicate()
            :base()
        {
        }

        public Predicate(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public Predicate(Jhu.SkyQuery.Sql.Parsing.Predicate old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.Predicate(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r331

                bool r331 = true;
                if (r331)
                { // alternatives a332 must
                    bool a332 = false;
                    if (!a332)
                    {
                        Checkpoint(parser); // r333

                        bool r333 = true;
                        r333 = r333 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ComparisonPredicate());
                        CommitOrRollback(r333, parser);
                        a332 = r333;
                    }

                    if (!a332)
                    {
                        Checkpoint(parser); // r334

                        bool r334 = true;
                        r334 = r334 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.LikePredicate());
                        CommitOrRollback(r334, parser);
                        a332 = r334;
                    }

                    if (!a332)
                    {
                        Checkpoint(parser); // r335

                        bool r335 = true;
                        r335 = r335 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.BetweenPredicate());
                        CommitOrRollback(r335, parser);
                        a332 = r335;
                    }

                    if (!a332)
                    {
                        Checkpoint(parser); // r336

                        bool r336 = true;
                        r336 = r336 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.IsNullPredicate());
                        CommitOrRollback(r336, parser);
                        a332 = r336;
                    }

                    if (!a332)
                    {
                        Checkpoint(parser); // r337

                        bool r337 = true;
                        r337 = r337 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.InExpressionListPredicate());
                        CommitOrRollback(r337, parser);
                        a332 = r337;
                    }

                    if (!a332)
                    {
                        Checkpoint(parser); // r338

                        bool r338 = true;
                        r338 = r338 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.InSemiJoinPredicate());
                        CommitOrRollback(r338, parser);
                        a332 = r338;
                    }

                    if (!a332)
                    {
                        Checkpoint(parser); // r339

                        bool r339 = true;
                        r339 = r339 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ComparisonSemiJoinPredicate());
                        CommitOrRollback(r339, parser);
                        a332 = r339;
                    }

                    if (!a332)
                    {
                        Checkpoint(parser); // r340

                        bool r340 = true;
                        r340 = r340 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ExistsSemiJoinPredicate());
                        CommitOrRollback(r340, parser);
                        a332 = r340;
                    }

                    r331 &= a332;

                } // end alternatives a332

                CommitOrRollback(r331, parser);
                res = r331;
            }



            return res;
        }
    }

    public partial class LogicalExpression : Jhu.Graywulf.Sql.Parsing.LogicalExpression, ICloneable
    {
        public LogicalExpression()
            :base()
        {
        }

        public LogicalExpression(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public LogicalExpression(Jhu.SkyQuery.Sql.Parsing.LogicalExpression old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.LogicalExpression(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r341

                bool r341 = true;
                if (r341)
                { // may a342
                    bool a342 = false;
                    {
                        Checkpoint(parser); // r343

                        bool r343 = true;
                        r343 = r343 && Match(parser, new Jhu.Graywulf.Sql.Parsing.LogicalNotOperator());
                        if (r343)
                        { // may a344
                            bool a344 = false;
                            {
                                Checkpoint(parser); // r345

                                bool r345 = true;
                                r345 = r345 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r345, parser);
                                a344 = r345;
                            }

                            r343 |= a344;
                        } // end may a344

                        CommitOrRollback(r343, parser);
                        a342 = r343;
                    }

                    r341 |= a342;
                } // end may a342

                if (r341)
                { // alternatives a346 must
                    bool a346 = false;
                    if (!a346)
                    {
                        Checkpoint(parser); // r347

                        bool r347 = true;
                        r347 = r347 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Predicate());
                        CommitOrRollback(r347, parser);
                        a346 = r347;
                    }

                    if (!a346)
                    {
                        Checkpoint(parser); // r348

                        bool r348 = true;
                        r348 = r348 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.LogicalExpressionBrackets());
                        CommitOrRollback(r348, parser);
                        a346 = r348;
                    }

                    r341 &= a346;

                } // end alternatives a346

                if (r341)
                { // may a349
                    bool a349 = false;
                    {
                        Checkpoint(parser); // r350

                        bool r350 = true;
                        if (r350)
                        { // may a351
                            bool a351 = false;
                            {
                                Checkpoint(parser); // r352

                                bool r352 = true;
                                r352 = r352 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r352, parser);
                                a351 = r352;
                            }

                            r350 |= a351;
                        } // end may a351

                        r350 = r350 && Match(parser, new Jhu.Graywulf.Sql.Parsing.LogicalOperator());
                        if (r350)
                        { // may a353
                            bool a353 = false;
                            {
                                Checkpoint(parser); // r354

                                bool r354 = true;
                                r354 = r354 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r354, parser);
                                a353 = r354;
                            }

                            r350 |= a353;
                        } // end may a353

                        r350 = r350 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.LogicalExpression());
                        CommitOrRollback(r350, parser);
                        a349 = r350;
                    }

                    r341 |= a349;
                } // end may a349

                CommitOrRollback(r341, parser);
                res = r341;
            }



            return res;
        }
    }

    public partial class LogicalArgument : Jhu.Graywulf.Sql.Parsing.LogicalArgument, ICloneable
    {
        public LogicalArgument()
            :base()
        {
        }

        public LogicalArgument(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public LogicalArgument(Jhu.SkyQuery.Sql.Parsing.LogicalArgument old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.LogicalArgument(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r355

                bool r355 = true;
                r355 = r355 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.LogicalExpression());
                CommitOrRollback(r355, parser);
                res = r355;
            }



            return res;
        }
    }

    public partial class IifFunctionCall : Jhu.Graywulf.Sql.Parsing.IifFunctionCall, ICloneable
    {
        public IifFunctionCall()
            :base()
        {
        }

        public IifFunctionCall(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public IifFunctionCall(Jhu.SkyQuery.Sql.Parsing.IifFunctionCall old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.IifFunctionCall(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r356

                bool r356 = true;
                r356 = r356 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"IIF"));
                if (r356)
                { // may a357
                    bool a357 = false;
                    {
                        Checkpoint(parser); // r358

                        bool r358 = true;
                        r358 = r358 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r358, parser);
                        a357 = r358;
                    }

                    r356 |= a357;
                } // end may a357

                r356 = r356 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r356)
                { // may a359
                    bool a359 = false;
                    {
                        Checkpoint(parser); // r360

                        bool r360 = true;
                        r360 = r360 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r360, parser);
                        a359 = r360;
                    }

                    r356 |= a359;
                } // end may a359

                r356 = r356 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.LogicalArgument());
                if (r356)
                { // may a361
                    bool a361 = false;
                    {
                        Checkpoint(parser); // r362

                        bool r362 = true;
                        r362 = r362 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r362, parser);
                        a361 = r362;
                    }

                    r356 |= a361;
                } // end may a361

                r356 = r356 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                if (r356)
                { // may a363
                    bool a363 = false;
                    {
                        Checkpoint(parser); // r364

                        bool r364 = true;
                        r364 = r364 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r364, parser);
                        a363 = r364;
                    }

                    r356 |= a363;
                } // end may a363

                r356 = r356 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ArgumentList());
                if (r356)
                { // may a365
                    bool a365 = false;
                    {
                        Checkpoint(parser); // r366

                        bool r366 = true;
                        r366 = r366 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r366, parser);
                        a365 = r366;
                    }

                    r356 |= a365;
                } // end may a365

                r356 = r356 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r356, parser);
                res = r356;
            }



            return res;
        }
    }

    public partial class LogicalExpressionBrackets : Jhu.Graywulf.Sql.Parsing.LogicalExpressionBrackets, ICloneable
    {
        public LogicalExpressionBrackets()
            :base()
        {
        }

        public LogicalExpressionBrackets(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public LogicalExpressionBrackets(Jhu.SkyQuery.Sql.Parsing.LogicalExpressionBrackets old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.LogicalExpressionBrackets(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r367

                bool r367 = true;
                r367 = r367 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r367)
                { // may a368
                    bool a368 = false;
                    {
                        Checkpoint(parser); // r369

                        bool r369 = true;
                        r369 = r369 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r369, parser);
                        a368 = r369;
                    }

                    r367 |= a368;
                } // end may a368

                r367 = r367 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.LogicalExpression());
                if (r367)
                { // may a370
                    bool a370 = false;
                    {
                        Checkpoint(parser); // r371

                        bool r371 = true;
                        r371 = r371 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r371, parser);
                        a370 = r371;
                    }

                    r367 |= a370;
                } // end may a370

                r367 = r367 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r367, parser);
                res = r367;
            }



            return res;
        }
    }

    public partial class SearchedCaseWhen : Jhu.Graywulf.Sql.Parsing.SearchedCaseWhen, ICloneable
    {
        public SearchedCaseWhen()
            :base()
        {
        }

        public SearchedCaseWhen(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public SearchedCaseWhen(Jhu.SkyQuery.Sql.Parsing.SearchedCaseWhen old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.SearchedCaseWhen(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r372

                bool r372 = true;
                r372 = r372 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"WHEN"));
                if (r372)
                { // may a373
                    bool a373 = false;
                    {
                        Checkpoint(parser); // r374

                        bool r374 = true;
                        r374 = r374 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r374, parser);
                        a373 = r374;
                    }

                    r372 |= a373;
                } // end may a373

                r372 = r372 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.LogicalExpression());
                if (r372)
                { // may a375
                    bool a375 = false;
                    {
                        Checkpoint(parser); // r376

                        bool r376 = true;
                        r376 = r376 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r376, parser);
                        a375 = r376;
                    }

                    r372 |= a375;
                } // end may a375

                r372 = r372 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"THEN"));
                if (r372)
                { // may a377
                    bool a377 = false;
                    {
                        Checkpoint(parser); // r378

                        bool r378 = true;
                        r378 = r378 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r378, parser);
                        a377 = r378;
                    }

                    r372 |= a377;
                } // end may a377

                r372 = r372 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                CommitOrRollback(r372, parser);
                res = r372;
            }



            return res;
        }
    }

    public partial class SearchedCaseWhenList : Jhu.Graywulf.Sql.Parsing.SearchedCaseWhenList, ICloneable
    {
        public SearchedCaseWhenList()
            :base()
        {
        }

        public SearchedCaseWhenList(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public SearchedCaseWhenList(Jhu.SkyQuery.Sql.Parsing.SearchedCaseWhenList old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.SearchedCaseWhenList(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r379

                bool r379 = true;
                r379 = r379 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SearchedCaseWhen());
                if (r379)
                { // may a380
                    bool a380 = false;
                    {
                        Checkpoint(parser); // r381

                        bool r381 = true;
                        r381 = r381 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r381, parser);
                        a380 = r381;
                    }

                    r379 |= a380;
                } // end may a380

                if (r379)
                { // may a382
                    bool a382 = false;
                    {
                        Checkpoint(parser); // r383

                        bool r383 = true;
                        r383 = r383 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SearchedCaseWhenList());
                        CommitOrRollback(r383, parser);
                        a382 = r383;
                    }

                    r379 |= a382;
                } // end may a382

                CommitOrRollback(r379, parser);
                res = r379;
            }



            return res;
        }
    }

    public partial class SearchedCaseExpression : Jhu.Graywulf.Sql.Parsing.SearchedCaseExpression, ICloneable
    {
        public SearchedCaseExpression()
            :base()
        {
        }

        public SearchedCaseExpression(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public SearchedCaseExpression(Jhu.SkyQuery.Sql.Parsing.SearchedCaseExpression old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.SearchedCaseExpression(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r384

                bool r384 = true;
                r384 = r384 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CASE"));
                r384 = r384 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r384 = r384 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SearchedCaseWhenList());
                if (r384)
                { // may a385
                    bool a385 = false;
                    {
                        Checkpoint(parser); // r386

                        bool r386 = true;
                        if (r386)
                        { // may a387
                            bool a387 = false;
                            {
                                Checkpoint(parser); // r388

                                bool r388 = true;
                                r388 = r388 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r388, parser);
                                a387 = r388;
                            }

                            r386 |= a387;
                        } // end may a387

                        r386 = r386 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ELSE"));
                        if (r386)
                        { // may a389
                            bool a389 = false;
                            {
                                Checkpoint(parser); // r390

                                bool r390 = true;
                                r390 = r390 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r390, parser);
                                a389 = r390;
                            }

                            r386 |= a389;
                        } // end may a389

                        r386 = r386 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        CommitOrRollback(r386, parser);
                        a385 = r386;
                    }

                    r384 |= a385;
                } // end may a385

                if (r384)
                { // may a391
                    bool a391 = false;
                    {
                        Checkpoint(parser); // r392

                        bool r392 = true;
                        r392 = r392 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r392, parser);
                        a391 = r392;
                    }

                    r384 |= a391;
                } // end may a391

                r384 = r384 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"END"));
                CommitOrRollback(r384, parser);
                res = r384;
            }



            return res;
        }
    }

    public partial class JoinCondition : Jhu.Graywulf.Sql.Parsing.JoinCondition, ICloneable
    {
        public JoinCondition()
            :base()
        {
        }

        public JoinCondition(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public JoinCondition(Jhu.SkyQuery.Sql.Parsing.JoinCondition old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.JoinCondition(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r393

                bool r393 = true;
                r393 = r393 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ON"));
                if (r393)
                { // may a394
                    bool a394 = false;
                    {
                        Checkpoint(parser); // r395

                        bool r395 = true;
                        r395 = r395 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r395, parser);
                        a394 = r395;
                    }

                    r393 |= a394;
                } // end may a394

                r393 = r393 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.LogicalExpression());
                CommitOrRollback(r393, parser);
                res = r393;
            }



            return res;
        }
    }

    public partial class JoinedTable : Jhu.Graywulf.Sql.Parsing.JoinedTable, ICloneable
    {
        public JoinedTable()
            :base()
        {
        }

        public JoinedTable(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public JoinedTable(Jhu.SkyQuery.Sql.Parsing.JoinedTable old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.JoinedTable(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r396

                bool r396 = true;
                if (r396)
                { // alternatives a397 must
                    bool a397 = false;
                    if (!a397)
                    {
                        Checkpoint(parser); // r398

                        bool r398 = true;
                        r398 = r398 && Match(parser, new Jhu.Graywulf.Sql.Parsing.InnerOuterJoinOperator());
                        if (r398)
                        { // may a399
                            bool a399 = false;
                            {
                                Checkpoint(parser); // r400

                                bool r400 = true;
                                r400 = r400 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r400, parser);
                                a399 = r400;
                            }

                            r398 |= a399;
                        } // end may a399

                        r398 = r398 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableSourceSpecification());
                        if (r398)
                        { // may a401
                            bool a401 = false;
                            {
                                Checkpoint(parser); // r402

                                bool r402 = true;
                                r402 = r402 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r402, parser);
                                a401 = r402;
                            }

                            r398 |= a401;
                        } // end may a401

                        r398 = r398 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.JoinCondition());
                        CommitOrRollback(r398, parser);
                        a397 = r398;
                    }

                    if (!a397)
                    {
                        Checkpoint(parser); // r403

                        bool r403 = true;
                        r403 = r403 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CrossJoinOperator());
                        if (r403)
                        { // may a404
                            bool a404 = false;
                            {
                                Checkpoint(parser); // r405

                                bool r405 = true;
                                r405 = r405 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r405, parser);
                                a404 = r405;
                            }

                            r403 |= a404;
                        } // end may a404

                        r403 = r403 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableSourceSpecification());
                        CommitOrRollback(r403, parser);
                        a397 = r403;
                    }

                    if (!a397)
                    {
                        Checkpoint(parser); // r406

                        bool r406 = true;
                        r406 = r406 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CrossApplyOperator());
                        if (r406)
                        { // may a407
                            bool a407 = false;
                            {
                                Checkpoint(parser); // r408

                                bool r408 = true;
                                r408 = r408 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r408, parser);
                                a407 = r408;
                            }

                            r406 |= a407;
                        } // end may a407

                        r406 = r406 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableSourceSpecification());
                        CommitOrRollback(r406, parser);
                        a397 = r406;
                    }

                    r396 &= a397;

                } // end alternatives a397

                if (r396)
                { // may a409
                    bool a409 = false;
                    {
                        Checkpoint(parser); // r410

                        bool r410 = true;
                        if (r410)
                        { // may a411
                            bool a411 = false;
                            {
                                Checkpoint(parser); // r412

                                bool r412 = true;
                                r412 = r412 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r412, parser);
                                a411 = r412;
                            }

                            r410 |= a411;
                        } // end may a411

                        r410 = r410 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.JoinedTable());
                        CommitOrRollback(r410, parser);
                        a409 = r410;
                    }

                    r396 |= a409;
                } // end may a409

                CommitOrRollback(r396, parser);
                res = r396;
            }



            return res;
        }
    }

    public partial class TableSourceExpression : Jhu.Graywulf.Sql.Parsing.TableSourceExpression, ICloneable
    {
        public TableSourceExpression()
            :base()
        {
        }

        public TableSourceExpression(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public TableSourceExpression(Jhu.SkyQuery.Sql.Parsing.TableSourceExpression old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.TableSourceExpression(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r413

                bool r413 = true;
                r413 = r413 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableSourceSpecification());
                if (r413)
                { // may a414
                    bool a414 = false;
                    {
                        Checkpoint(parser); // r415

                        bool r415 = true;
                        if (r415)
                        { // may a416
                            bool a416 = false;
                            {
                                Checkpoint(parser); // r417

                                bool r417 = true;
                                r417 = r417 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r417, parser);
                                a416 = r417;
                            }

                            r415 |= a416;
                        } // end may a416

                        r415 = r415 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.JoinedTable());
                        CommitOrRollback(r415, parser);
                        a414 = r415;
                    }

                    r413 |= a414;
                } // end may a414

                CommitOrRollback(r413, parser);
                res = r413;
            }



            return res;
        }
    }

    public partial class FromClause : Jhu.Graywulf.Sql.Parsing.FromClause, ICloneable
    {
        public FromClause()
            :base()
        {
        }

        public FromClause(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public FromClause(Jhu.SkyQuery.Sql.Parsing.FromClause old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.FromClause(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r418

                bool r418 = true;
                r418 = r418 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"FROM"));
                if (r418)
                { // may a419
                    bool a419 = false;
                    {
                        Checkpoint(parser); // r420

                        bool r420 = true;
                        r420 = r420 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r420, parser);
                        a419 = r420;
                    }

                    r418 |= a419;
                } // end may a419

                r418 = r418 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableSourceExpression());
                CommitOrRollback(r418, parser);
                res = r418;
            }



            return res;
        }
    }

    public partial class QuerySpecification : Jhu.Graywulf.Sql.Parsing.QuerySpecification, ICloneable
    {
        public QuerySpecification()
            :base()
        {
        }

        public QuerySpecification(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public QuerySpecification(Jhu.SkyQuery.Sql.Parsing.QuerySpecification old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.QuerySpecification(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r421

                bool r421 = true;
                r421 = r421 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"SELECT"));
                if (r421)
                { // may a422
                    bool a422 = false;
                    {
                        Checkpoint(parser); // r423

                        bool r423 = true;
                        r423 = r423 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        if (r423)
                        { // alternatives a424 must
                            bool a424 = false;
                            if (!a424)
                            {
                                Checkpoint(parser); // r425

                                bool r425 = true;
                                r425 = r425 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ALL"));
                                CommitOrRollback(r425, parser);
                                a424 = r425;
                            }

                            if (!a424)
                            {
                                Checkpoint(parser); // r426

                                bool r426 = true;
                                r426 = r426 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DISTINCT"));
                                CommitOrRollback(r426, parser);
                                a424 = r426;
                            }

                            r423 &= a424;

                        } // end alternatives a424

                        CommitOrRollback(r423, parser);
                        a422 = r423;
                    }

                    r421 |= a422;
                } // end may a422

                if (r421)
                { // may a427
                    bool a427 = false;
                    {
                        Checkpoint(parser); // r428

                        bool r428 = true;
                        r428 = r428 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        r428 = r428 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TopExpression());
                        CommitOrRollback(r428, parser);
                        a427 = r428;
                    }

                    r421 |= a427;
                } // end may a427

                if (r421)
                { // may a429
                    bool a429 = false;
                    {
                        Checkpoint(parser); // r430

                        bool r430 = true;
                        r430 = r430 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r430, parser);
                        a429 = r430;
                    }

                    r421 |= a429;
                } // end may a429

                r421 = r421 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SelectList());
                if (r421)
                { // may a431
                    bool a431 = false;
                    {
                        Checkpoint(parser); // r432

                        bool r432 = true;
                        if (r432)
                        { // may a433
                            bool a433 = false;
                            {
                                Checkpoint(parser); // r434

                                bool r434 = true;
                                r434 = r434 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r434, parser);
                                a433 = r434;
                            }

                            r432 |= a433;
                        } // end may a433

                        r432 = r432 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.IntoClause());
                        CommitOrRollback(r432, parser);
                        a431 = r432;
                    }

                    r421 |= a431;
                } // end may a431

                if (r421)
                { // may a435
                    bool a435 = false;
                    {
                        Checkpoint(parser); // r436

                        bool r436 = true;
                        if (r436)
                        { // may a437
                            bool a437 = false;
                            {
                                Checkpoint(parser); // r438

                                bool r438 = true;
                                r438 = r438 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r438, parser);
                                a437 = r438;
                            }

                            r436 |= a437;
                        } // end may a437

                        r436 = r436 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.FromClause());
                        CommitOrRollback(r436, parser);
                        a435 = r436;
                    }

                    r421 |= a435;
                } // end may a435

                if (r421)
                { // may a439
                    bool a439 = false;
                    {
                        Checkpoint(parser); // r440

                        bool r440 = true;
                        if (r440)
                        { // may a441
                            bool a441 = false;
                            {
                                Checkpoint(parser); // r442

                                bool r442 = true;
                                r442 = r442 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r442, parser);
                                a441 = r442;
                            }

                            r440 |= a441;
                        } // end may a441

                        r440 = r440 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.WhereClause());
                        CommitOrRollback(r440, parser);
                        a439 = r440;
                    }

                    r421 |= a439;
                } // end may a439

                if (r421)
                { // may a443
                    bool a443 = false;
                    {
                        Checkpoint(parser); // r444

                        bool r444 = true;
                        if (r444)
                        { // may a445
                            bool a445 = false;
                            {
                                Checkpoint(parser); // r446

                                bool r446 = true;
                                r446 = r446 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r446, parser);
                                a445 = r446;
                            }

                            r444 |= a445;
                        } // end may a445

                        r444 = r444 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.GroupByClause());
                        CommitOrRollback(r444, parser);
                        a443 = r444;
                    }

                    r421 |= a443;
                } // end may a443

                if (r421)
                { // may a447
                    bool a447 = false;
                    {
                        Checkpoint(parser); // r448

                        bool r448 = true;
                        if (r448)
                        { // may a449
                            bool a449 = false;
                            {
                                Checkpoint(parser); // r450

                                bool r450 = true;
                                r450 = r450 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r450, parser);
                                a449 = r450;
                            }

                            r448 |= a449;
                        } // end may a449

                        r448 = r448 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.HavingClause());
                        CommitOrRollback(r448, parser);
                        a447 = r448;
                    }

                    r421 |= a447;
                } // end may a447

                CommitOrRollback(r421, parser);
                res = r421;
            }



            return res;
        }
    }

    public partial class UpdateStatement : Jhu.Graywulf.Sql.Parsing.UpdateStatement, ICloneable
    {
        public UpdateStatement()
            :base()
        {
        }

        public UpdateStatement(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public UpdateStatement(Jhu.SkyQuery.Sql.Parsing.UpdateStatement old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.UpdateStatement(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r451

                bool r451 = true;
                if (r451)
                { // may a452
                    bool a452 = false;
                    {
                        Checkpoint(parser); // r453

                        bool r453 = true;
                        r453 = r453 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommonTableExpression());
                        if (r453)
                        { // may a454
                            bool a454 = false;
                            {
                                Checkpoint(parser); // r455

                                bool r455 = true;
                                r455 = r455 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r455, parser);
                                a454 = r455;
                            }

                            r453 |= a454;
                        } // end may a454

                        CommitOrRollback(r453, parser);
                        a452 = r453;
                    }

                    r451 |= a452;
                } // end may a452

                r451 = r451 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"UPDATE"));
                if (r451)
                { // may a456
                    bool a456 = false;
                    {
                        Checkpoint(parser); // r457

                        bool r457 = true;
                        r457 = r457 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r457, parser);
                        a456 = r457;
                    }

                    r451 |= a456;
                } // end may a456

                r451 = r451 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TargetTableSpecification());
                if (r451)
                { // may a458
                    bool a458 = false;
                    {
                        Checkpoint(parser); // r459

                        bool r459 = true;
                        r459 = r459 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r459, parser);
                        a458 = r459;
                    }

                    r451 |= a458;
                } // end may a458

                r451 = r451 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"SET"));
                if (r451)
                { // may a460
                    bool a460 = false;
                    {
                        Checkpoint(parser); // r461

                        bool r461 = true;
                        r461 = r461 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r461, parser);
                        a460 = r461;
                    }

                    r451 |= a460;
                } // end may a460

                r451 = r451 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.UpdateSetList());
                if (r451)
                { // may a462
                    bool a462 = false;
                    {
                        Checkpoint(parser); // r463

                        bool r463 = true;
                        if (r463)
                        { // may a464
                            bool a464 = false;
                            {
                                Checkpoint(parser); // r465

                                bool r465 = true;
                                r465 = r465 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r465, parser);
                                a464 = r465;
                            }

                            r463 |= a464;
                        } // end may a464

                        r463 = r463 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.FromClause());
                        CommitOrRollback(r463, parser);
                        a462 = r463;
                    }

                    r451 |= a462;
                } // end may a462

                if (r451)
                { // may a466
                    bool a466 = false;
                    {
                        Checkpoint(parser); // r467

                        bool r467 = true;
                        if (r467)
                        { // may a468
                            bool a468 = false;
                            {
                                Checkpoint(parser); // r469

                                bool r469 = true;
                                r469 = r469 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r469, parser);
                                a468 = r469;
                            }

                            r467 |= a468;
                        } // end may a468

                        r467 = r467 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.WhereClause());
                        CommitOrRollback(r467, parser);
                        a466 = r467;
                    }

                    r451 |= a466;
                } // end may a466

                if (r451)
                { // may a470
                    bool a470 = false;
                    {
                        Checkpoint(parser); // r471

                        bool r471 = true;
                        if (r471)
                        { // may a472
                            bool a472 = false;
                            {
                                Checkpoint(parser); // r473

                                bool r473 = true;
                                r473 = r473 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r473, parser);
                                a472 = r473;
                            }

                            r471 |= a472;
                        } // end may a472

                        r471 = r471 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OptionClause());
                        CommitOrRollback(r471, parser);
                        a470 = r471;
                    }

                    r451 |= a470;
                } // end may a470

                CommitOrRollback(r451, parser);
                res = r451;
            }



            return res;
        }
    }

    public partial class DeleteStatement : Jhu.Graywulf.Sql.Parsing.DeleteStatement, ICloneable
    {
        public DeleteStatement()
            :base()
        {
        }

        public DeleteStatement(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public DeleteStatement(Jhu.SkyQuery.Sql.Parsing.DeleteStatement old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.DeleteStatement(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r474

                bool r474 = true;
                if (r474)
                { // may a475
                    bool a475 = false;
                    {
                        Checkpoint(parser); // r476

                        bool r476 = true;
                        r476 = r476 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommonTableExpression());
                        if (r476)
                        { // may a477
                            bool a477 = false;
                            {
                                Checkpoint(parser); // r478

                                bool r478 = true;
                                r478 = r478 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r478, parser);
                                a477 = r478;
                            }

                            r476 |= a477;
                        } // end may a477

                        CommitOrRollback(r476, parser);
                        a475 = r476;
                    }

                    r474 |= a475;
                } // end may a475

                r474 = r474 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DELETE"));
                if (r474)
                { // may a479
                    bool a479 = false;
                    {
                        Checkpoint(parser); // r480

                        bool r480 = true;
                        r480 = r480 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        r480 = r480 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"FROM"));
                        CommitOrRollback(r480, parser);
                        a479 = r480;
                    }

                    r474 |= a479;
                } // end may a479

                if (r474)
                { // may a481
                    bool a481 = false;
                    {
                        Checkpoint(parser); // r482

                        bool r482 = true;
                        r482 = r482 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r482, parser);
                        a481 = r482;
                    }

                    r474 |= a481;
                } // end may a481

                r474 = r474 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TargetTableSpecification());
                if (r474)
                { // may a483
                    bool a483 = false;
                    {
                        Checkpoint(parser); // r484

                        bool r484 = true;
                        if (r484)
                        { // may a485
                            bool a485 = false;
                            {
                                Checkpoint(parser); // r486

                                bool r486 = true;
                                r486 = r486 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r486, parser);
                                a485 = r486;
                            }

                            r484 |= a485;
                        } // end may a485

                        r484 = r484 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.FromClause());
                        CommitOrRollback(r484, parser);
                        a483 = r484;
                    }

                    r474 |= a483;
                } // end may a483

                if (r474)
                { // may a487
                    bool a487 = false;
                    {
                        Checkpoint(parser); // r488

                        bool r488 = true;
                        if (r488)
                        { // may a489
                            bool a489 = false;
                            {
                                Checkpoint(parser); // r490

                                bool r490 = true;
                                r490 = r490 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r490, parser);
                                a489 = r490;
                            }

                            r488 |= a489;
                        } // end may a489

                        r488 = r488 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.WhereClause());
                        CommitOrRollback(r488, parser);
                        a487 = r488;
                    }

                    r474 |= a487;
                } // end may a487

                if (r474)
                { // may a491
                    bool a491 = false;
                    {
                        Checkpoint(parser); // r492

                        bool r492 = true;
                        if (r492)
                        { // may a493
                            bool a493 = false;
                            {
                                Checkpoint(parser); // r494

                                bool r494 = true;
                                r494 = r494 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r494, parser);
                                a493 = r494;
                            }

                            r492 |= a493;
                        } // end may a493

                        r492 = r492 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OptionClause());
                        CommitOrRollback(r492, parser);
                        a491 = r492;
                    }

                    r474 |= a491;
                } // end may a491

                CommitOrRollback(r474, parser);
                res = r474;
            }



            return res;
        }
    }

    public partial class WhereClause : Jhu.Graywulf.Sql.Parsing.WhereClause, ICloneable
    {
        public WhereClause()
            :base()
        {
        }

        public WhereClause(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public WhereClause(Jhu.SkyQuery.Sql.Parsing.WhereClause old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.WhereClause(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r495

                bool r495 = true;
                r495 = r495 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"WHERE"));
                if (r495)
                { // may a496
                    bool a496 = false;
                    {
                        Checkpoint(parser); // r497

                        bool r497 = true;
                        r497 = r497 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r497, parser);
                        a496 = r497;
                    }

                    r495 |= a496;
                } // end may a496

                r495 = r495 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.LogicalExpression());
                CommitOrRollback(r495, parser);
                res = r495;
            }



            return res;
        }
    }

    public partial class HavingClause : Jhu.Graywulf.Sql.Parsing.HavingClause, ICloneable
    {
        public HavingClause()
            :base()
        {
        }

        public HavingClause(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public HavingClause(Jhu.SkyQuery.Sql.Parsing.HavingClause old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.HavingClause(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r498

                bool r498 = true;
                r498 = r498 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"HAVING"));
                if (r498)
                { // may a499
                    bool a499 = false;
                    {
                        Checkpoint(parser); // r500

                        bool r500 = true;
                        r500 = r500 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r500, parser);
                        a499 = r500;
                    }

                    r498 |= a499;
                } // end may a499

                r498 = r498 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.LogicalExpression());
                CommitOrRollback(r498, parser);
                res = r498;
            }



            return res;
        }
    }

    public partial class LikePredicate : Jhu.Graywulf.Sql.Parsing.LikePredicate, ICloneable
    {
        public LikePredicate()
            :base()
        {
        }

        public LikePredicate(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public LikePredicate(Jhu.SkyQuery.Sql.Parsing.LikePredicate old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.LikePredicate(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r501

                bool r501 = true;
                r501 = r501 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r501)
                { // may a502
                    bool a502 = false;
                    {
                        Checkpoint(parser); // r503

                        bool r503 = true;
                        r503 = r503 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r503, parser);
                        a502 = r503;
                    }

                    r501 |= a502;
                } // end may a502

                if (r501)
                { // may a504
                    bool a504 = false;
                    {
                        Checkpoint(parser); // r505

                        bool r505 = true;
                        r505 = r505 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"NOT"));
                        r505 = r505 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r505, parser);
                        a504 = r505;
                    }

                    r501 |= a504;
                } // end may a504

                r501 = r501 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"LIKE"));
                if (r501)
                { // may a506
                    bool a506 = false;
                    {
                        Checkpoint(parser); // r507

                        bool r507 = true;
                        r507 = r507 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r507, parser);
                        a506 = r507;
                    }

                    r501 |= a506;
                } // end may a506

                r501 = r501 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r501)
                { // may a508
                    bool a508 = false;
                    {
                        Checkpoint(parser); // r509

                        bool r509 = true;
                        if (r509)
                        { // may a510
                            bool a510 = false;
                            {
                                Checkpoint(parser); // r511

                                bool r511 = true;
                                r511 = r511 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r511, parser);
                                a510 = r511;
                            }

                            r509 |= a510;
                        } // end may a510

                        r509 = r509 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ESCAPE"));
                        if (r509)
                        { // may a512
                            bool a512 = false;
                            {
                                Checkpoint(parser); // r513

                                bool r513 = true;
                                r513 = r513 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r513, parser);
                                a512 = r513;
                            }

                            r509 |= a512;
                        } // end may a512

                        r509 = r509 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        CommitOrRollback(r509, parser);
                        a508 = r509;
                    }

                    r501 |= a508;
                } // end may a508

                CommitOrRollback(r501, parser);
                res = r501;
            }



            return res;
        }
    }

    public partial class BetweenPredicate : Jhu.Graywulf.Sql.Parsing.BetweenPredicate, ICloneable
    {
        public BetweenPredicate()
            :base()
        {
        }

        public BetweenPredicate(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public BetweenPredicate(Jhu.SkyQuery.Sql.Parsing.BetweenPredicate old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.BetweenPredicate(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r514

                bool r514 = true;
                r514 = r514 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r514)
                { // may a515
                    bool a515 = false;
                    {
                        Checkpoint(parser); // r516

                        bool r516 = true;
                        r516 = r516 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r516, parser);
                        a515 = r516;
                    }

                    r514 |= a515;
                } // end may a515

                if (r514)
                { // may a517
                    bool a517 = false;
                    {
                        Checkpoint(parser); // r518

                        bool r518 = true;
                        r518 = r518 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"NOT"));
                        r518 = r518 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r518, parser);
                        a517 = r518;
                    }

                    r514 |= a517;
                } // end may a517

                r514 = r514 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"BETWEEN"));
                if (r514)
                { // may a519
                    bool a519 = false;
                    {
                        Checkpoint(parser); // r520

                        bool r520 = true;
                        r520 = r520 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r520, parser);
                        a519 = r520;
                    }

                    r514 |= a519;
                } // end may a519

                r514 = r514 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r514)
                { // may a521
                    bool a521 = false;
                    {
                        Checkpoint(parser); // r522

                        bool r522 = true;
                        r522 = r522 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r522, parser);
                        a521 = r522;
                    }

                    r514 |= a521;
                } // end may a521

                r514 = r514 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"AND"));
                if (r514)
                { // may a523
                    bool a523 = false;
                    {
                        Checkpoint(parser); // r524

                        bool r524 = true;
                        r524 = r524 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r524, parser);
                        a523 = r524;
                    }

                    r514 |= a523;
                } // end may a523

                r514 = r514 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                CommitOrRollback(r514, parser);
                res = r514;
            }



            return res;
        }
    }

    public partial class IsNullPredicate : Jhu.Graywulf.Sql.Parsing.IsNullPredicate, ICloneable
    {
        public IsNullPredicate()
            :base()
        {
        }

        public IsNullPredicate(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public IsNullPredicate(Jhu.SkyQuery.Sql.Parsing.IsNullPredicate old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.IsNullPredicate(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r525

                bool r525 = true;
                r525 = r525 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r525)
                { // may a526
                    bool a526 = false;
                    {
                        Checkpoint(parser); // r527

                        bool r527 = true;
                        r527 = r527 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r527, parser);
                        a526 = r527;
                    }

                    r525 |= a526;
                } // end may a526

                r525 = r525 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"IS"));
                if (r525)
                { // may a528
                    bool a528 = false;
                    {
                        Checkpoint(parser); // r529

                        bool r529 = true;
                        r529 = r529 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        r529 = r529 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"NOT"));
                        CommitOrRollback(r529, parser);
                        a528 = r529;
                    }

                    r525 |= a528;
                } // end may a528

                r525 = r525 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r525 = r525 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"NULL"));
                CommitOrRollback(r525, parser);
                res = r525;
            }



            return res;
        }
    }

    public partial class InExpressionListPredicate : Jhu.Graywulf.Sql.Parsing.InExpressionListPredicate, ICloneable
    {
        public InExpressionListPredicate()
            :base()
        {
        }

        public InExpressionListPredicate(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public InExpressionListPredicate(Jhu.SkyQuery.Sql.Parsing.InExpressionListPredicate old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.InExpressionListPredicate(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r530

                bool r530 = true;
                r530 = r530 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r530)
                { // may a531
                    bool a531 = false;
                    {
                        Checkpoint(parser); // r532

                        bool r532 = true;
                        r532 = r532 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r532, parser);
                        a531 = r532;
                    }

                    r530 |= a531;
                } // end may a531

                if (r530)
                { // may a533
                    bool a533 = false;
                    {
                        Checkpoint(parser); // r534

                        bool r534 = true;
                        r534 = r534 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"NOT"));
                        r534 = r534 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r534, parser);
                        a533 = r534;
                    }

                    r530 |= a533;
                } // end may a533

                r530 = r530 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"IN"));
                if (r530)
                { // may a535
                    bool a535 = false;
                    {
                        Checkpoint(parser); // r536

                        bool r536 = true;
                        r536 = r536 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r536, parser);
                        a535 = r536;
                    }

                    r530 |= a535;
                } // end may a535

                if (r530)
                {
                    Checkpoint(parser); // r537

                    bool r537 = true;
                    r537 = r537 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                    if (r537)
                    { // may a538
                        bool a538 = false;
                        {
                            Checkpoint(parser); // r539

                            bool r539 = true;
                            r539 = r539 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                            CommitOrRollback(r539, parser);
                            a538 = r539;
                        }

                        r537 |= a538;
                    } // end may a538

                    r537 = r537 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ArgumentList());
                    if (r537)
                    { // may a540
                        bool a540 = false;
                        {
                            Checkpoint(parser); // r541

                            bool r541 = true;
                            r541 = r541 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                            CommitOrRollback(r541, parser);
                            a540 = r541;
                        }

                        r537 |= a540;
                    } // end may a540

                    r537 = r537 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                    CommitOrRollback(r537, parser);
                    r530 = r537;
                }

                CommitOrRollback(r530, parser);
                res = r530;
            }



            return res;
        }
    }

    public partial class InSemiJoinPredicate : Jhu.Graywulf.Sql.Parsing.InSemiJoinPredicate, ICloneable
    {
        public InSemiJoinPredicate()
            :base()
        {
        }

        public InSemiJoinPredicate(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public InSemiJoinPredicate(Jhu.SkyQuery.Sql.Parsing.InSemiJoinPredicate old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.InSemiJoinPredicate(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r542

                bool r542 = true;
                r542 = r542 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r542)
                { // may a543
                    bool a543 = false;
                    {
                        Checkpoint(parser); // r544

                        bool r544 = true;
                        r544 = r544 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r544, parser);
                        a543 = r544;
                    }

                    r542 |= a543;
                } // end may a543

                if (r542)
                { // may a545
                    bool a545 = false;
                    {
                        Checkpoint(parser); // r546

                        bool r546 = true;
                        r546 = r546 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"NOT"));
                        r546 = r546 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r546, parser);
                        a545 = r546;
                    }

                    r542 |= a545;
                } // end may a545

                r542 = r542 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"IN"));
                if (r542)
                { // may a547
                    bool a547 = false;
                    {
                        Checkpoint(parser); // r548

                        bool r548 = true;
                        r548 = r548 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r548, parser);
                        a547 = r548;
                    }

                    r542 |= a547;
                } // end may a547

                r542 = r542 && Match(parser, new Jhu.Graywulf.Sql.Parsing.SemiJoinSubquery());
                CommitOrRollback(r542, parser);
                res = r542;
            }



            return res;
        }
    }

    public partial class ComparisonSemiJoinPredicate : Jhu.Graywulf.Sql.Parsing.ComparisonSemiJoinPredicate, ICloneable
    {
        public ComparisonSemiJoinPredicate()
            :base()
        {
        }

        public ComparisonSemiJoinPredicate(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public ComparisonSemiJoinPredicate(Jhu.SkyQuery.Sql.Parsing.ComparisonSemiJoinPredicate old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.ComparisonSemiJoinPredicate(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r549

                bool r549 = true;
                r549 = r549 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r549)
                { // may a550
                    bool a550 = false;
                    {
                        Checkpoint(parser); // r551

                        bool r551 = true;
                        r551 = r551 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r551, parser);
                        a550 = r551;
                    }

                    r549 |= a550;
                } // end may a550

                r549 = r549 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ComparisonOperator());
                if (r549)
                { // may a552
                    bool a552 = false;
                    {
                        Checkpoint(parser); // r553

                        bool r553 = true;
                        r553 = r553 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r553, parser);
                        a552 = r553;
                    }

                    r549 |= a552;
                } // end may a552

                if (r549)
                { // alternatives a554 must
                    bool a554 = false;
                    if (!a554)
                    {
                        Checkpoint(parser); // r555

                        bool r555 = true;
                        r555 = r555 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ALL"));
                        CommitOrRollback(r555, parser);
                        a554 = r555;
                    }

                    if (!a554)
                    {
                        Checkpoint(parser); // r556

                        bool r556 = true;
                        r556 = r556 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"SOME"));
                        CommitOrRollback(r556, parser);
                        a554 = r556;
                    }

                    if (!a554)
                    {
                        Checkpoint(parser); // r557

                        bool r557 = true;
                        r557 = r557 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ANY"));
                        CommitOrRollback(r557, parser);
                        a554 = r557;
                    }

                    r549 &= a554;

                } // end alternatives a554

                if (r549)
                { // may a558
                    bool a558 = false;
                    {
                        Checkpoint(parser); // r559

                        bool r559 = true;
                        r559 = r559 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r559, parser);
                        a558 = r559;
                    }

                    r549 |= a558;
                } // end may a558

                r549 = r549 && Match(parser, new Jhu.Graywulf.Sql.Parsing.SemiJoinSubquery());
                CommitOrRollback(r549, parser);
                res = r549;
            }



            return res;
        }
    }

    public partial class SimpleCaseExpression : Jhu.Graywulf.Sql.Parsing.SimpleCaseExpression, ICloneable
    {
        public SimpleCaseExpression()
            :base()
        {
        }

        public SimpleCaseExpression(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public SimpleCaseExpression(Jhu.SkyQuery.Sql.Parsing.SimpleCaseExpression old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.SimpleCaseExpression(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r560

                bool r560 = true;
                r560 = r560 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CASE"));
                if (r560)
                { // may a561
                    bool a561 = false;
                    {
                        Checkpoint(parser); // r562

                        bool r562 = true;
                        r562 = r562 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r562, parser);
                        a561 = r562;
                    }

                    r560 |= a561;
                } // end may a561

                r560 = r560 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r560)
                { // may a563
                    bool a563 = false;
                    {
                        Checkpoint(parser); // r564

                        bool r564 = true;
                        r564 = r564 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r564, parser);
                        a563 = r564;
                    }

                    r560 |= a563;
                } // end may a563

                r560 = r560 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SimpleCaseWhenList());
                if (r560)
                { // may a565
                    bool a565 = false;
                    {
                        Checkpoint(parser); // r566

                        bool r566 = true;
                        if (r566)
                        { // may a567
                            bool a567 = false;
                            {
                                Checkpoint(parser); // r568

                                bool r568 = true;
                                r568 = r568 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r568, parser);
                                a567 = r568;
                            }

                            r566 |= a567;
                        } // end may a567

                        r566 = r566 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ELSE"));
                        if (r566)
                        { // may a569
                            bool a569 = false;
                            {
                                Checkpoint(parser); // r570

                                bool r570 = true;
                                r570 = r570 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r570, parser);
                                a569 = r570;
                            }

                            r566 |= a569;
                        } // end may a569

                        r566 = r566 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        CommitOrRollback(r566, parser);
                        a565 = r566;
                    }

                    r560 |= a565;
                } // end may a565

                if (r560)
                { // may a571
                    bool a571 = false;
                    {
                        Checkpoint(parser); // r572

                        bool r572 = true;
                        r572 = r572 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r572, parser);
                        a571 = r572;
                    }

                    r560 |= a571;
                } // end may a571

                r560 = r560 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"END"));
                CommitOrRollback(r560, parser);
                res = r560;
            }



            return res;
        }
    }

    public partial class SimpleCaseWhen : Jhu.Graywulf.Sql.Parsing.SimpleCaseWhen, ICloneable
    {
        public SimpleCaseWhen()
            :base()
        {
        }

        public SimpleCaseWhen(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public SimpleCaseWhen(Jhu.SkyQuery.Sql.Parsing.SimpleCaseWhen old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.SimpleCaseWhen(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r573

                bool r573 = true;
                r573 = r573 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"WHEN"));
                if (r573)
                { // may a574
                    bool a574 = false;
                    {
                        Checkpoint(parser); // r575

                        bool r575 = true;
                        r575 = r575 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r575, parser);
                        a574 = r575;
                    }

                    r573 |= a574;
                } // end may a574

                r573 = r573 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r573)
                { // may a576
                    bool a576 = false;
                    {
                        Checkpoint(parser); // r577

                        bool r577 = true;
                        r577 = r577 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r577, parser);
                        a576 = r577;
                    }

                    r573 |= a576;
                } // end may a576

                r573 = r573 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"THEN"));
                if (r573)
                { // may a578
                    bool a578 = false;
                    {
                        Checkpoint(parser); // r579

                        bool r579 = true;
                        r579 = r579 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r579, parser);
                        a578 = r579;
                    }

                    r573 |= a578;
                } // end may a578

                r573 = r573 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                CommitOrRollback(r573, parser);
                res = r573;
            }



            return res;
        }
    }

    public partial class SimpleCaseWhenList : Jhu.Graywulf.Sql.Parsing.SimpleCaseWhenList, ICloneable
    {
        public SimpleCaseWhenList()
            :base()
        {
        }

        public SimpleCaseWhenList(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public SimpleCaseWhenList(Jhu.SkyQuery.Sql.Parsing.SimpleCaseWhenList old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.SimpleCaseWhenList(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r580

                bool r580 = true;
                r580 = r580 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SimpleCaseWhen());
                if (r580)
                { // may a581
                    bool a581 = false;
                    {
                        Checkpoint(parser); // r582

                        bool r582 = true;
                        r582 = r582 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r582, parser);
                        a581 = r582;
                    }

                    r580 |= a581;
                } // end may a581

                if (r580)
                { // may a583
                    bool a583 = false;
                    {
                        Checkpoint(parser); // r584

                        bool r584 = true;
                        r584 = r584 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SimpleCaseWhenList());
                        CommitOrRollback(r584, parser);
                        a583 = r584;
                    }

                    r580 |= a583;
                } // end may a583

                CommitOrRollback(r580, parser);
                res = r580;
            }



            return res;
        }
    }

    public partial class Argument : Jhu.Graywulf.Sql.Parsing.Argument, ICloneable
    {
        public Argument()
            :base()
        {
        }

        public Argument(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public Argument(Jhu.SkyQuery.Sql.Parsing.Argument old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.Argument(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r585

                bool r585 = true;
                r585 = r585 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                CommitOrRollback(r585, parser);
                res = r585;
            }



            return res;
        }
    }

    public partial class CastAndParseFunctionCall : Jhu.Graywulf.Sql.Parsing.CastAndParseFunctionCall, ICloneable
    {
        public CastAndParseFunctionCall()
            :base()
        {
        }

        public CastAndParseFunctionCall(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public CastAndParseFunctionCall(Jhu.SkyQuery.Sql.Parsing.CastAndParseFunctionCall old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.CastAndParseFunctionCall(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r586

                bool r586 = true;
                if (r586)
                { // alternatives a587 must
                    bool a587 = false;
                    if (!a587)
                    {
                        Checkpoint(parser); // r588

                        bool r588 = true;
                        r588 = r588 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CAST"));
                        CommitOrRollback(r588, parser);
                        a587 = r588;
                    }

                    if (!a587)
                    {
                        Checkpoint(parser); // r589

                        bool r589 = true;
                        r589 = r589 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TRY_CAST"));
                        CommitOrRollback(r589, parser);
                        a587 = r589;
                    }

                    if (!a587)
                    {
                        Checkpoint(parser); // r590

                        bool r590 = true;
                        r590 = r590 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"PARSE"));
                        CommitOrRollback(r590, parser);
                        a587 = r590;
                    }

                    if (!a587)
                    {
                        Checkpoint(parser); // r591

                        bool r591 = true;
                        r591 = r591 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TRY_PARSE"));
                        CommitOrRollback(r591, parser);
                        a587 = r591;
                    }

                    r586 &= a587;

                } // end alternatives a587

                if (r586)
                { // may a592
                    bool a592 = false;
                    {
                        Checkpoint(parser); // r593

                        bool r593 = true;
                        r593 = r593 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r593, parser);
                        a592 = r593;
                    }

                    r586 |= a592;
                } // end may a592

                r586 = r586 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r586)
                { // may a594
                    bool a594 = false;
                    {
                        Checkpoint(parser); // r595

                        bool r595 = true;
                        r595 = r595 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r595, parser);
                        a594 = r595;
                    }

                    r586 |= a594;
                } // end may a594

                r586 = r586 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Argument());
                if (r586)
                { // may a596
                    bool a596 = false;
                    {
                        Checkpoint(parser); // r597

                        bool r597 = true;
                        r597 = r597 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r597, parser);
                        a596 = r597;
                    }

                    r586 |= a596;
                } // end may a596

                r586 = r586 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"AS"));
                if (r586)
                { // may a598
                    bool a598 = false;
                    {
                        Checkpoint(parser); // r599

                        bool r599 = true;
                        r599 = r599 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r599, parser);
                        a598 = r599;
                    }

                    r586 |= a598;
                } // end may a598

                r586 = r586 && Match(parser, new Jhu.Graywulf.Sql.Parsing.DataTypeArgument());
                if (r586)
                { // may a600
                    bool a600 = false;
                    {
                        Checkpoint(parser); // r601

                        bool r601 = true;
                        if (r601)
                        { // may a602
                            bool a602 = false;
                            {
                                Checkpoint(parser); // r603

                                bool r603 = true;
                                r603 = r603 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r603, parser);
                                a602 = r603;
                            }

                            r601 |= a602;
                        } // end may a602

                        r601 = r601 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"USING"));
                        if (r601)
                        { // may a604
                            bool a604 = false;
                            {
                                Checkpoint(parser); // r605

                                bool r605 = true;
                                r605 = r605 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r605, parser);
                                a604 = r605;
                            }

                            r601 |= a604;
                        } // end may a604

                        r601 = r601 && Match(parser, new Jhu.Graywulf.Sql.Parsing.StringConstant());
                        CommitOrRollback(r601, parser);
                        a600 = r601;
                    }

                    r586 |= a600;
                } // end may a600

                if (r586)
                { // may a606
                    bool a606 = false;
                    {
                        Checkpoint(parser); // r607

                        bool r607 = true;
                        r607 = r607 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r607, parser);
                        a606 = r607;
                    }

                    r586 |= a606;
                } // end may a606

                r586 = r586 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r586, parser);
                res = r586;
            }



            return res;
        }
    }

    public partial class ConvertFunctionCall : Jhu.Graywulf.Sql.Parsing.ConvertFunctionCall, ICloneable
    {
        public ConvertFunctionCall()
            :base()
        {
        }

        public ConvertFunctionCall(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public ConvertFunctionCall(Jhu.SkyQuery.Sql.Parsing.ConvertFunctionCall old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.ConvertFunctionCall(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r608

                bool r608 = true;
                if (r608)
                { // alternatives a609 must
                    bool a609 = false;
                    if (!a609)
                    {
                        Checkpoint(parser); // r610

                        bool r610 = true;
                        r610 = r610 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CONVERT"));
                        CommitOrRollback(r610, parser);
                        a609 = r610;
                    }

                    if (!a609)
                    {
                        Checkpoint(parser); // r611

                        bool r611 = true;
                        r611 = r611 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TRY_CONVERT"));
                        CommitOrRollback(r611, parser);
                        a609 = r611;
                    }

                    r608 &= a609;

                } // end alternatives a609

                if (r608)
                { // may a612
                    bool a612 = false;
                    {
                        Checkpoint(parser); // r613

                        bool r613 = true;
                        r613 = r613 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r613, parser);
                        a612 = r613;
                    }

                    r608 |= a612;
                } // end may a612

                r608 = r608 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r608)
                { // may a614
                    bool a614 = false;
                    {
                        Checkpoint(parser); // r615

                        bool r615 = true;
                        r615 = r615 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r615, parser);
                        a614 = r615;
                    }

                    r608 |= a614;
                } // end may a614

                r608 = r608 && Match(parser, new Jhu.Graywulf.Sql.Parsing.DataTypeArgument());
                if (r608)
                { // may a616
                    bool a616 = false;
                    {
                        Checkpoint(parser); // r617

                        bool r617 = true;
                        r617 = r617 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r617, parser);
                        a616 = r617;
                    }

                    r608 |= a616;
                } // end may a616

                r608 = r608 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                if (r608)
                { // may a618
                    bool a618 = false;
                    {
                        Checkpoint(parser); // r619

                        bool r619 = true;
                        r619 = r619 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r619, parser);
                        a618 = r619;
                    }

                    r608 |= a618;
                } // end may a618

                r608 = r608 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Argument());
                if (r608)
                { // may a620
                    bool a620 = false;
                    {
                        Checkpoint(parser); // r621

                        bool r621 = true;
                        r621 = r621 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r621, parser);
                        a620 = r621;
                    }

                    r608 |= a620;
                } // end may a620

                r608 = r608 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r608, parser);
                res = r608;
            }



            return res;
        }
    }

    public partial class ArgumentList : Jhu.Graywulf.Sql.Parsing.ArgumentList, ICloneable
    {
        public ArgumentList()
            :base()
        {
        }

        public ArgumentList(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public ArgumentList(Jhu.SkyQuery.Sql.Parsing.ArgumentList old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.ArgumentList(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r622

                bool r622 = true;
                r622 = r622 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Argument());
                if (r622)
                { // may a623
                    bool a623 = false;
                    {
                        Checkpoint(parser); // r624

                        bool r624 = true;
                        if (r624)
                        { // may a625
                            bool a625 = false;
                            {
                                Checkpoint(parser); // r626

                                bool r626 = true;
                                r626 = r626 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r626, parser);
                                a625 = r626;
                            }

                            r624 |= a625;
                        } // end may a625

                        r624 = r624 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r624)
                        { // may a627
                            bool a627 = false;
                            {
                                Checkpoint(parser); // r628

                                bool r628 = true;
                                r628 = r628 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r628, parser);
                                a627 = r628;
                            }

                            r624 |= a627;
                        } // end may a627

                        r624 = r624 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ArgumentList());
                        CommitOrRollback(r624, parser);
                        a623 = r624;
                    }

                    r622 |= a623;
                } // end may a623

                CommitOrRollback(r622, parser);
                res = r622;
            }



            return res;
        }
    }

    public partial class MemberCall : Jhu.Graywulf.Sql.Parsing.MemberCall, ICloneable
    {
        public MemberCall()
            :base()
        {
        }

        public MemberCall(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public MemberCall(Jhu.SkyQuery.Sql.Parsing.MemberCall old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.MemberCall(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r629

                bool r629 = true;
                r629 = r629 && Match(parser, new Jhu.Graywulf.Sql.Parsing.MemberName());
                if (r629)
                { // may a630
                    bool a630 = false;
                    {
                        Checkpoint(parser); // r631

                        bool r631 = true;
                        r631 = r631 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r631, parser);
                        a630 = r631;
                    }

                    r629 |= a630;
                } // end may a630

                r629 = r629 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r629)
                { // may a632
                    bool a632 = false;
                    {
                        Checkpoint(parser); // r633

                        bool r633 = true;
                        r633 = r633 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r633, parser);
                        a632 = r633;
                    }

                    r629 |= a632;
                } // end may a632

                if (r629)
                { // may a634
                    bool a634 = false;
                    {
                        Checkpoint(parser); // r635

                        bool r635 = true;
                        r635 = r635 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ArgumentList());
                        CommitOrRollback(r635, parser);
                        a634 = r635;
                    }

                    r629 |= a634;
                } // end may a634

                if (r629)
                { // may a636
                    bool a636 = false;
                    {
                        Checkpoint(parser); // r637

                        bool r637 = true;
                        r637 = r637 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r637, parser);
                        a636 = r637;
                    }

                    r629 |= a636;
                } // end may a636

                r629 = r629 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r629, parser);
                res = r629;
            }



            return res;
        }
    }

    public partial class MemberAccessList : Jhu.Graywulf.Sql.Parsing.MemberAccessList, ICloneable
    {
        public MemberAccessList()
            :base()
        {
        }

        public MemberAccessList(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public MemberAccessList(Jhu.SkyQuery.Sql.Parsing.MemberAccessList old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.MemberAccessList(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r638

                bool r638 = true;
                r638 = r638 && Match(parser, new Jhu.Graywulf.Sql.Parsing.MemberAccessOperator());
                if (r638)
                { // may a639
                    bool a639 = false;
                    {
                        Checkpoint(parser); // r640

                        bool r640 = true;
                        r640 = r640 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r640, parser);
                        a639 = r640;
                    }

                    r638 |= a639;
                } // end may a639

                if (r638)
                { // alternatives a641 must
                    bool a641 = false;
                    if (!a641)
                    {
                        Checkpoint(parser); // r642

                        bool r642 = true;
                        r642 = r642 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.MemberCall());
                        CommitOrRollback(r642, parser);
                        a641 = r642;
                    }

                    if (!a641)
                    {
                        Checkpoint(parser); // r643

                        bool r643 = true;
                        r643 = r643 && Match(parser, new Jhu.Graywulf.Sql.Parsing.MemberAccess());
                        CommitOrRollback(r643, parser);
                        a641 = r643;
                    }

                    r638 &= a641;

                } // end alternatives a641

                if (r638)
                { // may a644
                    bool a644 = false;
                    {
                        Checkpoint(parser); // r645

                        bool r645 = true;
                        if (r645)
                        { // may a646
                            bool a646 = false;
                            {
                                Checkpoint(parser); // r647

                                bool r647 = true;
                                r647 = r647 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r647, parser);
                                a646 = r647;
                            }

                            r645 |= a646;
                        } // end may a646

                        r645 = r645 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.MemberAccessList());
                        CommitOrRollback(r645, parser);
                        a644 = r645;
                    }

                    r638 |= a644;
                } // end may a644

                CommitOrRollback(r638, parser);
                res = r638;
            }



            return res;
        }
    }

    public partial class DateFunctionCall : Jhu.Graywulf.Sql.Parsing.DateFunctionCall, ICloneable
    {
        public DateFunctionCall()
            :base()
        {
        }

        public DateFunctionCall(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public DateFunctionCall(Jhu.SkyQuery.Sql.Parsing.DateFunctionCall old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.DateFunctionCall(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r648

                bool r648 = true;
                if (r648)
                { // alternatives a649 must
                    bool a649 = false;
                    if (!a649)
                    {
                        Checkpoint(parser); // r650

                        bool r650 = true;
                        r650 = r650 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DATEADD"));
                        CommitOrRollback(r650, parser);
                        a649 = r650;
                    }

                    if (!a649)
                    {
                        Checkpoint(parser); // r651

                        bool r651 = true;
                        r651 = r651 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DATEDIFF"));
                        CommitOrRollback(r651, parser);
                        a649 = r651;
                    }

                    if (!a649)
                    {
                        Checkpoint(parser); // r652

                        bool r652 = true;
                        r652 = r652 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DATEDIFF_BIG"));
                        CommitOrRollback(r652, parser);
                        a649 = r652;
                    }

                    if (!a649)
                    {
                        Checkpoint(parser); // r653

                        bool r653 = true;
                        r653 = r653 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DATENAME"));
                        CommitOrRollback(r653, parser);
                        a649 = r653;
                    }

                    if (!a649)
                    {
                        Checkpoint(parser); // r654

                        bool r654 = true;
                        r654 = r654 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DATEPART"));
                        CommitOrRollback(r654, parser);
                        a649 = r654;
                    }

                    r648 &= a649;

                } // end alternatives a649

                if (r648)
                { // may a655
                    bool a655 = false;
                    {
                        Checkpoint(parser); // r656

                        bool r656 = true;
                        r656 = r656 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r656, parser);
                        a655 = r656;
                    }

                    r648 |= a655;
                } // end may a655

                r648 = r648 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r648)
                { // may a657
                    bool a657 = false;
                    {
                        Checkpoint(parser); // r658

                        bool r658 = true;
                        r658 = r658 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r658, parser);
                        a657 = r658;
                    }

                    r648 |= a657;
                } // end may a657

                r648 = r648 && Match(parser, new Jhu.Graywulf.Sql.Parsing.DatePart());
                if (r648)
                { // may a659
                    bool a659 = false;
                    {
                        Checkpoint(parser); // r660

                        bool r660 = true;
                        r660 = r660 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r660, parser);
                        a659 = r660;
                    }

                    r648 |= a659;
                } // end may a659

                r648 = r648 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                if (r648)
                { // may a661
                    bool a661 = false;
                    {
                        Checkpoint(parser); // r662

                        bool r662 = true;
                        r662 = r662 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r662, parser);
                        a661 = r662;
                    }

                    r648 |= a661;
                } // end may a661

                r648 = r648 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ArgumentList());
                if (r648)
                { // may a663
                    bool a663 = false;
                    {
                        Checkpoint(parser); // r664

                        bool r664 = true;
                        r664 = r664 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r664, parser);
                        a663 = r664;
                    }

                    r648 |= a663;
                } // end may a663

                r648 = r648 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r648, parser);
                res = r648;
            }



            return res;
        }
    }

    public partial class SystemFunctionCall : Jhu.Graywulf.Sql.Parsing.SystemFunctionCall, ICloneable
    {
        public SystemFunctionCall()
            :base()
        {
        }

        public SystemFunctionCall(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public SystemFunctionCall(Jhu.SkyQuery.Sql.Parsing.SystemFunctionCall old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.SystemFunctionCall(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r665

                bool r665 = true;
                r665 = r665 && Match(parser, new Jhu.Graywulf.Sql.Parsing.FunctionName());
                if (r665)
                { // may a666
                    bool a666 = false;
                    {
                        Checkpoint(parser); // r667

                        bool r667 = true;
                        r667 = r667 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r667, parser);
                        a666 = r667;
                    }

                    r665 |= a666;
                } // end may a666

                r665 = r665 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r665)
                { // may a668
                    bool a668 = false;
                    {
                        Checkpoint(parser); // r669

                        bool r669 = true;
                        r669 = r669 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r669, parser);
                        a668 = r669;
                    }

                    r665 |= a668;
                } // end may a668

                if (r665)
                { // may a670
                    bool a670 = false;
                    {
                        Checkpoint(parser); // r671

                        bool r671 = true;
                        r671 = r671 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ArgumentList());
                        CommitOrRollback(r671, parser);
                        a670 = r671;
                    }

                    r665 |= a670;
                } // end may a670

                if (r665)
                { // may a672
                    bool a672 = false;
                    {
                        Checkpoint(parser); // r673

                        bool r673 = true;
                        r673 = r673 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r673, parser);
                        a672 = r673;
                    }

                    r665 |= a672;
                } // end may a672

                r665 = r665 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r665, parser);
                res = r665;
            }



            return res;
        }
    }

    public partial class AggregateFunctionCall : Jhu.Graywulf.Sql.Parsing.AggregateFunctionCall, ICloneable
    {
        public AggregateFunctionCall()
            :base()
        {
        }

        public AggregateFunctionCall(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public AggregateFunctionCall(Jhu.SkyQuery.Sql.Parsing.AggregateFunctionCall old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.AggregateFunctionCall(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r674

                bool r674 = true;
                r674 = r674 && Match(parser, new Jhu.Graywulf.Sql.Parsing.FunctionIdentifier());
                if (r674)
                { // may a675
                    bool a675 = false;
                    {
                        Checkpoint(parser); // r676

                        bool r676 = true;
                        r676 = r676 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r676, parser);
                        a675 = r676;
                    }

                    r674 |= a675;
                } // end may a675

                r674 = r674 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r674)
                { // may a677
                    bool a677 = false;
                    {
                        Checkpoint(parser); // r678

                        bool r678 = true;
                        r678 = r678 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r678, parser);
                        a677 = r678;
                    }

                    r674 |= a677;
                } // end may a677

                if (r674)
                { // alternatives a679 must
                    bool a679 = false;
                    if (!a679)
                    {
                        Checkpoint(parser); // r680

                        bool r680 = true;
                        r680 = r680 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ALL"));
                        CommitOrRollback(r680, parser);
                        a679 = r680;
                    }

                    if (!a679)
                    {
                        Checkpoint(parser); // r681

                        bool r681 = true;
                        r681 = r681 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DISTINCT"));
                        CommitOrRollback(r681, parser);
                        a679 = r681;
                    }

                    r674 &= a679;

                } // end alternatives a679

                if (r674)
                { // may a682
                    bool a682 = false;
                    {
                        Checkpoint(parser); // r683

                        bool r683 = true;
                        r683 = r683 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r683, parser);
                        a682 = r683;
                    }

                    r674 |= a682;
                } // end may a682

                r674 = r674 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ArgumentList());
                if (r674)
                { // may a684
                    bool a684 = false;
                    {
                        Checkpoint(parser); // r685

                        bool r685 = true;
                        r685 = r685 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r685, parser);
                        a684 = r685;
                    }

                    r674 |= a684;
                } // end may a684

                r674 = r674 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                if (r674)
                { // may a686
                    bool a686 = false;
                    {
                        Checkpoint(parser); // r687

                        bool r687 = true;
                        if (r687)
                        { // may a688
                            bool a688 = false;
                            {
                                Checkpoint(parser); // r689

                                bool r689 = true;
                                r689 = r689 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r689, parser);
                                a688 = r689;
                            }

                            r687 |= a688;
                        } // end may a688

                        r687 = r687 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OverClause());
                        CommitOrRollback(r687, parser);
                        a686 = r687;
                    }

                    r674 |= a686;
                } // end may a686

                CommitOrRollback(r674, parser);
                res = r674;
            }



            return res;
        }
    }

    public partial class WindowedFunctionCall : Jhu.Graywulf.Sql.Parsing.WindowedFunctionCall, ICloneable
    {
        public WindowedFunctionCall()
            :base()
        {
        }

        public WindowedFunctionCall(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public WindowedFunctionCall(Jhu.SkyQuery.Sql.Parsing.WindowedFunctionCall old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.WindowedFunctionCall(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r690

                bool r690 = true;
                r690 = r690 && Match(parser, new Jhu.Graywulf.Sql.Parsing.FunctionIdentifier());
                if (r690)
                { // may a691
                    bool a691 = false;
                    {
                        Checkpoint(parser); // r692

                        bool r692 = true;
                        r692 = r692 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r692, parser);
                        a691 = r692;
                    }

                    r690 |= a691;
                } // end may a691

                r690 = r690 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r690)
                { // may a693
                    bool a693 = false;
                    {
                        Checkpoint(parser); // r694

                        bool r694 = true;
                        if (r694)
                        { // may a695
                            bool a695 = false;
                            {
                                Checkpoint(parser); // r696

                                bool r696 = true;
                                r696 = r696 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r696, parser);
                                a695 = r696;
                            }

                            r694 |= a695;
                        } // end may a695

                        r694 = r694 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ArgumentList());
                        CommitOrRollback(r694, parser);
                        a693 = r694;
                    }

                    r690 |= a693;
                } // end may a693

                if (r690)
                { // may a697
                    bool a697 = false;
                    {
                        Checkpoint(parser); // r698

                        bool r698 = true;
                        r698 = r698 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r698, parser);
                        a697 = r698;
                    }

                    r690 |= a697;
                } // end may a697

                r690 = r690 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                if (r690)
                { // may a699
                    bool a699 = false;
                    {
                        Checkpoint(parser); // r700

                        bool r700 = true;
                        r700 = r700 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r700, parser);
                        a699 = r700;
                    }

                    r690 |= a699;
                } // end may a699

                r690 = r690 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OverClause());
                CommitOrRollback(r690, parser);
                res = r690;
            }



            return res;
        }
    }

    public partial class TableValuedFunctionCall : Jhu.Graywulf.Sql.Parsing.TableValuedFunctionCall, ICloneable
    {
        public TableValuedFunctionCall()
            :base()
        {
        }

        public TableValuedFunctionCall(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public TableValuedFunctionCall(Jhu.SkyQuery.Sql.Parsing.TableValuedFunctionCall old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.TableValuedFunctionCall(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r701

                bool r701 = true;
                r701 = r701 && Match(parser, new Jhu.Graywulf.Sql.Parsing.FunctionIdentifier());
                if (r701)
                { // may a702
                    bool a702 = false;
                    {
                        Checkpoint(parser); // r703

                        bool r703 = true;
                        r703 = r703 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r703, parser);
                        a702 = r703;
                    }

                    r701 |= a702;
                } // end may a702

                r701 = r701 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r701)
                { // may a704
                    bool a704 = false;
                    {
                        Checkpoint(parser); // r705

                        bool r705 = true;
                        r705 = r705 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r705, parser);
                        a704 = r705;
                    }

                    r701 |= a704;
                } // end may a704

                if (r701)
                { // may a706
                    bool a706 = false;
                    {
                        Checkpoint(parser); // r707

                        bool r707 = true;
                        r707 = r707 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ArgumentList());
                        CommitOrRollback(r707, parser);
                        a706 = r707;
                    }

                    r701 |= a706;
                } // end may a706

                if (r701)
                { // may a708
                    bool a708 = false;
                    {
                        Checkpoint(parser); // r709

                        bool r709 = true;
                        r709 = r709 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r709, parser);
                        a708 = r709;
                    }

                    r701 |= a708;
                } // end may a708

                r701 = r701 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r701, parser);
                res = r701;
            }



            return res;
        }
    }

    public partial class FunctionTableSource : Jhu.Graywulf.Sql.Parsing.FunctionTableSource, ICloneable
    {
        public FunctionTableSource()
            :base()
        {
        }

        public FunctionTableSource(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public FunctionTableSource(Jhu.SkyQuery.Sql.Parsing.FunctionTableSource old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.FunctionTableSource(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r710

                bool r710 = true;
                r710 = r710 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableValuedFunctionCall());
                if (r710)
                { // may a711
                    bool a711 = false;
                    {
                        Checkpoint(parser); // r712

                        bool r712 = true;
                        r712 = r712 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r712, parser);
                        a711 = r712;
                    }

                    r710 |= a711;
                } // end may a711

                if (r710)
                { // may a713
                    bool a713 = false;
                    {
                        Checkpoint(parser); // r714

                        bool r714 = true;
                        r714 = r714 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"AS"));
                        if (r714)
                        { // may a715
                            bool a715 = false;
                            {
                                Checkpoint(parser); // r716

                                bool r716 = true;
                                r716 = r716 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r716, parser);
                                a715 = r716;
                            }

                            r714 |= a715;
                        } // end may a715

                        CommitOrRollback(r714, parser);
                        a713 = r714;
                    }

                    r710 |= a713;
                } // end may a713

                r710 = r710 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableAlias());
                CommitOrRollback(r710, parser);
                res = r710;
            }



            return res;
        }
    }

    public partial class TableSourceSpecification : Jhu.Graywulf.Sql.Parsing.TableSourceSpecification, ICloneable
    {
        public TableSourceSpecification()
            :base()
        {
        }

        public TableSourceSpecification(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public TableSourceSpecification(Jhu.SkyQuery.Sql.Parsing.TableSourceSpecification old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.TableSourceSpecification(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r717

                bool r717 = true;
                if (r717)
                { // alternatives a718 must
                    bool a718 = false;
                    if (!a718)
                    {
                        Checkpoint(parser); // r719

                        bool r719 = true;
                        r719 = r719 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.FunctionTableSource());
                        CommitOrRollback(r719, parser);
                        a718 = r719;
                    }

                    if (!a718)
                    {
                        Checkpoint(parser); // r720

                        bool r720 = true;
                        r720 = r720 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SimpleTableSource());
                        CommitOrRollback(r720, parser);
                        a718 = r720;
                    }

                    if (!a718)
                    {
                        Checkpoint(parser); // r721

                        bool r721 = true;
                        r721 = r721 && Match(parser, new Jhu.Graywulf.Sql.Parsing.VariableTableSource());
                        CommitOrRollback(r721, parser);
                        a718 = r721;
                    }

                    if (!a718)
                    {
                        Checkpoint(parser); // r722

                        bool r722 = true;
                        r722 = r722 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SubqueryTableSource());
                        CommitOrRollback(r722, parser);
                        a718 = r722;
                    }

                    r717 &= a718;

                } // end alternatives a718

                CommitOrRollback(r717, parser);
                res = r717;
            }



            return res;
        }
    }

    public partial class UdtMethodCall : Jhu.Graywulf.Sql.Parsing.UdtMethodCall, ICloneable
    {
        public UdtMethodCall()
            :base()
        {
        }

        public UdtMethodCall(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public UdtMethodCall(Jhu.SkyQuery.Sql.Parsing.UdtMethodCall old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.UdtMethodCall(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r723

                bool r723 = true;
                r723 = r723 && Match(parser, new Jhu.Graywulf.Sql.Parsing.MethodName());
                if (r723)
                { // may a724
                    bool a724 = false;
                    {
                        Checkpoint(parser); // r725

                        bool r725 = true;
                        r725 = r725 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r725, parser);
                        a724 = r725;
                    }

                    r723 |= a724;
                } // end may a724

                r723 = r723 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r723)
                { // may a726
                    bool a726 = false;
                    {
                        Checkpoint(parser); // r727

                        bool r727 = true;
                        r727 = r727 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r727, parser);
                        a726 = r727;
                    }

                    r723 |= a726;
                } // end may a726

                if (r723)
                { // may a728
                    bool a728 = false;
                    {
                        Checkpoint(parser); // r729

                        bool r729 = true;
                        r729 = r729 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ArgumentList());
                        CommitOrRollback(r729, parser);
                        a728 = r729;
                    }

                    r723 |= a728;
                } // end may a728

                if (r723)
                { // may a730
                    bool a730 = false;
                    {
                        Checkpoint(parser); // r731

                        bool r731 = true;
                        r731 = r731 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r731, parser);
                        a730 = r731;
                    }

                    r723 |= a730;
                } // end may a730

                r723 = r723 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r723, parser);
                res = r723;
            }



            return res;
        }
    }

    public partial class UpdateSetMutator : Jhu.Graywulf.Sql.Parsing.UpdateSetMutator, ICloneable
    {
        public UpdateSetMutator()
            :base()
        {
        }

        public UpdateSetMutator(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public UpdateSetMutator(Jhu.SkyQuery.Sql.Parsing.UpdateSetMutator old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.UpdateSetMutator(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r732

                bool r732 = true;
                r732 = r732 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ColumnName());
                if (r732)
                { // may a733
                    bool a733 = false;
                    {
                        Checkpoint(parser); // r734

                        bool r734 = true;
                        r734 = r734 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r734, parser);
                        a733 = r734;
                    }

                    r732 |= a733;
                } // end may a733

                r732 = r732 && Match(parser, new Jhu.Graywulf.Sql.Parsing.MemberAccessOperator());
                if (r732)
                { // may a735
                    bool a735 = false;
                    {
                        Checkpoint(parser); // r736

                        bool r736 = true;
                        r736 = r736 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r736, parser);
                        a735 = r736;
                    }

                    r732 |= a735;
                } // end may a735

                r732 = r732 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.UdtMethodCall());
                CommitOrRollback(r732, parser);
                res = r732;
            }



            return res;
        }
    }

    public partial class UpdateSetList : Jhu.Graywulf.Sql.Parsing.UpdateSetList, ICloneable
    {
        public UpdateSetList()
            :base()
        {
        }

        public UpdateSetList(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public UpdateSetList(Jhu.SkyQuery.Sql.Parsing.UpdateSetList old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.UpdateSetList(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r737

                bool r737 = true;
                if (r737)
                { // alternatives a738 must
                    bool a738 = false;
                    if (!a738)
                    {
                        Checkpoint(parser); // r739

                        bool r739 = true;
                        r739 = r739 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.UpdateSetColumn());
                        CommitOrRollback(r739, parser);
                        a738 = r739;
                    }

                    if (!a738)
                    {
                        Checkpoint(parser); // r740

                        bool r740 = true;
                        r740 = r740 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.UpdateSetMutator());
                        CommitOrRollback(r740, parser);
                        a738 = r740;
                    }

                    r737 &= a738;

                } // end alternatives a738

                if (r737)
                { // may a741
                    bool a741 = false;
                    {
                        Checkpoint(parser); // r742

                        bool r742 = true;
                        if (r742)
                        { // may a743
                            bool a743 = false;
                            {
                                Checkpoint(parser); // r744

                                bool r744 = true;
                                r744 = r744 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r744, parser);
                                a743 = r744;
                            }

                            r742 |= a743;
                        } // end may a743

                        r742 = r742 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r742)
                        { // may a745
                            bool a745 = false;
                            {
                                Checkpoint(parser); // r746

                                bool r746 = true;
                                r746 = r746 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r746, parser);
                                a745 = r746;
                            }

                            r742 |= a745;
                        } // end may a745

                        r742 = r742 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.UpdateSetList());
                        CommitOrRollback(r742, parser);
                        a741 = r742;
                    }

                    r737 |= a741;
                } // end may a741

                CommitOrRollback(r737, parser);
                res = r737;
            }



            return res;
        }
    }

    public partial class UdtStaticMethodCall : Jhu.Graywulf.Sql.Parsing.UdtStaticMethodCall, ICloneable
    {
        public UdtStaticMethodCall()
            :base()
        {
        }

        public UdtStaticMethodCall(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public UdtStaticMethodCall(Jhu.SkyQuery.Sql.Parsing.UdtStaticMethodCall old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.UdtStaticMethodCall(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r747

                bool r747 = true;
                r747 = r747 && Match(parser, new Jhu.Graywulf.Sql.Parsing.MethodName());
                if (r747)
                { // may a748
                    bool a748 = false;
                    {
                        Checkpoint(parser); // r749

                        bool r749 = true;
                        r749 = r749 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r749, parser);
                        a748 = r749;
                    }

                    r747 |= a748;
                } // end may a748

                r747 = r747 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r747)
                { // may a750
                    bool a750 = false;
                    {
                        Checkpoint(parser); // r751

                        bool r751 = true;
                        r751 = r751 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r751, parser);
                        a750 = r751;
                    }

                    r747 |= a750;
                } // end may a750

                if (r747)
                { // may a752
                    bool a752 = false;
                    {
                        Checkpoint(parser); // r753

                        bool r753 = true;
                        r753 = r753 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ArgumentList());
                        CommitOrRollback(r753, parser);
                        a752 = r753;
                    }

                    r747 |= a752;
                } // end may a752

                if (r747)
                { // may a754
                    bool a754 = false;
                    {
                        Checkpoint(parser); // r755

                        bool r755 = true;
                        r755 = r755 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r755, parser);
                        a754 = r755;
                    }

                    r747 |= a754;
                } // end may a754

                r747 = r747 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r747, parser);
                res = r747;
            }



            return res;
        }
    }

    public partial class UdtStaticMemberAccessList : Jhu.Graywulf.Sql.Parsing.UdtStaticMemberAccessList, ICloneable
    {
        public UdtStaticMemberAccessList()
            :base()
        {
        }

        public UdtStaticMemberAccessList(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public UdtStaticMemberAccessList(Jhu.SkyQuery.Sql.Parsing.UdtStaticMemberAccessList old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.UdtStaticMemberAccessList(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r756

                bool r756 = true;
                r756 = r756 && Match(parser, new Jhu.Graywulf.Sql.Parsing.DataTypeIdentifier());
                if (r756)
                { // may a757
                    bool a757 = false;
                    {
                        Checkpoint(parser); // r758

                        bool r758 = true;
                        r758 = r758 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r758, parser);
                        a757 = r758;
                    }

                    r756 |= a757;
                } // end may a757

                r756 = r756 && Match(parser, new Jhu.Graywulf.Sql.Parsing.StaticMemberAccessOperator());
                if (r756)
                { // may a759
                    bool a759 = false;
                    {
                        Checkpoint(parser); // r760

                        bool r760 = true;
                        r760 = r760 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r760, parser);
                        a759 = r760;
                    }

                    r756 |= a759;
                } // end may a759

                if (r756)
                { // alternatives a761 must
                    bool a761 = false;
                    if (!a761)
                    {
                        Checkpoint(parser); // r762

                        bool r762 = true;
                        r762 = r762 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.UdtStaticMethodCall());
                        CommitOrRollback(r762, parser);
                        a761 = r762;
                    }

                    if (!a761)
                    {
                        Checkpoint(parser); // r763

                        bool r763 = true;
                        r763 = r763 && Match(parser, new Jhu.Graywulf.Sql.Parsing.UdtStaticPropertyAccess());
                        CommitOrRollback(r763, parser);
                        a761 = r763;
                    }

                    r756 &= a761;

                } // end alternatives a761

                CommitOrRollback(r756, parser);
                res = r756;
            }



            return res;
        }
    }

    public partial class PartitionByClause : Jhu.Graywulf.Sql.Parsing.PartitionByClause, ICloneable
    {
        public PartitionByClause()
            :base()
        {
        }

        public PartitionByClause(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public PartitionByClause(Jhu.SkyQuery.Sql.Parsing.PartitionByClause old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.PartitionByClause(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r764

                bool r764 = true;
                r764 = r764 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"PARTITION"));
                r764 = r764 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r764 = r764 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"BY"));
                if (r764)
                { // may a765
                    bool a765 = false;
                    {
                        Checkpoint(parser); // r766

                        bool r766 = true;
                        r766 = r766 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r766, parser);
                        a765 = r766;
                    }

                    r764 |= a765;
                } // end may a765

                r764 = r764 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Argument());
                CommitOrRollback(r764, parser);
                res = r764;
            }



            return res;
        }
    }

    public partial class OverClause : Jhu.Graywulf.Sql.Parsing.OverClause, ICloneable
    {
        public OverClause()
            :base()
        {
        }

        public OverClause(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public OverClause(Jhu.SkyQuery.Sql.Parsing.OverClause old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.OverClause(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r767

                bool r767 = true;
                r767 = r767 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"OVER"));
                if (r767)
                { // may a768
                    bool a768 = false;
                    {
                        Checkpoint(parser); // r769

                        bool r769 = true;
                        r769 = r769 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r769, parser);
                        a768 = r769;
                    }

                    r767 |= a768;
                } // end may a768

                r767 = r767 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r767)
                { // may a770
                    bool a770 = false;
                    {
                        Checkpoint(parser); // r771

                        bool r771 = true;
                        if (r771)
                        { // may a772
                            bool a772 = false;
                            {
                                Checkpoint(parser); // r773

                                bool r773 = true;
                                r773 = r773 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r773, parser);
                                a772 = r773;
                            }

                            r771 |= a772;
                        } // end may a772

                        r771 = r771 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.PartitionByClause());
                        CommitOrRollback(r771, parser);
                        a770 = r771;
                    }

                    r767 |= a770;
                } // end may a770

                if (r767)
                { // may a774
                    bool a774 = false;
                    {
                        Checkpoint(parser); // r775

                        bool r775 = true;
                        if (r775)
                        { // may a776
                            bool a776 = false;
                            {
                                Checkpoint(parser); // r777

                                bool r777 = true;
                                r777 = r777 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r777, parser);
                                a776 = r777;
                            }

                            r775 |= a776;
                        } // end may a776

                        r775 = r775 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OrderByClause());
                        CommitOrRollback(r775, parser);
                        a774 = r775;
                    }

                    r767 |= a774;
                } // end may a774

                if (r767)
                { // may a778
                    bool a778 = false;
                    {
                        Checkpoint(parser); // r779

                        bool r779 = true;
                        r779 = r779 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r779, parser);
                        a778 = r779;
                    }

                    r767 |= a778;
                } // end may a778

                r767 = r767 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r767, parser);
                res = r767;
            }



            return res;
        }
    }

    public partial class StarFunctionCall : Jhu.Graywulf.Sql.Parsing.StarFunctionCall, ICloneable
    {
        public StarFunctionCall()
            :base()
        {
        }

        public StarFunctionCall(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public StarFunctionCall(Jhu.SkyQuery.Sql.Parsing.StarFunctionCall old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.StarFunctionCall(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r780

                bool r780 = true;
                r780 = r780 && Match(parser, new Jhu.Graywulf.Sql.Parsing.FunctionIdentifier());
                if (r780)
                { // may a781
                    bool a781 = false;
                    {
                        Checkpoint(parser); // r782

                        bool r782 = true;
                        r782 = r782 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r782, parser);
                        a781 = r782;
                    }

                    r780 |= a781;
                } // end may a781

                r780 = r780 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r780)
                { // may a783
                    bool a783 = false;
                    {
                        Checkpoint(parser); // r784

                        bool r784 = true;
                        r784 = r784 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r784, parser);
                        a783 = r784;
                    }

                    r780 |= a783;
                } // end may a783

                r780 = r780 && Match(parser, new Jhu.Graywulf.Sql.Parsing.StarArgument());
                if (r780)
                { // may a785
                    bool a785 = false;
                    {
                        Checkpoint(parser); // r786

                        bool r786 = true;
                        r786 = r786 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r786, parser);
                        a785 = r786;
                    }

                    r780 |= a785;
                } // end may a785

                r780 = r780 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                if (r780)
                { // may a787
                    bool a787 = false;
                    {
                        Checkpoint(parser); // r788

                        bool r788 = true;
                        if (r788)
                        { // may a789
                            bool a789 = false;
                            {
                                Checkpoint(parser); // r790

                                bool r790 = true;
                                r790 = r790 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r790, parser);
                                a789 = r790;
                            }

                            r788 |= a789;
                        } // end may a789

                        r788 = r788 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OverClause());
                        CommitOrRollback(r788, parser);
                        a787 = r788;
                    }

                    r780 |= a787;
                } // end may a787

                CommitOrRollback(r780, parser);
                res = r780;
            }



            return res;
        }
    }

    public partial class PrintStatement : Jhu.Graywulf.Sql.Parsing.PrintStatement, ICloneable
    {
        public PrintStatement()
            :base()
        {
        }

        public PrintStatement(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public PrintStatement(Jhu.SkyQuery.Sql.Parsing.PrintStatement old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.PrintStatement(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r791

                bool r791 = true;
                r791 = r791 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"PRINT"));
                r791 = r791 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r791 = r791 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                CommitOrRollback(r791, parser);
                res = r791;
            }



            return res;
        }
    }

    public partial class VariableDeclaration : Jhu.Graywulf.Sql.Parsing.VariableDeclaration, ICloneable
    {
        public VariableDeclaration()
            :base()
        {
        }

        public VariableDeclaration(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public VariableDeclaration(Jhu.SkyQuery.Sql.Parsing.VariableDeclaration old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.VariableDeclaration(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r792

                bool r792 = true;
                r792 = r792 && Match(parser, new Jhu.Graywulf.Sql.Parsing.UserVariable());
                if (r792)
                { // may a793
                    bool a793 = false;
                    {
                        Checkpoint(parser); // r794

                        bool r794 = true;
                        r794 = r794 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        r794 = r794 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"AS"));
                        CommitOrRollback(r794, parser);
                        a793 = r794;
                    }

                    r792 |= a793;
                } // end may a793

                if (r792)
                { // may a795
                    bool a795 = false;
                    {
                        Checkpoint(parser); // r796

                        bool r796 = true;
                        r796 = r796 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r796, parser);
                        a795 = r796;
                    }

                    r792 |= a795;
                } // end may a795

                r792 = r792 && Match(parser, new Jhu.Graywulf.Sql.Parsing.DataTypeSpecification());
                if (r792)
                { // may a797
                    bool a797 = false;
                    {
                        Checkpoint(parser); // r798

                        bool r798 = true;
                        if (r798)
                        { // may a799
                            bool a799 = false;
                            {
                                Checkpoint(parser); // r800

                                bool r800 = true;
                                r800 = r800 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r800, parser);
                                a799 = r800;
                            }

                            r798 |= a799;
                        } // end may a799

                        r798 = r798 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ValueAssignmentOperator());
                        if (r798)
                        { // may a801
                            bool a801 = false;
                            {
                                Checkpoint(parser); // r802

                                bool r802 = true;
                                r802 = r802 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r802, parser);
                                a801 = r802;
                            }

                            r798 |= a801;
                        } // end may a801

                        r798 = r798 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        CommitOrRollback(r798, parser);
                        a797 = r798;
                    }

                    r792 |= a797;
                } // end may a797

                CommitOrRollback(r792, parser);
                res = r792;
            }



            return res;
        }
    }

    public partial class VariableDeclarationList : Jhu.Graywulf.Sql.Parsing.VariableDeclarationList, ICloneable
    {
        public VariableDeclarationList()
            :base()
        {
        }

        public VariableDeclarationList(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public VariableDeclarationList(Jhu.SkyQuery.Sql.Parsing.VariableDeclarationList old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.VariableDeclarationList(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r803

                bool r803 = true;
                r803 = r803 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.VariableDeclaration());
                if (r803)
                { // may a804
                    bool a804 = false;
                    {
                        Checkpoint(parser); // r805

                        bool r805 = true;
                        if (r805)
                        { // may a806
                            bool a806 = false;
                            {
                                Checkpoint(parser); // r807

                                bool r807 = true;
                                r807 = r807 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r807, parser);
                                a806 = r807;
                            }

                            r805 |= a806;
                        } // end may a806

                        r805 = r805 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r805)
                        { // may a808
                            bool a808 = false;
                            {
                                Checkpoint(parser); // r809

                                bool r809 = true;
                                r809 = r809 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r809, parser);
                                a808 = r809;
                            }

                            r805 |= a808;
                        } // end may a808

                        r805 = r805 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.VariableDeclarationList());
                        CommitOrRollback(r805, parser);
                        a804 = r805;
                    }

                    r803 |= a804;
                } // end may a804

                CommitOrRollback(r803, parser);
                res = r803;
            }



            return res;
        }
    }

    public partial class DeclareVariableStatement : Jhu.Graywulf.Sql.Parsing.DeclareVariableStatement, ICloneable
    {
        public DeclareVariableStatement()
            :base()
        {
        }

        public DeclareVariableStatement(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public DeclareVariableStatement(Jhu.SkyQuery.Sql.Parsing.DeclareVariableStatement old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.DeclareVariableStatement(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r810

                bool r810 = true;
                r810 = r810 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DECLARE"));
                if (r810)
                { // may a811
                    bool a811 = false;
                    {
                        Checkpoint(parser); // r812

                        bool r812 = true;
                        r812 = r812 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r812, parser);
                        a811 = r812;
                    }

                    r810 |= a811;
                } // end may a811

                r810 = r810 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.VariableDeclarationList());
                CommitOrRollback(r810, parser);
                res = r810;
            }



            return res;
        }
    }

    public partial class SetVariableStatement : Jhu.Graywulf.Sql.Parsing.SetVariableStatement, ICloneable
    {
        public SetVariableStatement()
            :base()
        {
        }

        public SetVariableStatement(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public SetVariableStatement(Jhu.SkyQuery.Sql.Parsing.SetVariableStatement old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.SetVariableStatement(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r813

                bool r813 = true;
                r813 = r813 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"SET"));
                r813 = r813 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r813 = r813 && Match(parser, new Jhu.Graywulf.Sql.Parsing.UserVariable());
                if (r813)
                { // may a814
                    bool a814 = false;
                    {
                        Checkpoint(parser); // r815

                        bool r815 = true;
                        r815 = r815 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r815, parser);
                        a814 = r815;
                    }

                    r813 |= a814;
                } // end may a814

                r813 = r813 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ValueAssignmentOperator());
                if (r813)
                { // may a816
                    bool a816 = false;
                    {
                        Checkpoint(parser); // r817

                        bool r817 = true;
                        r817 = r817 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r817, parser);
                        a816 = r817;
                    }

                    r813 |= a816;
                } // end may a816

                r813 = r813 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                CommitOrRollback(r813, parser);
                res = r813;
            }



            return res;
        }
    }

    public partial class TopExpression : Jhu.Graywulf.Sql.Parsing.TopExpression, ICloneable
    {
        public TopExpression()
            :base()
        {
        }

        public TopExpression(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public TopExpression(Jhu.SkyQuery.Sql.Parsing.TopExpression old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.TopExpression(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r818

                bool r818 = true;
                r818 = r818 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TOP"));
                if (r818)
                { // may a819
                    bool a819 = false;
                    {
                        Checkpoint(parser); // r820

                        bool r820 = true;
                        r820 = r820 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r820, parser);
                        a819 = r820;
                    }

                    r818 |= a819;
                } // end may a819

                if (r818)
                { // alternatives a821 must
                    bool a821 = false;
                    if (!a821)
                    {
                        Checkpoint(parser); // r822

                        bool r822 = true;
                        r822 = r822 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        CommitOrRollback(r822, parser);
                        a821 = r822;
                    }

                    r818 &= a821;

                } // end alternatives a821

                if (r818)
                { // may a823
                    bool a823 = false;
                    {
                        Checkpoint(parser); // r824

                        bool r824 = true;
                        r824 = r824 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        r824 = r824 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"PERCENT"));
                        CommitOrRollback(r824, parser);
                        a823 = r824;
                    }

                    r818 |= a823;
                } // end may a823

                if (r818)
                { // may a825
                    bool a825 = false;
                    {
                        Checkpoint(parser); // r826

                        bool r826 = true;
                        r826 = r826 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        r826 = r826 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"WITH"));
                        r826 = r826 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        r826 = r826 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TIES"));
                        CommitOrRollback(r826, parser);
                        a825 = r826;
                    }

                    r818 |= a825;
                } // end may a825

                CommitOrRollback(r818, parser);
                res = r818;
            }



            return res;
        }
    }

    public partial class ColumnExpression : Jhu.Graywulf.Sql.Parsing.ColumnExpression, ICloneable
    {
        public ColumnExpression()
            :base()
        {
        }

        public ColumnExpression(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public ColumnExpression(Jhu.SkyQuery.Sql.Parsing.ColumnExpression old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.ColumnExpression(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r827

                bool r827 = true;
                if (r827)
                { // alternatives a828 must
                    bool a828 = false;
                    if (!a828)
                    {
                        Checkpoint(parser); // r829

                        bool r829 = true;
                        r829 = r829 && Match(parser, new Jhu.Graywulf.Sql.Parsing.UserVariable());
                        if (r829)
                        { // may a830
                            bool a830 = false;
                            {
                                Checkpoint(parser); // r831

                                bool r831 = true;
                                r831 = r831 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r831, parser);
                                a830 = r831;
                            }

                            r829 |= a830;
                        } // end may a830

                        r829 = r829 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ValueAssignmentOperator());
                        if (r829)
                        { // may a832
                            bool a832 = false;
                            {
                                Checkpoint(parser); // r833

                                bool r833 = true;
                                r833 = r833 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r833, parser);
                                a832 = r833;
                            }

                            r829 |= a832;
                        } // end may a832

                        r829 = r829 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        CommitOrRollback(r829, parser);
                        a828 = r829;
                    }

                    if (!a828)
                    {
                        Checkpoint(parser); // r834

                        bool r834 = true;
                        r834 = r834 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ColumnAlias());
                        if (r834)
                        { // may a835
                            bool a835 = false;
                            {
                                Checkpoint(parser); // r836

                                bool r836 = true;
                                r836 = r836 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r836, parser);
                                a835 = r836;
                            }

                            r834 |= a835;
                        } // end may a835

                        r834 = r834 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ValueAssignmentOperator());
                        if (r834)
                        { // may a837
                            bool a837 = false;
                            {
                                Checkpoint(parser); // r838

                                bool r838 = true;
                                r838 = r838 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r838, parser);
                                a837 = r838;
                            }

                            r834 |= a837;
                        } // end may a837

                        r834 = r834 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        CommitOrRollback(r834, parser);
                        a828 = r834;
                    }

                    if (!a828)
                    {
                        Checkpoint(parser); // r839

                        bool r839 = true;
                        r839 = r839 && Match(parser, new Jhu.Graywulf.Sql.Parsing.StarColumnIdentifier());
                        CommitOrRollback(r839, parser);
                        a828 = r839;
                    }

                    if (!a828)
                    {
                        Checkpoint(parser); // r840

                        bool r840 = true;
                        r840 = r840 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        if (r840)
                        { // may a841
                            bool a841 = false;
                            {
                                Checkpoint(parser); // r842

                                bool r842 = true;
                                r842 = r842 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r842, parser);
                                a841 = r842;
                            }

                            r840 |= a841;
                        } // end may a841

                        if (r840)
                        { // may a843
                            bool a843 = false;
                            {
                                Checkpoint(parser); // r844

                                bool r844 = true;
                                r844 = r844 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"AS"));
                                if (r844)
                                { // may a845
                                    bool a845 = false;
                                    {
                                        Checkpoint(parser); // r846

                                        bool r846 = true;
                                        r846 = r846 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                        CommitOrRollback(r846, parser);
                                        a845 = r846;
                                    }

                                    r844 |= a845;
                                } // end may a845

                                CommitOrRollback(r844, parser);
                                a843 = r844;
                            }

                            r840 |= a843;
                        } // end may a843

                        r840 = r840 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ColumnAlias());
                        CommitOrRollback(r840, parser);
                        a828 = r840;
                    }

                    if (!a828)
                    {
                        Checkpoint(parser); // r847

                        bool r847 = true;
                        r847 = r847 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        CommitOrRollback(r847, parser);
                        a828 = r847;
                    }

                    r827 &= a828;

                } // end alternatives a828

                CommitOrRollback(r827, parser);
                res = r827;
            }



            return res;
        }
    }

    public partial class SelectList : Jhu.Graywulf.Sql.Parsing.SelectList, ICloneable
    {
        public SelectList()
            :base()
        {
        }

        public SelectList(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public SelectList(Jhu.SkyQuery.Sql.Parsing.SelectList old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.SelectList(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r848

                bool r848 = true;
                r848 = r848 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ColumnExpression());
                if (r848)
                { // may a849
                    bool a849 = false;
                    {
                        Checkpoint(parser); // r850

                        bool r850 = true;
                        if (r850)
                        { // may a851
                            bool a851 = false;
                            {
                                Checkpoint(parser); // r852

                                bool r852 = true;
                                r852 = r852 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r852, parser);
                                a851 = r852;
                            }

                            r850 |= a851;
                        } // end may a851

                        r850 = r850 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r850)
                        { // may a853
                            bool a853 = false;
                            {
                                Checkpoint(parser); // r854

                                bool r854 = true;
                                r854 = r854 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r854, parser);
                                a853 = r854;
                            }

                            r850 |= a853;
                        } // end may a853

                        r850 = r850 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SelectList());
                        CommitOrRollback(r850, parser);
                        a849 = r850;
                    }

                    r848 |= a849;
                } // end may a849

                CommitOrRollback(r848, parser);
                res = r848;
            }



            return res;
        }
    }

    public partial class GroupByList : Jhu.Graywulf.Sql.Parsing.GroupByList, ICloneable
    {
        public GroupByList()
            :base()
        {
        }

        public GroupByList(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public GroupByList(Jhu.SkyQuery.Sql.Parsing.GroupByList old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.GroupByList(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r855

                bool r855 = true;
                r855 = r855 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r855)
                { // may a856
                    bool a856 = false;
                    {
                        Checkpoint(parser); // r857

                        bool r857 = true;
                        if (r857)
                        { // may a858
                            bool a858 = false;
                            {
                                Checkpoint(parser); // r859

                                bool r859 = true;
                                r859 = r859 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r859, parser);
                                a858 = r859;
                            }

                            r857 |= a858;
                        } // end may a858

                        r857 = r857 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r857)
                        { // may a860
                            bool a860 = false;
                            {
                                Checkpoint(parser); // r861

                                bool r861 = true;
                                r861 = r861 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r861, parser);
                                a860 = r861;
                            }

                            r857 |= a860;
                        } // end may a860

                        r857 = r857 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.GroupByList());
                        CommitOrRollback(r857, parser);
                        a856 = r857;
                    }

                    r855 |= a856;
                } // end may a856

                CommitOrRollback(r855, parser);
                res = r855;
            }



            return res;
        }
    }

    public partial class GroupByClause : Jhu.Graywulf.Sql.Parsing.GroupByClause, ICloneable
    {
        public GroupByClause()
            :base()
        {
        }

        public GroupByClause(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public GroupByClause(Jhu.SkyQuery.Sql.Parsing.GroupByClause old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.GroupByClause(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r862

                bool r862 = true;
                r862 = r862 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"GROUP"));
                r862 = r862 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r862 = r862 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"BY"));
                if (r862)
                { // alternatives a863 must
                    bool a863 = false;
                    if (!a863)
                    {
                        Checkpoint(parser); // r864

                        bool r864 = true;
                        r864 = r864 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        r864 = r864 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ALL"));
                        CommitOrRollback(r864, parser);
                        a863 = r864;
                    }

                    if (!a863)
                    {
                        Checkpoint(parser); // r865

                        bool r865 = true;
                        if (r865)
                        { // may a866
                            bool a866 = false;
                            {
                                Checkpoint(parser); // r867

                                bool r867 = true;
                                r867 = r867 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r867, parser);
                                a866 = r867;
                            }

                            r865 |= a866;
                        } // end may a866

                        r865 = r865 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.GroupByList());
                        CommitOrRollback(r865, parser);
                        a863 = r865;
                    }

                    r862 &= a863;

                } // end alternatives a863

                CommitOrRollback(r862, parser);
                res = r862;
            }



            return res;
        }
    }

    public partial class OrderByArgument : Jhu.Graywulf.Sql.Parsing.OrderByArgument, ICloneable
    {
        public OrderByArgument()
            :base()
        {
        }

        public OrderByArgument(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public OrderByArgument(Jhu.SkyQuery.Sql.Parsing.OrderByArgument old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.OrderByArgument(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r868

                bool r868 = true;
                r868 = r868 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r868)
                { // may a869
                    bool a869 = false;
                    {
                        Checkpoint(parser); // r870

                        bool r870 = true;
                        if (r870)
                        { // may a871
                            bool a871 = false;
                            {
                                Checkpoint(parser); // r872

                                bool r872 = true;
                                r872 = r872 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r872, parser);
                                a871 = r872;
                            }

                            r870 |= a871;
                        } // end may a871

                        if (r870)
                        { // alternatives a873 must
                            bool a873 = false;
                            if (!a873)
                            {
                                Checkpoint(parser); // r874

                                bool r874 = true;
                                r874 = r874 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ASC"));
                                CommitOrRollback(r874, parser);
                                a873 = r874;
                            }

                            if (!a873)
                            {
                                Checkpoint(parser); // r875

                                bool r875 = true;
                                r875 = r875 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DESC"));
                                CommitOrRollback(r875, parser);
                                a873 = r875;
                            }

                            r870 &= a873;

                        } // end alternatives a873

                        CommitOrRollback(r870, parser);
                        a869 = r870;
                    }

                    r868 |= a869;
                } // end may a869

                CommitOrRollback(r868, parser);
                res = r868;
            }



            return res;
        }
    }

    public partial class OrderByArgumentList : Jhu.Graywulf.Sql.Parsing.OrderByArgumentList, ICloneable
    {
        public OrderByArgumentList()
            :base()
        {
        }

        public OrderByArgumentList(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public OrderByArgumentList(Jhu.SkyQuery.Sql.Parsing.OrderByArgumentList old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.OrderByArgumentList(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r876

                bool r876 = true;
                r876 = r876 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OrderByArgument());
                if (r876)
                { // may a877
                    bool a877 = false;
                    {
                        Checkpoint(parser); // r878

                        bool r878 = true;
                        if (r878)
                        { // may a879
                            bool a879 = false;
                            {
                                Checkpoint(parser); // r880

                                bool r880 = true;
                                r880 = r880 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r880, parser);
                                a879 = r880;
                            }

                            r878 |= a879;
                        } // end may a879

                        r878 = r878 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r878)
                        { // may a881
                            bool a881 = false;
                            {
                                Checkpoint(parser); // r882

                                bool r882 = true;
                                r882 = r882 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r882, parser);
                                a881 = r882;
                            }

                            r878 |= a881;
                        } // end may a881

                        r878 = r878 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OrderByArgumentList());
                        CommitOrRollback(r878, parser);
                        a877 = r878;
                    }

                    r876 |= a877;
                } // end may a877

                CommitOrRollback(r876, parser);
                res = r876;
            }



            return res;
        }
    }

    public partial class OrderByClause : Jhu.Graywulf.Sql.Parsing.OrderByClause, ICloneable
    {
        public OrderByClause()
            :base()
        {
        }

        public OrderByClause(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public OrderByClause(Jhu.SkyQuery.Sql.Parsing.OrderByClause old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.OrderByClause(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r883

                bool r883 = true;
                r883 = r883 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ORDER"));
                r883 = r883 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r883 = r883 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"BY"));
                if (r883)
                { // may a884
                    bool a884 = false;
                    {
                        Checkpoint(parser); // r885

                        bool r885 = true;
                        r885 = r885 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r885, parser);
                        a884 = r885;
                    }

                    r883 |= a884;
                } // end may a884

                r883 = r883 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OrderByArgumentList());
                CommitOrRollback(r883, parser);
                res = r883;
            }



            return res;
        }
    }

    public partial class InsertStatement : Jhu.Graywulf.Sql.Parsing.InsertStatement, ICloneable
    {
        public InsertStatement()
            :base()
        {
        }

        public InsertStatement(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public InsertStatement(Jhu.SkyQuery.Sql.Parsing.InsertStatement old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.InsertStatement(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r886

                bool r886 = true;
                if (r886)
                { // may a887
                    bool a887 = false;
                    {
                        Checkpoint(parser); // r888

                        bool r888 = true;
                        r888 = r888 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommonTableExpression());
                        if (r888)
                        { // may a889
                            bool a889 = false;
                            {
                                Checkpoint(parser); // r890

                                bool r890 = true;
                                r890 = r890 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r890, parser);
                                a889 = r890;
                            }

                            r888 |= a889;
                        } // end may a889

                        CommitOrRollback(r888, parser);
                        a887 = r888;
                    }

                    r886 |= a887;
                } // end may a887

                r886 = r886 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"INSERT"));
                if (r886)
                { // may a891
                    bool a891 = false;
                    {
                        Checkpoint(parser); // r892

                        bool r892 = true;
                        r892 = r892 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r892, parser);
                        a891 = r892;
                    }

                    r886 |= a891;
                } // end may a891

                if (r886)
                { // alternatives a893 must
                    bool a893 = false;
                    if (!a893)
                    {
                        Checkpoint(parser); // r894

                        bool r894 = true;
                        r894 = r894 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.IntoClause());
                        CommitOrRollback(r894, parser);
                        a893 = r894;
                    }

                    if (!a893)
                    {
                        Checkpoint(parser); // r895

                        bool r895 = true;
                        r895 = r895 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TargetTableSpecification());
                        CommitOrRollback(r895, parser);
                        a893 = r895;
                    }

                    r886 &= a893;

                } // end alternatives a893

                if (r886)
                { // may a896
                    bool a896 = false;
                    {
                        Checkpoint(parser); // r897

                        bool r897 = true;
                        if (r897)
                        { // may a898
                            bool a898 = false;
                            {
                                Checkpoint(parser); // r899

                                bool r899 = true;
                                r899 = r899 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r899, parser);
                                a898 = r899;
                            }

                            r897 |= a898;
                        } // end may a898

                        r897 = r897 && Match(parser, new Jhu.Graywulf.Sql.Parsing.InsertColumnList());
                        CommitOrRollback(r897, parser);
                        a896 = r897;
                    }

                    r886 |= a896;
                } // end may a896

                if (r886)
                { // may a900
                    bool a900 = false;
                    {
                        Checkpoint(parser); // r901

                        bool r901 = true;
                        r901 = r901 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r901, parser);
                        a900 = r901;
                    }

                    r886 |= a900;
                } // end may a900

                if (r886)
                { // alternatives a902 must
                    bool a902 = false;
                    if (!a902)
                    {
                        Checkpoint(parser); // r903

                        bool r903 = true;
                        r903 = r903 && Match(parser, new Jhu.Graywulf.Sql.Parsing.DefaultValues());
                        CommitOrRollback(r903, parser);
                        a902 = r903;
                    }

                    if (!a902)
                    {
                        Checkpoint(parser); // r904

                        bool r904 = true;
                        r904 = r904 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ValuesClause());
                        CommitOrRollback(r904, parser);
                        a902 = r904;
                    }

                    if (!a902)
                    {
                        Checkpoint(parser); // r905

                        bool r905 = true;
                        r905 = r905 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.QueryExpression());
                        if (r905)
                        { // may a906
                            bool a906 = false;
                            {
                                Checkpoint(parser); // r907

                                bool r907 = true;
                                if (r907)
                                { // may a908
                                    bool a908 = false;
                                    {
                                        Checkpoint(parser); // r909

                                        bool r909 = true;
                                        r909 = r909 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                        CommitOrRollback(r909, parser);
                                        a908 = r909;
                                    }

                                    r907 |= a908;
                                } // end may a908

                                r907 = r907 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OrderByClause());
                                CommitOrRollback(r907, parser);
                                a906 = r907;
                            }

                            r905 |= a906;
                        } // end may a906

                        if (r905)
                        { // may a910
                            bool a910 = false;
                            {
                                Checkpoint(parser); // r911

                                bool r911 = true;
                                if (r911)
                                { // may a912
                                    bool a912 = false;
                                    {
                                        Checkpoint(parser); // r913

                                        bool r913 = true;
                                        r913 = r913 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                        CommitOrRollback(r913, parser);
                                        a912 = r913;
                                    }

                                    r911 |= a912;
                                } // end may a912

                                r911 = r911 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OptionClause());
                                CommitOrRollback(r911, parser);
                                a910 = r911;
                            }

                            r905 |= a910;
                        } // end may a910

                        CommitOrRollback(r905, parser);
                        a902 = r905;
                    }

                    r886 &= a902;

                } // end alternatives a902

                CommitOrRollback(r886, parser);
                res = r886;
            }



            return res;
        }
    }

    public partial class HintArgument : Jhu.Graywulf.Sql.Parsing.HintArgument, ICloneable
    {
        public HintArgument()
            :base()
        {
        }

        public HintArgument(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public HintArgument(Jhu.SkyQuery.Sql.Parsing.HintArgument old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.HintArgument(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r914

                bool r914 = true;
                r914 = r914 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                CommitOrRollback(r914, parser);
                res = r914;
            }



            return res;
        }
    }

    public partial class HintArgumentList : Jhu.Graywulf.Sql.Parsing.HintArgumentList, ICloneable
    {
        public HintArgumentList()
            :base()
        {
        }

        public HintArgumentList(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public HintArgumentList(Jhu.SkyQuery.Sql.Parsing.HintArgumentList old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.HintArgumentList(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r915

                bool r915 = true;
                r915 = r915 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.HintArgument());
                if (r915)
                { // may a916
                    bool a916 = false;
                    {
                        Checkpoint(parser); // r917

                        bool r917 = true;
                        if (r917)
                        { // may a918
                            bool a918 = false;
                            {
                                Checkpoint(parser); // r919

                                bool r919 = true;
                                r919 = r919 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r919, parser);
                                a918 = r919;
                            }

                            r917 |= a918;
                        } // end may a918

                        r917 = r917 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r917)
                        { // may a920
                            bool a920 = false;
                            {
                                Checkpoint(parser); // r921

                                bool r921 = true;
                                r921 = r921 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r921, parser);
                                a920 = r921;
                            }

                            r917 |= a920;
                        } // end may a920

                        r917 = r917 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.HintArgumentList());
                        CommitOrRollback(r917, parser);
                        a916 = r917;
                    }

                    r915 |= a916;
                } // end may a916

                CommitOrRollback(r915, parser);
                res = r915;
            }



            return res;
        }
    }

    public partial class HintArguments : Jhu.Graywulf.Sql.Parsing.HintArguments, ICloneable
    {
        public HintArguments()
            :base()
        {
        }

        public HintArguments(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public HintArguments(Jhu.SkyQuery.Sql.Parsing.HintArguments old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.HintArguments(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r922

                bool r922 = true;
                r922 = r922 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r922)
                { // may a923
                    bool a923 = false;
                    {
                        Checkpoint(parser); // r924

                        bool r924 = true;
                        r924 = r924 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r924, parser);
                        a923 = r924;
                    }

                    r922 |= a923;
                } // end may a923

                if (r922)
                { // may a925
                    bool a925 = false;
                    {
                        Checkpoint(parser); // r926

                        bool r926 = true;
                        r926 = r926 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.HintArgumentList());
                        CommitOrRollback(r926, parser);
                        a925 = r926;
                    }

                    r922 |= a925;
                } // end may a925

                if (r922)
                { // may a927
                    bool a927 = false;
                    {
                        Checkpoint(parser); // r928

                        bool r928 = true;
                        r928 = r928 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r928, parser);
                        a927 = r928;
                    }

                    r922 |= a927;
                } // end may a927

                r922 = r922 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r922, parser);
                res = r922;
            }



            return res;
        }
    }

    public partial class TableHint : Jhu.Graywulf.Sql.Parsing.TableHint, ICloneable
    {
        public TableHint()
            :base()
        {
        }

        public TableHint(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public TableHint(Jhu.SkyQuery.Sql.Parsing.TableHint old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.TableHint(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r929

                bool r929 = true;
                if (r929)
                { // alternatives a930 must
                    bool a930 = false;
                    if (!a930)
                    {
                        Checkpoint(parser); // r931

                        bool r931 = true;
                        r931 = r931 && Match(parser, new Jhu.Graywulf.Sql.Parsing.HintName());
                        if (r931)
                        { // may a932
                            bool a932 = false;
                            {
                                Checkpoint(parser); // r933

                                bool r933 = true;
                                r933 = r933 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r933, parser);
                                a932 = r933;
                            }

                            r931 |= a932;
                        } // end may a932

                        r931 = r931 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.HintArguments());
                        CommitOrRollback(r931, parser);
                        a930 = r931;
                    }

                    if (!a930)
                    {
                        Checkpoint(parser); // r934

                        bool r934 = true;
                        r934 = r934 && Match(parser, new Jhu.Graywulf.Sql.Parsing.HintName());
                        CommitOrRollback(r934, parser);
                        a930 = r934;
                    }

                    r929 &= a930;

                } // end alternatives a930

                CommitOrRollback(r929, parser);
                res = r929;
            }



            return res;
        }
    }

    public partial class TableHintList : Jhu.Graywulf.Sql.Parsing.TableHintList, ICloneable
    {
        public TableHintList()
            :base()
        {
        }

        public TableHintList(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public TableHintList(Jhu.SkyQuery.Sql.Parsing.TableHintList old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.TableHintList(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r935

                bool r935 = true;
                r935 = r935 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableHint());
                if (r935)
                { // may a936
                    bool a936 = false;
                    {
                        Checkpoint(parser); // r937

                        bool r937 = true;
                        if (r937)
                        { // may a938
                            bool a938 = false;
                            {
                                Checkpoint(parser); // r939

                                bool r939 = true;
                                r939 = r939 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r939, parser);
                                a938 = r939;
                            }

                            r937 |= a938;
                        } // end may a938

                        r937 = r937 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r937)
                        { // may a940
                            bool a940 = false;
                            {
                                Checkpoint(parser); // r941

                                bool r941 = true;
                                r941 = r941 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r941, parser);
                                a940 = r941;
                            }

                            r937 |= a940;
                        } // end may a940

                        r937 = r937 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableHintList());
                        CommitOrRollback(r937, parser);
                        a936 = r937;
                    }

                    r935 |= a936;
                } // end may a936

                CommitOrRollback(r935, parser);
                res = r935;
            }



            return res;
        }
    }

    public partial class TableHintClause : Jhu.Graywulf.Sql.Parsing.TableHintClause, ICloneable
    {
        public TableHintClause()
            :base()
        {
        }

        public TableHintClause(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public TableHintClause(Jhu.SkyQuery.Sql.Parsing.TableHintClause old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.TableHintClause(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r942

                bool r942 = true;
                r942 = r942 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"WITH"));
                if (r942)
                { // may a943
                    bool a943 = false;
                    {
                        Checkpoint(parser); // r944

                        bool r944 = true;
                        r944 = r944 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r944, parser);
                        a943 = r944;
                    }

                    r942 |= a943;
                } // end may a943

                r942 = r942 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r942)
                { // may a945
                    bool a945 = false;
                    {
                        Checkpoint(parser); // r946

                        bool r946 = true;
                        r946 = r946 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r946, parser);
                        a945 = r946;
                    }

                    r942 |= a945;
                } // end may a945

                if (r942)
                { // may a947
                    bool a947 = false;
                    {
                        Checkpoint(parser); // r948

                        bool r948 = true;
                        r948 = r948 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableHintList());
                        CommitOrRollback(r948, parser);
                        a947 = r948;
                    }

                    r942 |= a947;
                } // end may a947

                if (r942)
                { // may a949
                    bool a949 = false;
                    {
                        Checkpoint(parser); // r950

                        bool r950 = true;
                        r950 = r950 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r950, parser);
                        a949 = r950;
                    }

                    r942 |= a949;
                } // end may a949

                r942 = r942 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r942, parser);
                res = r942;
            }



            return res;
        }
    }

    public partial class TargetTableSpecification : Jhu.Graywulf.Sql.Parsing.TargetTableSpecification, ICloneable
    {
        public TargetTableSpecification()
            :base()
        {
        }

        public TargetTableSpecification(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public TargetTableSpecification(Jhu.SkyQuery.Sql.Parsing.TargetTableSpecification old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.TargetTableSpecification(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r951

                bool r951 = true;
                if (r951)
                { // alternatives a952 must
                    bool a952 = false;
                    if (!a952)
                    {
                        Checkpoint(parser); // r953

                        bool r953 = true;
                        r953 = r953 && Match(parser, new Jhu.Graywulf.Sql.Parsing.UserVariable());
                        CommitOrRollback(r953, parser);
                        a952 = r953;
                    }

                    if (!a952)
                    {
                        Checkpoint(parser); // r954

                        bool r954 = true;
                        r954 = r954 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableOrViewIdentifier());
                        CommitOrRollback(r954, parser);
                        a952 = r954;
                    }

                    r951 &= a952;

                } // end alternatives a952

                if (r951)
                { // may a955
                    bool a955 = false;
                    {
                        Checkpoint(parser); // r956

                        bool r956 = true;
                        if (r956)
                        { // may a957
                            bool a957 = false;
                            {
                                Checkpoint(parser); // r958

                                bool r958 = true;
                                r958 = r958 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r958, parser);
                                a957 = r958;
                            }

                            r956 |= a957;
                        } // end may a957

                        r956 = r956 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableHintClause());
                        CommitOrRollback(r956, parser);
                        a955 = r956;
                    }

                    r951 |= a955;
                } // end may a955

                CommitOrRollback(r951, parser);
                res = r951;
            }



            return res;
        }
    }

    public partial class IntoClause : Jhu.Graywulf.Sql.Parsing.IntoClause, ICloneable
    {
        public IntoClause()
            :base()
        {
        }

        public IntoClause(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public IntoClause(Jhu.SkyQuery.Sql.Parsing.IntoClause old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.IntoClause(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r959

                bool r959 = true;
                r959 = r959 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"INTO"));
                if (r959)
                { // may a960
                    bool a960 = false;
                    {
                        Checkpoint(parser); // r961

                        bool r961 = true;
                        r961 = r961 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r961, parser);
                        a960 = r961;
                    }

                    r959 |= a960;
                } // end may a960

                r959 = r959 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TargetTableSpecification());
                CommitOrRollback(r959, parser);
                res = r959;
            }



            return res;
        }
    }

    public partial class SimpleTableSource : Jhu.Graywulf.Sql.Parsing.SimpleTableSource, ICloneable
    {
        public SimpleTableSource()
            :base()
        {
        }

        public SimpleTableSource(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public SimpleTableSource(Jhu.SkyQuery.Sql.Parsing.SimpleTableSource old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.SimpleTableSource(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r962

                bool r962 = true;
                r962 = r962 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableOrViewIdentifier());
                if (r962)
                { // may a963
                    bool a963 = false;
                    {
                        Checkpoint(parser); // r964

                        bool r964 = true;
                        if (r964)
                        { // may a965
                            bool a965 = false;
                            {
                                Checkpoint(parser); // r966

                                bool r966 = true;
                                r966 = r966 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r966, parser);
                                a965 = r966;
                            }

                            r964 |= a965;
                        } // end may a965

                        if (r964)
                        { // may a967
                            bool a967 = false;
                            {
                                Checkpoint(parser); // r968

                                bool r968 = true;
                                r968 = r968 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"AS"));
                                if (r968)
                                { // may a969
                                    bool a969 = false;
                                    {
                                        Checkpoint(parser); // r970

                                        bool r970 = true;
                                        r970 = r970 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                        CommitOrRollback(r970, parser);
                                        a969 = r970;
                                    }

                                    r968 |= a969;
                                } // end may a969

                                CommitOrRollback(r968, parser);
                                a967 = r968;
                            }

                            r964 |= a967;
                        } // end may a967

                        r964 = r964 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableAlias());
                        CommitOrRollback(r964, parser);
                        a963 = r964;
                    }

                    r962 |= a963;
                } // end may a963

                if (r962)
                { // may a971
                    bool a971 = false;
                    {
                        Checkpoint(parser); // r972

                        bool r972 = true;
                        if (r972)
                        { // may a973
                            bool a973 = false;
                            {
                                Checkpoint(parser); // r974

                                bool r974 = true;
                                r974 = r974 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r974, parser);
                                a973 = r974;
                            }

                            r972 |= a973;
                        } // end may a973

                        r972 = r972 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableSampleClause());
                        CommitOrRollback(r972, parser);
                        a971 = r972;
                    }

                    r962 |= a971;
                } // end may a971

                if (r962)
                { // may a975
                    bool a975 = false;
                    {
                        Checkpoint(parser); // r976

                        bool r976 = true;
                        if (r976)
                        { // may a977
                            bool a977 = false;
                            {
                                Checkpoint(parser); // r978

                                bool r978 = true;
                                r978 = r978 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r978, parser);
                                a977 = r978;
                            }

                            r976 |= a977;
                        } // end may a977

                        r976 = r976 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableHintClause());
                        CommitOrRollback(r976, parser);
                        a975 = r976;
                    }

                    r962 |= a975;
                } // end may a975

                CommitOrRollback(r962, parser);
                res = r962;
            }



            return res;
        }
    }

    public partial class QueryHint : Jhu.Graywulf.Sql.Parsing.QueryHint, ICloneable
    {
        public QueryHint()
            :base()
        {
        }

        public QueryHint(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public QueryHint(Jhu.SkyQuery.Sql.Parsing.QueryHint old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.QueryHint(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r979

                bool r979 = true;
                if (r979)
                { // alternatives a980 must
                    bool a980 = false;
                    if (!a980)
                    {
                        Checkpoint(parser); // r981

                        bool r981 = true;
                        r981 = r981 && Match(parser, new Jhu.Graywulf.Sql.Parsing.HintName());
                        if (r981)
                        { // may a982
                            bool a982 = false;
                            {
                                Checkpoint(parser); // r983

                                bool r983 = true;
                                r983 = r983 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r983, parser);
                                a982 = r983;
                            }

                            r981 |= a982;
                        } // end may a982

                        r981 = r981 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.HintArguments());
                        CommitOrRollback(r981, parser);
                        a980 = r981;
                    }

                    if (!a980)
                    {
                        Checkpoint(parser); // r984

                        bool r984 = true;
                        r984 = r984 && Match(parser, new Jhu.Graywulf.Sql.Parsing.HintName());
                        if (r984)
                        { // may a985
                            bool a985 = false;
                            {
                                Checkpoint(parser); // r986

                                bool r986 = true;
                                r986 = r986 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r986, parser);
                                a985 = r986;
                            }

                            r984 |= a985;
                        } // end may a985

                        r984 = r984 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Equals1());
                        if (r984)
                        { // may a987
                            bool a987 = false;
                            {
                                Checkpoint(parser); // r988

                                bool r988 = true;
                                r988 = r988 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r988, parser);
                                a987 = r988;
                            }

                            r984 |= a987;
                        } // end may a987

                        r984 = r984 && Match(parser, new Jhu.Graywulf.Sql.Parsing.NumericConstant());
                        CommitOrRollback(r984, parser);
                        a980 = r984;
                    }

                    if (!a980)
                    {
                        Checkpoint(parser); // r989

                        bool r989 = true;
                        r989 = r989 && Match(parser, new Jhu.Graywulf.Sql.Parsing.HintName());
                        r989 = r989 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        r989 = r989 && Match(parser, new Jhu.Graywulf.Sql.Parsing.NumericConstant());
                        CommitOrRollback(r989, parser);
                        a980 = r989;
                    }

                    if (!a980)
                    {
                        Checkpoint(parser); // r990

                        bool r990 = true;
                        r990 = r990 && Match(parser, new Jhu.Graywulf.Sql.Parsing.QueryHintNameList());
                        CommitOrRollback(r990, parser);
                        a980 = r990;
                    }

                    if (!a980)
                    {
                        Checkpoint(parser); // r991

                        bool r991 = true;
                        r991 = r991 && Match(parser, new Jhu.Graywulf.Sql.Parsing.HintName());
                        CommitOrRollback(r991, parser);
                        a980 = r991;
                    }

                    r979 &= a980;

                } // end alternatives a980

                CommitOrRollback(r979, parser);
                res = r979;
            }



            return res;
        }
    }

    public partial class QueryHintList : Jhu.Graywulf.Sql.Parsing.QueryHintList, ICloneable
    {
        public QueryHintList()
            :base()
        {
        }

        public QueryHintList(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public QueryHintList(Jhu.SkyQuery.Sql.Parsing.QueryHintList old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.QueryHintList(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r992

                bool r992 = true;
                r992 = r992 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.QueryHint());
                if (r992)
                { // may a993
                    bool a993 = false;
                    {
                        Checkpoint(parser); // r994

                        bool r994 = true;
                        if (r994)
                        { // may a995
                            bool a995 = false;
                            {
                                Checkpoint(parser); // r996

                                bool r996 = true;
                                r996 = r996 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r996, parser);
                                a995 = r996;
                            }

                            r994 |= a995;
                        } // end may a995

                        r994 = r994 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r994)
                        { // may a997
                            bool a997 = false;
                            {
                                Checkpoint(parser); // r998

                                bool r998 = true;
                                r998 = r998 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r998, parser);
                                a997 = r998;
                            }

                            r994 |= a997;
                        } // end may a997

                        r994 = r994 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.QueryHintList());
                        CommitOrRollback(r994, parser);
                        a993 = r994;
                    }

                    r992 |= a993;
                } // end may a993

                CommitOrRollback(r992, parser);
                res = r992;
            }



            return res;
        }
    }

    public partial class OptionClause : Jhu.Graywulf.Sql.Parsing.OptionClause, ICloneable
    {
        public OptionClause()
            :base()
        {
        }

        public OptionClause(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public OptionClause(Jhu.SkyQuery.Sql.Parsing.OptionClause old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.OptionClause(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r999

                bool r999 = true;
                r999 = r999 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"OPTION"));
                if (r999)
                { // may a1000
                    bool a1000 = false;
                    {
                        Checkpoint(parser); // r1001

                        bool r1001 = true;
                        r1001 = r1001 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1001, parser);
                        a1000 = r1001;
                    }

                    r999 |= a1000;
                } // end may a1000

                r999 = r999 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r999)
                { // may a1002
                    bool a1002 = false;
                    {
                        Checkpoint(parser); // r1003

                        bool r1003 = true;
                        r1003 = r1003 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1003, parser);
                        a1002 = r1003;
                    }

                    r999 |= a1002;
                } // end may a1002

                r999 = r999 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.QueryHintList());
                if (r999)
                { // may a1004
                    bool a1004 = false;
                    {
                        Checkpoint(parser); // r1005

                        bool r1005 = true;
                        r1005 = r1005 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1005, parser);
                        a1004 = r1005;
                    }

                    r999 |= a1004;
                } // end may a1004

                r999 = r999 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r999, parser);
                res = r999;
            }



            return res;
        }
    }

    public partial class ValueList : Jhu.Graywulf.Sql.Parsing.ValueList, ICloneable
    {
        public ValueList()
            :base()
        {
        }

        public ValueList(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public ValueList(Jhu.SkyQuery.Sql.Parsing.ValueList old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.ValueList(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r1006

                bool r1006 = true;
                if (r1006)
                { // alternatives a1007 must
                    bool a1007 = false;
                    if (!a1007)
                    {
                        Checkpoint(parser); // r1008

                        bool r1008 = true;
                        r1008 = r1008 && Match(parser, new Jhu.Graywulf.Sql.Parsing.DefaultValue());
                        CommitOrRollback(r1008, parser);
                        a1007 = r1008;
                    }

                    if (!a1007)
                    {
                        Checkpoint(parser); // r1009

                        bool r1009 = true;
                        r1009 = r1009 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        CommitOrRollback(r1009, parser);
                        a1007 = r1009;
                    }

                    r1006 &= a1007;

                } // end alternatives a1007

                if (r1006)
                { // may a1010
                    bool a1010 = false;
                    {
                        Checkpoint(parser); // r1011

                        bool r1011 = true;
                        if (r1011)
                        { // may a1012
                            bool a1012 = false;
                            {
                                Checkpoint(parser); // r1013

                                bool r1013 = true;
                                r1013 = r1013 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r1013, parser);
                                a1012 = r1013;
                            }

                            r1011 |= a1012;
                        } // end may a1012

                        r1011 = r1011 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r1011)
                        { // may a1014
                            bool a1014 = false;
                            {
                                Checkpoint(parser); // r1015

                                bool r1015 = true;
                                r1015 = r1015 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r1015, parser);
                                a1014 = r1015;
                            }

                            r1011 |= a1014;
                        } // end may a1014

                        r1011 = r1011 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ValueList());
                        CommitOrRollback(r1011, parser);
                        a1010 = r1011;
                    }

                    r1006 |= a1010;
                } // end may a1010

                CommitOrRollback(r1006, parser);
                res = r1006;
            }



            return res;
        }
    }

    public partial class ValueGroup : Jhu.Graywulf.Sql.Parsing.ValueGroup, ICloneable
    {
        public ValueGroup()
            :base()
        {
        }

        public ValueGroup(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public ValueGroup(Jhu.SkyQuery.Sql.Parsing.ValueGroup old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.ValueGroup(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r1016

                bool r1016 = true;
                r1016 = r1016 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r1016)
                { // may a1017
                    bool a1017 = false;
                    {
                        Checkpoint(parser); // r1018

                        bool r1018 = true;
                        r1018 = r1018 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1018, parser);
                        a1017 = r1018;
                    }

                    r1016 |= a1017;
                } // end may a1017

                r1016 = r1016 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ValueList());
                if (r1016)
                { // may a1019
                    bool a1019 = false;
                    {
                        Checkpoint(parser); // r1020

                        bool r1020 = true;
                        r1020 = r1020 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1020, parser);
                        a1019 = r1020;
                    }

                    r1016 |= a1019;
                } // end may a1019

                r1016 = r1016 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r1016, parser);
                res = r1016;
            }



            return res;
        }
    }

    public partial class ValueGroupList : Jhu.Graywulf.Sql.Parsing.ValueGroupList, ICloneable
    {
        public ValueGroupList()
            :base()
        {
        }

        public ValueGroupList(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public ValueGroupList(Jhu.SkyQuery.Sql.Parsing.ValueGroupList old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.ValueGroupList(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r1021

                bool r1021 = true;
                r1021 = r1021 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ValueGroup());
                if (r1021)
                { // may a1022
                    bool a1022 = false;
                    {
                        Checkpoint(parser); // r1023

                        bool r1023 = true;
                        if (r1023)
                        { // may a1024
                            bool a1024 = false;
                            {
                                Checkpoint(parser); // r1025

                                bool r1025 = true;
                                r1025 = r1025 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r1025, parser);
                                a1024 = r1025;
                            }

                            r1023 |= a1024;
                        } // end may a1024

                        r1023 = r1023 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r1023)
                        { // may a1026
                            bool a1026 = false;
                            {
                                Checkpoint(parser); // r1027

                                bool r1027 = true;
                                r1027 = r1027 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r1027, parser);
                                a1026 = r1027;
                            }

                            r1023 |= a1026;
                        } // end may a1026

                        r1023 = r1023 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ValueGroupList());
                        CommitOrRollback(r1023, parser);
                        a1022 = r1023;
                    }

                    r1021 |= a1022;
                } // end may a1022

                CommitOrRollback(r1021, parser);
                res = r1021;
            }



            return res;
        }
    }

    public partial class ValuesClause : Jhu.Graywulf.Sql.Parsing.ValuesClause, ICloneable
    {
        public ValuesClause()
            :base()
        {
        }

        public ValuesClause(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public ValuesClause(Jhu.SkyQuery.Sql.Parsing.ValuesClause old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.ValuesClause(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r1028

                bool r1028 = true;
                r1028 = r1028 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"VALUES"));
                if (r1028)
                { // may a1029
                    bool a1029 = false;
                    {
                        Checkpoint(parser); // r1030

                        bool r1030 = true;
                        r1030 = r1030 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1030, parser);
                        a1029 = r1030;
                    }

                    r1028 |= a1029;
                } // end may a1029

                r1028 = r1028 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ValueGroupList());
                CommitOrRollback(r1028, parser);
                res = r1028;
            }



            return res;
        }
    }

    public partial class UpdateSetColumnRightHandSide : Jhu.Graywulf.Sql.Parsing.UpdateSetColumnRightHandSide, ICloneable
    {
        public UpdateSetColumnRightHandSide()
            :base()
        {
        }

        public UpdateSetColumnRightHandSide(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public UpdateSetColumnRightHandSide(Jhu.SkyQuery.Sql.Parsing.UpdateSetColumnRightHandSide old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.UpdateSetColumnRightHandSide(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r1031

                bool r1031 = true;
                if (r1031)
                { // alternatives a1032 must
                    bool a1032 = false;
                    if (!a1032)
                    {
                        Checkpoint(parser); // r1033

                        bool r1033 = true;
                        r1033 = r1033 && Match(parser, new Jhu.Graywulf.Sql.Parsing.DefaultValue());
                        CommitOrRollback(r1033, parser);
                        a1032 = r1033;
                    }

                    if (!a1032)
                    {
                        Checkpoint(parser); // r1034

                        bool r1034 = true;
                        r1034 = r1034 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        CommitOrRollback(r1034, parser);
                        a1032 = r1034;
                    }

                    r1031 &= a1032;

                } // end alternatives a1032

                CommitOrRollback(r1031, parser);
                res = r1031;
            }



            return res;
        }
    }

    public partial class UpdateSetColumn : Jhu.Graywulf.Sql.Parsing.UpdateSetColumn, ICloneable
    {
        public UpdateSetColumn()
            :base()
        {
        }

        public UpdateSetColumn(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public UpdateSetColumn(Jhu.SkyQuery.Sql.Parsing.UpdateSetColumn old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.UpdateSetColumn(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r1035

                bool r1035 = true;
                r1035 = r1035 && Match(parser, new Jhu.Graywulf.Sql.Parsing.UpdateSetColumnLeftHandSide());
                if (r1035)
                { // may a1036
                    bool a1036 = false;
                    {
                        Checkpoint(parser); // r1037

                        bool r1037 = true;
                        r1037 = r1037 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1037, parser);
                        a1036 = r1037;
                    }

                    r1035 |= a1036;
                } // end may a1036

                r1035 = r1035 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ValueAssignmentOperator());
                if (r1035)
                { // may a1038
                    bool a1038 = false;
                    {
                        Checkpoint(parser); // r1039

                        bool r1039 = true;
                        r1039 = r1039 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1039, parser);
                        a1038 = r1039;
                    }

                    r1035 |= a1038;
                } // end may a1038

                r1035 = r1035 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.UpdateSetColumnRightHandSide());
                CommitOrRollback(r1035, parser);
                res = r1035;
            }



            return res;
        }
    }

    public partial class ColumnDefaultSpecification : Jhu.Graywulf.Sql.Parsing.ColumnDefaultSpecification, ICloneable
    {
        public ColumnDefaultSpecification()
            :base()
        {
        }

        public ColumnDefaultSpecification(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public ColumnDefaultSpecification(Jhu.SkyQuery.Sql.Parsing.ColumnDefaultSpecification old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.ColumnDefaultSpecification(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r1040

                bool r1040 = true;
                r1040 = r1040 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DEFAULT"));
                if (r1040)
                { // may a1041
                    bool a1041 = false;
                    {
                        Checkpoint(parser); // r1042

                        bool r1042 = true;
                        r1042 = r1042 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1042, parser);
                        a1041 = r1042;
                    }

                    r1040 |= a1041;
                } // end may a1041

                r1040 = r1040 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                CommitOrRollback(r1040, parser);
                res = r1040;
            }



            return res;
        }
    }

    public partial class ColumnSpecificationList : Jhu.Graywulf.Sql.Parsing.ColumnSpecificationList, ICloneable
    {
        public ColumnSpecificationList()
            :base()
        {
        }

        public ColumnSpecificationList(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public ColumnSpecificationList(Jhu.SkyQuery.Sql.Parsing.ColumnSpecificationList old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.ColumnSpecificationList(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r1043

                bool r1043 = true;
                if (r1043)
                { // alternatives a1044 must
                    bool a1044 = false;
                    if (!a1044)
                    {
                        Checkpoint(parser); // r1045

                        bool r1045 = true;
                        r1045 = r1045 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ColumnNullSpecification());
                        CommitOrRollback(r1045, parser);
                        a1044 = r1045;
                    }

                    if (!a1044)
                    {
                        Checkpoint(parser); // r1046

                        bool r1046 = true;
                        r1046 = r1046 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ColumnDefaultSpecification());
                        CommitOrRollback(r1046, parser);
                        a1044 = r1046;
                    }

                    if (!a1044)
                    {
                        Checkpoint(parser); // r1047

                        bool r1047 = true;
                        r1047 = r1047 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ColumnIdentitySpecification());
                        CommitOrRollback(r1047, parser);
                        a1044 = r1047;
                    }

                    if (!a1044)
                    {
                        Checkpoint(parser); // r1048

                        bool r1048 = true;
                        r1048 = r1048 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ColumnConstraint());
                        CommitOrRollback(r1048, parser);
                        a1044 = r1048;
                    }

                    if (!a1044)
                    {
                        Checkpoint(parser); // r1049

                        bool r1049 = true;
                        r1049 = r1049 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ColumnIndex());
                        CommitOrRollback(r1049, parser);
                        a1044 = r1049;
                    }

                    r1043 &= a1044;

                } // end alternatives a1044

                if (r1043)
                { // may a1050
                    bool a1050 = false;
                    {
                        Checkpoint(parser); // r1051

                        bool r1051 = true;
                        if (r1051)
                        { // may a1052
                            bool a1052 = false;
                            {
                                Checkpoint(parser); // r1053

                                bool r1053 = true;
                                r1053 = r1053 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r1053, parser);
                                a1052 = r1053;
                            }

                            r1051 |= a1052;
                        } // end may a1052

                        r1051 = r1051 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ColumnSpecificationList());
                        CommitOrRollback(r1051, parser);
                        a1050 = r1051;
                    }

                    r1043 |= a1050;
                } // end may a1050

                CommitOrRollback(r1043, parser);
                res = r1043;
            }



            return res;
        }
    }

    public partial class ColumnDefinition : Jhu.Graywulf.Sql.Parsing.ColumnDefinition, ICloneable
    {
        public ColumnDefinition()
            :base()
        {
        }

        public ColumnDefinition(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public ColumnDefinition(Jhu.SkyQuery.Sql.Parsing.ColumnDefinition old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.ColumnDefinition(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r1054

                bool r1054 = true;
                r1054 = r1054 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ColumnName());
                if (r1054)
                { // may a1055
                    bool a1055 = false;
                    {
                        Checkpoint(parser); // r1056

                        bool r1056 = true;
                        r1056 = r1056 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1056, parser);
                        a1055 = r1056;
                    }

                    r1054 |= a1055;
                } // end may a1055

                r1054 = r1054 && Match(parser, new Jhu.Graywulf.Sql.Parsing.DataTypeSpecification());
                if (r1054)
                { // may a1057
                    bool a1057 = false;
                    {
                        Checkpoint(parser); // r1058

                        bool r1058 = true;
                        if (r1058)
                        { // may a1059
                            bool a1059 = false;
                            {
                                Checkpoint(parser); // r1060

                                bool r1060 = true;
                                r1060 = r1060 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r1060, parser);
                                a1059 = r1060;
                            }

                            r1058 |= a1059;
                        } // end may a1059

                        r1058 = r1058 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ColumnSpecificationList());
                        CommitOrRollback(r1058, parser);
                        a1057 = r1058;
                    }

                    r1054 |= a1057;
                } // end may a1057

                CommitOrRollback(r1054, parser);
                res = r1054;
            }



            return res;
        }
    }

    public partial class TableDefinitionList : Jhu.Graywulf.Sql.Parsing.TableDefinitionList, ICloneable
    {
        public TableDefinitionList()
            :base()
        {
        }

        public TableDefinitionList(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public TableDefinitionList(Jhu.SkyQuery.Sql.Parsing.TableDefinitionList old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.TableDefinitionList(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r1061

                bool r1061 = true;
                if (r1061)
                { // alternatives a1062 must
                    bool a1062 = false;
                    if (!a1062)
                    {
                        Checkpoint(parser); // r1063

                        bool r1063 = true;
                        r1063 = r1063 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ColumnDefinition());
                        CommitOrRollback(r1063, parser);
                        a1062 = r1063;
                    }

                    if (!a1062)
                    {
                        Checkpoint(parser); // r1064

                        bool r1064 = true;
                        r1064 = r1064 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableConstraint());
                        CommitOrRollback(r1064, parser);
                        a1062 = r1064;
                    }

                    if (!a1062)
                    {
                        Checkpoint(parser); // r1065

                        bool r1065 = true;
                        r1065 = r1065 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableIndex());
                        CommitOrRollback(r1065, parser);
                        a1062 = r1065;
                    }

                    r1061 &= a1062;

                } // end alternatives a1062

                if (r1061)
                { // may a1066
                    bool a1066 = false;
                    {
                        Checkpoint(parser); // r1067

                        bool r1067 = true;
                        if (r1067)
                        { // may a1068
                            bool a1068 = false;
                            {
                                Checkpoint(parser); // r1069

                                bool r1069 = true;
                                r1069 = r1069 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r1069, parser);
                                a1068 = r1069;
                            }

                            r1067 |= a1068;
                        } // end may a1068

                        r1067 = r1067 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r1067)
                        { // may a1070
                            bool a1070 = false;
                            {
                                Checkpoint(parser); // r1071

                                bool r1071 = true;
                                r1071 = r1071 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r1071, parser);
                                a1070 = r1071;
                            }

                            r1067 |= a1070;
                        } // end may a1070

                        r1067 = r1067 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableDefinitionList());
                        CommitOrRollback(r1067, parser);
                        a1066 = r1067;
                    }

                    r1061 |= a1066;
                } // end may a1066

                CommitOrRollback(r1061, parser);
                res = r1061;
            }



            return res;
        }
    }

    public partial class TableDefinition : Jhu.Graywulf.Sql.Parsing.TableDefinition, ICloneable
    {
        public TableDefinition()
            :base()
        {
        }

        public TableDefinition(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public TableDefinition(Jhu.SkyQuery.Sql.Parsing.TableDefinition old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.TableDefinition(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r1072

                bool r1072 = true;
                r1072 = r1072 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r1072)
                { // may a1073
                    bool a1073 = false;
                    {
                        Checkpoint(parser); // r1074

                        bool r1074 = true;
                        r1074 = r1074 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1074, parser);
                        a1073 = r1074;
                    }

                    r1072 |= a1073;
                } // end may a1073

                r1072 = r1072 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableDefinitionList());
                if (r1072)
                { // may a1075
                    bool a1075 = false;
                    {
                        Checkpoint(parser); // r1076

                        bool r1076 = true;
                        r1076 = r1076 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1076, parser);
                        a1075 = r1076;
                    }

                    r1072 |= a1075;
                } // end may a1075

                r1072 = r1072 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r1072, parser);
                res = r1072;
            }



            return res;
        }
    }

    public partial class TableDeclaration : Jhu.Graywulf.Sql.Parsing.TableDeclaration, ICloneable
    {
        public TableDeclaration()
            :base()
        {
        }

        public TableDeclaration(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public TableDeclaration(Jhu.SkyQuery.Sql.Parsing.TableDeclaration old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.TableDeclaration(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r1077

                bool r1077 = true;
                r1077 = r1077 && Match(parser, new Jhu.Graywulf.Sql.Parsing.UserVariable());
                if (r1077)
                { // may a1078
                    bool a1078 = false;
                    {
                        Checkpoint(parser); // r1079

                        bool r1079 = true;
                        r1079 = r1079 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1079, parser);
                        a1078 = r1079;
                    }

                    r1077 |= a1078;
                } // end may a1078

                if (r1077)
                { // may a1080
                    bool a1080 = false;
                    {
                        Checkpoint(parser); // r1081

                        bool r1081 = true;
                        r1081 = r1081 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"AS"));
                        r1081 = r1081 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1081, parser);
                        a1080 = r1081;
                    }

                    r1077 |= a1080;
                } // end may a1080

                r1077 = r1077 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TABLE"));
                if (r1077)
                { // may a1082
                    bool a1082 = false;
                    {
                        Checkpoint(parser); // r1083

                        bool r1083 = true;
                        r1083 = r1083 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1083, parser);
                        a1082 = r1083;
                    }

                    r1077 |= a1082;
                } // end may a1082

                r1077 = r1077 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableDefinition());
                CommitOrRollback(r1077, parser);
                res = r1077;
            }



            return res;
        }
    }

    public partial class DeclareTableStatement : Jhu.Graywulf.Sql.Parsing.DeclareTableStatement, ICloneable
    {
        public DeclareTableStatement()
            :base()
        {
        }

        public DeclareTableStatement(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public DeclareTableStatement(Jhu.SkyQuery.Sql.Parsing.DeclareTableStatement old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.DeclareTableStatement(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r1084

                bool r1084 = true;
                r1084 = r1084 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DECLARE"));
                if (r1084)
                { // may a1085
                    bool a1085 = false;
                    {
                        Checkpoint(parser); // r1086

                        bool r1086 = true;
                        r1086 = r1086 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1086, parser);
                        a1085 = r1086;
                    }

                    r1084 |= a1085;
                } // end may a1085

                r1084 = r1084 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableDeclaration());
                CommitOrRollback(r1084, parser);
                res = r1084;
            }



            return res;
        }
    }

    public partial class CreateTableStatement : Jhu.Graywulf.Sql.Parsing.CreateTableStatement, ICloneable
    {
        public CreateTableStatement()
            :base()
        {
        }

        public CreateTableStatement(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public CreateTableStatement(Jhu.SkyQuery.Sql.Parsing.CreateTableStatement old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.CreateTableStatement(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r1087

                bool r1087 = true;
                r1087 = r1087 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CREATE"));
                r1087 = r1087 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r1087 = r1087 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TABLE"));
                if (r1087)
                { // may a1088
                    bool a1088 = false;
                    {
                        Checkpoint(parser); // r1089

                        bool r1089 = true;
                        r1089 = r1089 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1089, parser);
                        a1088 = r1089;
                    }

                    r1087 |= a1088;
                } // end may a1088

                r1087 = r1087 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableOrViewIdentifier());
                if (r1087)
                { // may a1090
                    bool a1090 = false;
                    {
                        Checkpoint(parser); // r1091

                        bool r1091 = true;
                        r1091 = r1091 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1091, parser);
                        a1090 = r1091;
                    }

                    r1087 |= a1090;
                } // end may a1090

                r1087 = r1087 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableDefinition());
                CommitOrRollback(r1087, parser);
                res = r1087;
            }



            return res;
        }
    }

    public partial class ColumnConstraint : Jhu.Graywulf.Sql.Parsing.ColumnConstraint, ICloneable
    {
        public ColumnConstraint()
            :base()
        {
        }

        public ColumnConstraint(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public ColumnConstraint(Jhu.SkyQuery.Sql.Parsing.ColumnConstraint old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.ColumnConstraint(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r1092

                bool r1092 = true;
                if (r1092)
                { // may a1093
                    bool a1093 = false;
                    {
                        Checkpoint(parser); // r1094

                        bool r1094 = true;
                        r1094 = r1094 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CONSTRAINT"));
                        if (r1094)
                        { // may a1095
                            bool a1095 = false;
                            {
                                Checkpoint(parser); // r1096

                                bool r1096 = true;
                                r1096 = r1096 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r1096, parser);
                                a1095 = r1096;
                            }

                            r1094 |= a1095;
                        } // end may a1095

                        r1094 = r1094 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ConstraintName());
                        if (r1094)
                        { // may a1097
                            bool a1097 = false;
                            {
                                Checkpoint(parser); // r1098

                                bool r1098 = true;
                                r1098 = r1098 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r1098, parser);
                                a1097 = r1098;
                            }

                            r1094 |= a1097;
                        } // end may a1097

                        CommitOrRollback(r1094, parser);
                        a1093 = r1094;
                    }

                    r1092 |= a1093;
                } // end may a1093

                if (r1092)
                { // alternatives a1099 must
                    bool a1099 = false;
                    if (!a1099)
                    {
                        Checkpoint(parser); // r1100

                        bool r1100 = true;
                        r1100 = r1100 && Match(parser, new Jhu.Graywulf.Sql.Parsing.IndexTypeSpecification());
                        CommitOrRollback(r1100, parser);
                        a1099 = r1100;
                    }

                    if (!a1099)
                    {
                        Checkpoint(parser); // r1101

                        bool r1101 = true;
                        r1101 = r1101 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ColumnDefaultSpecification());
                        CommitOrRollback(r1101, parser);
                        a1099 = r1101;
                    }

                    r1092 &= a1099;

                } // end alternatives a1099

                CommitOrRollback(r1092, parser);
                res = r1092;
            }



            return res;
        }
    }

    public partial class SubqueryTableSource : Jhu.Graywulf.Sql.Parsing.SubqueryTableSource, ICloneable
    {
        public SubqueryTableSource()
            :base()
        {
        }

        public SubqueryTableSource(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public SubqueryTableSource(Jhu.SkyQuery.Sql.Parsing.SubqueryTableSource old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.SubqueryTableSource(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r1102

                bool r1102 = true;
                r1102 = r1102 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Subquery());
                if (r1102)
                { // may a1103
                    bool a1103 = false;
                    {
                        Checkpoint(parser); // r1104

                        bool r1104 = true;
                        r1104 = r1104 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1104, parser);
                        a1103 = r1104;
                    }

                    r1102 |= a1103;
                } // end may a1103

                if (r1102)
                { // may a1105
                    bool a1105 = false;
                    {
                        Checkpoint(parser); // r1106

                        bool r1106 = true;
                        r1106 = r1106 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"AS"));
                        r1106 = r1106 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1106, parser);
                        a1105 = r1106;
                    }

                    r1102 |= a1105;
                } // end may a1105

                r1102 = r1102 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableAlias());
                CommitOrRollback(r1102, parser);
                res = r1102;
            }



            return res;
        }
    }

    public partial class QueryExpressionBrackets : Jhu.Graywulf.Sql.Parsing.QueryExpressionBrackets, ICloneable
    {
        public QueryExpressionBrackets()
            :base()
        {
        }

        public QueryExpressionBrackets(params Jhu.Graywulf.Parsing.Token[] tokens)
            :base(tokens)
        {
        }

        public QueryExpressionBrackets(Jhu.SkyQuery.Sql.Parsing.QueryExpressionBrackets old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new Jhu.SkyQuery.Sql.Parsing.QueryExpressionBrackets(this);
        }

        public override bool Match(Jhu.Graywulf.Parsing.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r1107

                bool r1107 = true;
                r1107 = r1107 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r1107)
                { // may a1108
                    bool a1108 = false;
                    {
                        Checkpoint(parser); // r1109

                        bool r1109 = true;
                        r1109 = r1109 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1109, parser);
                        a1108 = r1109;
                    }

                    r1107 |= a1108;
                } // end may a1108

                r1107 = r1107 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.QueryExpression());
                if (r1107)
                { // may a1110
                    bool a1110 = false;
                    {
                        Checkpoint(parser); // r1111

                        bool r1111 = true;
                        r1111 = r1111 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1111, parser);
                        a1110 = r1111;
                    }

                    r1107 |= a1110;
                } // end may a1110

                r1107 = r1107 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r1107, parser);
                res = r1107;
            }



            return res;
        }
    }


}