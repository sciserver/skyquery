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
                if (r173)
                { // alternatives a174 must
                    bool a174 = false;
                    if (!a174)
                    {
                        Checkpoint(parser); // r175

                        bool r175 = true;
                        r175 = r175 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"XMATCH"));
                        if (r175)
                        { // may a176
                            bool a176 = false;
                            {
                                Checkpoint(parser); // r177

                                bool r177 = true;
                                r177 = r177 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r177, parser);
                                a176 = r177;
                            }

                            r175 |= a176;
                        } // end may a176

                        r175 = r175 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                        if (r175)
                        { // may a178
                            bool a178 = false;
                            {
                                Checkpoint(parser); // r179

                                bool r179 = true;
                                r179 = r179 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r179, parser);
                                a178 = r179;
                            }

                            r175 |= a178;
                        } // end may a178

                        r175 = r175 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableList());
                        if (r175)
                        { // may a180
                            bool a180 = false;
                            {
                                Checkpoint(parser); // r181

                                bool r181 = true;
                                r181 = r181 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r181, parser);
                                a180 = r181;
                            }

                            r175 |= a180;
                        } // end may a180

                        r175 = r175 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r175)
                        { // may a182
                            bool a182 = false;
                            {
                                Checkpoint(parser); // r183

                                bool r183 = true;
                                r183 = r183 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r183, parser);
                                a182 = r183;
                            }

                            r175 |= a182;
                        } // end may a182

                        r175 = r175 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchConstraint());
                        if (r175)
                        { // may a184
                            bool a184 = false;
                            {
                                Checkpoint(parser); // r185

                                bool r185 = true;
                                r185 = r185 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r185, parser);
                                a184 = r185;
                            }

                            r175 |= a184;
                        } // end may a184

                        r175 = r175 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                        if (r175)
                        { // may a186
                            bool a186 = false;
                            {
                                Checkpoint(parser); // r187

                                bool r187 = true;
                                r187 = r187 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r187, parser);
                                a186 = r187;
                            }

                            r175 |= a186;
                        } // end may a186

                        if (r175)
                        { // may a188
                            bool a188 = false;
                            {
                                Checkpoint(parser); // r189

                                bool r189 = true;
                                r189 = r189 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"AS"));
                                if (r189)
                                { // may a190
                                    bool a190 = false;
                                    {
                                        Checkpoint(parser); // r191

                                        bool r191 = true;
                                        r191 = r191 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                        CommitOrRollback(r191, parser);
                                        a190 = r191;
                                    }

                                    r189 |= a190;
                                } // end may a190

                                CommitOrRollback(r189, parser);
                                a188 = r189;
                            }

                            r175 |= a188;
                        } // end may a188

                        r175 = r175 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableAlias());
                        CommitOrRollback(r175, parser);
                        a174 = r175;
                    }

                    if (!a174)
                    {
                        Checkpoint(parser); // r192

                        bool r192 = true;
                        r192 = r192 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableAlias());
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

                        r192 = r192 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"AS"));
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

                        r192 = r192 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"XMATCH"));
                        if (r192)
                        { // may a197
                            bool a197 = false;
                            {
                                Checkpoint(parser); // r198

                                bool r198 = true;
                                r198 = r198 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r198, parser);
                                a197 = r198;
                            }

                            r192 |= a197;
                        } // end may a197

                        if (r192)
                        { // may a199
                            bool a199 = false;
                            {
                                Checkpoint(parser); // r200

                                bool r200 = true;
                                r200 = r200 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r200, parser);
                                a199 = r200;
                            }

                            r192 |= a199;
                        } // end may a199

                        r192 = r192 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                        if (r192)
                        { // may a201
                            bool a201 = false;
                            {
                                Checkpoint(parser); // r202

                                bool r202 = true;
                                r202 = r202 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r202, parser);
                                a201 = r202;
                            }

                            r192 |= a201;
                        } // end may a201

                        r192 = r192 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableList());
                        if (r192)
                        { // may a203
                            bool a203 = false;
                            {
                                Checkpoint(parser); // r204

                                bool r204 = true;
                                r204 = r204 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r204, parser);
                                a203 = r204;
                            }

                            r192 |= a203;
                        } // end may a203

                        r192 = r192 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r192)
                        { // may a205
                            bool a205 = false;
                            {
                                Checkpoint(parser); // r206

                                bool r206 = true;
                                r206 = r206 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r206, parser);
                                a205 = r206;
                            }

                            r192 |= a205;
                        } // end may a205

                        r192 = r192 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchConstraint());
                        if (r192)
                        { // may a207
                            bool a207 = false;
                            {
                                Checkpoint(parser); // r208

                                bool r208 = true;
                                r208 = r208 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r208, parser);
                                a207 = r208;
                            }

                            r192 |= a207;
                        } // end may a207

                        r192 = r192 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                        CommitOrRollback(r192, parser);
                        a174 = r192;
                    }

                    r173 &= a174;

                } // end alternatives a174

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
                Checkpoint(parser); // r209

                bool r209 = true;
                r209 = r209 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableSpecification());
                if (r209)
                { // may a210
                    bool a210 = false;
                    {
                        Checkpoint(parser); // r211

                        bool r211 = true;
                        if (r211)
                        { // may a212
                            bool a212 = false;
                            {
                                Checkpoint(parser); // r213

                                bool r213 = true;
                                r213 = r213 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r213, parser);
                                a212 = r213;
                            }

                            r211 |= a212;
                        } // end may a212

                        r211 = r211 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r211)
                        { // may a214
                            bool a214 = false;
                            {
                                Checkpoint(parser); // r215

                                bool r215 = true;
                                r215 = r215 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r215, parser);
                                a214 = r215;
                            }

                            r211 |= a214;
                        } // end may a214

                        r211 = r211 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableList());
                        CommitOrRollback(r211, parser);
                        a210 = r211;
                    }

                    r209 |= a210;
                } // end may a210

                CommitOrRollback(r209, parser);
                res = r209;
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
                Checkpoint(parser); // r216

                bool r216 = true;
                r216 = r216 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchTableInclusion());
                if (r216)
                { // may a217
                    bool a217 = false;
                    {
                        Checkpoint(parser); // r218

                        bool r218 = true;
                        r218 = r218 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r218, parser);
                        a217 = r218;
                    }

                    r216 |= a217;
                } // end may a217

                r216 = r216 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SimpleTableSource());
                CommitOrRollback(r216, parser);
                res = r216;
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
                Checkpoint(parser); // r219

                bool r219 = true;
                if (r219)
                { // may a220
                    bool a220 = false;
                    {
                        Checkpoint(parser); // r221

                        bool r221 = true;
                        if (r221)
                        { // alternatives a222 must
                            bool a222 = false;
                            if (!a222)
                            {
                                Checkpoint(parser); // r223

                                bool r223 = true;
                                r223 = r223 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"MUST"));
                                CommitOrRollback(r223, parser);
                                a222 = r223;
                            }

                            if (!a222)
                            {
                                Checkpoint(parser); // r224

                                bool r224 = true;
                                r224 = r224 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"MAY"));
                                CommitOrRollback(r224, parser);
                                a222 = r224;
                            }

                            if (!a222)
                            {
                                Checkpoint(parser); // r225

                                bool r225 = true;
                                r225 = r225 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"NOT"));
                                CommitOrRollback(r225, parser);
                                a222 = r225;
                            }

                            r221 &= a222;

                        } // end alternatives a222

                        r221 = r221 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r221, parser);
                        a220 = r221;
                    }

                    r219 |= a220;
                } // end may a220

                r219 = r219 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"EXIST"));
                r219 = r219 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r219 = r219 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"IN"));
                CommitOrRollback(r219, parser);
                res = r219;
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
                Checkpoint(parser); // r226

                bool r226 = true;
                r226 = r226 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"LIMIT"));
                r226 = r226 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r226 = r226 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.XMatchAlgorithm());
                r226 = r226 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r226 = r226 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TO"));
                r226 = r226 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                if (r226)
                { // alternatives a227 must
                    bool a227 = false;
                    if (!a227)
                    {
                        Checkpoint(parser); // r228

                        bool r228 = true;
                        r228 = r228 && Match(parser, new Jhu.Graywulf.Sql.Parsing.NumericConstant());
                        CommitOrRollback(r228, parser);
                        a227 = r228;
                    }

                    if (!a227)
                    {
                        Checkpoint(parser); // r229

                        bool r229 = true;
                        r229 = r229 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableHint());
                        CommitOrRollback(r229, parser);
                        a227 = r229;
                    }

                    r226 &= a227;

                } // end alternatives a227

                CommitOrRollback(r226, parser);
                res = r226;
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
                Checkpoint(parser); // r230

                bool r230 = true;
                if (r230)
                { // alternatives a231 must
                    bool a231 = false;
                    if (!a231)
                    {
                        Checkpoint(parser); // r232

                        bool r232 = true;
                        r232 = r232 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"BAYESFACTOR"));
                        CommitOrRollback(r232, parser);
                        a231 = r232;
                    }

                    if (!a231)
                    {
                        Checkpoint(parser); // r233

                        bool r233 = true;
                        r233 = r233 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CONE"));
                        CommitOrRollback(r233, parser);
                        a231 = r233;
                    }

                    r230 &= a231;

                } // end alternatives a231

                CommitOrRollback(r230, parser);
                res = r230;
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
                Checkpoint(parser); // r234

                bool r234 = true;
                if (r234)
                { // may a235
                    bool a235 = false;
                    {
                        Checkpoint(parser); // r236

                        bool r236 = true;
                        r236 = r236 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r236, parser);
                        a235 = r236;
                    }

                    r234 |= a235;
                } // end may a235

                if (r234)
                { // may a237
                    bool a237 = false;
                    {
                        Checkpoint(parser); // r238

                        bool r238 = true;
                        r238 = r238 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.AnyStatement());
                        CommitOrRollback(r238, parser);
                        a237 = r238;
                    }

                    r234 |= a237;
                } // end may a237

                if (r234)
                { // may a239
                    bool a239 = false;
                    {
                        Checkpoint(parser); // r240

                        bool r240 = true;
                        r240 = r240 && Match(parser, new Jhu.Graywulf.Sql.Parsing.StatementSeparator());
                        if (r240)
                        { // may a241
                            bool a241 = false;
                            {
                                Checkpoint(parser); // r242

                                bool r242 = true;
                                r242 = r242 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.StatementBlock());
                                CommitOrRollback(r242, parser);
                                a241 = r242;
                            }

                            r240 |= a241;
                        } // end may a241

                        CommitOrRollback(r240, parser);
                        a239 = r240;
                    }

                    r234 |= a239;
                } // end may a239

                if (r234)
                { // may a243
                    bool a243 = false;
                    {
                        Checkpoint(parser); // r244

                        bool r244 = true;
                        r244 = r244 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r244, parser);
                        a243 = r244;
                    }

                    r234 |= a243;
                } // end may a243

                CommitOrRollback(r234, parser);
                res = r234;
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
                Checkpoint(parser); // r245

                bool r245 = true;
                r245 = r245 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"BEGIN"));
                r245 = r245 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.StatementBlock());
                r245 = r245 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"END"));
                CommitOrRollback(r245, parser);
                res = r245;
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
                Checkpoint(parser); // r246

                bool r246 = true;
                r246 = r246 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"BEGIN"));
                r246 = r246 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r246 = r246 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TRY"));
                r246 = r246 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.StatementBlock());
                r246 = r246 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"END"));
                r246 = r246 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r246 = r246 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TRY"));
                r246 = r246 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r246 = r246 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"BEGIN"));
                r246 = r246 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r246 = r246 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CATCH"));
                if (r246)
                { // alternatives a247 must
                    bool a247 = false;
                    if (!a247)
                    {
                        Checkpoint(parser); // r248

                        bool r248 = true;
                        r248 = r248 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.StatementBlock());
                        CommitOrRollback(r248, parser);
                        a247 = r248;
                    }

                    if (!a247)
                    {
                        Checkpoint(parser); // r249

                        bool r249 = true;
                        r249 = r249 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r249, parser);
                        a247 = r249;
                    }

                    r246 &= a247;

                } // end alternatives a247

                r246 = r246 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"END"));
                r246 = r246 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r246 = r246 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CATCH"));
                CommitOrRollback(r246, parser);
                res = r246;
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
                Checkpoint(parser); // r250

                bool r250 = true;
                r250 = r250 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"WHILE"));
                if (r250)
                { // may a251
                    bool a251 = false;
                    {
                        Checkpoint(parser); // r252

                        bool r252 = true;
                        r252 = r252 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r252, parser);
                        a251 = r252;
                    }

                    r250 |= a251;
                } // end may a251

                r250 = r250 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.LogicalExpression());
                if (r250)
                { // may a253
                    bool a253 = false;
                    {
                        Checkpoint(parser); // r254

                        bool r254 = true;
                        r254 = r254 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r254, parser);
                        a253 = r254;
                    }

                    r250 |= a253;
                } // end may a253

                r250 = r250 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.AnyStatement());
                CommitOrRollback(r250, parser);
                res = r250;
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
                Checkpoint(parser); // r255

                bool r255 = true;
                r255 = r255 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"IF"));
                if (r255)
                { // may a256
                    bool a256 = false;
                    {
                        Checkpoint(parser); // r257

                        bool r257 = true;
                        r257 = r257 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r257, parser);
                        a256 = r257;
                    }

                    r255 |= a256;
                } // end may a256

                r255 = r255 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.LogicalExpression());
                if (r255)
                { // may a258
                    bool a258 = false;
                    {
                        Checkpoint(parser); // r259

                        bool r259 = true;
                        r259 = r259 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r259, parser);
                        a258 = r259;
                    }

                    r255 |= a258;
                } // end may a258

                r255 = r255 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.AnyStatement());
                if (r255)
                { // may a260
                    bool a260 = false;
                    {
                        Checkpoint(parser); // r261

                        bool r261 = true;
                        r261 = r261 && Match(parser, new Jhu.Graywulf.Sql.Parsing.StatementSeparator());
                        r261 = r261 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ELSE"));
                        r261 = r261 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        r261 = r261 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.AnyStatement());
                        CommitOrRollback(r261, parser);
                        a260 = r261;
                    }

                    r255 |= a260;
                } // end may a260

                CommitOrRollback(r255, parser);
                res = r255;
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
                Checkpoint(parser); // r262

                bool r262 = true;
                if (r262)
                { // may a263
                    bool a263 = false;
                    {
                        Checkpoint(parser); // r264

                        bool r264 = true;
                        r264 = r264 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommonTableExpression());
                        if (r264)
                        { // may a265
                            bool a265 = false;
                            {
                                Checkpoint(parser); // r266

                                bool r266 = true;
                                r266 = r266 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r266, parser);
                                a265 = r266;
                            }

                            r264 |= a265;
                        } // end may a265

                        CommitOrRollback(r264, parser);
                        a263 = r264;
                    }

                    r262 |= a263;
                } // end may a263

                r262 = r262 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.QueryExpression());
                if (r262)
                { // may a267
                    bool a267 = false;
                    {
                        Checkpoint(parser); // r268

                        bool r268 = true;
                        if (r268)
                        { // may a269
                            bool a269 = false;
                            {
                                Checkpoint(parser); // r270

                                bool r270 = true;
                                r270 = r270 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r270, parser);
                                a269 = r270;
                            }

                            r268 |= a269;
                        } // end may a269

                        r268 = r268 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OrderByClause());
                        CommitOrRollback(r268, parser);
                        a267 = r268;
                    }

                    r262 |= a267;
                } // end may a267

                if (r262)
                { // may a271
                    bool a271 = false;
                    {
                        Checkpoint(parser); // r272

                        bool r272 = true;
                        if (r272)
                        { // may a273
                            bool a273 = false;
                            {
                                Checkpoint(parser); // r274

                                bool r274 = true;
                                r274 = r274 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r274, parser);
                                a273 = r274;
                            }

                            r272 |= a273;
                        } // end may a273

                        r272 = r272 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OptionClause());
                        CommitOrRollback(r272, parser);
                        a271 = r272;
                    }

                    r262 |= a271;
                } // end may a271

                CommitOrRollback(r262, parser);
                res = r262;
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
                Checkpoint(parser); // r275

                bool r275 = true;
                r275 = r275 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CURSOR"));
                r275 = r275 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r275 = r275 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"FOR"));
                r275 = r275 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r275 = r275 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SelectStatement());
                CommitOrRollback(r275, parser);
                res = r275;
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
                Checkpoint(parser); // r276

                bool r276 = true;
                r276 = r276 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DECLARE"));
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

                if (r276)
                { // alternatives a279 must
                    bool a279 = false;
                    if (!a279)
                    {
                        Checkpoint(parser); // r280

                        bool r280 = true;
                        r280 = r280 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CursorName());
                        if (r280)
                        { // may a281
                            bool a281 = false;
                            {
                                Checkpoint(parser); // r282

                                bool r282 = true;
                                if (r282)
                                { // may a283
                                    bool a283 = false;
                                    {
                                        Checkpoint(parser); // r284

                                        bool r284 = true;
                                        r284 = r284 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                        CommitOrRollback(r284, parser);
                                        a283 = r284;
                                    }

                                    r282 |= a283;
                                } // end may a283

                                r282 = r282 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.CursorDefinition());
                                CommitOrRollback(r282, parser);
                                a281 = r282;
                            }

                            r280 |= a281;
                        } // end may a281

                        CommitOrRollback(r280, parser);
                        a279 = r280;
                    }

                    if (!a279)
                    {
                        Checkpoint(parser); // r285

                        bool r285 = true;
                        r285 = r285 && Match(parser, new Jhu.Graywulf.Sql.Parsing.UserVariable());
                        if (r285)
                        { // may a286
                            bool a286 = false;
                            {
                                Checkpoint(parser); // r287

                                bool r287 = true;
                                r287 = r287 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r287, parser);
                                a286 = r287;
                            }

                            r285 |= a286;
                        } // end may a286

                        r285 = r285 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CURSOR"));
                        CommitOrRollback(r285, parser);
                        a279 = r285;
                    }

                    r276 &= a279;

                } // end alternatives a279

                CommitOrRollback(r276, parser);
                res = r276;
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
                Checkpoint(parser); // r288

                bool r288 = true;
                r288 = r288 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"SET"));
                if (r288)
                { // may a289
                    bool a289 = false;
                    {
                        Checkpoint(parser); // r290

                        bool r290 = true;
                        r290 = r290 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r290, parser);
                        a289 = r290;
                    }

                    r288 |= a289;
                } // end may a289

                r288 = r288 && Match(parser, new Jhu.Graywulf.Sql.Parsing.UserVariable());
                if (r288)
                { // may a291
                    bool a291 = false;
                    {
                        Checkpoint(parser); // r292

                        bool r292 = true;
                        r292 = r292 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r292, parser);
                        a291 = r292;
                    }

                    r288 |= a291;
                } // end may a291

                r288 = r288 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Equals1());
                if (r288)
                { // may a293
                    bool a293 = false;
                    {
                        Checkpoint(parser); // r294

                        bool r294 = true;
                        r294 = r294 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r294, parser);
                        a293 = r294;
                    }

                    r288 |= a293;
                } // end may a293

                r288 = r288 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.CursorDefinition());
                CommitOrRollback(r288, parser);
                res = r288;
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
                Checkpoint(parser); // r295

                bool r295 = true;
                r295 = r295 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r295)
                { // may a296
                    bool a296 = false;
                    {
                        Checkpoint(parser); // r297

                        bool r297 = true;
                        r297 = r297 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r297, parser);
                        a296 = r297;
                    }

                    r295 |= a296;
                } // end may a296

                r295 = r295 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.QueryExpression());
                if (r295)
                { // may a298
                    bool a298 = false;
                    {
                        Checkpoint(parser); // r299

                        bool r299 = true;
                        if (r299)
                        { // may a300
                            bool a300 = false;
                            {
                                Checkpoint(parser); // r301

                                bool r301 = true;
                                r301 = r301 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r301, parser);
                                a300 = r301;
                            }

                            r299 |= a300;
                        } // end may a300

                        r299 = r299 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OrderByClause());
                        CommitOrRollback(r299, parser);
                        a298 = r299;
                    }

                    r295 |= a298;
                } // end may a298

                if (r295)
                { // may a302
                    bool a302 = false;
                    {
                        Checkpoint(parser); // r303

                        bool r303 = true;
                        r303 = r303 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r303, parser);
                        a302 = r303;
                    }

                    r295 |= a302;
                } // end may a302

                r295 = r295 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r295, parser);
                res = r295;
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
                Checkpoint(parser); // r304

                bool r304 = true;
                r304 = r304 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Subquery());
                CommitOrRollback(r304, parser);
                res = r304;
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
                Checkpoint(parser); // r305

                bool r305 = true;
                if (r305)
                { // alternatives a306 must
                    bool a306 = false;
                    if (!a306)
                    {
                        Checkpoint(parser); // r307

                        bool r307 = true;
                        r307 = r307 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Constant());
                        CommitOrRollback(r307, parser);
                        a306 = r307;
                    }

                    if (!a306)
                    {
                        Checkpoint(parser); // r308

                        bool r308 = true;
                        r308 = r308 && Match(parser, new Jhu.Graywulf.Sql.Parsing.SystemVariable());
                        CommitOrRollback(r308, parser);
                        a306 = r308;
                    }

                    if (!a306)
                    {
                        Checkpoint(parser); // r309

                        bool r309 = true;
                        r309 = r309 && Match(parser, new Jhu.Graywulf.Sql.Parsing.UserVariable());
                        CommitOrRollback(r309, parser);
                        a306 = r309;
                    }

                    if (!a306)
                    {
                        Checkpoint(parser); // r310

                        bool r310 = true;
                        r310 = r310 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ExpressionSubquery());
                        CommitOrRollback(r310, parser);
                        a306 = r310;
                    }

                    if (!a306)
                    {
                        Checkpoint(parser); // r311

                        bool r311 = true;
                        r311 = r311 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ExpressionBrackets());
                        CommitOrRollback(r311, parser);
                        a306 = r311;
                    }

                    if (!a306)
                    {
                        Checkpoint(parser); // r312

                        bool r312 = true;
                        r312 = r312 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SimpleCaseExpression());
                        CommitOrRollback(r312, parser);
                        a306 = r312;
                    }

                    if (!a306)
                    {
                        Checkpoint(parser); // r313

                        bool r313 = true;
                        r313 = r313 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SearchedCaseExpression());
                        CommitOrRollback(r313, parser);
                        a306 = r313;
                    }

                    if (!a306)
                    {
                        Checkpoint(parser); // r314

                        bool r314 = true;
                        r314 = r314 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.UdtStaticMemberAccessList());
                        CommitOrRollback(r314, parser);
                        a306 = r314;
                    }

                    if (!a306)
                    {
                        Checkpoint(parser); // r315

                        bool r315 = true;
                        r315 = r315 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.CastAndParseFunctionCall());
                        CommitOrRollback(r315, parser);
                        a306 = r315;
                    }

                    if (!a306)
                    {
                        Checkpoint(parser); // r316

                        bool r316 = true;
                        r316 = r316 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ConvertFunctionCall());
                        CommitOrRollback(r316, parser);
                        a306 = r316;
                    }

                    if (!a306)
                    {
                        Checkpoint(parser); // r317

                        bool r317 = true;
                        r317 = r317 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.DateFunctionCall());
                        CommitOrRollback(r317, parser);
                        a306 = r317;
                    }

                    if (!a306)
                    {
                        Checkpoint(parser); // r318

                        bool r318 = true;
                        r318 = r318 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.IifFunctionCall());
                        CommitOrRollback(r318, parser);
                        a306 = r318;
                    }

                    if (!a306)
                    {
                        Checkpoint(parser); // r319

                        bool r319 = true;
                        r319 = r319 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.StarFunctionCall());
                        CommitOrRollback(r319, parser);
                        a306 = r319;
                    }

                    if (!a306)
                    {
                        Checkpoint(parser); // r320

                        bool r320 = true;
                        r320 = r320 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.AggregateFunctionCall());
                        CommitOrRollback(r320, parser);
                        a306 = r320;
                    }

                    if (!a306)
                    {
                        Checkpoint(parser); // r321

                        bool r321 = true;
                        r321 = r321 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.WindowedFunctionCall());
                        CommitOrRollback(r321, parser);
                        a306 = r321;
                    }

                    if (!a306)
                    {
                        Checkpoint(parser); // r322

                        bool r322 = true;
                        r322 = r322 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SystemFunctionCall());
                        CommitOrRollback(r322, parser);
                        a306 = r322;
                    }

                    if (!a306)
                    {
                        Checkpoint(parser); // r323

                        bool r323 = true;
                        r323 = r323 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ObjectName());
                        CommitOrRollback(r323, parser);
                        a306 = r323;
                    }

                    r305 &= a306;

                } // end alternatives a306

                if (r305)
                { // may a324
                    bool a324 = false;
                    {
                        Checkpoint(parser); // r325

                        bool r325 = true;
                        if (r325)
                        { // may a326
                            bool a326 = false;
                            {
                                Checkpoint(parser); // r327

                                bool r327 = true;
                                r327 = r327 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r327, parser);
                                a326 = r327;
                            }

                            r325 |= a326;
                        } // end may a326

                        r325 = r325 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.MemberAccessList());
                        CommitOrRollback(r325, parser);
                        a324 = r325;
                    }

                    r305 |= a324;
                } // end may a324

                CommitOrRollback(r305, parser);
                res = r305;
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
                Checkpoint(parser); // r328

                bool r328 = true;
                if (r328)
                { // alternatives a329 must
                    bool a329 = false;
                    if (!a329)
                    {
                        Checkpoint(parser); // r330

                        bool r330 = true;
                        r330 = r330 && Match(parser, new Jhu.Graywulf.Sql.Parsing.UnaryOperator());
                        if (r330)
                        { // may a331
                            bool a331 = false;
                            {
                                Checkpoint(parser); // r332

                                bool r332 = true;
                                r332 = r332 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r332, parser);
                                a331 = r332;
                            }

                            r330 |= a331;
                        } // end may a331

                        r330 = r330 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        CommitOrRollback(r330, parser);
                        a329 = r330;
                    }

                    if (!a329)
                    {
                        Checkpoint(parser); // r333

                        bool r333 = true;
                        r333 = r333 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Operand());
                        CommitOrRollback(r333, parser);
                        a329 = r333;
                    }

                    r328 &= a329;

                } // end alternatives a329

                if (r328)
                { // may a334
                    bool a334 = false;
                    {
                        Checkpoint(parser); // r335

                        bool r335 = true;
                        if (r335)
                        { // may a336
                            bool a336 = false;
                            {
                                Checkpoint(parser); // r337

                                bool r337 = true;
                                r337 = r337 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r337, parser);
                                a336 = r337;
                            }

                            r335 |= a336;
                        } // end may a336

                        r335 = r335 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BinaryOperator());
                        if (r335)
                        { // may a338
                            bool a338 = false;
                            {
                                Checkpoint(parser); // r339

                                bool r339 = true;
                                r339 = r339 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r339, parser);
                                a338 = r339;
                            }

                            r335 |= a338;
                        } // end may a338

                        r335 = r335 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        CommitOrRollback(r335, parser);
                        a334 = r335;
                    }

                    r328 |= a334;
                } // end may a334

                CommitOrRollback(r328, parser);
                res = r328;
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
                Checkpoint(parser); // r340

                bool r340 = true;
                r340 = r340 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r340)
                { // may a341
                    bool a341 = false;
                    {
                        Checkpoint(parser); // r342

                        bool r342 = true;
                        r342 = r342 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r342, parser);
                        a341 = r342;
                    }

                    r340 |= a341;
                } // end may a341

                r340 = r340 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r340)
                { // may a343
                    bool a343 = false;
                    {
                        Checkpoint(parser); // r344

                        bool r344 = true;
                        r344 = r344 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r344, parser);
                        a343 = r344;
                    }

                    r340 |= a343;
                } // end may a343

                r340 = r340 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r340, parser);
                res = r340;
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
                Checkpoint(parser); // r345

                bool r345 = true;
                r345 = r345 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r345)
                { // may a346
                    bool a346 = false;
                    {
                        Checkpoint(parser); // r347

                        bool r347 = true;
                        r347 = r347 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r347, parser);
                        a346 = r347;
                    }

                    r345 |= a346;
                } // end may a346

                r345 = r345 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ComparisonOperator());
                if (r345)
                { // may a348
                    bool a348 = false;
                    {
                        Checkpoint(parser); // r349

                        bool r349 = true;
                        r349 = r349 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r349, parser);
                        a348 = r349;
                    }

                    r345 |= a348;
                } // end may a348

                r345 = r345 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                CommitOrRollback(r345, parser);
                res = r345;
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
                Checkpoint(parser); // r350

                bool r350 = true;
                if (r350)
                { // alternatives a351 must
                    bool a351 = false;
                    if (!a351)
                    {
                        Checkpoint(parser); // r352

                        bool r352 = true;
                        r352 = r352 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ComparisonPredicate());
                        CommitOrRollback(r352, parser);
                        a351 = r352;
                    }

                    if (!a351)
                    {
                        Checkpoint(parser); // r353

                        bool r353 = true;
                        r353 = r353 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.LikePredicate());
                        CommitOrRollback(r353, parser);
                        a351 = r353;
                    }

                    if (!a351)
                    {
                        Checkpoint(parser); // r354

                        bool r354 = true;
                        r354 = r354 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.BetweenPredicate());
                        CommitOrRollback(r354, parser);
                        a351 = r354;
                    }

                    if (!a351)
                    {
                        Checkpoint(parser); // r355

                        bool r355 = true;
                        r355 = r355 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.IsNullPredicate());
                        CommitOrRollback(r355, parser);
                        a351 = r355;
                    }

                    if (!a351)
                    {
                        Checkpoint(parser); // r356

                        bool r356 = true;
                        r356 = r356 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.InExpressionListPredicate());
                        CommitOrRollback(r356, parser);
                        a351 = r356;
                    }

                    if (!a351)
                    {
                        Checkpoint(parser); // r357

                        bool r357 = true;
                        r357 = r357 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.InSemiJoinPredicate());
                        CommitOrRollback(r357, parser);
                        a351 = r357;
                    }

                    if (!a351)
                    {
                        Checkpoint(parser); // r358

                        bool r358 = true;
                        r358 = r358 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ComparisonSemiJoinPredicate());
                        CommitOrRollback(r358, parser);
                        a351 = r358;
                    }

                    if (!a351)
                    {
                        Checkpoint(parser); // r359

                        bool r359 = true;
                        r359 = r359 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ExistsSemiJoinPredicate());
                        CommitOrRollback(r359, parser);
                        a351 = r359;
                    }

                    r350 &= a351;

                } // end alternatives a351

                CommitOrRollback(r350, parser);
                res = r350;
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
                Checkpoint(parser); // r360

                bool r360 = true;
                if (r360)
                { // may a361
                    bool a361 = false;
                    {
                        Checkpoint(parser); // r362

                        bool r362 = true;
                        r362 = r362 && Match(parser, new Jhu.Graywulf.Sql.Parsing.LogicalNotOperator());
                        if (r362)
                        { // may a363
                            bool a363 = false;
                            {
                                Checkpoint(parser); // r364

                                bool r364 = true;
                                r364 = r364 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r364, parser);
                                a363 = r364;
                            }

                            r362 |= a363;
                        } // end may a363

                        CommitOrRollback(r362, parser);
                        a361 = r362;
                    }

                    r360 |= a361;
                } // end may a361

                if (r360)
                { // alternatives a365 must
                    bool a365 = false;
                    if (!a365)
                    {
                        Checkpoint(parser); // r366

                        bool r366 = true;
                        r366 = r366 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Predicate());
                        CommitOrRollback(r366, parser);
                        a365 = r366;
                    }

                    if (!a365)
                    {
                        Checkpoint(parser); // r367

                        bool r367 = true;
                        r367 = r367 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.LogicalExpressionBrackets());
                        CommitOrRollback(r367, parser);
                        a365 = r367;
                    }

                    r360 &= a365;

                } // end alternatives a365

                if (r360)
                { // may a368
                    bool a368 = false;
                    {
                        Checkpoint(parser); // r369

                        bool r369 = true;
                        if (r369)
                        { // may a370
                            bool a370 = false;
                            {
                                Checkpoint(parser); // r371

                                bool r371 = true;
                                r371 = r371 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r371, parser);
                                a370 = r371;
                            }

                            r369 |= a370;
                        } // end may a370

                        r369 = r369 && Match(parser, new Jhu.Graywulf.Sql.Parsing.LogicalOperator());
                        if (r369)
                        { // may a372
                            bool a372 = false;
                            {
                                Checkpoint(parser); // r373

                                bool r373 = true;
                                r373 = r373 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r373, parser);
                                a372 = r373;
                            }

                            r369 |= a372;
                        } // end may a372

                        r369 = r369 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.LogicalExpression());
                        CommitOrRollback(r369, parser);
                        a368 = r369;
                    }

                    r360 |= a368;
                } // end may a368

                CommitOrRollback(r360, parser);
                res = r360;
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
                Checkpoint(parser); // r374

                bool r374 = true;
                r374 = r374 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.LogicalExpression());
                CommitOrRollback(r374, parser);
                res = r374;
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
                Checkpoint(parser); // r375

                bool r375 = true;
                r375 = r375 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"IIF"));
                if (r375)
                { // may a376
                    bool a376 = false;
                    {
                        Checkpoint(parser); // r377

                        bool r377 = true;
                        r377 = r377 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r377, parser);
                        a376 = r377;
                    }

                    r375 |= a376;
                } // end may a376

                r375 = r375 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r375)
                { // may a378
                    bool a378 = false;
                    {
                        Checkpoint(parser); // r379

                        bool r379 = true;
                        r379 = r379 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r379, parser);
                        a378 = r379;
                    }

                    r375 |= a378;
                } // end may a378

                r375 = r375 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.LogicalArgument());
                if (r375)
                { // may a380
                    bool a380 = false;
                    {
                        Checkpoint(parser); // r381

                        bool r381 = true;
                        r381 = r381 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r381, parser);
                        a380 = r381;
                    }

                    r375 |= a380;
                } // end may a380

                r375 = r375 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                if (r375)
                { // may a382
                    bool a382 = false;
                    {
                        Checkpoint(parser); // r383

                        bool r383 = true;
                        r383 = r383 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r383, parser);
                        a382 = r383;
                    }

                    r375 |= a382;
                } // end may a382

                r375 = r375 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ArgumentList());
                if (r375)
                { // may a384
                    bool a384 = false;
                    {
                        Checkpoint(parser); // r385

                        bool r385 = true;
                        r385 = r385 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r385, parser);
                        a384 = r385;
                    }

                    r375 |= a384;
                } // end may a384

                r375 = r375 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r375, parser);
                res = r375;
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
                Checkpoint(parser); // r386

                bool r386 = true;
                r386 = r386 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
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

                r386 = r386 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.LogicalExpression());
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

                r386 = r386 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r386, parser);
                res = r386;
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
                Checkpoint(parser); // r391

                bool r391 = true;
                r391 = r391 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"WHEN"));
                if (r391)
                { // may a392
                    bool a392 = false;
                    {
                        Checkpoint(parser); // r393

                        bool r393 = true;
                        r393 = r393 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r393, parser);
                        a392 = r393;
                    }

                    r391 |= a392;
                } // end may a392

                r391 = r391 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.LogicalExpression());
                if (r391)
                { // may a394
                    bool a394 = false;
                    {
                        Checkpoint(parser); // r395

                        bool r395 = true;
                        r395 = r395 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r395, parser);
                        a394 = r395;
                    }

                    r391 |= a394;
                } // end may a394

                r391 = r391 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"THEN"));
                if (r391)
                { // may a396
                    bool a396 = false;
                    {
                        Checkpoint(parser); // r397

                        bool r397 = true;
                        r397 = r397 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r397, parser);
                        a396 = r397;
                    }

                    r391 |= a396;
                } // end may a396

                r391 = r391 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                CommitOrRollback(r391, parser);
                res = r391;
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
                Checkpoint(parser); // r398

                bool r398 = true;
                r398 = r398 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SearchedCaseWhen());
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

                if (r398)
                { // may a401
                    bool a401 = false;
                    {
                        Checkpoint(parser); // r402

                        bool r402 = true;
                        r402 = r402 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SearchedCaseWhenList());
                        CommitOrRollback(r402, parser);
                        a401 = r402;
                    }

                    r398 |= a401;
                } // end may a401

                CommitOrRollback(r398, parser);
                res = r398;
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
                Checkpoint(parser); // r403

                bool r403 = true;
                r403 = r403 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CASE"));
                r403 = r403 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r403 = r403 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SearchedCaseWhenList());
                if (r403)
                { // may a404
                    bool a404 = false;
                    {
                        Checkpoint(parser); // r405

                        bool r405 = true;
                        if (r405)
                        { // may a406
                            bool a406 = false;
                            {
                                Checkpoint(parser); // r407

                                bool r407 = true;
                                r407 = r407 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r407, parser);
                                a406 = r407;
                            }

                            r405 |= a406;
                        } // end may a406

                        r405 = r405 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ELSE"));
                        if (r405)
                        { // may a408
                            bool a408 = false;
                            {
                                Checkpoint(parser); // r409

                                bool r409 = true;
                                r409 = r409 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r409, parser);
                                a408 = r409;
                            }

                            r405 |= a408;
                        } // end may a408

                        r405 = r405 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        CommitOrRollback(r405, parser);
                        a404 = r405;
                    }

                    r403 |= a404;
                } // end may a404

                if (r403)
                { // may a410
                    bool a410 = false;
                    {
                        Checkpoint(parser); // r411

                        bool r411 = true;
                        r411 = r411 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r411, parser);
                        a410 = r411;
                    }

                    r403 |= a410;
                } // end may a410

                r403 = r403 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"END"));
                CommitOrRollback(r403, parser);
                res = r403;
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
                Checkpoint(parser); // r412

                bool r412 = true;
                r412 = r412 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ON"));
                if (r412)
                { // may a413
                    bool a413 = false;
                    {
                        Checkpoint(parser); // r414

                        bool r414 = true;
                        r414 = r414 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r414, parser);
                        a413 = r414;
                    }

                    r412 |= a413;
                } // end may a413

                r412 = r412 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.LogicalExpression());
                CommitOrRollback(r412, parser);
                res = r412;
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
                Checkpoint(parser); // r415

                bool r415 = true;
                if (r415)
                { // alternatives a416 must
                    bool a416 = false;
                    if (!a416)
                    {
                        Checkpoint(parser); // r417

                        bool r417 = true;
                        r417 = r417 && Match(parser, new Jhu.Graywulf.Sql.Parsing.InnerOuterJoinOperator());
                        if (r417)
                        { // may a418
                            bool a418 = false;
                            {
                                Checkpoint(parser); // r419

                                bool r419 = true;
                                r419 = r419 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r419, parser);
                                a418 = r419;
                            }

                            r417 |= a418;
                        } // end may a418

                        r417 = r417 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableSourceSpecification());
                        if (r417)
                        { // may a420
                            bool a420 = false;
                            {
                                Checkpoint(parser); // r421

                                bool r421 = true;
                                r421 = r421 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r421, parser);
                                a420 = r421;
                            }

                            r417 |= a420;
                        } // end may a420

                        r417 = r417 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.JoinCondition());
                        CommitOrRollback(r417, parser);
                        a416 = r417;
                    }

                    if (!a416)
                    {
                        Checkpoint(parser); // r422

                        bool r422 = true;
                        r422 = r422 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CrossJoinOperator());
                        if (r422)
                        { // may a423
                            bool a423 = false;
                            {
                                Checkpoint(parser); // r424

                                bool r424 = true;
                                r424 = r424 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r424, parser);
                                a423 = r424;
                            }

                            r422 |= a423;
                        } // end may a423

                        r422 = r422 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableSourceSpecification());
                        CommitOrRollback(r422, parser);
                        a416 = r422;
                    }

                    if (!a416)
                    {
                        Checkpoint(parser); // r425

                        bool r425 = true;
                        r425 = r425 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CrossApplyOperator());
                        if (r425)
                        { // may a426
                            bool a426 = false;
                            {
                                Checkpoint(parser); // r427

                                bool r427 = true;
                                r427 = r427 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r427, parser);
                                a426 = r427;
                            }

                            r425 |= a426;
                        } // end may a426

                        r425 = r425 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableSourceSpecification());
                        CommitOrRollback(r425, parser);
                        a416 = r425;
                    }

                    r415 &= a416;

                } // end alternatives a416

                if (r415)
                { // may a428
                    bool a428 = false;
                    {
                        Checkpoint(parser); // r429

                        bool r429 = true;
                        if (r429)
                        { // may a430
                            bool a430 = false;
                            {
                                Checkpoint(parser); // r431

                                bool r431 = true;
                                r431 = r431 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r431, parser);
                                a430 = r431;
                            }

                            r429 |= a430;
                        } // end may a430

                        r429 = r429 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.JoinedTable());
                        CommitOrRollback(r429, parser);
                        a428 = r429;
                    }

                    r415 |= a428;
                } // end may a428

                CommitOrRollback(r415, parser);
                res = r415;
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
                Checkpoint(parser); // r432

                bool r432 = true;
                r432 = r432 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableSourceSpecification());
                if (r432)
                { // may a433
                    bool a433 = false;
                    {
                        Checkpoint(parser); // r434

                        bool r434 = true;
                        if (r434)
                        { // may a435
                            bool a435 = false;
                            {
                                Checkpoint(parser); // r436

                                bool r436 = true;
                                r436 = r436 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r436, parser);
                                a435 = r436;
                            }

                            r434 |= a435;
                        } // end may a435

                        r434 = r434 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.JoinedTable());
                        CommitOrRollback(r434, parser);
                        a433 = r434;
                    }

                    r432 |= a433;
                } // end may a433

                CommitOrRollback(r432, parser);
                res = r432;
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
                Checkpoint(parser); // r437

                bool r437 = true;
                r437 = r437 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"FROM"));
                if (r437)
                { // may a438
                    bool a438 = false;
                    {
                        Checkpoint(parser); // r439

                        bool r439 = true;
                        r439 = r439 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r439, parser);
                        a438 = r439;
                    }

                    r437 |= a438;
                } // end may a438

                r437 = r437 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableSourceExpression());
                CommitOrRollback(r437, parser);
                res = r437;
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
                Checkpoint(parser); // r440

                bool r440 = true;
                r440 = r440 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"SELECT"));
                if (r440)
                { // may a441
                    bool a441 = false;
                    {
                        Checkpoint(parser); // r442

                        bool r442 = true;
                        r442 = r442 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        if (r442)
                        { // alternatives a443 must
                            bool a443 = false;
                            if (!a443)
                            {
                                Checkpoint(parser); // r444

                                bool r444 = true;
                                r444 = r444 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ALL"));
                                CommitOrRollback(r444, parser);
                                a443 = r444;
                            }

                            if (!a443)
                            {
                                Checkpoint(parser); // r445

                                bool r445 = true;
                                r445 = r445 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DISTINCT"));
                                CommitOrRollback(r445, parser);
                                a443 = r445;
                            }

                            r442 &= a443;

                        } // end alternatives a443

                        CommitOrRollback(r442, parser);
                        a441 = r442;
                    }

                    r440 |= a441;
                } // end may a441

                if (r440)
                { // may a446
                    bool a446 = false;
                    {
                        Checkpoint(parser); // r447

                        bool r447 = true;
                        r447 = r447 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        r447 = r447 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TopExpression());
                        CommitOrRollback(r447, parser);
                        a446 = r447;
                    }

                    r440 |= a446;
                } // end may a446

                if (r440)
                { // may a448
                    bool a448 = false;
                    {
                        Checkpoint(parser); // r449

                        bool r449 = true;
                        r449 = r449 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r449, parser);
                        a448 = r449;
                    }

                    r440 |= a448;
                } // end may a448

                r440 = r440 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SelectList());
                if (r440)
                { // may a450
                    bool a450 = false;
                    {
                        Checkpoint(parser); // r451

                        bool r451 = true;
                        if (r451)
                        { // may a452
                            bool a452 = false;
                            {
                                Checkpoint(parser); // r453

                                bool r453 = true;
                                r453 = r453 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r453, parser);
                                a452 = r453;
                            }

                            r451 |= a452;
                        } // end may a452

                        r451 = r451 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.IntoClause());
                        CommitOrRollback(r451, parser);
                        a450 = r451;
                    }

                    r440 |= a450;
                } // end may a450

                if (r440)
                { // may a454
                    bool a454 = false;
                    {
                        Checkpoint(parser); // r455

                        bool r455 = true;
                        if (r455)
                        { // may a456
                            bool a456 = false;
                            {
                                Checkpoint(parser); // r457

                                bool r457 = true;
                                r457 = r457 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r457, parser);
                                a456 = r457;
                            }

                            r455 |= a456;
                        } // end may a456

                        r455 = r455 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.FromClause());
                        CommitOrRollback(r455, parser);
                        a454 = r455;
                    }

                    r440 |= a454;
                } // end may a454

                if (r440)
                { // may a458
                    bool a458 = false;
                    {
                        Checkpoint(parser); // r459

                        bool r459 = true;
                        if (r459)
                        { // may a460
                            bool a460 = false;
                            {
                                Checkpoint(parser); // r461

                                bool r461 = true;
                                r461 = r461 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r461, parser);
                                a460 = r461;
                            }

                            r459 |= a460;
                        } // end may a460

                        r459 = r459 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.WhereClause());
                        CommitOrRollback(r459, parser);
                        a458 = r459;
                    }

                    r440 |= a458;
                } // end may a458

                if (r440)
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

                        r463 = r463 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.GroupByClause());
                        CommitOrRollback(r463, parser);
                        a462 = r463;
                    }

                    r440 |= a462;
                } // end may a462

                if (r440)
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

                        r467 = r467 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.HavingClause());
                        CommitOrRollback(r467, parser);
                        a466 = r467;
                    }

                    r440 |= a466;
                } // end may a466

                CommitOrRollback(r440, parser);
                res = r440;
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
                Checkpoint(parser); // r470

                bool r470 = true;
                if (r470)
                { // may a471
                    bool a471 = false;
                    {
                        Checkpoint(parser); // r472

                        bool r472 = true;
                        r472 = r472 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommonTableExpression());
                        if (r472)
                        { // may a473
                            bool a473 = false;
                            {
                                Checkpoint(parser); // r474

                                bool r474 = true;
                                r474 = r474 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r474, parser);
                                a473 = r474;
                            }

                            r472 |= a473;
                        } // end may a473

                        CommitOrRollback(r472, parser);
                        a471 = r472;
                    }

                    r470 |= a471;
                } // end may a471

                r470 = r470 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"UPDATE"));
                if (r470)
                { // may a475
                    bool a475 = false;
                    {
                        Checkpoint(parser); // r476

                        bool r476 = true;
                        r476 = r476 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r476, parser);
                        a475 = r476;
                    }

                    r470 |= a475;
                } // end may a475

                r470 = r470 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TargetTableSpecification());
                if (r470)
                { // may a477
                    bool a477 = false;
                    {
                        Checkpoint(parser); // r478

                        bool r478 = true;
                        r478 = r478 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r478, parser);
                        a477 = r478;
                    }

                    r470 |= a477;
                } // end may a477

                r470 = r470 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"SET"));
                if (r470)
                { // may a479
                    bool a479 = false;
                    {
                        Checkpoint(parser); // r480

                        bool r480 = true;
                        r480 = r480 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r480, parser);
                        a479 = r480;
                    }

                    r470 |= a479;
                } // end may a479

                r470 = r470 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.UpdateSetList());
                if (r470)
                { // may a481
                    bool a481 = false;
                    {
                        Checkpoint(parser); // r482

                        bool r482 = true;
                        if (r482)
                        { // may a483
                            bool a483 = false;
                            {
                                Checkpoint(parser); // r484

                                bool r484 = true;
                                r484 = r484 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r484, parser);
                                a483 = r484;
                            }

                            r482 |= a483;
                        } // end may a483

                        r482 = r482 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.FromClause());
                        CommitOrRollback(r482, parser);
                        a481 = r482;
                    }

                    r470 |= a481;
                } // end may a481

                if (r470)
                { // may a485
                    bool a485 = false;
                    {
                        Checkpoint(parser); // r486

                        bool r486 = true;
                        if (r486)
                        { // may a487
                            bool a487 = false;
                            {
                                Checkpoint(parser); // r488

                                bool r488 = true;
                                r488 = r488 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r488, parser);
                                a487 = r488;
                            }

                            r486 |= a487;
                        } // end may a487

                        r486 = r486 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.WhereClause());
                        CommitOrRollback(r486, parser);
                        a485 = r486;
                    }

                    r470 |= a485;
                } // end may a485

                if (r470)
                { // may a489
                    bool a489 = false;
                    {
                        Checkpoint(parser); // r490

                        bool r490 = true;
                        if (r490)
                        { // may a491
                            bool a491 = false;
                            {
                                Checkpoint(parser); // r492

                                bool r492 = true;
                                r492 = r492 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r492, parser);
                                a491 = r492;
                            }

                            r490 |= a491;
                        } // end may a491

                        r490 = r490 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OptionClause());
                        CommitOrRollback(r490, parser);
                        a489 = r490;
                    }

                    r470 |= a489;
                } // end may a489

                CommitOrRollback(r470, parser);
                res = r470;
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
                Checkpoint(parser); // r493

                bool r493 = true;
                if (r493)
                { // may a494
                    bool a494 = false;
                    {
                        Checkpoint(parser); // r495

                        bool r495 = true;
                        r495 = r495 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommonTableExpression());
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

                        CommitOrRollback(r495, parser);
                        a494 = r495;
                    }

                    r493 |= a494;
                } // end may a494

                r493 = r493 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DELETE"));
                if (r493)
                { // may a498
                    bool a498 = false;
                    {
                        Checkpoint(parser); // r499

                        bool r499 = true;
                        r499 = r499 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        r499 = r499 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"FROM"));
                        CommitOrRollback(r499, parser);
                        a498 = r499;
                    }

                    r493 |= a498;
                } // end may a498

                if (r493)
                { // may a500
                    bool a500 = false;
                    {
                        Checkpoint(parser); // r501

                        bool r501 = true;
                        r501 = r501 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r501, parser);
                        a500 = r501;
                    }

                    r493 |= a500;
                } // end may a500

                r493 = r493 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TargetTableSpecification());
                if (r493)
                { // may a502
                    bool a502 = false;
                    {
                        Checkpoint(parser); // r503

                        bool r503 = true;
                        if (r503)
                        { // may a504
                            bool a504 = false;
                            {
                                Checkpoint(parser); // r505

                                bool r505 = true;
                                r505 = r505 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r505, parser);
                                a504 = r505;
                            }

                            r503 |= a504;
                        } // end may a504

                        r503 = r503 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.FromClause());
                        CommitOrRollback(r503, parser);
                        a502 = r503;
                    }

                    r493 |= a502;
                } // end may a502

                if (r493)
                { // may a506
                    bool a506 = false;
                    {
                        Checkpoint(parser); // r507

                        bool r507 = true;
                        if (r507)
                        { // may a508
                            bool a508 = false;
                            {
                                Checkpoint(parser); // r509

                                bool r509 = true;
                                r509 = r509 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r509, parser);
                                a508 = r509;
                            }

                            r507 |= a508;
                        } // end may a508

                        r507 = r507 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.WhereClause());
                        CommitOrRollback(r507, parser);
                        a506 = r507;
                    }

                    r493 |= a506;
                } // end may a506

                if (r493)
                { // may a510
                    bool a510 = false;
                    {
                        Checkpoint(parser); // r511

                        bool r511 = true;
                        if (r511)
                        { // may a512
                            bool a512 = false;
                            {
                                Checkpoint(parser); // r513

                                bool r513 = true;
                                r513 = r513 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r513, parser);
                                a512 = r513;
                            }

                            r511 |= a512;
                        } // end may a512

                        r511 = r511 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OptionClause());
                        CommitOrRollback(r511, parser);
                        a510 = r511;
                    }

                    r493 |= a510;
                } // end may a510

                CommitOrRollback(r493, parser);
                res = r493;
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
                Checkpoint(parser); // r514

                bool r514 = true;
                r514 = r514 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"WHERE"));
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

                r514 = r514 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.LogicalExpression());
                CommitOrRollback(r514, parser);
                res = r514;
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
                Checkpoint(parser); // r517

                bool r517 = true;
                r517 = r517 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"HAVING"));
                if (r517)
                { // may a518
                    bool a518 = false;
                    {
                        Checkpoint(parser); // r519

                        bool r519 = true;
                        r519 = r519 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r519, parser);
                        a518 = r519;
                    }

                    r517 |= a518;
                } // end may a518

                r517 = r517 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.LogicalExpression());
                CommitOrRollback(r517, parser);
                res = r517;
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
                Checkpoint(parser); // r520

                bool r520 = true;
                r520 = r520 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r520)
                { // may a521
                    bool a521 = false;
                    {
                        Checkpoint(parser); // r522

                        bool r522 = true;
                        r522 = r522 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r522, parser);
                        a521 = r522;
                    }

                    r520 |= a521;
                } // end may a521

                if (r520)
                { // may a523
                    bool a523 = false;
                    {
                        Checkpoint(parser); // r524

                        bool r524 = true;
                        r524 = r524 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"NOT"));
                        r524 = r524 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r524, parser);
                        a523 = r524;
                    }

                    r520 |= a523;
                } // end may a523

                r520 = r520 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"LIKE"));
                if (r520)
                { // may a525
                    bool a525 = false;
                    {
                        Checkpoint(parser); // r526

                        bool r526 = true;
                        r526 = r526 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r526, parser);
                        a525 = r526;
                    }

                    r520 |= a525;
                } // end may a525

                r520 = r520 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r520)
                { // may a527
                    bool a527 = false;
                    {
                        Checkpoint(parser); // r528

                        bool r528 = true;
                        if (r528)
                        { // may a529
                            bool a529 = false;
                            {
                                Checkpoint(parser); // r530

                                bool r530 = true;
                                r530 = r530 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r530, parser);
                                a529 = r530;
                            }

                            r528 |= a529;
                        } // end may a529

                        r528 = r528 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ESCAPE"));
                        if (r528)
                        { // may a531
                            bool a531 = false;
                            {
                                Checkpoint(parser); // r532

                                bool r532 = true;
                                r532 = r532 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r532, parser);
                                a531 = r532;
                            }

                            r528 |= a531;
                        } // end may a531

                        r528 = r528 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        CommitOrRollback(r528, parser);
                        a527 = r528;
                    }

                    r520 |= a527;
                } // end may a527

                CommitOrRollback(r520, parser);
                res = r520;
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
                Checkpoint(parser); // r533

                bool r533 = true;
                r533 = r533 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r533)
                { // may a534
                    bool a534 = false;
                    {
                        Checkpoint(parser); // r535

                        bool r535 = true;
                        r535 = r535 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r535, parser);
                        a534 = r535;
                    }

                    r533 |= a534;
                } // end may a534

                if (r533)
                { // may a536
                    bool a536 = false;
                    {
                        Checkpoint(parser); // r537

                        bool r537 = true;
                        r537 = r537 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"NOT"));
                        r537 = r537 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r537, parser);
                        a536 = r537;
                    }

                    r533 |= a536;
                } // end may a536

                r533 = r533 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"BETWEEN"));
                if (r533)
                { // may a538
                    bool a538 = false;
                    {
                        Checkpoint(parser); // r539

                        bool r539 = true;
                        r539 = r539 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r539, parser);
                        a538 = r539;
                    }

                    r533 |= a538;
                } // end may a538

                r533 = r533 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r533)
                { // may a540
                    bool a540 = false;
                    {
                        Checkpoint(parser); // r541

                        bool r541 = true;
                        r541 = r541 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r541, parser);
                        a540 = r541;
                    }

                    r533 |= a540;
                } // end may a540

                r533 = r533 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"AND"));
                if (r533)
                { // may a542
                    bool a542 = false;
                    {
                        Checkpoint(parser); // r543

                        bool r543 = true;
                        r543 = r543 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r543, parser);
                        a542 = r543;
                    }

                    r533 |= a542;
                } // end may a542

                r533 = r533 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                CommitOrRollback(r533, parser);
                res = r533;
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
                Checkpoint(parser); // r544

                bool r544 = true;
                r544 = r544 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r544)
                { // may a545
                    bool a545 = false;
                    {
                        Checkpoint(parser); // r546

                        bool r546 = true;
                        r546 = r546 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r546, parser);
                        a545 = r546;
                    }

                    r544 |= a545;
                } // end may a545

                r544 = r544 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"IS"));
                if (r544)
                { // may a547
                    bool a547 = false;
                    {
                        Checkpoint(parser); // r548

                        bool r548 = true;
                        r548 = r548 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        r548 = r548 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"NOT"));
                        CommitOrRollback(r548, parser);
                        a547 = r548;
                    }

                    r544 |= a547;
                } // end may a547

                r544 = r544 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r544 = r544 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"NULL"));
                CommitOrRollback(r544, parser);
                res = r544;
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

                if (r549)
                { // may a552
                    bool a552 = false;
                    {
                        Checkpoint(parser); // r553

                        bool r553 = true;
                        r553 = r553 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"NOT"));
                        r553 = r553 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r553, parser);
                        a552 = r553;
                    }

                    r549 |= a552;
                } // end may a552

                r549 = r549 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"IN"));
                if (r549)
                { // may a554
                    bool a554 = false;
                    {
                        Checkpoint(parser); // r555

                        bool r555 = true;
                        r555 = r555 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r555, parser);
                        a554 = r555;
                    }

                    r549 |= a554;
                } // end may a554

                if (r549)
                {
                    Checkpoint(parser); // r556

                    bool r556 = true;
                    r556 = r556 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                    if (r556)
                    { // may a557
                        bool a557 = false;
                        {
                            Checkpoint(parser); // r558

                            bool r558 = true;
                            r558 = r558 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                            CommitOrRollback(r558, parser);
                            a557 = r558;
                        }

                        r556 |= a557;
                    } // end may a557

                    r556 = r556 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ArgumentList());
                    if (r556)
                    { // may a559
                        bool a559 = false;
                        {
                            Checkpoint(parser); // r560

                            bool r560 = true;
                            r560 = r560 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                            CommitOrRollback(r560, parser);
                            a559 = r560;
                        }

                        r556 |= a559;
                    } // end may a559

                    r556 = r556 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                    CommitOrRollback(r556, parser);
                    r549 = r556;
                }

                CommitOrRollback(r549, parser);
                res = r549;
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
                Checkpoint(parser); // r561

                bool r561 = true;
                r561 = r561 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r561)
                { // may a562
                    bool a562 = false;
                    {
                        Checkpoint(parser); // r563

                        bool r563 = true;
                        r563 = r563 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r563, parser);
                        a562 = r563;
                    }

                    r561 |= a562;
                } // end may a562

                if (r561)
                { // may a564
                    bool a564 = false;
                    {
                        Checkpoint(parser); // r565

                        bool r565 = true;
                        r565 = r565 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"NOT"));
                        r565 = r565 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r565, parser);
                        a564 = r565;
                    }

                    r561 |= a564;
                } // end may a564

                r561 = r561 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"IN"));
                if (r561)
                { // may a566
                    bool a566 = false;
                    {
                        Checkpoint(parser); // r567

                        bool r567 = true;
                        r567 = r567 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r567, parser);
                        a566 = r567;
                    }

                    r561 |= a566;
                } // end may a566

                r561 = r561 && Match(parser, new Jhu.Graywulf.Sql.Parsing.SemiJoinSubquery());
                CommitOrRollback(r561, parser);
                res = r561;
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
                Checkpoint(parser); // r568

                bool r568 = true;
                r568 = r568 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r568)
                { // may a569
                    bool a569 = false;
                    {
                        Checkpoint(parser); // r570

                        bool r570 = true;
                        r570 = r570 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r570, parser);
                        a569 = r570;
                    }

                    r568 |= a569;
                } // end may a569

                r568 = r568 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ComparisonOperator());
                if (r568)
                { // may a571
                    bool a571 = false;
                    {
                        Checkpoint(parser); // r572

                        bool r572 = true;
                        r572 = r572 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r572, parser);
                        a571 = r572;
                    }

                    r568 |= a571;
                } // end may a571

                if (r568)
                { // alternatives a573 must
                    bool a573 = false;
                    if (!a573)
                    {
                        Checkpoint(parser); // r574

                        bool r574 = true;
                        r574 = r574 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ALL"));
                        CommitOrRollback(r574, parser);
                        a573 = r574;
                    }

                    if (!a573)
                    {
                        Checkpoint(parser); // r575

                        bool r575 = true;
                        r575 = r575 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"SOME"));
                        CommitOrRollback(r575, parser);
                        a573 = r575;
                    }

                    if (!a573)
                    {
                        Checkpoint(parser); // r576

                        bool r576 = true;
                        r576 = r576 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ANY"));
                        CommitOrRollback(r576, parser);
                        a573 = r576;
                    }

                    r568 &= a573;

                } // end alternatives a573

                if (r568)
                { // may a577
                    bool a577 = false;
                    {
                        Checkpoint(parser); // r578

                        bool r578 = true;
                        r578 = r578 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r578, parser);
                        a577 = r578;
                    }

                    r568 |= a577;
                } // end may a577

                r568 = r568 && Match(parser, new Jhu.Graywulf.Sql.Parsing.SemiJoinSubquery());
                CommitOrRollback(r568, parser);
                res = r568;
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
                Checkpoint(parser); // r579

                bool r579 = true;
                r579 = r579 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CASE"));
                if (r579)
                { // may a580
                    bool a580 = false;
                    {
                        Checkpoint(parser); // r581

                        bool r581 = true;
                        r581 = r581 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r581, parser);
                        a580 = r581;
                    }

                    r579 |= a580;
                } // end may a580

                r579 = r579 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r579)
                { // may a582
                    bool a582 = false;
                    {
                        Checkpoint(parser); // r583

                        bool r583 = true;
                        r583 = r583 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r583, parser);
                        a582 = r583;
                    }

                    r579 |= a582;
                } // end may a582

                r579 = r579 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SimpleCaseWhenList());
                if (r579)
                { // may a584
                    bool a584 = false;
                    {
                        Checkpoint(parser); // r585

                        bool r585 = true;
                        if (r585)
                        { // may a586
                            bool a586 = false;
                            {
                                Checkpoint(parser); // r587

                                bool r587 = true;
                                r587 = r587 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r587, parser);
                                a586 = r587;
                            }

                            r585 |= a586;
                        } // end may a586

                        r585 = r585 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ELSE"));
                        if (r585)
                        { // may a588
                            bool a588 = false;
                            {
                                Checkpoint(parser); // r589

                                bool r589 = true;
                                r589 = r589 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r589, parser);
                                a588 = r589;
                            }

                            r585 |= a588;
                        } // end may a588

                        r585 = r585 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        CommitOrRollback(r585, parser);
                        a584 = r585;
                    }

                    r579 |= a584;
                } // end may a584

                if (r579)
                { // may a590
                    bool a590 = false;
                    {
                        Checkpoint(parser); // r591

                        bool r591 = true;
                        r591 = r591 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r591, parser);
                        a590 = r591;
                    }

                    r579 |= a590;
                } // end may a590

                r579 = r579 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"END"));
                CommitOrRollback(r579, parser);
                res = r579;
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
                Checkpoint(parser); // r592

                bool r592 = true;
                r592 = r592 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"WHEN"));
                if (r592)
                { // may a593
                    bool a593 = false;
                    {
                        Checkpoint(parser); // r594

                        bool r594 = true;
                        r594 = r594 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r594, parser);
                        a593 = r594;
                    }

                    r592 |= a593;
                } // end may a593

                r592 = r592 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r592)
                { // may a595
                    bool a595 = false;
                    {
                        Checkpoint(parser); // r596

                        bool r596 = true;
                        r596 = r596 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r596, parser);
                        a595 = r596;
                    }

                    r592 |= a595;
                } // end may a595

                r592 = r592 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"THEN"));
                if (r592)
                { // may a597
                    bool a597 = false;
                    {
                        Checkpoint(parser); // r598

                        bool r598 = true;
                        r598 = r598 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r598, parser);
                        a597 = r598;
                    }

                    r592 |= a597;
                } // end may a597

                r592 = r592 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                CommitOrRollback(r592, parser);
                res = r592;
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
                Checkpoint(parser); // r599

                bool r599 = true;
                r599 = r599 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SimpleCaseWhen());
                if (r599)
                { // may a600
                    bool a600 = false;
                    {
                        Checkpoint(parser); // r601

                        bool r601 = true;
                        r601 = r601 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r601, parser);
                        a600 = r601;
                    }

                    r599 |= a600;
                } // end may a600

                if (r599)
                { // may a602
                    bool a602 = false;
                    {
                        Checkpoint(parser); // r603

                        bool r603 = true;
                        r603 = r603 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SimpleCaseWhenList());
                        CommitOrRollback(r603, parser);
                        a602 = r603;
                    }

                    r599 |= a602;
                } // end may a602

                CommitOrRollback(r599, parser);
                res = r599;
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
                Checkpoint(parser); // r604

                bool r604 = true;
                r604 = r604 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                CommitOrRollback(r604, parser);
                res = r604;
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
                Checkpoint(parser); // r605

                bool r605 = true;
                if (r605)
                { // alternatives a606 must
                    bool a606 = false;
                    if (!a606)
                    {
                        Checkpoint(parser); // r607

                        bool r607 = true;
                        r607 = r607 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CAST"));
                        CommitOrRollback(r607, parser);
                        a606 = r607;
                    }

                    if (!a606)
                    {
                        Checkpoint(parser); // r608

                        bool r608 = true;
                        r608 = r608 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TRY_CAST"));
                        CommitOrRollback(r608, parser);
                        a606 = r608;
                    }

                    if (!a606)
                    {
                        Checkpoint(parser); // r609

                        bool r609 = true;
                        r609 = r609 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"PARSE"));
                        CommitOrRollback(r609, parser);
                        a606 = r609;
                    }

                    if (!a606)
                    {
                        Checkpoint(parser); // r610

                        bool r610 = true;
                        r610 = r610 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TRY_PARSE"));
                        CommitOrRollback(r610, parser);
                        a606 = r610;
                    }

                    r605 &= a606;

                } // end alternatives a606

                if (r605)
                { // may a611
                    bool a611 = false;
                    {
                        Checkpoint(parser); // r612

                        bool r612 = true;
                        r612 = r612 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r612, parser);
                        a611 = r612;
                    }

                    r605 |= a611;
                } // end may a611

                r605 = r605 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r605)
                { // may a613
                    bool a613 = false;
                    {
                        Checkpoint(parser); // r614

                        bool r614 = true;
                        r614 = r614 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r614, parser);
                        a613 = r614;
                    }

                    r605 |= a613;
                } // end may a613

                r605 = r605 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Argument());
                if (r605)
                { // may a615
                    bool a615 = false;
                    {
                        Checkpoint(parser); // r616

                        bool r616 = true;
                        r616 = r616 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r616, parser);
                        a615 = r616;
                    }

                    r605 |= a615;
                } // end may a615

                r605 = r605 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"AS"));
                if (r605)
                { // may a617
                    bool a617 = false;
                    {
                        Checkpoint(parser); // r618

                        bool r618 = true;
                        r618 = r618 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r618, parser);
                        a617 = r618;
                    }

                    r605 |= a617;
                } // end may a617

                r605 = r605 && Match(parser, new Jhu.Graywulf.Sql.Parsing.DataTypeArgument());
                if (r605)
                { // may a619
                    bool a619 = false;
                    {
                        Checkpoint(parser); // r620

                        bool r620 = true;
                        if (r620)
                        { // may a621
                            bool a621 = false;
                            {
                                Checkpoint(parser); // r622

                                bool r622 = true;
                                r622 = r622 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r622, parser);
                                a621 = r622;
                            }

                            r620 |= a621;
                        } // end may a621

                        r620 = r620 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"USING"));
                        if (r620)
                        { // may a623
                            bool a623 = false;
                            {
                                Checkpoint(parser); // r624

                                bool r624 = true;
                                r624 = r624 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r624, parser);
                                a623 = r624;
                            }

                            r620 |= a623;
                        } // end may a623

                        r620 = r620 && Match(parser, new Jhu.Graywulf.Sql.Parsing.StringConstant());
                        CommitOrRollback(r620, parser);
                        a619 = r620;
                    }

                    r605 |= a619;
                } // end may a619

                if (r605)
                { // may a625
                    bool a625 = false;
                    {
                        Checkpoint(parser); // r626

                        bool r626 = true;
                        r626 = r626 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r626, parser);
                        a625 = r626;
                    }

                    r605 |= a625;
                } // end may a625

                r605 = r605 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r605, parser);
                res = r605;
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
                Checkpoint(parser); // r627

                bool r627 = true;
                if (r627)
                { // alternatives a628 must
                    bool a628 = false;
                    if (!a628)
                    {
                        Checkpoint(parser); // r629

                        bool r629 = true;
                        r629 = r629 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CONVERT"));
                        CommitOrRollback(r629, parser);
                        a628 = r629;
                    }

                    if (!a628)
                    {
                        Checkpoint(parser); // r630

                        bool r630 = true;
                        r630 = r630 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TRY_CONVERT"));
                        CommitOrRollback(r630, parser);
                        a628 = r630;
                    }

                    r627 &= a628;

                } // end alternatives a628

                if (r627)
                { // may a631
                    bool a631 = false;
                    {
                        Checkpoint(parser); // r632

                        bool r632 = true;
                        r632 = r632 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r632, parser);
                        a631 = r632;
                    }

                    r627 |= a631;
                } // end may a631

                r627 = r627 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r627)
                { // may a633
                    bool a633 = false;
                    {
                        Checkpoint(parser); // r634

                        bool r634 = true;
                        r634 = r634 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r634, parser);
                        a633 = r634;
                    }

                    r627 |= a633;
                } // end may a633

                r627 = r627 && Match(parser, new Jhu.Graywulf.Sql.Parsing.DataTypeArgument());
                if (r627)
                { // may a635
                    bool a635 = false;
                    {
                        Checkpoint(parser); // r636

                        bool r636 = true;
                        r636 = r636 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r636, parser);
                        a635 = r636;
                    }

                    r627 |= a635;
                } // end may a635

                r627 = r627 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                if (r627)
                { // may a637
                    bool a637 = false;
                    {
                        Checkpoint(parser); // r638

                        bool r638 = true;
                        r638 = r638 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r638, parser);
                        a637 = r638;
                    }

                    r627 |= a637;
                } // end may a637

                r627 = r627 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Argument());
                if (r627)
                { // may a639
                    bool a639 = false;
                    {
                        Checkpoint(parser); // r640

                        bool r640 = true;
                        r640 = r640 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r640, parser);
                        a639 = r640;
                    }

                    r627 |= a639;
                } // end may a639

                r627 = r627 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r627, parser);
                res = r627;
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
                Checkpoint(parser); // r641

                bool r641 = true;
                r641 = r641 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Argument());
                if (r641)
                { // may a642
                    bool a642 = false;
                    {
                        Checkpoint(parser); // r643

                        bool r643 = true;
                        if (r643)
                        { // may a644
                            bool a644 = false;
                            {
                                Checkpoint(parser); // r645

                                bool r645 = true;
                                r645 = r645 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r645, parser);
                                a644 = r645;
                            }

                            r643 |= a644;
                        } // end may a644

                        r643 = r643 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r643)
                        { // may a646
                            bool a646 = false;
                            {
                                Checkpoint(parser); // r647

                                bool r647 = true;
                                r647 = r647 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r647, parser);
                                a646 = r647;
                            }

                            r643 |= a646;
                        } // end may a646

                        r643 = r643 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ArgumentList());
                        CommitOrRollback(r643, parser);
                        a642 = r643;
                    }

                    r641 |= a642;
                } // end may a642

                CommitOrRollback(r641, parser);
                res = r641;
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
                Checkpoint(parser); // r648

                bool r648 = true;
                r648 = r648 && Match(parser, new Jhu.Graywulf.Sql.Parsing.MemberName());
                if (r648)
                { // may a649
                    bool a649 = false;
                    {
                        Checkpoint(parser); // r650

                        bool r650 = true;
                        r650 = r650 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r650, parser);
                        a649 = r650;
                    }

                    r648 |= a649;
                } // end may a649

                r648 = r648 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r648)
                { // may a651
                    bool a651 = false;
                    {
                        Checkpoint(parser); // r652

                        bool r652 = true;
                        r652 = r652 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r652, parser);
                        a651 = r652;
                    }

                    r648 |= a651;
                } // end may a651

                if (r648)
                { // may a653
                    bool a653 = false;
                    {
                        Checkpoint(parser); // r654

                        bool r654 = true;
                        r654 = r654 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ArgumentList());
                        CommitOrRollback(r654, parser);
                        a653 = r654;
                    }

                    r648 |= a653;
                } // end may a653

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

                r648 = r648 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r648, parser);
                res = r648;
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
                Checkpoint(parser); // r657

                bool r657 = true;
                r657 = r657 && Match(parser, new Jhu.Graywulf.Sql.Parsing.MemberAccessOperator());
                if (r657)
                { // may a658
                    bool a658 = false;
                    {
                        Checkpoint(parser); // r659

                        bool r659 = true;
                        r659 = r659 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r659, parser);
                        a658 = r659;
                    }

                    r657 |= a658;
                } // end may a658

                if (r657)
                { // alternatives a660 must
                    bool a660 = false;
                    if (!a660)
                    {
                        Checkpoint(parser); // r661

                        bool r661 = true;
                        r661 = r661 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.MemberCall());
                        CommitOrRollback(r661, parser);
                        a660 = r661;
                    }

                    if (!a660)
                    {
                        Checkpoint(parser); // r662

                        bool r662 = true;
                        r662 = r662 && Match(parser, new Jhu.Graywulf.Sql.Parsing.MemberAccess());
                        CommitOrRollback(r662, parser);
                        a660 = r662;
                    }

                    r657 &= a660;

                } // end alternatives a660

                if (r657)
                { // may a663
                    bool a663 = false;
                    {
                        Checkpoint(parser); // r664

                        bool r664 = true;
                        if (r664)
                        { // may a665
                            bool a665 = false;
                            {
                                Checkpoint(parser); // r666

                                bool r666 = true;
                                r666 = r666 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r666, parser);
                                a665 = r666;
                            }

                            r664 |= a665;
                        } // end may a665

                        r664 = r664 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.MemberAccessList());
                        CommitOrRollback(r664, parser);
                        a663 = r664;
                    }

                    r657 |= a663;
                } // end may a663

                CommitOrRollback(r657, parser);
                res = r657;
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
                Checkpoint(parser); // r667

                bool r667 = true;
                if (r667)
                { // alternatives a668 must
                    bool a668 = false;
                    if (!a668)
                    {
                        Checkpoint(parser); // r669

                        bool r669 = true;
                        r669 = r669 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DATEADD"));
                        CommitOrRollback(r669, parser);
                        a668 = r669;
                    }

                    if (!a668)
                    {
                        Checkpoint(parser); // r670

                        bool r670 = true;
                        r670 = r670 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DATEDIFF"));
                        CommitOrRollback(r670, parser);
                        a668 = r670;
                    }

                    if (!a668)
                    {
                        Checkpoint(parser); // r671

                        bool r671 = true;
                        r671 = r671 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DATEDIFF_BIG"));
                        CommitOrRollback(r671, parser);
                        a668 = r671;
                    }

                    if (!a668)
                    {
                        Checkpoint(parser); // r672

                        bool r672 = true;
                        r672 = r672 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DATENAME"));
                        CommitOrRollback(r672, parser);
                        a668 = r672;
                    }

                    if (!a668)
                    {
                        Checkpoint(parser); // r673

                        bool r673 = true;
                        r673 = r673 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DATEPART"));
                        CommitOrRollback(r673, parser);
                        a668 = r673;
                    }

                    r667 &= a668;

                } // end alternatives a668

                if (r667)
                { // may a674
                    bool a674 = false;
                    {
                        Checkpoint(parser); // r675

                        bool r675 = true;
                        r675 = r675 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r675, parser);
                        a674 = r675;
                    }

                    r667 |= a674;
                } // end may a674

                r667 = r667 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r667)
                { // may a676
                    bool a676 = false;
                    {
                        Checkpoint(parser); // r677

                        bool r677 = true;
                        r677 = r677 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r677, parser);
                        a676 = r677;
                    }

                    r667 |= a676;
                } // end may a676

                r667 = r667 && Match(parser, new Jhu.Graywulf.Sql.Parsing.DatePart());
                if (r667)
                { // may a678
                    bool a678 = false;
                    {
                        Checkpoint(parser); // r679

                        bool r679 = true;
                        r679 = r679 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r679, parser);
                        a678 = r679;
                    }

                    r667 |= a678;
                } // end may a678

                r667 = r667 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                if (r667)
                { // may a680
                    bool a680 = false;
                    {
                        Checkpoint(parser); // r681

                        bool r681 = true;
                        r681 = r681 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r681, parser);
                        a680 = r681;
                    }

                    r667 |= a680;
                } // end may a680

                r667 = r667 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ArgumentList());
                if (r667)
                { // may a682
                    bool a682 = false;
                    {
                        Checkpoint(parser); // r683

                        bool r683 = true;
                        r683 = r683 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r683, parser);
                        a682 = r683;
                    }

                    r667 |= a682;
                } // end may a682

                r667 = r667 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r667, parser);
                res = r667;
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
                Checkpoint(parser); // r684

                bool r684 = true;
                r684 = r684 && Match(parser, new Jhu.Graywulf.Sql.Parsing.FunctionName());
                if (r684)
                { // may a685
                    bool a685 = false;
                    {
                        Checkpoint(parser); // r686

                        bool r686 = true;
                        r686 = r686 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r686, parser);
                        a685 = r686;
                    }

                    r684 |= a685;
                } // end may a685

                r684 = r684 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r684)
                { // may a687
                    bool a687 = false;
                    {
                        Checkpoint(parser); // r688

                        bool r688 = true;
                        r688 = r688 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r688, parser);
                        a687 = r688;
                    }

                    r684 |= a687;
                } // end may a687

                if (r684)
                { // may a689
                    bool a689 = false;
                    {
                        Checkpoint(parser); // r690

                        bool r690 = true;
                        r690 = r690 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ArgumentList());
                        CommitOrRollback(r690, parser);
                        a689 = r690;
                    }

                    r684 |= a689;
                } // end may a689

                if (r684)
                { // may a691
                    bool a691 = false;
                    {
                        Checkpoint(parser); // r692

                        bool r692 = true;
                        r692 = r692 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r692, parser);
                        a691 = r692;
                    }

                    r684 |= a691;
                } // end may a691

                r684 = r684 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r684, parser);
                res = r684;
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
                Checkpoint(parser); // r693

                bool r693 = true;
                r693 = r693 && Match(parser, new Jhu.Graywulf.Sql.Parsing.FunctionIdentifier());
                if (r693)
                { // may a694
                    bool a694 = false;
                    {
                        Checkpoint(parser); // r695

                        bool r695 = true;
                        r695 = r695 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r695, parser);
                        a694 = r695;
                    }

                    r693 |= a694;
                } // end may a694

                r693 = r693 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r693)
                { // may a696
                    bool a696 = false;
                    {
                        Checkpoint(parser); // r697

                        bool r697 = true;
                        r697 = r697 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r697, parser);
                        a696 = r697;
                    }

                    r693 |= a696;
                } // end may a696

                if (r693)
                { // alternatives a698 must
                    bool a698 = false;
                    if (!a698)
                    {
                        Checkpoint(parser); // r699

                        bool r699 = true;
                        r699 = r699 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ALL"));
                        CommitOrRollback(r699, parser);
                        a698 = r699;
                    }

                    if (!a698)
                    {
                        Checkpoint(parser); // r700

                        bool r700 = true;
                        r700 = r700 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DISTINCT"));
                        CommitOrRollback(r700, parser);
                        a698 = r700;
                    }

                    r693 &= a698;

                } // end alternatives a698

                if (r693)
                { // may a701
                    bool a701 = false;
                    {
                        Checkpoint(parser); // r702

                        bool r702 = true;
                        r702 = r702 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r702, parser);
                        a701 = r702;
                    }

                    r693 |= a701;
                } // end may a701

                r693 = r693 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ArgumentList());
                if (r693)
                { // may a703
                    bool a703 = false;
                    {
                        Checkpoint(parser); // r704

                        bool r704 = true;
                        r704 = r704 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r704, parser);
                        a703 = r704;
                    }

                    r693 |= a703;
                } // end may a703

                r693 = r693 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                if (r693)
                { // may a705
                    bool a705 = false;
                    {
                        Checkpoint(parser); // r706

                        bool r706 = true;
                        if (r706)
                        { // may a707
                            bool a707 = false;
                            {
                                Checkpoint(parser); // r708

                                bool r708 = true;
                                r708 = r708 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r708, parser);
                                a707 = r708;
                            }

                            r706 |= a707;
                        } // end may a707

                        r706 = r706 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OverClause());
                        CommitOrRollback(r706, parser);
                        a705 = r706;
                    }

                    r693 |= a705;
                } // end may a705

                CommitOrRollback(r693, parser);
                res = r693;
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
                Checkpoint(parser); // r709

                bool r709 = true;
                r709 = r709 && Match(parser, new Jhu.Graywulf.Sql.Parsing.FunctionIdentifier());
                if (r709)
                { // may a710
                    bool a710 = false;
                    {
                        Checkpoint(parser); // r711

                        bool r711 = true;
                        r711 = r711 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r711, parser);
                        a710 = r711;
                    }

                    r709 |= a710;
                } // end may a710

                r709 = r709 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r709)
                { // may a712
                    bool a712 = false;
                    {
                        Checkpoint(parser); // r713

                        bool r713 = true;
                        if (r713)
                        { // may a714
                            bool a714 = false;
                            {
                                Checkpoint(parser); // r715

                                bool r715 = true;
                                r715 = r715 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r715, parser);
                                a714 = r715;
                            }

                            r713 |= a714;
                        } // end may a714

                        r713 = r713 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ArgumentList());
                        CommitOrRollback(r713, parser);
                        a712 = r713;
                    }

                    r709 |= a712;
                } // end may a712

                if (r709)
                { // may a716
                    bool a716 = false;
                    {
                        Checkpoint(parser); // r717

                        bool r717 = true;
                        r717 = r717 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r717, parser);
                        a716 = r717;
                    }

                    r709 |= a716;
                } // end may a716

                r709 = r709 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                if (r709)
                { // may a718
                    bool a718 = false;
                    {
                        Checkpoint(parser); // r719

                        bool r719 = true;
                        r719 = r719 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r719, parser);
                        a718 = r719;
                    }

                    r709 |= a718;
                } // end may a718

                r709 = r709 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OverClause());
                CommitOrRollback(r709, parser);
                res = r709;
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
                Checkpoint(parser); // r720

                bool r720 = true;
                r720 = r720 && Match(parser, new Jhu.Graywulf.Sql.Parsing.FunctionIdentifier());
                if (r720)
                { // may a721
                    bool a721 = false;
                    {
                        Checkpoint(parser); // r722

                        bool r722 = true;
                        r722 = r722 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r722, parser);
                        a721 = r722;
                    }

                    r720 |= a721;
                } // end may a721

                r720 = r720 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r720)
                { // may a723
                    bool a723 = false;
                    {
                        Checkpoint(parser); // r724

                        bool r724 = true;
                        r724 = r724 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r724, parser);
                        a723 = r724;
                    }

                    r720 |= a723;
                } // end may a723

                if (r720)
                { // may a725
                    bool a725 = false;
                    {
                        Checkpoint(parser); // r726

                        bool r726 = true;
                        r726 = r726 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ArgumentList());
                        CommitOrRollback(r726, parser);
                        a725 = r726;
                    }

                    r720 |= a725;
                } // end may a725

                if (r720)
                { // may a727
                    bool a727 = false;
                    {
                        Checkpoint(parser); // r728

                        bool r728 = true;
                        r728 = r728 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r728, parser);
                        a727 = r728;
                    }

                    r720 |= a727;
                } // end may a727

                r720 = r720 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r720, parser);
                res = r720;
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
                Checkpoint(parser); // r729

                bool r729 = true;
                r729 = r729 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableValuedFunctionCall());
                if (r729)
                { // may a730
                    bool a730 = false;
                    {
                        Checkpoint(parser); // r731

                        bool r731 = true;
                        r731 = r731 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r731, parser);
                        a730 = r731;
                    }

                    r729 |= a730;
                } // end may a730

                if (r729)
                { // may a732
                    bool a732 = false;
                    {
                        Checkpoint(parser); // r733

                        bool r733 = true;
                        r733 = r733 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"AS"));
                        if (r733)
                        { // may a734
                            bool a734 = false;
                            {
                                Checkpoint(parser); // r735

                                bool r735 = true;
                                r735 = r735 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r735, parser);
                                a734 = r735;
                            }

                            r733 |= a734;
                        } // end may a734

                        CommitOrRollback(r733, parser);
                        a732 = r733;
                    }

                    r729 |= a732;
                } // end may a732

                r729 = r729 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableAlias());
                CommitOrRollback(r729, parser);
                res = r729;
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
                Checkpoint(parser); // r736

                bool r736 = true;
                if (r736)
                { // alternatives a737 must
                    bool a737 = false;
                    if (!a737)
                    {
                        Checkpoint(parser); // r738

                        bool r738 = true;
                        r738 = r738 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.FunctionTableSource());
                        CommitOrRollback(r738, parser);
                        a737 = r738;
                    }

                    if (!a737)
                    {
                        Checkpoint(parser); // r739

                        bool r739 = true;
                        r739 = r739 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SimpleTableSource());
                        CommitOrRollback(r739, parser);
                        a737 = r739;
                    }

                    if (!a737)
                    {
                        Checkpoint(parser); // r740

                        bool r740 = true;
                        r740 = r740 && Match(parser, new Jhu.Graywulf.Sql.Parsing.VariableTableSource());
                        CommitOrRollback(r740, parser);
                        a737 = r740;
                    }

                    if (!a737)
                    {
                        Checkpoint(parser); // r741

                        bool r741 = true;
                        r741 = r741 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SubqueryTableSource());
                        CommitOrRollback(r741, parser);
                        a737 = r741;
                    }

                    r736 &= a737;

                } // end alternatives a737

                CommitOrRollback(r736, parser);
                res = r736;
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
                Checkpoint(parser); // r742

                bool r742 = true;
                r742 = r742 && Match(parser, new Jhu.Graywulf.Sql.Parsing.MethodName());
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

                r742 = r742 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
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

                if (r742)
                { // may a747
                    bool a747 = false;
                    {
                        Checkpoint(parser); // r748

                        bool r748 = true;
                        r748 = r748 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ArgumentList());
                        CommitOrRollback(r748, parser);
                        a747 = r748;
                    }

                    r742 |= a747;
                } // end may a747

                if (r742)
                { // may a749
                    bool a749 = false;
                    {
                        Checkpoint(parser); // r750

                        bool r750 = true;
                        r750 = r750 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r750, parser);
                        a749 = r750;
                    }

                    r742 |= a749;
                } // end may a749

                r742 = r742 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r742, parser);
                res = r742;
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
                Checkpoint(parser); // r751

                bool r751 = true;
                r751 = r751 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ColumnName());
                if (r751)
                { // may a752
                    bool a752 = false;
                    {
                        Checkpoint(parser); // r753

                        bool r753 = true;
                        r753 = r753 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r753, parser);
                        a752 = r753;
                    }

                    r751 |= a752;
                } // end may a752

                r751 = r751 && Match(parser, new Jhu.Graywulf.Sql.Parsing.MemberAccessOperator());
                if (r751)
                { // may a754
                    bool a754 = false;
                    {
                        Checkpoint(parser); // r755

                        bool r755 = true;
                        r755 = r755 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r755, parser);
                        a754 = r755;
                    }

                    r751 |= a754;
                } // end may a754

                r751 = r751 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.UdtMethodCall());
                CommitOrRollback(r751, parser);
                res = r751;
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
                Checkpoint(parser); // r756

                bool r756 = true;
                if (r756)
                { // alternatives a757 must
                    bool a757 = false;
                    if (!a757)
                    {
                        Checkpoint(parser); // r758

                        bool r758 = true;
                        r758 = r758 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.UpdateSetColumn());
                        CommitOrRollback(r758, parser);
                        a757 = r758;
                    }

                    if (!a757)
                    {
                        Checkpoint(parser); // r759

                        bool r759 = true;
                        r759 = r759 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.UpdateSetMutator());
                        CommitOrRollback(r759, parser);
                        a757 = r759;
                    }

                    r756 &= a757;

                } // end alternatives a757

                if (r756)
                { // may a760
                    bool a760 = false;
                    {
                        Checkpoint(parser); // r761

                        bool r761 = true;
                        if (r761)
                        { // may a762
                            bool a762 = false;
                            {
                                Checkpoint(parser); // r763

                                bool r763 = true;
                                r763 = r763 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r763, parser);
                                a762 = r763;
                            }

                            r761 |= a762;
                        } // end may a762

                        r761 = r761 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r761)
                        { // may a764
                            bool a764 = false;
                            {
                                Checkpoint(parser); // r765

                                bool r765 = true;
                                r765 = r765 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r765, parser);
                                a764 = r765;
                            }

                            r761 |= a764;
                        } // end may a764

                        r761 = r761 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.UpdateSetList());
                        CommitOrRollback(r761, parser);
                        a760 = r761;
                    }

                    r756 |= a760;
                } // end may a760

                CommitOrRollback(r756, parser);
                res = r756;
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
                Checkpoint(parser); // r766

                bool r766 = true;
                r766 = r766 && Match(parser, new Jhu.Graywulf.Sql.Parsing.MethodName());
                if (r766)
                { // may a767
                    bool a767 = false;
                    {
                        Checkpoint(parser); // r768

                        bool r768 = true;
                        r768 = r768 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r768, parser);
                        a767 = r768;
                    }

                    r766 |= a767;
                } // end may a767

                r766 = r766 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r766)
                { // may a769
                    bool a769 = false;
                    {
                        Checkpoint(parser); // r770

                        bool r770 = true;
                        r770 = r770 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r770, parser);
                        a769 = r770;
                    }

                    r766 |= a769;
                } // end may a769

                if (r766)
                { // may a771
                    bool a771 = false;
                    {
                        Checkpoint(parser); // r772

                        bool r772 = true;
                        r772 = r772 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ArgumentList());
                        CommitOrRollback(r772, parser);
                        a771 = r772;
                    }

                    r766 |= a771;
                } // end may a771

                if (r766)
                { // may a773
                    bool a773 = false;
                    {
                        Checkpoint(parser); // r774

                        bool r774 = true;
                        r774 = r774 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r774, parser);
                        a773 = r774;
                    }

                    r766 |= a773;
                } // end may a773

                r766 = r766 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r766, parser);
                res = r766;
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
                Checkpoint(parser); // r775

                bool r775 = true;
                r775 = r775 && Match(parser, new Jhu.Graywulf.Sql.Parsing.DataTypeIdentifier());
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

                r775 = r775 && Match(parser, new Jhu.Graywulf.Sql.Parsing.StaticMemberAccessOperator());
                if (r775)
                { // may a778
                    bool a778 = false;
                    {
                        Checkpoint(parser); // r779

                        bool r779 = true;
                        r779 = r779 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r779, parser);
                        a778 = r779;
                    }

                    r775 |= a778;
                } // end may a778

                if (r775)
                { // alternatives a780 must
                    bool a780 = false;
                    if (!a780)
                    {
                        Checkpoint(parser); // r781

                        bool r781 = true;
                        r781 = r781 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.UdtStaticMethodCall());
                        CommitOrRollback(r781, parser);
                        a780 = r781;
                    }

                    if (!a780)
                    {
                        Checkpoint(parser); // r782

                        bool r782 = true;
                        r782 = r782 && Match(parser, new Jhu.Graywulf.Sql.Parsing.UdtStaticPropertyAccess());
                        CommitOrRollback(r782, parser);
                        a780 = r782;
                    }

                    r775 &= a780;

                } // end alternatives a780

                CommitOrRollback(r775, parser);
                res = r775;
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
                Checkpoint(parser); // r783

                bool r783 = true;
                r783 = r783 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"PARTITION"));
                r783 = r783 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r783 = r783 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"BY"));
                if (r783)
                { // may a784
                    bool a784 = false;
                    {
                        Checkpoint(parser); // r785

                        bool r785 = true;
                        r785 = r785 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r785, parser);
                        a784 = r785;
                    }

                    r783 |= a784;
                } // end may a784

                r783 = r783 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Argument());
                CommitOrRollback(r783, parser);
                res = r783;
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
                Checkpoint(parser); // r786

                bool r786 = true;
                r786 = r786 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"OVER"));
                if (r786)
                { // may a787
                    bool a787 = false;
                    {
                        Checkpoint(parser); // r788

                        bool r788 = true;
                        r788 = r788 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r788, parser);
                        a787 = r788;
                    }

                    r786 |= a787;
                } // end may a787

                r786 = r786 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r786)
                { // may a789
                    bool a789 = false;
                    {
                        Checkpoint(parser); // r790

                        bool r790 = true;
                        if (r790)
                        { // may a791
                            bool a791 = false;
                            {
                                Checkpoint(parser); // r792

                                bool r792 = true;
                                r792 = r792 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r792, parser);
                                a791 = r792;
                            }

                            r790 |= a791;
                        } // end may a791

                        r790 = r790 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.PartitionByClause());
                        CommitOrRollback(r790, parser);
                        a789 = r790;
                    }

                    r786 |= a789;
                } // end may a789

                if (r786)
                { // may a793
                    bool a793 = false;
                    {
                        Checkpoint(parser); // r794

                        bool r794 = true;
                        if (r794)
                        { // may a795
                            bool a795 = false;
                            {
                                Checkpoint(parser); // r796

                                bool r796 = true;
                                r796 = r796 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r796, parser);
                                a795 = r796;
                            }

                            r794 |= a795;
                        } // end may a795

                        r794 = r794 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OrderByClause());
                        CommitOrRollback(r794, parser);
                        a793 = r794;
                    }

                    r786 |= a793;
                } // end may a793

                if (r786)
                { // may a797
                    bool a797 = false;
                    {
                        Checkpoint(parser); // r798

                        bool r798 = true;
                        r798 = r798 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r798, parser);
                        a797 = r798;
                    }

                    r786 |= a797;
                } // end may a797

                r786 = r786 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r786, parser);
                res = r786;
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
                Checkpoint(parser); // r799

                bool r799 = true;
                r799 = r799 && Match(parser, new Jhu.Graywulf.Sql.Parsing.FunctionIdentifier());
                if (r799)
                { // may a800
                    bool a800 = false;
                    {
                        Checkpoint(parser); // r801

                        bool r801 = true;
                        r801 = r801 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r801, parser);
                        a800 = r801;
                    }

                    r799 |= a800;
                } // end may a800

                r799 = r799 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r799)
                { // may a802
                    bool a802 = false;
                    {
                        Checkpoint(parser); // r803

                        bool r803 = true;
                        r803 = r803 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r803, parser);
                        a802 = r803;
                    }

                    r799 |= a802;
                } // end may a802

                r799 = r799 && Match(parser, new Jhu.Graywulf.Sql.Parsing.StarArgument());
                if (r799)
                { // may a804
                    bool a804 = false;
                    {
                        Checkpoint(parser); // r805

                        bool r805 = true;
                        r805 = r805 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r805, parser);
                        a804 = r805;
                    }

                    r799 |= a804;
                } // end may a804

                r799 = r799 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                if (r799)
                { // may a806
                    bool a806 = false;
                    {
                        Checkpoint(parser); // r807

                        bool r807 = true;
                        if (r807)
                        { // may a808
                            bool a808 = false;
                            {
                                Checkpoint(parser); // r809

                                bool r809 = true;
                                r809 = r809 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r809, parser);
                                a808 = r809;
                            }

                            r807 |= a808;
                        } // end may a808

                        r807 = r807 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OverClause());
                        CommitOrRollback(r807, parser);
                        a806 = r807;
                    }

                    r799 |= a806;
                } // end may a806

                CommitOrRollback(r799, parser);
                res = r799;
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
                Checkpoint(parser); // r810

                bool r810 = true;
                r810 = r810 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"PRINT"));
                r810 = r810 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r810 = r810 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                CommitOrRollback(r810, parser);
                res = r810;
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
                Checkpoint(parser); // r811

                bool r811 = true;
                r811 = r811 && Match(parser, new Jhu.Graywulf.Sql.Parsing.UserVariable());
                if (r811)
                { // may a812
                    bool a812 = false;
                    {
                        Checkpoint(parser); // r813

                        bool r813 = true;
                        r813 = r813 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        r813 = r813 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"AS"));
                        CommitOrRollback(r813, parser);
                        a812 = r813;
                    }

                    r811 |= a812;
                } // end may a812

                if (r811)
                { // may a814
                    bool a814 = false;
                    {
                        Checkpoint(parser); // r815

                        bool r815 = true;
                        r815 = r815 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r815, parser);
                        a814 = r815;
                    }

                    r811 |= a814;
                } // end may a814

                r811 = r811 && Match(parser, new Jhu.Graywulf.Sql.Parsing.DataTypeSpecification());
                if (r811)
                { // may a816
                    bool a816 = false;
                    {
                        Checkpoint(parser); // r817

                        bool r817 = true;
                        if (r817)
                        { // may a818
                            bool a818 = false;
                            {
                                Checkpoint(parser); // r819

                                bool r819 = true;
                                r819 = r819 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r819, parser);
                                a818 = r819;
                            }

                            r817 |= a818;
                        } // end may a818

                        r817 = r817 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ValueAssignmentOperator());
                        if (r817)
                        { // may a820
                            bool a820 = false;
                            {
                                Checkpoint(parser); // r821

                                bool r821 = true;
                                r821 = r821 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r821, parser);
                                a820 = r821;
                            }

                            r817 |= a820;
                        } // end may a820

                        r817 = r817 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        CommitOrRollback(r817, parser);
                        a816 = r817;
                    }

                    r811 |= a816;
                } // end may a816

                CommitOrRollback(r811, parser);
                res = r811;
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
                Checkpoint(parser); // r822

                bool r822 = true;
                r822 = r822 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.VariableDeclaration());
                if (r822)
                { // may a823
                    bool a823 = false;
                    {
                        Checkpoint(parser); // r824

                        bool r824 = true;
                        if (r824)
                        { // may a825
                            bool a825 = false;
                            {
                                Checkpoint(parser); // r826

                                bool r826 = true;
                                r826 = r826 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r826, parser);
                                a825 = r826;
                            }

                            r824 |= a825;
                        } // end may a825

                        r824 = r824 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r824)
                        { // may a827
                            bool a827 = false;
                            {
                                Checkpoint(parser); // r828

                                bool r828 = true;
                                r828 = r828 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r828, parser);
                                a827 = r828;
                            }

                            r824 |= a827;
                        } // end may a827

                        r824 = r824 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.VariableDeclarationList());
                        CommitOrRollback(r824, parser);
                        a823 = r824;
                    }

                    r822 |= a823;
                } // end may a823

                CommitOrRollback(r822, parser);
                res = r822;
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
                Checkpoint(parser); // r829

                bool r829 = true;
                r829 = r829 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DECLARE"));
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

                r829 = r829 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.VariableDeclarationList());
                CommitOrRollback(r829, parser);
                res = r829;
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
                Checkpoint(parser); // r832

                bool r832 = true;
                r832 = r832 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"SET"));
                r832 = r832 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r832 = r832 && Match(parser, new Jhu.Graywulf.Sql.Parsing.UserVariable());
                if (r832)
                { // may a833
                    bool a833 = false;
                    {
                        Checkpoint(parser); // r834

                        bool r834 = true;
                        r834 = r834 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r834, parser);
                        a833 = r834;
                    }

                    r832 |= a833;
                } // end may a833

                r832 = r832 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ValueAssignmentOperator());
                if (r832)
                { // may a835
                    bool a835 = false;
                    {
                        Checkpoint(parser); // r836

                        bool r836 = true;
                        r836 = r836 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r836, parser);
                        a835 = r836;
                    }

                    r832 |= a835;
                } // end may a835

                r832 = r832 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                CommitOrRollback(r832, parser);
                res = r832;
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
                Checkpoint(parser); // r837

                bool r837 = true;
                r837 = r837 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TOP"));
                if (r837)
                { // may a838
                    bool a838 = false;
                    {
                        Checkpoint(parser); // r839

                        bool r839 = true;
                        r839 = r839 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r839, parser);
                        a838 = r839;
                    }

                    r837 |= a838;
                } // end may a838

                if (r837)
                { // alternatives a840 must
                    bool a840 = false;
                    if (!a840)
                    {
                        Checkpoint(parser); // r841

                        bool r841 = true;
                        r841 = r841 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        CommitOrRollback(r841, parser);
                        a840 = r841;
                    }

                    r837 &= a840;

                } // end alternatives a840

                if (r837)
                { // may a842
                    bool a842 = false;
                    {
                        Checkpoint(parser); // r843

                        bool r843 = true;
                        r843 = r843 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        r843 = r843 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"PERCENT"));
                        CommitOrRollback(r843, parser);
                        a842 = r843;
                    }

                    r837 |= a842;
                } // end may a842

                if (r837)
                { // may a844
                    bool a844 = false;
                    {
                        Checkpoint(parser); // r845

                        bool r845 = true;
                        r845 = r845 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        r845 = r845 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"WITH"));
                        r845 = r845 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        r845 = r845 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TIES"));
                        CommitOrRollback(r845, parser);
                        a844 = r845;
                    }

                    r837 |= a844;
                } // end may a844

                CommitOrRollback(r837, parser);
                res = r837;
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
                Checkpoint(parser); // r846

                bool r846 = true;
                if (r846)
                { // alternatives a847 must
                    bool a847 = false;
                    if (!a847)
                    {
                        Checkpoint(parser); // r848

                        bool r848 = true;
                        r848 = r848 && Match(parser, new Jhu.Graywulf.Sql.Parsing.UserVariable());
                        if (r848)
                        { // may a849
                            bool a849 = false;
                            {
                                Checkpoint(parser); // r850

                                bool r850 = true;
                                r850 = r850 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r850, parser);
                                a849 = r850;
                            }

                            r848 |= a849;
                        } // end may a849

                        r848 = r848 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ValueAssignmentOperator());
                        if (r848)
                        { // may a851
                            bool a851 = false;
                            {
                                Checkpoint(parser); // r852

                                bool r852 = true;
                                r852 = r852 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r852, parser);
                                a851 = r852;
                            }

                            r848 |= a851;
                        } // end may a851

                        r848 = r848 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        CommitOrRollback(r848, parser);
                        a847 = r848;
                    }

                    if (!a847)
                    {
                        Checkpoint(parser); // r853

                        bool r853 = true;
                        r853 = r853 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ColumnAlias());
                        if (r853)
                        { // may a854
                            bool a854 = false;
                            {
                                Checkpoint(parser); // r855

                                bool r855 = true;
                                r855 = r855 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r855, parser);
                                a854 = r855;
                            }

                            r853 |= a854;
                        } // end may a854

                        r853 = r853 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ValueAssignmentOperator());
                        if (r853)
                        { // may a856
                            bool a856 = false;
                            {
                                Checkpoint(parser); // r857

                                bool r857 = true;
                                r857 = r857 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r857, parser);
                                a856 = r857;
                            }

                            r853 |= a856;
                        } // end may a856

                        r853 = r853 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        CommitOrRollback(r853, parser);
                        a847 = r853;
                    }

                    if (!a847)
                    {
                        Checkpoint(parser); // r858

                        bool r858 = true;
                        r858 = r858 && Match(parser, new Jhu.Graywulf.Sql.Parsing.StarColumnIdentifier());
                        CommitOrRollback(r858, parser);
                        a847 = r858;
                    }

                    if (!a847)
                    {
                        Checkpoint(parser); // r859

                        bool r859 = true;
                        r859 = r859 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        if (r859)
                        { // may a860
                            bool a860 = false;
                            {
                                Checkpoint(parser); // r861

                                bool r861 = true;
                                r861 = r861 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r861, parser);
                                a860 = r861;
                            }

                            r859 |= a860;
                        } // end may a860

                        if (r859)
                        { // may a862
                            bool a862 = false;
                            {
                                Checkpoint(parser); // r863

                                bool r863 = true;
                                r863 = r863 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"AS"));
                                if (r863)
                                { // may a864
                                    bool a864 = false;
                                    {
                                        Checkpoint(parser); // r865

                                        bool r865 = true;
                                        r865 = r865 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                        CommitOrRollback(r865, parser);
                                        a864 = r865;
                                    }

                                    r863 |= a864;
                                } // end may a864

                                CommitOrRollback(r863, parser);
                                a862 = r863;
                            }

                            r859 |= a862;
                        } // end may a862

                        r859 = r859 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ColumnAlias());
                        CommitOrRollback(r859, parser);
                        a847 = r859;
                    }

                    if (!a847)
                    {
                        Checkpoint(parser); // r866

                        bool r866 = true;
                        r866 = r866 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        CommitOrRollback(r866, parser);
                        a847 = r866;
                    }

                    r846 &= a847;

                } // end alternatives a847

                CommitOrRollback(r846, parser);
                res = r846;
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
                Checkpoint(parser); // r867

                bool r867 = true;
                r867 = r867 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ColumnExpression());
                if (r867)
                { // may a868
                    bool a868 = false;
                    {
                        Checkpoint(parser); // r869

                        bool r869 = true;
                        if (r869)
                        { // may a870
                            bool a870 = false;
                            {
                                Checkpoint(parser); // r871

                                bool r871 = true;
                                r871 = r871 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r871, parser);
                                a870 = r871;
                            }

                            r869 |= a870;
                        } // end may a870

                        r869 = r869 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r869)
                        { // may a872
                            bool a872 = false;
                            {
                                Checkpoint(parser); // r873

                                bool r873 = true;
                                r873 = r873 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r873, parser);
                                a872 = r873;
                            }

                            r869 |= a872;
                        } // end may a872

                        r869 = r869 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.SelectList());
                        CommitOrRollback(r869, parser);
                        a868 = r869;
                    }

                    r867 |= a868;
                } // end may a868

                CommitOrRollback(r867, parser);
                res = r867;
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
                Checkpoint(parser); // r874

                bool r874 = true;
                r874 = r874 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r874)
                { // may a875
                    bool a875 = false;
                    {
                        Checkpoint(parser); // r876

                        bool r876 = true;
                        if (r876)
                        { // may a877
                            bool a877 = false;
                            {
                                Checkpoint(parser); // r878

                                bool r878 = true;
                                r878 = r878 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r878, parser);
                                a877 = r878;
                            }

                            r876 |= a877;
                        } // end may a877

                        r876 = r876 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r876)
                        { // may a879
                            bool a879 = false;
                            {
                                Checkpoint(parser); // r880

                                bool r880 = true;
                                r880 = r880 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r880, parser);
                                a879 = r880;
                            }

                            r876 |= a879;
                        } // end may a879

                        r876 = r876 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.GroupByList());
                        CommitOrRollback(r876, parser);
                        a875 = r876;
                    }

                    r874 |= a875;
                } // end may a875

                CommitOrRollback(r874, parser);
                res = r874;
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
                Checkpoint(parser); // r881

                bool r881 = true;
                r881 = r881 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"GROUP"));
                r881 = r881 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r881 = r881 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"BY"));
                if (r881)
                { // alternatives a882 must
                    bool a882 = false;
                    if (!a882)
                    {
                        Checkpoint(parser); // r883

                        bool r883 = true;
                        r883 = r883 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        r883 = r883 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ALL"));
                        CommitOrRollback(r883, parser);
                        a882 = r883;
                    }

                    if (!a882)
                    {
                        Checkpoint(parser); // r884

                        bool r884 = true;
                        if (r884)
                        { // may a885
                            bool a885 = false;
                            {
                                Checkpoint(parser); // r886

                                bool r886 = true;
                                r886 = r886 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r886, parser);
                                a885 = r886;
                            }

                            r884 |= a885;
                        } // end may a885

                        r884 = r884 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.GroupByList());
                        CommitOrRollback(r884, parser);
                        a882 = r884;
                    }

                    r881 &= a882;

                } // end alternatives a882

                CommitOrRollback(r881, parser);
                res = r881;
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
                Checkpoint(parser); // r887

                bool r887 = true;
                r887 = r887 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                if (r887)
                { // may a888
                    bool a888 = false;
                    {
                        Checkpoint(parser); // r889

                        bool r889 = true;
                        if (r889)
                        { // may a890
                            bool a890 = false;
                            {
                                Checkpoint(parser); // r891

                                bool r891 = true;
                                r891 = r891 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r891, parser);
                                a890 = r891;
                            }

                            r889 |= a890;
                        } // end may a890

                        if (r889)
                        { // alternatives a892 must
                            bool a892 = false;
                            if (!a892)
                            {
                                Checkpoint(parser); // r893

                                bool r893 = true;
                                r893 = r893 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ASC"));
                                CommitOrRollback(r893, parser);
                                a892 = r893;
                            }

                            if (!a892)
                            {
                                Checkpoint(parser); // r894

                                bool r894 = true;
                                r894 = r894 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DESC"));
                                CommitOrRollback(r894, parser);
                                a892 = r894;
                            }

                            r889 &= a892;

                        } // end alternatives a892

                        CommitOrRollback(r889, parser);
                        a888 = r889;
                    }

                    r887 |= a888;
                } // end may a888

                CommitOrRollback(r887, parser);
                res = r887;
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
                Checkpoint(parser); // r895

                bool r895 = true;
                r895 = r895 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OrderByArgument());
                if (r895)
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

                        r897 = r897 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r897)
                        { // may a900
                            bool a900 = false;
                            {
                                Checkpoint(parser); // r901

                                bool r901 = true;
                                r901 = r901 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r901, parser);
                                a900 = r901;
                            }

                            r897 |= a900;
                        } // end may a900

                        r897 = r897 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OrderByArgumentList());
                        CommitOrRollback(r897, parser);
                        a896 = r897;
                    }

                    r895 |= a896;
                } // end may a896

                CommitOrRollback(r895, parser);
                res = r895;
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
                Checkpoint(parser); // r902

                bool r902 = true;
                r902 = r902 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"ORDER"));
                r902 = r902 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r902 = r902 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"BY"));
                if (r902)
                { // may a903
                    bool a903 = false;
                    {
                        Checkpoint(parser); // r904

                        bool r904 = true;
                        r904 = r904 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r904, parser);
                        a903 = r904;
                    }

                    r902 |= a903;
                } // end may a903

                r902 = r902 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OrderByArgumentList());
                CommitOrRollback(r902, parser);
                res = r902;
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
                Checkpoint(parser); // r905

                bool r905 = true;
                if (r905)
                { // may a906
                    bool a906 = false;
                    {
                        Checkpoint(parser); // r907

                        bool r907 = true;
                        r907 = r907 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommonTableExpression());
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

                        CommitOrRollback(r907, parser);
                        a906 = r907;
                    }

                    r905 |= a906;
                } // end may a906

                r905 = r905 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"INSERT"));
                if (r905)
                { // may a910
                    bool a910 = false;
                    {
                        Checkpoint(parser); // r911

                        bool r911 = true;
                        r911 = r911 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r911, parser);
                        a910 = r911;
                    }

                    r905 |= a910;
                } // end may a910

                if (r905)
                { // alternatives a912 must
                    bool a912 = false;
                    if (!a912)
                    {
                        Checkpoint(parser); // r913

                        bool r913 = true;
                        r913 = r913 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.IntoClause());
                        CommitOrRollback(r913, parser);
                        a912 = r913;
                    }

                    if (!a912)
                    {
                        Checkpoint(parser); // r914

                        bool r914 = true;
                        r914 = r914 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TargetTableSpecification());
                        CommitOrRollback(r914, parser);
                        a912 = r914;
                    }

                    r905 &= a912;

                } // end alternatives a912

                if (r905)
                { // may a915
                    bool a915 = false;
                    {
                        Checkpoint(parser); // r916

                        bool r916 = true;
                        if (r916)
                        { // may a917
                            bool a917 = false;
                            {
                                Checkpoint(parser); // r918

                                bool r918 = true;
                                r918 = r918 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r918, parser);
                                a917 = r918;
                            }

                            r916 |= a917;
                        } // end may a917

                        r916 = r916 && Match(parser, new Jhu.Graywulf.Sql.Parsing.InsertColumnList());
                        CommitOrRollback(r916, parser);
                        a915 = r916;
                    }

                    r905 |= a915;
                } // end may a915

                if (r905)
                { // may a919
                    bool a919 = false;
                    {
                        Checkpoint(parser); // r920

                        bool r920 = true;
                        r920 = r920 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r920, parser);
                        a919 = r920;
                    }

                    r905 |= a919;
                } // end may a919

                if (r905)
                { // alternatives a921 must
                    bool a921 = false;
                    if (!a921)
                    {
                        Checkpoint(parser); // r922

                        bool r922 = true;
                        r922 = r922 && Match(parser, new Jhu.Graywulf.Sql.Parsing.DefaultValues());
                        CommitOrRollback(r922, parser);
                        a921 = r922;
                    }

                    if (!a921)
                    {
                        Checkpoint(parser); // r923

                        bool r923 = true;
                        r923 = r923 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ValuesClause());
                        CommitOrRollback(r923, parser);
                        a921 = r923;
                    }

                    if (!a921)
                    {
                        Checkpoint(parser); // r924

                        bool r924 = true;
                        r924 = r924 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.QueryExpression());
                        if (r924)
                        { // may a925
                            bool a925 = false;
                            {
                                Checkpoint(parser); // r926

                                bool r926 = true;
                                if (r926)
                                { // may a927
                                    bool a927 = false;
                                    {
                                        Checkpoint(parser); // r928

                                        bool r928 = true;
                                        r928 = r928 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                        CommitOrRollback(r928, parser);
                                        a927 = r928;
                                    }

                                    r926 |= a927;
                                } // end may a927

                                r926 = r926 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OrderByClause());
                                CommitOrRollback(r926, parser);
                                a925 = r926;
                            }

                            r924 |= a925;
                        } // end may a925

                        if (r924)
                        { // may a929
                            bool a929 = false;
                            {
                                Checkpoint(parser); // r930

                                bool r930 = true;
                                if (r930)
                                { // may a931
                                    bool a931 = false;
                                    {
                                        Checkpoint(parser); // r932

                                        bool r932 = true;
                                        r932 = r932 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                        CommitOrRollback(r932, parser);
                                        a931 = r932;
                                    }

                                    r930 |= a931;
                                } // end may a931

                                r930 = r930 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.OptionClause());
                                CommitOrRollback(r930, parser);
                                a929 = r930;
                            }

                            r924 |= a929;
                        } // end may a929

                        CommitOrRollback(r924, parser);
                        a921 = r924;
                    }

                    r905 &= a921;

                } // end alternatives a921

                CommitOrRollback(r905, parser);
                res = r905;
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
                Checkpoint(parser); // r933

                bool r933 = true;
                r933 = r933 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                CommitOrRollback(r933, parser);
                res = r933;
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
                Checkpoint(parser); // r934

                bool r934 = true;
                r934 = r934 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.HintArgument());
                if (r934)
                { // may a935
                    bool a935 = false;
                    {
                        Checkpoint(parser); // r936

                        bool r936 = true;
                        if (r936)
                        { // may a937
                            bool a937 = false;
                            {
                                Checkpoint(parser); // r938

                                bool r938 = true;
                                r938 = r938 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r938, parser);
                                a937 = r938;
                            }

                            r936 |= a937;
                        } // end may a937

                        r936 = r936 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r936)
                        { // may a939
                            bool a939 = false;
                            {
                                Checkpoint(parser); // r940

                                bool r940 = true;
                                r940 = r940 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r940, parser);
                                a939 = r940;
                            }

                            r936 |= a939;
                        } // end may a939

                        r936 = r936 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.HintArgumentList());
                        CommitOrRollback(r936, parser);
                        a935 = r936;
                    }

                    r934 |= a935;
                } // end may a935

                CommitOrRollback(r934, parser);
                res = r934;
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
                Checkpoint(parser); // r941

                bool r941 = true;
                r941 = r941 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r941)
                { // may a942
                    bool a942 = false;
                    {
                        Checkpoint(parser); // r943

                        bool r943 = true;
                        r943 = r943 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r943, parser);
                        a942 = r943;
                    }

                    r941 |= a942;
                } // end may a942

                if (r941)
                { // may a944
                    bool a944 = false;
                    {
                        Checkpoint(parser); // r945

                        bool r945 = true;
                        r945 = r945 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.HintArgumentList());
                        CommitOrRollback(r945, parser);
                        a944 = r945;
                    }

                    r941 |= a944;
                } // end may a944

                if (r941)
                { // may a946
                    bool a946 = false;
                    {
                        Checkpoint(parser); // r947

                        bool r947 = true;
                        r947 = r947 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r947, parser);
                        a946 = r947;
                    }

                    r941 |= a946;
                } // end may a946

                r941 = r941 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r941, parser);
                res = r941;
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
                Checkpoint(parser); // r948

                bool r948 = true;
                if (r948)
                { // alternatives a949 must
                    bool a949 = false;
                    if (!a949)
                    {
                        Checkpoint(parser); // r950

                        bool r950 = true;
                        r950 = r950 && Match(parser, new Jhu.Graywulf.Sql.Parsing.HintName());
                        if (r950)
                        { // may a951
                            bool a951 = false;
                            {
                                Checkpoint(parser); // r952

                                bool r952 = true;
                                r952 = r952 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r952, parser);
                                a951 = r952;
                            }

                            r950 |= a951;
                        } // end may a951

                        r950 = r950 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.HintArguments());
                        CommitOrRollback(r950, parser);
                        a949 = r950;
                    }

                    if (!a949)
                    {
                        Checkpoint(parser); // r953

                        bool r953 = true;
                        r953 = r953 && Match(parser, new Jhu.Graywulf.Sql.Parsing.HintName());
                        CommitOrRollback(r953, parser);
                        a949 = r953;
                    }

                    r948 &= a949;

                } // end alternatives a949

                CommitOrRollback(r948, parser);
                res = r948;
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
                Checkpoint(parser); // r954

                bool r954 = true;
                r954 = r954 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableHint());
                if (r954)
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

                        r956 = r956 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r956)
                        { // may a959
                            bool a959 = false;
                            {
                                Checkpoint(parser); // r960

                                bool r960 = true;
                                r960 = r960 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r960, parser);
                                a959 = r960;
                            }

                            r956 |= a959;
                        } // end may a959

                        r956 = r956 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableHintList());
                        CommitOrRollback(r956, parser);
                        a955 = r956;
                    }

                    r954 |= a955;
                } // end may a955

                CommitOrRollback(r954, parser);
                res = r954;
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
                Checkpoint(parser); // r961

                bool r961 = true;
                r961 = r961 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"WITH"));
                if (r961)
                { // may a962
                    bool a962 = false;
                    {
                        Checkpoint(parser); // r963

                        bool r963 = true;
                        r963 = r963 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r963, parser);
                        a962 = r963;
                    }

                    r961 |= a962;
                } // end may a962

                r961 = r961 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r961)
                { // may a964
                    bool a964 = false;
                    {
                        Checkpoint(parser); // r965

                        bool r965 = true;
                        r965 = r965 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r965, parser);
                        a964 = r965;
                    }

                    r961 |= a964;
                } // end may a964

                if (r961)
                { // may a966
                    bool a966 = false;
                    {
                        Checkpoint(parser); // r967

                        bool r967 = true;
                        r967 = r967 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableHintList());
                        CommitOrRollback(r967, parser);
                        a966 = r967;
                    }

                    r961 |= a966;
                } // end may a966

                if (r961)
                { // may a968
                    bool a968 = false;
                    {
                        Checkpoint(parser); // r969

                        bool r969 = true;
                        r969 = r969 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r969, parser);
                        a968 = r969;
                    }

                    r961 |= a968;
                } // end may a968

                r961 = r961 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r961, parser);
                res = r961;
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
                Checkpoint(parser); // r970

                bool r970 = true;
                if (r970)
                { // alternatives a971 must
                    bool a971 = false;
                    if (!a971)
                    {
                        Checkpoint(parser); // r972

                        bool r972 = true;
                        r972 = r972 && Match(parser, new Jhu.Graywulf.Sql.Parsing.UserVariable());
                        CommitOrRollback(r972, parser);
                        a971 = r972;
                    }

                    if (!a971)
                    {
                        Checkpoint(parser); // r973

                        bool r973 = true;
                        r973 = r973 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableOrViewIdentifier());
                        CommitOrRollback(r973, parser);
                        a971 = r973;
                    }

                    r970 &= a971;

                } // end alternatives a971

                if (r970)
                { // may a974
                    bool a974 = false;
                    {
                        Checkpoint(parser); // r975

                        bool r975 = true;
                        if (r975)
                        { // may a976
                            bool a976 = false;
                            {
                                Checkpoint(parser); // r977

                                bool r977 = true;
                                r977 = r977 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r977, parser);
                                a976 = r977;
                            }

                            r975 |= a976;
                        } // end may a976

                        r975 = r975 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableHintClause());
                        CommitOrRollback(r975, parser);
                        a974 = r975;
                    }

                    r970 |= a974;
                } // end may a974

                CommitOrRollback(r970, parser);
                res = r970;
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
                Checkpoint(parser); // r978

                bool r978 = true;
                r978 = r978 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"INTO"));
                if (r978)
                { // may a979
                    bool a979 = false;
                    {
                        Checkpoint(parser); // r980

                        bool r980 = true;
                        r980 = r980 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r980, parser);
                        a979 = r980;
                    }

                    r978 |= a979;
                } // end may a979

                r978 = r978 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TargetTableSpecification());
                CommitOrRollback(r978, parser);
                res = r978;
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
                Checkpoint(parser); // r981

                bool r981 = true;
                r981 = r981 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableOrViewIdentifier());
                if (r981)
                { // may a982
                    bool a982 = false;
                    {
                        Checkpoint(parser); // r983

                        bool r983 = true;
                        if (r983)
                        { // may a984
                            bool a984 = false;
                            {
                                Checkpoint(parser); // r985

                                bool r985 = true;
                                r985 = r985 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r985, parser);
                                a984 = r985;
                            }

                            r983 |= a984;
                        } // end may a984

                        if (r983)
                        { // may a986
                            bool a986 = false;
                            {
                                Checkpoint(parser); // r987

                                bool r987 = true;
                                r987 = r987 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"AS"));
                                if (r987)
                                { // may a988
                                    bool a988 = false;
                                    {
                                        Checkpoint(parser); // r989

                                        bool r989 = true;
                                        r989 = r989 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                        CommitOrRollback(r989, parser);
                                        a988 = r989;
                                    }

                                    r987 |= a988;
                                } // end may a988

                                CommitOrRollback(r987, parser);
                                a986 = r987;
                            }

                            r983 |= a986;
                        } // end may a986

                        r983 = r983 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableAlias());
                        CommitOrRollback(r983, parser);
                        a982 = r983;
                    }

                    r981 |= a982;
                } // end may a982

                if (r981)
                { // may a990
                    bool a990 = false;
                    {
                        Checkpoint(parser); // r991

                        bool r991 = true;
                        if (r991)
                        { // may a992
                            bool a992 = false;
                            {
                                Checkpoint(parser); // r993

                                bool r993 = true;
                                r993 = r993 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r993, parser);
                                a992 = r993;
                            }

                            r991 |= a992;
                        } // end may a992

                        r991 = r991 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableSampleClause());
                        CommitOrRollback(r991, parser);
                        a990 = r991;
                    }

                    r981 |= a990;
                } // end may a990

                if (r981)
                { // may a994
                    bool a994 = false;
                    {
                        Checkpoint(parser); // r995

                        bool r995 = true;
                        if (r995)
                        { // may a996
                            bool a996 = false;
                            {
                                Checkpoint(parser); // r997

                                bool r997 = true;
                                r997 = r997 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r997, parser);
                                a996 = r997;
                            }

                            r995 |= a996;
                        } // end may a996

                        r995 = r995 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableHintClause());
                        CommitOrRollback(r995, parser);
                        a994 = r995;
                    }

                    r981 |= a994;
                } // end may a994

                CommitOrRollback(r981, parser);
                res = r981;
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
                Checkpoint(parser); // r998

                bool r998 = true;
                if (r998)
                { // alternatives a999 must
                    bool a999 = false;
                    if (!a999)
                    {
                        Checkpoint(parser); // r1000

                        bool r1000 = true;
                        r1000 = r1000 && Match(parser, new Jhu.Graywulf.Sql.Parsing.HintName());
                        if (r1000)
                        { // may a1001
                            bool a1001 = false;
                            {
                                Checkpoint(parser); // r1002

                                bool r1002 = true;
                                r1002 = r1002 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r1002, parser);
                                a1001 = r1002;
                            }

                            r1000 |= a1001;
                        } // end may a1001

                        r1000 = r1000 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.HintArguments());
                        CommitOrRollback(r1000, parser);
                        a999 = r1000;
                    }

                    if (!a999)
                    {
                        Checkpoint(parser); // r1003

                        bool r1003 = true;
                        r1003 = r1003 && Match(parser, new Jhu.Graywulf.Sql.Parsing.HintName());
                        if (r1003)
                        { // may a1004
                            bool a1004 = false;
                            {
                                Checkpoint(parser); // r1005

                                bool r1005 = true;
                                r1005 = r1005 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r1005, parser);
                                a1004 = r1005;
                            }

                            r1003 |= a1004;
                        } // end may a1004

                        r1003 = r1003 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Equals1());
                        if (r1003)
                        { // may a1006
                            bool a1006 = false;
                            {
                                Checkpoint(parser); // r1007

                                bool r1007 = true;
                                r1007 = r1007 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r1007, parser);
                                a1006 = r1007;
                            }

                            r1003 |= a1006;
                        } // end may a1006

                        r1003 = r1003 && Match(parser, new Jhu.Graywulf.Sql.Parsing.NumericConstant());
                        CommitOrRollback(r1003, parser);
                        a999 = r1003;
                    }

                    if (!a999)
                    {
                        Checkpoint(parser); // r1008

                        bool r1008 = true;
                        r1008 = r1008 && Match(parser, new Jhu.Graywulf.Sql.Parsing.HintName());
                        r1008 = r1008 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        r1008 = r1008 && Match(parser, new Jhu.Graywulf.Sql.Parsing.NumericConstant());
                        CommitOrRollback(r1008, parser);
                        a999 = r1008;
                    }

                    if (!a999)
                    {
                        Checkpoint(parser); // r1009

                        bool r1009 = true;
                        r1009 = r1009 && Match(parser, new Jhu.Graywulf.Sql.Parsing.QueryHintNameList());
                        CommitOrRollback(r1009, parser);
                        a999 = r1009;
                    }

                    if (!a999)
                    {
                        Checkpoint(parser); // r1010

                        bool r1010 = true;
                        r1010 = r1010 && Match(parser, new Jhu.Graywulf.Sql.Parsing.HintName());
                        CommitOrRollback(r1010, parser);
                        a999 = r1010;
                    }

                    r998 &= a999;

                } // end alternatives a999

                CommitOrRollback(r998, parser);
                res = r998;
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
                Checkpoint(parser); // r1011

                bool r1011 = true;
                r1011 = r1011 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.QueryHint());
                if (r1011)
                { // may a1012
                    bool a1012 = false;
                    {
                        Checkpoint(parser); // r1013

                        bool r1013 = true;
                        if (r1013)
                        { // may a1014
                            bool a1014 = false;
                            {
                                Checkpoint(parser); // r1015

                                bool r1015 = true;
                                r1015 = r1015 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r1015, parser);
                                a1014 = r1015;
                            }

                            r1013 |= a1014;
                        } // end may a1014

                        r1013 = r1013 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r1013)
                        { // may a1016
                            bool a1016 = false;
                            {
                                Checkpoint(parser); // r1017

                                bool r1017 = true;
                                r1017 = r1017 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r1017, parser);
                                a1016 = r1017;
                            }

                            r1013 |= a1016;
                        } // end may a1016

                        r1013 = r1013 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.QueryHintList());
                        CommitOrRollback(r1013, parser);
                        a1012 = r1013;
                    }

                    r1011 |= a1012;
                } // end may a1012

                CommitOrRollback(r1011, parser);
                res = r1011;
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
                Checkpoint(parser); // r1018

                bool r1018 = true;
                r1018 = r1018 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"OPTION"));
                if (r1018)
                { // may a1019
                    bool a1019 = false;
                    {
                        Checkpoint(parser); // r1020

                        bool r1020 = true;
                        r1020 = r1020 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1020, parser);
                        a1019 = r1020;
                    }

                    r1018 |= a1019;
                } // end may a1019

                r1018 = r1018 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r1018)
                { // may a1021
                    bool a1021 = false;
                    {
                        Checkpoint(parser); // r1022

                        bool r1022 = true;
                        r1022 = r1022 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1022, parser);
                        a1021 = r1022;
                    }

                    r1018 |= a1021;
                } // end may a1021

                r1018 = r1018 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.QueryHintList());
                if (r1018)
                { // may a1023
                    bool a1023 = false;
                    {
                        Checkpoint(parser); // r1024

                        bool r1024 = true;
                        r1024 = r1024 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1024, parser);
                        a1023 = r1024;
                    }

                    r1018 |= a1023;
                } // end may a1023

                r1018 = r1018 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r1018, parser);
                res = r1018;
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
                Checkpoint(parser); // r1025

                bool r1025 = true;
                if (r1025)
                { // alternatives a1026 must
                    bool a1026 = false;
                    if (!a1026)
                    {
                        Checkpoint(parser); // r1027

                        bool r1027 = true;
                        r1027 = r1027 && Match(parser, new Jhu.Graywulf.Sql.Parsing.DefaultValue());
                        CommitOrRollback(r1027, parser);
                        a1026 = r1027;
                    }

                    if (!a1026)
                    {
                        Checkpoint(parser); // r1028

                        bool r1028 = true;
                        r1028 = r1028 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        CommitOrRollback(r1028, parser);
                        a1026 = r1028;
                    }

                    r1025 &= a1026;

                } // end alternatives a1026

                if (r1025)
                { // may a1029
                    bool a1029 = false;
                    {
                        Checkpoint(parser); // r1030

                        bool r1030 = true;
                        if (r1030)
                        { // may a1031
                            bool a1031 = false;
                            {
                                Checkpoint(parser); // r1032

                                bool r1032 = true;
                                r1032 = r1032 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r1032, parser);
                                a1031 = r1032;
                            }

                            r1030 |= a1031;
                        } // end may a1031

                        r1030 = r1030 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r1030)
                        { // may a1033
                            bool a1033 = false;
                            {
                                Checkpoint(parser); // r1034

                                bool r1034 = true;
                                r1034 = r1034 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r1034, parser);
                                a1033 = r1034;
                            }

                            r1030 |= a1033;
                        } // end may a1033

                        r1030 = r1030 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ValueList());
                        CommitOrRollback(r1030, parser);
                        a1029 = r1030;
                    }

                    r1025 |= a1029;
                } // end may a1029

                CommitOrRollback(r1025, parser);
                res = r1025;
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
                Checkpoint(parser); // r1035

                bool r1035 = true;
                r1035 = r1035 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
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

                r1035 = r1035 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ValueList());
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

                r1035 = r1035 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r1035, parser);
                res = r1035;
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
                Checkpoint(parser); // r1040

                bool r1040 = true;
                r1040 = r1040 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ValueGroup());
                if (r1040)
                { // may a1041
                    bool a1041 = false;
                    {
                        Checkpoint(parser); // r1042

                        bool r1042 = true;
                        if (r1042)
                        { // may a1043
                            bool a1043 = false;
                            {
                                Checkpoint(parser); // r1044

                                bool r1044 = true;
                                r1044 = r1044 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r1044, parser);
                                a1043 = r1044;
                            }

                            r1042 |= a1043;
                        } // end may a1043

                        r1042 = r1042 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r1042)
                        { // may a1045
                            bool a1045 = false;
                            {
                                Checkpoint(parser); // r1046

                                bool r1046 = true;
                                r1046 = r1046 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r1046, parser);
                                a1045 = r1046;
                            }

                            r1042 |= a1045;
                        } // end may a1045

                        r1042 = r1042 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ValueGroupList());
                        CommitOrRollback(r1042, parser);
                        a1041 = r1042;
                    }

                    r1040 |= a1041;
                } // end may a1041

                CommitOrRollback(r1040, parser);
                res = r1040;
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
                Checkpoint(parser); // r1047

                bool r1047 = true;
                r1047 = r1047 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"VALUES"));
                if (r1047)
                { // may a1048
                    bool a1048 = false;
                    {
                        Checkpoint(parser); // r1049

                        bool r1049 = true;
                        r1049 = r1049 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1049, parser);
                        a1048 = r1049;
                    }

                    r1047 |= a1048;
                } // end may a1048

                r1047 = r1047 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ValueGroupList());
                CommitOrRollback(r1047, parser);
                res = r1047;
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
                Checkpoint(parser); // r1050

                bool r1050 = true;
                if (r1050)
                { // alternatives a1051 must
                    bool a1051 = false;
                    if (!a1051)
                    {
                        Checkpoint(parser); // r1052

                        bool r1052 = true;
                        r1052 = r1052 && Match(parser, new Jhu.Graywulf.Sql.Parsing.DefaultValue());
                        CommitOrRollback(r1052, parser);
                        a1051 = r1052;
                    }

                    if (!a1051)
                    {
                        Checkpoint(parser); // r1053

                        bool r1053 = true;
                        r1053 = r1053 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                        CommitOrRollback(r1053, parser);
                        a1051 = r1053;
                    }

                    r1050 &= a1051;

                } // end alternatives a1051

                CommitOrRollback(r1050, parser);
                res = r1050;
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
                Checkpoint(parser); // r1054

                bool r1054 = true;
                r1054 = r1054 && Match(parser, new Jhu.Graywulf.Sql.Parsing.UpdateSetColumnLeftHandSide());
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

                r1054 = r1054 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ValueAssignmentOperator());
                if (r1054)
                { // may a1057
                    bool a1057 = false;
                    {
                        Checkpoint(parser); // r1058

                        bool r1058 = true;
                        r1058 = r1058 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1058, parser);
                        a1057 = r1058;
                    }

                    r1054 |= a1057;
                } // end may a1057

                r1054 = r1054 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.UpdateSetColumnRightHandSide());
                CommitOrRollback(r1054, parser);
                res = r1054;
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
                Checkpoint(parser); // r1059

                bool r1059 = true;
                r1059 = r1059 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DEFAULT"));
                if (r1059)
                { // may a1060
                    bool a1060 = false;
                    {
                        Checkpoint(parser); // r1061

                        bool r1061 = true;
                        r1061 = r1061 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1061, parser);
                        a1060 = r1061;
                    }

                    r1059 |= a1060;
                } // end may a1060

                r1059 = r1059 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Expression());
                CommitOrRollback(r1059, parser);
                res = r1059;
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
                Checkpoint(parser); // r1062

                bool r1062 = true;
                if (r1062)
                { // alternatives a1063 must
                    bool a1063 = false;
                    if (!a1063)
                    {
                        Checkpoint(parser); // r1064

                        bool r1064 = true;
                        r1064 = r1064 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ColumnNullSpecification());
                        CommitOrRollback(r1064, parser);
                        a1063 = r1064;
                    }

                    if (!a1063)
                    {
                        Checkpoint(parser); // r1065

                        bool r1065 = true;
                        r1065 = r1065 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ColumnDefaultSpecification());
                        CommitOrRollback(r1065, parser);
                        a1063 = r1065;
                    }

                    if (!a1063)
                    {
                        Checkpoint(parser); // r1066

                        bool r1066 = true;
                        r1066 = r1066 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ColumnIdentitySpecification());
                        CommitOrRollback(r1066, parser);
                        a1063 = r1066;
                    }

                    if (!a1063)
                    {
                        Checkpoint(parser); // r1067

                        bool r1067 = true;
                        r1067 = r1067 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ColumnConstraint());
                        CommitOrRollback(r1067, parser);
                        a1063 = r1067;
                    }

                    if (!a1063)
                    {
                        Checkpoint(parser); // r1068

                        bool r1068 = true;
                        r1068 = r1068 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ColumnIndex());
                        CommitOrRollback(r1068, parser);
                        a1063 = r1068;
                    }

                    r1062 &= a1063;

                } // end alternatives a1063

                if (r1062)
                { // may a1069
                    bool a1069 = false;
                    {
                        Checkpoint(parser); // r1070

                        bool r1070 = true;
                        if (r1070)
                        { // may a1071
                            bool a1071 = false;
                            {
                                Checkpoint(parser); // r1072

                                bool r1072 = true;
                                r1072 = r1072 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r1072, parser);
                                a1071 = r1072;
                            }

                            r1070 |= a1071;
                        } // end may a1071

                        r1070 = r1070 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ColumnSpecificationList());
                        CommitOrRollback(r1070, parser);
                        a1069 = r1070;
                    }

                    r1062 |= a1069;
                } // end may a1069

                CommitOrRollback(r1062, parser);
                res = r1062;
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
                Checkpoint(parser); // r1073

                bool r1073 = true;
                r1073 = r1073 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ColumnName());
                if (r1073)
                { // may a1074
                    bool a1074 = false;
                    {
                        Checkpoint(parser); // r1075

                        bool r1075 = true;
                        r1075 = r1075 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1075, parser);
                        a1074 = r1075;
                    }

                    r1073 |= a1074;
                } // end may a1074

                r1073 = r1073 && Match(parser, new Jhu.Graywulf.Sql.Parsing.DataTypeSpecification());
                if (r1073)
                { // may a1076
                    bool a1076 = false;
                    {
                        Checkpoint(parser); // r1077

                        bool r1077 = true;
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

                        r1077 = r1077 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ColumnSpecificationList());
                        CommitOrRollback(r1077, parser);
                        a1076 = r1077;
                    }

                    r1073 |= a1076;
                } // end may a1076

                CommitOrRollback(r1073, parser);
                res = r1073;
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
                Checkpoint(parser); // r1080

                bool r1080 = true;
                if (r1080)
                { // alternatives a1081 must
                    bool a1081 = false;
                    if (!a1081)
                    {
                        Checkpoint(parser); // r1082

                        bool r1082 = true;
                        r1082 = r1082 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ColumnDefinition());
                        CommitOrRollback(r1082, parser);
                        a1081 = r1082;
                    }

                    if (!a1081)
                    {
                        Checkpoint(parser); // r1083

                        bool r1083 = true;
                        r1083 = r1083 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableConstraint());
                        CommitOrRollback(r1083, parser);
                        a1081 = r1083;
                    }

                    if (!a1081)
                    {
                        Checkpoint(parser); // r1084

                        bool r1084 = true;
                        r1084 = r1084 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableIndex());
                        CommitOrRollback(r1084, parser);
                        a1081 = r1084;
                    }

                    r1080 &= a1081;

                } // end alternatives a1081

                if (r1080)
                { // may a1085
                    bool a1085 = false;
                    {
                        Checkpoint(parser); // r1086

                        bool r1086 = true;
                        if (r1086)
                        { // may a1087
                            bool a1087 = false;
                            {
                                Checkpoint(parser); // r1088

                                bool r1088 = true;
                                r1088 = r1088 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r1088, parser);
                                a1087 = r1088;
                            }

                            r1086 |= a1087;
                        } // end may a1087

                        r1086 = r1086 && Match(parser, new Jhu.Graywulf.Sql.Parsing.Comma());
                        if (r1086)
                        { // may a1089
                            bool a1089 = false;
                            {
                                Checkpoint(parser); // r1090

                                bool r1090 = true;
                                r1090 = r1090 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r1090, parser);
                                a1089 = r1090;
                            }

                            r1086 |= a1089;
                        } // end may a1089

                        r1086 = r1086 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableDefinitionList());
                        CommitOrRollback(r1086, parser);
                        a1085 = r1086;
                    }

                    r1080 |= a1085;
                } // end may a1085

                CommitOrRollback(r1080, parser);
                res = r1080;
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
                Checkpoint(parser); // r1091

                bool r1091 = true;
                r1091 = r1091 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r1091)
                { // may a1092
                    bool a1092 = false;
                    {
                        Checkpoint(parser); // r1093

                        bool r1093 = true;
                        r1093 = r1093 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1093, parser);
                        a1092 = r1093;
                    }

                    r1091 |= a1092;
                } // end may a1092

                r1091 = r1091 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableDefinitionList());
                if (r1091)
                { // may a1094
                    bool a1094 = false;
                    {
                        Checkpoint(parser); // r1095

                        bool r1095 = true;
                        r1095 = r1095 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1095, parser);
                        a1094 = r1095;
                    }

                    r1091 |= a1094;
                } // end may a1094

                r1091 = r1091 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r1091, parser);
                res = r1091;
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
                Checkpoint(parser); // r1096

                bool r1096 = true;
                r1096 = r1096 && Match(parser, new Jhu.Graywulf.Sql.Parsing.UserVariable());
                if (r1096)
                { // may a1097
                    bool a1097 = false;
                    {
                        Checkpoint(parser); // r1098

                        bool r1098 = true;
                        r1098 = r1098 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1098, parser);
                        a1097 = r1098;
                    }

                    r1096 |= a1097;
                } // end may a1097

                if (r1096)
                { // may a1099
                    bool a1099 = false;
                    {
                        Checkpoint(parser); // r1100

                        bool r1100 = true;
                        r1100 = r1100 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"AS"));
                        r1100 = r1100 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1100, parser);
                        a1099 = r1100;
                    }

                    r1096 |= a1099;
                } // end may a1099

                r1096 = r1096 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TABLE"));
                if (r1096)
                { // may a1101
                    bool a1101 = false;
                    {
                        Checkpoint(parser); // r1102

                        bool r1102 = true;
                        r1102 = r1102 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1102, parser);
                        a1101 = r1102;
                    }

                    r1096 |= a1101;
                } // end may a1101

                r1096 = r1096 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableDefinition());
                CommitOrRollback(r1096, parser);
                res = r1096;
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
                Checkpoint(parser); // r1103

                bool r1103 = true;
                r1103 = r1103 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"DECLARE"));
                if (r1103)
                { // may a1104
                    bool a1104 = false;
                    {
                        Checkpoint(parser); // r1105

                        bool r1105 = true;
                        r1105 = r1105 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1105, parser);
                        a1104 = r1105;
                    }

                    r1103 |= a1104;
                } // end may a1104

                r1103 = r1103 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableDeclaration());
                CommitOrRollback(r1103, parser);
                res = r1103;
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
                Checkpoint(parser); // r1106

                bool r1106 = true;
                r1106 = r1106 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CREATE"));
                r1106 = r1106 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                r1106 = r1106 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"TABLE"));
                if (r1106)
                { // may a1107
                    bool a1107 = false;
                    {
                        Checkpoint(parser); // r1108

                        bool r1108 = true;
                        r1108 = r1108 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1108, parser);
                        a1107 = r1108;
                    }

                    r1106 |= a1107;
                } // end may a1107

                r1106 = r1106 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableOrViewIdentifier());
                if (r1106)
                { // may a1109
                    bool a1109 = false;
                    {
                        Checkpoint(parser); // r1110

                        bool r1110 = true;
                        r1110 = r1110 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1110, parser);
                        a1109 = r1110;
                    }

                    r1106 |= a1109;
                } // end may a1109

                r1106 = r1106 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.TableDefinition());
                CommitOrRollback(r1106, parser);
                res = r1106;
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
                Checkpoint(parser); // r1111

                bool r1111 = true;
                if (r1111)
                { // may a1112
                    bool a1112 = false;
                    {
                        Checkpoint(parser); // r1113

                        bool r1113 = true;
                        r1113 = r1113 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"CONSTRAINT"));
                        if (r1113)
                        { // may a1114
                            bool a1114 = false;
                            {
                                Checkpoint(parser); // r1115

                                bool r1115 = true;
                                r1115 = r1115 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r1115, parser);
                                a1114 = r1115;
                            }

                            r1113 |= a1114;
                        } // end may a1114

                        r1113 = r1113 && Match(parser, new Jhu.Graywulf.Sql.Parsing.ConstraintName());
                        if (r1113)
                        { // may a1116
                            bool a1116 = false;
                            {
                                Checkpoint(parser); // r1117

                                bool r1117 = true;
                                r1117 = r1117 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                                CommitOrRollback(r1117, parser);
                                a1116 = r1117;
                            }

                            r1113 |= a1116;
                        } // end may a1116

                        CommitOrRollback(r1113, parser);
                        a1112 = r1113;
                    }

                    r1111 |= a1112;
                } // end may a1112

                if (r1111)
                { // alternatives a1118 must
                    bool a1118 = false;
                    if (!a1118)
                    {
                        Checkpoint(parser); // r1119

                        bool r1119 = true;
                        r1119 = r1119 && Match(parser, new Jhu.Graywulf.Sql.Parsing.IndexTypeSpecification());
                        CommitOrRollback(r1119, parser);
                        a1118 = r1119;
                    }

                    if (!a1118)
                    {
                        Checkpoint(parser); // r1120

                        bool r1120 = true;
                        r1120 = r1120 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.ColumnDefaultSpecification());
                        CommitOrRollback(r1120, parser);
                        a1118 = r1120;
                    }

                    r1111 &= a1118;

                } // end alternatives a1118

                CommitOrRollback(r1111, parser);
                res = r1111;
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
                Checkpoint(parser); // r1121

                bool r1121 = true;
                r1121 = r1121 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.Subquery());
                if (r1121)
                { // may a1122
                    bool a1122 = false;
                    {
                        Checkpoint(parser); // r1123

                        bool r1123 = true;
                        r1123 = r1123 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1123, parser);
                        a1122 = r1123;
                    }

                    r1121 |= a1122;
                } // end may a1122

                if (r1121)
                { // may a1124
                    bool a1124 = false;
                    {
                        Checkpoint(parser); // r1125

                        bool r1125 = true;
                        r1125 = r1125 && Match(parser, new Jhu.Graywulf.Parsing.Literal(@"AS"));
                        r1125 = r1125 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1125, parser);
                        a1124 = r1125;
                    }

                    r1121 |= a1124;
                } // end may a1124

                r1121 = r1121 && Match(parser, new Jhu.Graywulf.Sql.Parsing.TableAlias());
                CommitOrRollback(r1121, parser);
                res = r1121;
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
                Checkpoint(parser); // r1126

                bool r1126 = true;
                r1126 = r1126 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketOpen());
                if (r1126)
                { // may a1127
                    bool a1127 = false;
                    {
                        Checkpoint(parser); // r1128

                        bool r1128 = true;
                        r1128 = r1128 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1128, parser);
                        a1127 = r1128;
                    }

                    r1126 |= a1127;
                } // end may a1127

                r1126 = r1126 && Match(parser, new Jhu.SkyQuery.Sql.Parsing.QueryExpression());
                if (r1126)
                { // may a1129
                    bool a1129 = false;
                    {
                        Checkpoint(parser); // r1130

                        bool r1130 = true;
                        r1130 = r1130 && Match(parser, new Jhu.Graywulf.Sql.Parsing.CommentOrWhitespace());
                        CommitOrRollback(r1130, parser);
                        a1129 = r1130;
                    }

                    r1126 |= a1129;
                } // end may a1129

                r1126 = r1126 && Match(parser, new Jhu.Graywulf.Sql.Parsing.BracketClose());
                CommitOrRollback(r1126, parser);
                res = r1126;
            }



            return res;
        }
    }


}