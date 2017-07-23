<%@ Page Language="C#" AutoEventWireup="true" Title="Functions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Text" runat="server">
    <h1>Query syntax</h1>
    <p>
        The xmatch query syntax is based on an extension to the SELECT statement of standard SQL, with some restriction listed below. The basic syntax of a cross-match query is the following:
    </p>
    <pre>select_statement =
{ SELECT 
    select_list
    [ INTO table_name ]
    [ FROM table_expression ]
    [ WHERE condition ]
    [ REGION region_string ]
}</pre>
    <p>
        Thre rule describing the select list is as follows:
    </p>
    <pre>select_list =
{   
    *   
    | { table_name | view_name | table_alias }.*   
    | {  
        [ { table_alias } . ]  { column_name }
        | expression  
        [ [ AS ] column_alias ]
        }  
} [ ,...n ]</pre>
    <p>
        Table aliases are single identifiers made up of alphanumeric characters
        and the underscore character. Table names, view names and aliases in the select list
        must match the table names, view names and table aliases in the FROM 
        clause and must not be ambigous. If * with no table name or alias is
        specified, all columns of all tables referenced in the FROM claused are returned.
        If the select list contains table_name.*, all columns of the table table_name
        are returned.
    </p>
    <p>
        Expressions are combinations of constants, columns, function calls and arithemtic operators.
    </p>
    <p>
        The column alias is an identifier made up of alphanumeric characters and underscores.
        If no column alias is used, the output table will have the name of the original column.
        In case of expressions, a column alias must be specified. SkyQuery, in contrast
        to standard SQL, automatically generates column aliases from the table alias
        and the column name in the form of alias_column, or if it is not possible,
        in the form of Col_n where n is a number to make column aliases unique.
    </p>
    <p>
        The into table is a full reference to a table in a writable database, i.e.
        MYDB or MYSCRATCH.
    </p>
    <p>
        Table names and view names differ from standard SQL since they contain a dataset part but usually do not contain a database part. The main difference is that a dataset refers to multiple databases on a cluster of machines.
        The schema name is optional. If specified, it must be dbo for the source catalogs,
        mydb for MYDB and the username for MYSCRATCH.
    </p>
    <pre>table_or_view_name =
{
    [ dataset_name : ] [ schema_name . ] { table_or_view_name } [ [ AS ] table_alias ]
}    </pre>
    <p>
        The FROM clause if followed by an expression of table sources combined with JOIN
        operators. Table sources can be table-valued functions, simple tables, subqueries
        or cross-match expressions. Only one cross-match expression is permitted in a
        FROM clause, and it is always evaluated before any of the joins. Aliasing tables is
        not always mandatory but is strongly advised for clarity. Table-valued functions,
        subqueries and xmatch expression must always be aliased. Aliases must be
        unique within a query statement.
    </p>
    <pre>table_expression =
{
        table_or_view_name [ [ AS ] table_alias ]
        | table_valued_function_call [AS] table_alias
        | subquery [ AS ] table_alias
        | xmatch AS table_alias
}</pre>
    <p>
        The syntax of a user-defined function call follows standard SQL with the only
        exception that all functions reside in the CODE dataset, hence no dataset, nor
        database name should be specified. On the other hand, the schema name is
        mandatory.
    </p>
    <pre>table_valued_function_call =
{
        { schema_name } . { function_name } ( { argument_list } )
}</pre>
    <p>
        The argument list is a comma separated list of expressions.
    </p>
    <p>
        Subqueries are complete SELECT statements wrapped in round brackets. Subqueries
        must always be aliased in the FROM clause and columns of its select list must 
        have unique aliases.
    </p>
    <pre>subquery =
{ ( { select_statement } ) }</pre>
    <p>
        The XMATCH expression is similar to a function call, in the sense that the keyword
        is followed by round brackets and a parameter list. Cross-match parameters are
        table specifications except the very last one, which is the matching constraint.
    </p>
    <pre>xmatch =
{ XMATCH ( { xmatch_table_list } , { xmatch_costraint } ) }</pre>
    <p>The xmatch_table_list is a comma-separated list of xmatch_table_specification items:</p>
    <pre>xmatch_table_specification =
{ MUST EXIST IN { table_name } [ [ AS ] table_alias ] [ WITH ( table_hint_list ) ] }</pre>
    <p>Currently only the MUST EXIST IN expression is supported and no partial
        matching is possible, i.e. a match must have detections in all catalogs. With the
        table hints, the user can provide additional information on the table, such as
        the columns containing the coordinates, the astrometric error and the
        availability of the HTM and Zone indexes. The table_hint_list is a comma separated
        list of xmatch_table_hint expressions.
    </p>
    <pre>xmatch_table_hint =
{
    POINT ( argument_list )
    | ERROR ( argument_list )
    | HTMID ( argument )
    | ZONEID ( argument )
}
    </pre>
    <p>When the POINT hint is specified, it overrides the default columns to be used as coordinates. Note, that overriding default columns can result in a significant increase in runtime since spatial indexes required from cross-matching need to be computed on the fly. The POINT hint can take 2, 3 or five parameters:</p>
    <ul>
        <li>2 parameters: RA, Dec</li>
        <li>3 parameters: Cx, Cy, Cz</li>
        <li>5 parameters: RA, Dec, Cx, Cy, Cz</li>
    </ul>
    <p>
        Spatial filtering requires the coordinates in Cartesian representation while
        cross-matching works with equatorial coordinates. For this reason, SkyQuery
        support specifying all five components and assumes that the two representations
        are the same. If only one representation is specified, conversion between
        the equatorial and Cartesian coordinates might be necessary which can add
        significant time to query execution.
    </p>
    <p>
        The astrometric error can be specified with the ERROR hint. It can take either a single parameter or three parameters:</p>
    <ul>
        <li>A single, constant number parameter defines the astrometric error in decimal arc seconds.</li>
        <li>A single column name or expression takes the astrometric error from the catalog table. The unit must be arc seconds.</li>
        <li>3 parameters: the first is a column name or expression takes the astrometric error from the catalog table. The unit must be arc seconds. The second and third parameters are constant number specifying the minimum and maximum possible astrometric error in decimal arc seconds.</li>
    </ul>
    <p>
        The HTMID and ZONEID hints can be used to specify the columns containing the special indexes used for spatial filtering and cross-matching. Specifying them for the built-in catalogs is not necessary and they are currently not supported for user tables.
    </p>
    <h2>Restrictions</h2>
    <p>
        Features of SQL that are not currently supported by cross-match queries:
    </p>
    <ul>
        <li>Multi-statement SQL scripts. Only single SELECT commands are supported</li>
        <li>SQL statements other than SELECT</li>
        <li>Common table expressions (WITH statement)</li>
        <li>Aggregates, such as AVG, MAX etc, including the GROUP BY and HAVING clauses. Run the cross-match query first and compute aggregates on the output table.</li>
        <li>Ranking functions, such as ROW_NUMBER, NTILE, etc.</li>
        <li>Subqueries in expressions.</li>
    </ul>
</asp:Content>
