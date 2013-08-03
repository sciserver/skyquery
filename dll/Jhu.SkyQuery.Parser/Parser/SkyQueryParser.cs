using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Jhu.SkyQuery.Parser
{
    public partial class SkyQueryParser : Jhu.Graywulf.SqlParser.SqlParser
    {
        private static HashSet<string> keywords = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {
            "ALL", "AND", "ANY", "APPLY", "AS", "ASC", "BAYESFACTOR", "BETWEEN", "BY", 
            "CASE", "CROSS", "DESC", "DISTANCE", "DISTINCT", "ELSE", "END", "ESCAPE", "EXCEPT", "EXIST", 
            "EXISTS", "FASTFIRSTROW", "FROM", "FULL", "GROUP", "HASH", "HAVING", "HOLDLOCK", "IN", "INDEX", 
            "INNER", "INTERSECT", "INTO", "IS", "JOIN", "LEFT", "LIKE", "LIMIT", "LOOP", "MAY", 
            "MERGE", "Must", "NOEXPAND", "NOLOCK", "NOT", "NOWAIT", "NULL", "ON", "OR", "ORDER", 
            "OUTER", "PAGLOCK", "PARTITION", "PERCENT", "READCOMMITTED", "READCOMMITTEDLOCK", "READPAST", "READUNCOMMITTED", "REMOTE", "REPEATABLE", 
            "REPEATABLEREAD", "RIGHT", "ROWLOCK", "ROWS", "SELECT", "SERIALIZABLE", "SOME", "SYSTEM", "TABLESAMPLE", "TABLOCK", 
            "TABLOCKX", "THEN", "TIES", "TOP", "UNION", "UPDLOCK", "WHEN", "WHERE", "WITH", "XLOCK", 
            "XMATCH", 

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

        public override Jhu.Graywulf.ParserLib.Token Execute(string code)
        {
            return Execute(new SelectStatement(), code);
        }
    }





    public partial class QuerySpecification : Jhu.Graywulf.SqlParser.QuerySpecification
    {
        public override bool Match(Jhu.Graywulf.ParserLib.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r1

                bool r1 = true;
                r1 = r1 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"SELECT"));
                if (r1)
                { // may a2
                    bool a2 = false;
                    {
                        Checkpoint(parser); // r3

                        bool r3 = true;
                        r3 = r3 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        if (r3)
                        { // alternatives a4 must
                            bool a4 = false;
                            if (!a4)
                            {
                                Checkpoint(parser); // r5

                                bool r5 = true;
                                r5 = r5 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"ALL"));
                                CommitOrRollback(r5, parser);
                                a4 = r5;
                            }

                            if (!a4)
                            {
                                Checkpoint(parser); // r6

                                bool r6 = true;
                                r6 = r6 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r6, parser);
                                a4 = r6;
                            }

                            if (!a4)
                            {
                                Checkpoint(parser); // r7

                                bool r7 = true;
                                r7 = r7 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"DISTINCT"));
                                CommitOrRollback(r7, parser);
                                a4 = r7;
                            }

                            r3 &= a4;

                        } // end alternatives a4

                        CommitOrRollback(r3, parser);
                        a2 = r3;
                    }

                    r1 |= a2;
                } // end may a2

                if (r1)
                { // may a8
                    bool a8 = false;
                    {
                        Checkpoint(parser); // r9

                        bool r9 = true;
                        r9 = r9 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        r9 = r9 && Match(parser, new Jhu.Graywulf.SqlParser.TopExpression());
                        CommitOrRollback(r9, parser);
                        a8 = r9;
                    }

                    r1 |= a8;
                } // end may a8

                r1 = r1 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                r1 = r1 && Match(parser, new Jhu.Graywulf.SqlParser.SelectList());
                if (r1)
                { // may a10
                    bool a10 = false;
                    {
                        Checkpoint(parser); // r11

                        bool r11 = true;
                        r11 = r11 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        r11 = r11 && Match(parser, new Jhu.Graywulf.SqlParser.IntoClause());
                        CommitOrRollback(r11, parser);
                        a10 = r11;
                    }

                    r1 |= a10;
                } // end may a10

                if (r1)
                { // may a12
                    bool a12 = false;
                    {
                        Checkpoint(parser); // r13

                        bool r13 = true;
                        r13 = r13 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        r13 = r13 && Match(parser, new Jhu.SkyQuery.Parser.FromClause());
                        CommitOrRollback(r13, parser);
                        a12 = r13;
                    }

                    r1 |= a12;
                } // end may a12

                if (r1)
                { // may a14
                    bool a14 = false;
                    {
                        Checkpoint(parser); // r15

                        bool r15 = true;
                        r15 = r15 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        r15 = r15 && Match(parser, new Jhu.SkyQuery.Parser.XMatchClause());
                        CommitOrRollback(r15, parser);
                        a14 = r15;
                    }

                    r1 |= a14;
                } // end may a14

                if (r1)
                { // may a16
                    bool a16 = false;
                    {
                        Checkpoint(parser); // r17

                        bool r17 = true;
                        r17 = r17 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        r17 = r17 && Match(parser, new Jhu.SkyQuery.Parser.WhereClause());
                        CommitOrRollback(r17, parser);
                        a16 = r17;
                    }

                    r1 |= a16;
                } // end may a16

                if (r1)
                { // may a18
                    bool a18 = false;
                    {
                        Checkpoint(parser); // r19

                        bool r19 = true;
                        r19 = r19 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        r19 = r19 && Match(parser, new Jhu.Graywulf.SqlParser.GroupByClause());
                        CommitOrRollback(r19, parser);
                        a18 = r19;
                    }

                    r1 |= a18;
                } // end may a18

                if (r1)
                { // may a20
                    bool a20 = false;
                    {
                        Checkpoint(parser); // r21

                        bool r21 = true;
                        r21 = r21 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        r21 = r21 && Match(parser, new Jhu.SkyQuery.Parser.HavingClause());
                        CommitOrRollback(r21, parser);
                        a20 = r21;
                    }

                    r1 |= a20;
                } // end may a20

                CommitOrRollback(r1, parser);
                res = r1;
            }

            return res;

        }
    }

    public partial class XMatchClause : Jhu.Graywulf.ParserLib.Node
    {
        public override bool Match(Jhu.Graywulf.ParserLib.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r22

                bool r22 = true;
                r22 = r22 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"XMATCH"));
                r22 = r22 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                r22 = r22 && Match(parser, new Jhu.SkyQuery.Parser.XMatchAlgorithm());
                if (r22)
                { // may a23
                    bool a23 = false;
                    {
                        Checkpoint(parser); // r24

                        bool r24 = true;
                        r24 = r24 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        r24 = r24 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"AS"));
                        CommitOrRollback(r24, parser);
                        a23 = r24;
                    }

                    r22 |= a23;
                } // end may a23

                r22 = r22 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                r22 = r22 && Match(parser, new Jhu.Graywulf.SqlParser.TableAlias());
                r22 = r22 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                r22 = r22 && Match(parser, new Jhu.SkyQuery.Parser.XMatchTableList());
                r22 = r22 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                r22 = r22 && Match(parser, new Jhu.SkyQuery.Parser.XMatchHavingClause());
                CommitOrRollback(r22, parser);
                res = r22;
            }

            return res;

        }
    }

    public partial class XMatchAlgorithm : Jhu.Graywulf.ParserLib.Node
    {
        public override bool Match(Jhu.Graywulf.ParserLib.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r25

                bool r25 = true;
                if (r25)
                { // alternatives a26 must
                    bool a26 = false;
                    if (!a26)
                    {
                        Checkpoint(parser); // r27

                        bool r27 = true;
                        r27 = r27 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"BAYESFACTOR"));
                        CommitOrRollback(r27, parser);
                        a26 = r27;
                    }

                    if (!a26)
                    {
                        Checkpoint(parser); // r28

                        bool r28 = true;
                        r28 = r28 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"DISTANCE"));
                        CommitOrRollback(r28, parser);
                        a26 = r28;
                    }

                    r25 &= a26;

                } // end alternatives a26

                CommitOrRollback(r25, parser);
                res = r25;
            }

            return res;

        }
    }

    public partial class XMatchTableList : Jhu.Graywulf.ParserLib.Node
    {
        public override bool Match(Jhu.Graywulf.ParserLib.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r29

                bool r29 = true;
                r29 = r29 && Match(parser, new Jhu.SkyQuery.Parser.XMatchTableSpecification());
                if (r29)
                { // may a30
                    bool a30 = false;
                    {
                        Checkpoint(parser); // r31

                        bool r31 = true;
                        r31 = r31 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        r31 = r31 && Match(parser, new Jhu.SkyQuery.Parser.XMatchTableList());
                        CommitOrRollback(r31, parser);
                        a30 = r31;
                    }

                    r29 |= a30;
                } // end may a30

                CommitOrRollback(r29, parser);
                res = r29;
            }

            return res;

        }
    }

    public partial class XMatchTableSpecification : Jhu.Graywulf.ParserLib.Node
    {
        public override bool Match(Jhu.Graywulf.ParserLib.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r32

                bool r32 = true;
                r32 = r32 && Match(parser, new Jhu.SkyQuery.Parser.XMatchTableInclusion());
                r32 = r32 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                r32 = r32 && Match(parser, new Jhu.Graywulf.SqlParser.TableOrViewName());
                r32 = r32 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                r32 = r32 && Match(parser, new Jhu.SkyQuery.Parser.XMatchArgumentList());
                CommitOrRollback(r32, parser);
                res = r32;
            }

            return res;

        }
    }

    public partial class XMatchTableInclusion : Jhu.Graywulf.ParserLib.Node
    {
        public override bool Match(Jhu.Graywulf.ParserLib.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r33

                bool r33 = true;
                if (r33)
                { // may a34
                    bool a34 = false;
                    {
                        Checkpoint(parser); // r35

                        bool r35 = true;
                        if (r35)
                        { // alternatives a36 must
                            bool a36 = false;
                            if (!a36)
                            {
                                Checkpoint(parser); // r37

                                bool r37 = true;
                                r37 = r37 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"Must"));
                                CommitOrRollback(r37, parser);
                                a36 = r37;
                            }

                            if (!a36)
                            {
                                Checkpoint(parser); // r38

                                bool r38 = true;
                                r38 = r38 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"MAY"));
                                CommitOrRollback(r38, parser);
                                a36 = r38;
                            }

                            if (!a36)
                            {
                                Checkpoint(parser); // r39

                                bool r39 = true;
                                r39 = r39 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"NOT"));
                                CommitOrRollback(r39, parser);
                                a36 = r39;
                            }

                            r35 &= a36;

                        } // end alternatives a36

                        r35 = r35 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        CommitOrRollback(r35, parser);
                        a34 = r35;
                    }

                    r33 |= a34;
                } // end may a34

                r33 = r33 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"EXIST"));
                CommitOrRollback(r33, parser);
                res = r33;
            }

            return res;

        }
    }

    public partial class XMatchArgumentList : Jhu.Graywulf.ParserLib.Node
    {
        public override bool Match(Jhu.Graywulf.ParserLib.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r40

                bool r40 = true;
                r40 = r40 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"ON"));
                r40 = r40 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                r40 = r40 && Match(parser, new Jhu.SkyQuery.Parser.AdqlPoint());
                if (r40)
                { // may a41
                    bool a41 = false;
                    {
                        Checkpoint(parser); // r42

                        bool r42 = true;
                        if (r42)
                        { // may a43
                            bool a43 = false;
                            {
                                Checkpoint(parser); // r44

                                bool r44 = true;
                                r44 = r44 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r44, parser);
                                a43 = r44;
                            }

                            r42 |= a43;
                        } // end may a43

                        r42 = r42 && Match(parser, new Jhu.Graywulf.SqlParser.Comma());
                        if (r42)
                        { // may a45
                            bool a45 = false;
                            {
                                Checkpoint(parser); // r46

                                bool r46 = true;
                                r46 = r46 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r46, parser);
                                a45 = r46;
                            }

                            r42 |= a45;
                        } // end may a45

                        r42 = r42 && Match(parser, new Jhu.Graywulf.SqlParser.ArgumentList());
                        CommitOrRollback(r42, parser);
                        a41 = r42;
                    }

                    r40 |= a41;
                } // end may a41

                CommitOrRollback(r40, parser);
                res = r40;
            }

            return res;

        }
    }

    public partial class XMatchHavingClause : Jhu.Graywulf.ParserLib.Node
    {
        public override bool Match(Jhu.Graywulf.ParserLib.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r47

                bool r47 = true;
                r47 = r47 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"HAVING"));
                r47 = r47 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                r47 = r47 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"LIMIT"));
                r47 = r47 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                r47 = r47 && Match(parser, new Jhu.Graywulf.SqlParser.Number());
                CommitOrRollback(r47, parser);
                res = r47;
            }

            return res;

        }
    }

    public partial class AdqlPoint : Jhu.Graywulf.ParserLib.Node
    {
        public override bool Match(Jhu.Graywulf.ParserLib.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r48

                bool r48 = true;
                r48 = r48 && Match(parser, new Jhu.Graywulf.SqlParser.FunctionCall());
                CommitOrRollback(r48, parser);
                res = r48;
            }

            return res;

        }
    }

    public partial class QueryExpression : Jhu.Graywulf.SqlParser.QueryExpression
    {
        public override bool Match(Jhu.Graywulf.ParserLib.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r49

                bool r49 = true;
                if (r49)
                { // alternatives a50 must
                    bool a50 = false;
                    if (!a50)
                    {
                        Checkpoint(parser); // r51

                        bool r51 = true;
                        r51 = r51 && Match(parser, new Jhu.Graywulf.SqlParser.BracketOpen());
                        if (r51)
                        { // may a52
                            bool a52 = false;
                            {
                                Checkpoint(parser); // r53

                                bool r53 = true;
                                r53 = r53 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r53, parser);
                                a52 = r53;
                            }

                            r51 |= a52;
                        } // end may a52

                        r51 = r51 && Match(parser, new Jhu.SkyQuery.Parser.QueryExpression());
                        if (r51)
                        { // may a54
                            bool a54 = false;
                            {
                                Checkpoint(parser); // r55

                                bool r55 = true;
                                r55 = r55 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r55, parser);
                                a54 = r55;
                            }

                            r51 |= a54;
                        } // end may a54

                        r51 = r51 && Match(parser, new Jhu.Graywulf.SqlParser.BracketClose());
                        CommitOrRollback(r51, parser);
                        a50 = r51;
                    }

                    if (!a50)
                    {
                        Checkpoint(parser); // r56

                        bool r56 = true;
                        r56 = r56 && Match(parser, new Jhu.SkyQuery.Parser.QuerySpecification());
                        CommitOrRollback(r56, parser);
                        a50 = r56;
                    }

                    r49 &= a50;

                } // end alternatives a50

                if (r49)
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
                                r60 = r60 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r60, parser);
                                a59 = r60;
                            }

                            r58 |= a59;
                        } // end may a59

                        r58 = r58 && Match(parser, new Jhu.Graywulf.SqlParser.QueryOperator());
                        if (r58)
                        { // may a61
                            bool a61 = false;
                            {
                                Checkpoint(parser); // r62

                                bool r62 = true;
                                r62 = r62 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r62, parser);
                                a61 = r62;
                            }

                            r58 |= a61;
                        } // end may a61

                        r58 = r58 && Match(parser, new Jhu.SkyQuery.Parser.QueryExpression());
                        CommitOrRollback(r58, parser);
                        a57 = r58;
                    }

                    r49 |= a57;
                } // end may a57

                CommitOrRollback(r49, parser);
                res = r49;
            }

            return res;

        }
    }

    public partial class SelectStatement : Jhu.Graywulf.SqlParser.SelectStatement
    {
        public override bool Match(Jhu.Graywulf.ParserLib.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r63

                bool r63 = true;
                if (r63)
                { // may a64
                    bool a64 = false;
                    {
                        Checkpoint(parser); // r65

                        bool r65 = true;
                        r65 = r65 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        CommitOrRollback(r65, parser);
                        a64 = r65;
                    }

                    r63 |= a64;
                } // end may a64

                r63 = r63 && Match(parser, new Jhu.SkyQuery.Parser.QueryExpression());
                if (r63)
                { // may a66
                    bool a66 = false;
                    {
                        Checkpoint(parser); // r67

                        bool r67 = true;
                        r67 = r67 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        CommitOrRollback(r67, parser);
                        a66 = r67;
                    }

                    r63 |= a66;
                } // end may a66

                if (r63)
                { // may a68
                    bool a68 = false;
                    {
                        Checkpoint(parser); // r69

                        bool r69 = true;
                        r69 = r69 && Match(parser, new Jhu.Graywulf.SqlParser.OrderByClause());
                        CommitOrRollback(r69, parser);
                        a68 = r69;
                    }

                    r63 |= a68;
                } // end may a68

                if (r63)
                { // may a70
                    bool a70 = false;
                    {
                        Checkpoint(parser); // r71

                        bool r71 = true;
                        r71 = r71 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        CommitOrRollback(r71, parser);
                        a70 = r71;
                    }

                    r63 |= a70;
                } // end may a70

                CommitOrRollback(r63, parser);
                res = r63;
            }

            return res;

        }
    }

    public partial class Subquery : Jhu.Graywulf.SqlParser.Subquery
    {
        public override bool Match(Jhu.Graywulf.ParserLib.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r72

                bool r72 = true;
                r72 = r72 && Match(parser, new Jhu.Graywulf.SqlParser.BracketOpen());
                if (r72)
                { // may a73
                    bool a73 = false;
                    {
                        Checkpoint(parser); // r74

                        bool r74 = true;
                        r74 = r74 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        CommitOrRollback(r74, parser);
                        a73 = r74;
                    }

                    r72 |= a73;
                } // end may a73

                r72 = r72 && Match(parser, new Jhu.SkyQuery.Parser.SelectStatement());
                if (r72)
                { // may a75
                    bool a75 = false;
                    {
                        Checkpoint(parser); // r76

                        bool r76 = true;
                        r76 = r76 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        CommitOrRollback(r76, parser);
                        a75 = r76;
                    }

                    r72 |= a75;
                } // end may a75

                r72 = r72 && Match(parser, new Jhu.Graywulf.SqlParser.BracketClose());
                CommitOrRollback(r72, parser);
                res = r72;
            }

            return res;

        }
    }

    public partial class SubqueryTableSource : Jhu.Graywulf.SqlParser.SubqueryTableSource
    {
        public override bool Match(Jhu.Graywulf.ParserLib.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r77

                bool r77 = true;
                r77 = r77 && Match(parser, new Jhu.SkyQuery.Parser.Subquery());
                if (r77)
                { // may a78
                    bool a78 = false;
                    {
                        Checkpoint(parser); // r79

                        bool r79 = true;
                        r79 = r79 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        CommitOrRollback(r79, parser);
                        a78 = r79;
                    }

                    r77 |= a78;
                } // end may a78

                if (r77)
                { // may a80
                    bool a80 = false;
                    {
                        Checkpoint(parser); // r81

                        bool r81 = true;
                        r81 = r81 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"AS"));
                        r81 = r81 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        CommitOrRollback(r81, parser);
                        a80 = r81;
                    }

                    r77 |= a80;
                } // end may a80

                r77 = r77 && Match(parser, new Jhu.Graywulf.SqlParser.TableAlias());
                CommitOrRollback(r77, parser);
                res = r77;
            }

            return res;

        }
    }

    public partial class TableSource : Jhu.Graywulf.SqlParser.TableSource
    {
        public override bool Match(Jhu.Graywulf.ParserLib.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r82

                bool r82 = true;
                if (r82)
                { // alternatives a83 must
                    bool a83 = false;
                    if (!a83)
                    {
                        Checkpoint(parser); // r84

                        bool r84 = true;
                        r84 = r84 && Match(parser, new Jhu.Graywulf.SqlParser.FunctionTableSource());
                        CommitOrRollback(r84, parser);
                        a83 = r84;
                    }

                    if (!a83)
                    {
                        Checkpoint(parser); // r85

                        bool r85 = true;
                        r85 = r85 && Match(parser, new Jhu.Graywulf.SqlParser.SimpleTableSource());
                        CommitOrRollback(r85, parser);
                        a83 = r85;
                    }

                    if (!a83)
                    {
                        Checkpoint(parser); // r86

                        bool r86 = true;
                        r86 = r86 && Match(parser, new Jhu.Graywulf.SqlParser.VariableTableSource());
                        CommitOrRollback(r86, parser);
                        a83 = r86;
                    }

                    if (!a83)
                    {
                        Checkpoint(parser); // r87

                        bool r87 = true;
                        r87 = r87 && Match(parser, new Jhu.SkyQuery.Parser.SubqueryTableSource());
                        CommitOrRollback(r87, parser);
                        a83 = r87;
                    }

                    r82 &= a83;

                } // end alternatives a83

                CommitOrRollback(r82, parser);
                res = r82;
            }

            return res;

        }
    }

    public partial class TableSourceExpression : Jhu.Graywulf.SqlParser.TableSourceExpression
    {
        public override bool Match(Jhu.Graywulf.ParserLib.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r88

                bool r88 = true;
                r88 = r88 && Match(parser, new Jhu.SkyQuery.Parser.TableSource());
                if (r88)
                { // may a89
                    bool a89 = false;
                    {
                        Checkpoint(parser); // r90

                        bool r90 = true;
                        if (r90)
                        { // may a91
                            bool a91 = false;
                            {
                                Checkpoint(parser); // r92

                                bool r92 = true;
                                r92 = r92 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r92, parser);
                                a91 = r92;
                            }

                            r90 |= a91;
                        } // end may a91

                        r90 = r90 && Match(parser, new Jhu.SkyQuery.Parser.JoinedTable());
                        CommitOrRollback(r90, parser);
                        a89 = r90;
                    }

                    r88 |= a89;
                } // end may a89

                CommitOrRollback(r88, parser);
                res = r88;
            }

            return res;

        }
    }

    public partial class FromClause : Jhu.Graywulf.SqlParser.FromClause
    {
        public override bool Match(Jhu.Graywulf.ParserLib.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r93

                bool r93 = true;
                r93 = r93 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"FROM"));
                if (r93)
                { // may a94
                    bool a94 = false;
                    {
                        Checkpoint(parser); // r95

                        bool r95 = true;
                        r95 = r95 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        CommitOrRollback(r95, parser);
                        a94 = r95;
                    }

                    r93 |= a94;
                } // end may a94

                r93 = r93 && Match(parser, new Jhu.SkyQuery.Parser.TableSourceExpression());
                CommitOrRollback(r93, parser);
                res = r93;
            }

            return res;

        }
    }

    public partial class JoinedTable : Jhu.Graywulf.SqlParser.JoinedTable
    {
        public override bool Match(Jhu.Graywulf.ParserLib.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r96

                bool r96 = true;
                if (r96)
                { // alternatives a97 must
                    bool a97 = false;
                    if (!a97)
                    {
                        Checkpoint(parser); // r98

                        bool r98 = true;
                        r98 = r98 && Match(parser, new Jhu.Graywulf.SqlParser.JoinType());
                        if (r98)
                        { // may a99
                            bool a99 = false;
                            {
                                Checkpoint(parser); // r100

                                bool r100 = true;
                                r100 = r100 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r100, parser);
                                a99 = r100;
                            }

                            r98 |= a99;
                        } // end may a99

                        r98 = r98 && Match(parser, new Jhu.SkyQuery.Parser.TableSource());
                        if (r98)
                        { // may a101
                            bool a101 = false;
                            {
                                Checkpoint(parser); // r102

                                bool r102 = true;
                                r102 = r102 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r102, parser);
                                a101 = r102;
                            }

                            r98 |= a101;
                        } // end may a101

                        r98 = r98 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"ON"));
                        if (r98)
                        { // may a103
                            bool a103 = false;
                            {
                                Checkpoint(parser); // r104

                                bool r104 = true;
                                r104 = r104 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r104, parser);
                                a103 = r104;
                            }

                            r98 |= a103;
                        } // end may a103

                        r98 = r98 && Match(parser, new Jhu.SkyQuery.Parser.SearchCondition());
                        CommitOrRollback(r98, parser);
                        a97 = r98;
                    }

                    if (!a97)
                    {
                        Checkpoint(parser); // r105

                        bool r105 = true;
                        r105 = r105 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"CROSS"));
                        r105 = r105 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        r105 = r105 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"JOIN"));
                        if (r105)
                        { // may a106
                            bool a106 = false;
                            {
                                Checkpoint(parser); // r107

                                bool r107 = true;
                                r107 = r107 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r107, parser);
                                a106 = r107;
                            }

                            r105 |= a106;
                        } // end may a106

                        r105 = r105 && Match(parser, new Jhu.SkyQuery.Parser.TableSource());
                        CommitOrRollback(r105, parser);
                        a97 = r105;
                    }

                    if (!a97)
                    {
                        Checkpoint(parser); // r108

                        bool r108 = true;
                        r108 = r108 && Match(parser, new Jhu.Graywulf.SqlParser.Comma());
                        if (r108)
                        { // may a109
                            bool a109 = false;
                            {
                                Checkpoint(parser); // r110

                                bool r110 = true;
                                r110 = r110 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r110, parser);
                                a109 = r110;
                            }

                            r108 |= a109;
                        } // end may a109

                        r108 = r108 && Match(parser, new Jhu.SkyQuery.Parser.TableSource());
                        CommitOrRollback(r108, parser);
                        a97 = r108;
                    }

                    if (!a97)
                    {
                        Checkpoint(parser); // r111

                        bool r111 = true;
                        if (r111)
                        { // alternatives a112 must
                            bool a112 = false;
                            if (!a112)
                            {
                                Checkpoint(parser); // r113

                                bool r113 = true;
                                r113 = r113 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"CROSS"));
                                CommitOrRollback(r113, parser);
                                a112 = r113;
                            }

                            if (!a112)
                            {
                                Checkpoint(parser); // r114

                                bool r114 = true;
                                r114 = r114 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"OUTER"));
                                CommitOrRollback(r114, parser);
                                a112 = r114;
                            }

                            r111 &= a112;

                        } // end alternatives a112

                        r111 = r111 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        r111 = r111 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"APPLY"));
                        if (r111)
                        { // may a115
                            bool a115 = false;
                            {
                                Checkpoint(parser); // r116

                                bool r116 = true;
                                r116 = r116 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r116, parser);
                                a115 = r116;
                            }

                            r111 |= a115;
                        } // end may a115

                        r111 = r111 && Match(parser, new Jhu.SkyQuery.Parser.TableSource());
                        CommitOrRollback(r111, parser);
                        a97 = r111;
                    }

                    r96 &= a97;

                } // end alternatives a97

                if (r96)
                { // may a117
                    bool a117 = false;
                    {
                        Checkpoint(parser); // r118

                        bool r118 = true;
                        if (r118)
                        { // may a119
                            bool a119 = false;
                            {
                                Checkpoint(parser); // r120

                                bool r120 = true;
                                r120 = r120 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r120, parser);
                                a119 = r120;
                            }

                            r118 |= a119;
                        } // end may a119

                        r118 = r118 && Match(parser, new Jhu.SkyQuery.Parser.JoinedTable());
                        CommitOrRollback(r118, parser);
                        a117 = r118;
                    }

                    r96 |= a117;
                } // end may a117

                CommitOrRollback(r96, parser);
                res = r96;
            }

            return res;

        }
    }

    public partial class Predicate : Jhu.Graywulf.SqlParser.Predicate
    {
        public override bool Match(Jhu.Graywulf.ParserLib.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r121

                bool r121 = true;
                if (r121)
                { // alternatives a122 must
                    bool a122 = false;
                    if (!a122)
                    {
                        Checkpoint(parser); // r123

                        bool r123 = true;
                        r123 = r123 && Match(parser, new Jhu.Graywulf.SqlParser.Expression());
                        if (r123)
                        { // may a124
                            bool a124 = false;
                            {
                                Checkpoint(parser); // r125

                                bool r125 = true;
                                r125 = r125 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r125, parser);
                                a124 = r125;
                            }

                            r123 |= a124;
                        } // end may a124

                        r123 = r123 && Match(parser, new Jhu.Graywulf.SqlParser.ComparisonOperator());
                        if (r123)
                        { // may a126
                            bool a126 = false;
                            {
                                Checkpoint(parser); // r127

                                bool r127 = true;
                                r127 = r127 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r127, parser);
                                a126 = r127;
                            }

                            r123 |= a126;
                        } // end may a126

                        r123 = r123 && Match(parser, new Jhu.Graywulf.SqlParser.Expression());
                        CommitOrRollback(r123, parser);
                        a122 = r123;
                    }

                    if (!a122)
                    {
                        Checkpoint(parser); // r128

                        bool r128 = true;
                        r128 = r128 && Match(parser, new Jhu.Graywulf.SqlParser.Expression());
                        if (r128)
                        { // may a129
                            bool a129 = false;
                            {
                                Checkpoint(parser); // r130

                                bool r130 = true;
                                r130 = r130 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"NOT"));
                                CommitOrRollback(r130, parser);
                                a129 = r130;
                            }

                            r128 |= a129;
                        } // end may a129

                        if (r128)
                        { // may a131
                            bool a131 = false;
                            {
                                Checkpoint(parser); // r132

                                bool r132 = true;
                                r132 = r132 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r132, parser);
                                a131 = r132;
                            }

                            r128 |= a131;
                        } // end may a131

                        r128 = r128 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"LIKE"));
                        if (r128)
                        { // may a133
                            bool a133 = false;
                            {
                                Checkpoint(parser); // r134

                                bool r134 = true;
                                r134 = r134 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r134, parser);
                                a133 = r134;
                            }

                            r128 |= a133;
                        } // end may a133

                        r128 = r128 && Match(parser, new Jhu.Graywulf.SqlParser.Expression());
                        if (r128)
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
                                        r138 = r138 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                        CommitOrRollback(r138, parser);
                                        a137 = r138;
                                    }

                                    r136 |= a137;
                                } // end may a137

                                r136 = r136 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"ESCAPE"));
                                if (r136)
                                { // may a139
                                    bool a139 = false;
                                    {
                                        Checkpoint(parser); // r140

                                        bool r140 = true;
                                        r140 = r140 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                        CommitOrRollback(r140, parser);
                                        a139 = r140;
                                    }

                                    r136 |= a139;
                                } // end may a139

                                r136 = r136 && Match(parser, new Jhu.Graywulf.SqlParser.Expression());
                                CommitOrRollback(r136, parser);
                                a135 = r136;
                            }

                            r128 |= a135;
                        } // end may a135

                        CommitOrRollback(r128, parser);
                        a122 = r128;
                    }

                    if (!a122)
                    {
                        Checkpoint(parser); // r141

                        bool r141 = true;
                        r141 = r141 && Match(parser, new Jhu.Graywulf.SqlParser.Expression());
                        if (r141)
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
                                        r145 = r145 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                        CommitOrRollback(r145, parser);
                                        a144 = r145;
                                    }

                                    r143 |= a144;
                                } // end may a144

                                r143 = r143 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"NOT"));
                                CommitOrRollback(r143, parser);
                                a142 = r143;
                            }

                            r141 |= a142;
                        } // end may a142

                        if (r141)
                        { // may a146
                            bool a146 = false;
                            {
                                Checkpoint(parser); // r147

                                bool r147 = true;
                                r147 = r147 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r147, parser);
                                a146 = r147;
                            }

                            r141 |= a146;
                        } // end may a146

                        r141 = r141 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"BETWEEN"));
                        if (r141)
                        { // may a148
                            bool a148 = false;
                            {
                                Checkpoint(parser); // r149

                                bool r149 = true;
                                r149 = r149 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r149, parser);
                                a148 = r149;
                            }

                            r141 |= a148;
                        } // end may a148

                        r141 = r141 && Match(parser, new Jhu.Graywulf.SqlParser.Expression());
                        if (r141)
                        { // may a150
                            bool a150 = false;
                            {
                                Checkpoint(parser); // r151

                                bool r151 = true;
                                r151 = r151 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r151, parser);
                                a150 = r151;
                            }

                            r141 |= a150;
                        } // end may a150

                        r141 = r141 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"AND"));
                        if (r141)
                        { // may a152
                            bool a152 = false;
                            {
                                Checkpoint(parser); // r153

                                bool r153 = true;
                                r153 = r153 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r153, parser);
                                a152 = r153;
                            }

                            r141 |= a152;
                        } // end may a152

                        r141 = r141 && Match(parser, new Jhu.Graywulf.SqlParser.Expression());
                        CommitOrRollback(r141, parser);
                        a122 = r141;
                    }

                    if (!a122)
                    {
                        Checkpoint(parser); // r154

                        bool r154 = true;
                        r154 = r154 && Match(parser, new Jhu.Graywulf.SqlParser.Expression());
                        if (r154)
                        { // may a155
                            bool a155 = false;
                            {
                                Checkpoint(parser); // r156

                                bool r156 = true;
                                r156 = r156 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r156, parser);
                                a155 = r156;
                            }

                            r154 |= a155;
                        } // end may a155

                        r154 = r154 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"IS"));
                        if (r154)
                        { // may a157
                            bool a157 = false;
                            {
                                Checkpoint(parser); // r158

                                bool r158 = true;
                                r158 = r158 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                r158 = r158 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"NOT"));
                                CommitOrRollback(r158, parser);
                                a157 = r158;
                            }

                            r154 |= a157;
                        } // end may a157

                        r154 = r154 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        r154 = r154 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"NULL"));
                        CommitOrRollback(r154, parser);
                        a122 = r154;
                    }

                    if (!a122)
                    {
                        Checkpoint(parser); // r159

                        bool r159 = true;
                        r159 = r159 && Match(parser, new Jhu.Graywulf.SqlParser.Expression());
                        if (r159)
                        { // may a160
                            bool a160 = false;
                            {
                                Checkpoint(parser); // r161

                                bool r161 = true;
                                r161 = r161 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"NOT"));
                                CommitOrRollback(r161, parser);
                                a160 = r161;
                            }

                            r159 |= a160;
                        } // end may a160

                        if (r159)
                        { // may a162
                            bool a162 = false;
                            {
                                Checkpoint(parser); // r163

                                bool r163 = true;
                                r163 = r163 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r163, parser);
                                a162 = r163;
                            }

                            r159 |= a162;
                        } // end may a162

                        r159 = r159 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"IN"));
                        if (r159)
                        { // may a164
                            bool a164 = false;
                            {
                                Checkpoint(parser); // r165

                                bool r165 = true;
                                r165 = r165 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r165, parser);
                                a164 = r165;
                            }

                            r159 |= a164;
                        } // end may a164

                        if (r159)
                        { // alternatives a166 must
                            bool a166 = false;
                            if (!a166)
                            {
                                Checkpoint(parser); // r167

                                bool r167 = true;
                                r167 = r167 && Match(parser, new Jhu.SkyQuery.Parser.Subquery());
                                CommitOrRollback(r167, parser);
                                a166 = r167;
                            }

                            if (!a166)
                            {
                                Checkpoint(parser); // r168

                                bool r168 = true;
                                r168 = r168 && Match(parser, new Jhu.Graywulf.SqlParser.BracketOpen());
                                if (r168)
                                { // may a169
                                    bool a169 = false;
                                    {
                                        Checkpoint(parser); // r170

                                        bool r170 = true;
                                        r170 = r170 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                        CommitOrRollback(r170, parser);
                                        a169 = r170;
                                    }

                                    r168 |= a169;
                                } // end may a169

                                r168 = r168 && Match(parser, new Jhu.Graywulf.SqlParser.ArgumentList());
                                if (r168)
                                { // may a171
                                    bool a171 = false;
                                    {
                                        Checkpoint(parser); // r172

                                        bool r172 = true;
                                        r172 = r172 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                        CommitOrRollback(r172, parser);
                                        a171 = r172;
                                    }

                                    r168 |= a171;
                                } // end may a171

                                r168 = r168 && Match(parser, new Jhu.Graywulf.SqlParser.BracketClose());
                                CommitOrRollback(r168, parser);
                                a166 = r168;
                            }

                            r159 &= a166;

                        } // end alternatives a166

                        CommitOrRollback(r159, parser);
                        a122 = r159;
                    }

                    if (!a122)
                    {
                        Checkpoint(parser); // r173

                        bool r173 = true;
                        r173 = r173 && Match(parser, new Jhu.Graywulf.SqlParser.Expression());
                        if (r173)
                        { // may a174
                            bool a174 = false;
                            {
                                Checkpoint(parser); // r175

                                bool r175 = true;
                                r175 = r175 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r175, parser);
                                a174 = r175;
                            }

                            r173 |= a174;
                        } // end may a174

                        r173 = r173 && Match(parser, new Jhu.Graywulf.SqlParser.ComparisonOperator());
                        if (r173)
                        { // may a176
                            bool a176 = false;
                            {
                                Checkpoint(parser); // r177

                                bool r177 = true;
                                r177 = r177 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r177, parser);
                                a176 = r177;
                            }

                            r173 |= a176;
                        } // end may a176

                        if (r173)
                        { // alternatives a178 must
                            bool a178 = false;
                            if (!a178)
                            {
                                Checkpoint(parser); // r179

                                bool r179 = true;
                                r179 = r179 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"ALL"));
                                CommitOrRollback(r179, parser);
                                a178 = r179;
                            }

                            if (!a178)
                            {
                                Checkpoint(parser); // r180

                                bool r180 = true;
                                r180 = r180 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"SOME"));
                                CommitOrRollback(r180, parser);
                                a178 = r180;
                            }

                            if (!a178)
                            {
                                Checkpoint(parser); // r181

                                bool r181 = true;
                                r181 = r181 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"ANY"));
                                CommitOrRollback(r181, parser);
                                a178 = r181;
                            }

                            r173 &= a178;

                        } // end alternatives a178

                        if (r173)
                        { // may a182
                            bool a182 = false;
                            {
                                Checkpoint(parser); // r183

                                bool r183 = true;
                                r183 = r183 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r183, parser);
                                a182 = r183;
                            }

                            r173 |= a182;
                        } // end may a182

                        r173 = r173 && Match(parser, new Jhu.SkyQuery.Parser.Subquery());
                        CommitOrRollback(r173, parser);
                        a122 = r173;
                    }

                    if (!a122)
                    {
                        Checkpoint(parser); // r184

                        bool r184 = true;
                        r184 = r184 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"EXISTS"));
                        if (r184)
                        { // may a185
                            bool a185 = false;
                            {
                                Checkpoint(parser); // r186

                                bool r186 = true;
                                r186 = r186 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r186, parser);
                                a185 = r186;
                            }

                            r184 |= a185;
                        } // end may a185

                        r184 = r184 && Match(parser, new Jhu.SkyQuery.Parser.Subquery());
                        CommitOrRollback(r184, parser);
                        a122 = r184;
                    }

                    r121 &= a122;

                } // end alternatives a122

                CommitOrRollback(r121, parser);
                res = r121;
            }

            return res;

        }
    }

    public partial class SearchCondition : Jhu.Graywulf.SqlParser.SearchCondition
    {
        public override bool Match(Jhu.Graywulf.ParserLib.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r187

                bool r187 = true;
                if (r187)
                { // may a188
                    bool a188 = false;
                    {
                        Checkpoint(parser); // r189

                        bool r189 = true;
                        r189 = r189 && Match(parser, new Jhu.Graywulf.SqlParser.LogicalNot());
                        CommitOrRollback(r189, parser);
                        a188 = r189;
                    }

                    r187 |= a188;
                } // end may a188

                if (r187)
                { // may a190
                    bool a190 = false;
                    {
                        Checkpoint(parser); // r191

                        bool r191 = true;
                        r191 = r191 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        CommitOrRollback(r191, parser);
                        a190 = r191;
                    }

                    r187 |= a190;
                } // end may a190

                if (r187)
                { // alternatives a192 must
                    bool a192 = false;
                    if (!a192)
                    {
                        Checkpoint(parser); // r193

                        bool r193 = true;
                        r193 = r193 && Match(parser, new Jhu.SkyQuery.Parser.Predicate());
                        CommitOrRollback(r193, parser);
                        a192 = r193;
                    }

                    if (!a192)
                    {
                        Checkpoint(parser); // r194

                        bool r194 = true;
                        r194 = r194 && Match(parser, new Jhu.SkyQuery.Parser.SearchConditionBrackets());
                        CommitOrRollback(r194, parser);
                        a192 = r194;
                    }

                    r187 &= a192;

                } // end alternatives a192

                if (r187)
                { // may a195
                    bool a195 = false;
                    {
                        Checkpoint(parser); // r196

                        bool r196 = true;
                        if (r196)
                        { // may a197
                            bool a197 = false;
                            {
                                Checkpoint(parser); // r198

                                bool r198 = true;
                                r198 = r198 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r198, parser);
                                a197 = r198;
                            }

                            r196 |= a197;
                        } // end may a197

                        r196 = r196 && Match(parser, new Jhu.Graywulf.SqlParser.LogicalOperator());
                        if (r196)
                        { // may a199
                            bool a199 = false;
                            {
                                Checkpoint(parser); // r200

                                bool r200 = true;
                                r200 = r200 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                                CommitOrRollback(r200, parser);
                                a199 = r200;
                            }

                            r196 |= a199;
                        } // end may a199

                        r196 = r196 && Match(parser, new Jhu.SkyQuery.Parser.SearchCondition());
                        CommitOrRollback(r196, parser);
                        a195 = r196;
                    }

                    r187 |= a195;
                } // end may a195

                CommitOrRollback(r187, parser);
                res = r187;
            }

            return res;

        }
    }

    public partial class WhereClause : Jhu.Graywulf.SqlParser.WhereClause
    {
        public override bool Match(Jhu.Graywulf.ParserLib.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r201

                bool r201 = true;
                r201 = r201 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"WHERE"));
                if (r201)
                { // may a202
                    bool a202 = false;
                    {
                        Checkpoint(parser); // r203

                        bool r203 = true;
                        r203 = r203 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        CommitOrRollback(r203, parser);
                        a202 = r203;
                    }

                    r201 |= a202;
                } // end may a202

                r201 = r201 && Match(parser, new Jhu.SkyQuery.Parser.SearchCondition());
                CommitOrRollback(r201, parser);
                res = r201;
            }

            return res;

        }
    }

    public partial class SearchConditionBrackets : Jhu.Graywulf.SqlParser.SearchConditionBrackets
    {
        public override bool Match(Jhu.Graywulf.ParserLib.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r204

                bool r204 = true;
                r204 = r204 && Match(parser, new Jhu.Graywulf.SqlParser.BracketOpen());
                if (r204)
                { // may a205
                    bool a205 = false;
                    {
                        Checkpoint(parser); // r206

                        bool r206 = true;
                        r206 = r206 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        CommitOrRollback(r206, parser);
                        a205 = r206;
                    }

                    r204 |= a205;
                } // end may a205

                r204 = r204 && Match(parser, new Jhu.SkyQuery.Parser.SearchCondition());
                if (r204)
                { // may a207
                    bool a207 = false;
                    {
                        Checkpoint(parser); // r208

                        bool r208 = true;
                        r208 = r208 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        CommitOrRollback(r208, parser);
                        a207 = r208;
                    }

                    r204 |= a207;
                } // end may a207

                r204 = r204 && Match(parser, new Jhu.Graywulf.SqlParser.BracketClose());
                CommitOrRollback(r204, parser);
                res = r204;
            }

            return res;

        }
    }

    public partial class HavingClause : Jhu.Graywulf.SqlParser.HavingClause
    {
        public override bool Match(Jhu.Graywulf.ParserLib.Parser parser)
        {
            bool res = true;

            {
                Checkpoint(parser); // r209

                bool r209 = true;
                r209 = r209 && Match(parser, new Jhu.Graywulf.ParserLib.Keyword(@"HAVING"));
                if (r209)
                { // may a210
                    bool a210 = false;
                    {
                        Checkpoint(parser); // r211

                        bool r211 = true;
                        r211 = r211 && Match(parser, new Jhu.Graywulf.SqlParser.CommentOrWhitespace());
                        CommitOrRollback(r211, parser);
                        a210 = r211;
                    }

                    r209 |= a210;
                } // end may a210

                r209 = r209 && Match(parser, new Jhu.SkyQuery.Parser.SearchCondition());
                CommitOrRollback(r209, parser);
                res = r209;
            }

            return res;

        }
    }


}