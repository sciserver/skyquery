<%@ Page Language="C#" AutoEventWireup="true" Title="Schema" %>

<asp:Content ContentPlaceHolderID="Text" runat="server">
    <h3>Schema</h3>
    <p>
        SkyQuery is based on a cluster of relational database management servers and uses a modified SQL as its programming language. To access scientific data, you have to implement data filtering tasks and cross-match problems as SQL queries. To formulate SQL queries, you need to be aware of the structure of the data stored in the database. Relational databases consist of tables with a predefined set of columns (fields) and an arbitrary number of rows containing data. Each database table has a name unique to the database and each column has a name unique to the table. Columns also have a pre-defined data type, such as integer number, floating-point number, string, etc. In typical scenarios, a database contains data from a single sky survey or mission archive. While most archives consist of a single table storing information on all detected objects, many newer catalogs contain multiple tables storing important meta-data about the observations.
    </p>
    <p>
        Use the &#39;schema&#39; menu to get information about the available catalogs. First select an astronomical catalog from the first dropdown on the top of the page, then select the type of objects you want to display. Select &#39;Tables&#39; to the list of available data tables. Select the table or a view, etc. to display its columns, indices, parameters, etc.
    </p>
    <h3>MyDB and MyScratch datasets</h3>
    <p>
        <strong>MyDB</strong> are <strong>MyScratch</strong> are only visible after signing in. These are the catalogs containing your own tables, either created from uploaded files or generated as query outputs. Go to the &#39;my data&#39; menu to see more detailed information on your own tables and to import and export data.
    </p>
    <h3>CODE dataset</h3>
    <p>
        The <strong>CODE</strong> dataset does not contain any data but provides access to spatial and scientific scalar and table-values functions.
    </p>
</asp:Content>
