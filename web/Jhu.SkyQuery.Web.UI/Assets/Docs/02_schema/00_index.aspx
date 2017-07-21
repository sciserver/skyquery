<%@ Page Language="C#" AutoEventWireup="true" Title="Schema" %>

<asp:Content ContentPlaceHolderID="Text" runat="server">
    <h1>Schema</h1>
    <p>SkyQuery is built on relational databases and uses the data structures and algorithms of databases to store and process astronomical data.
        SkyQuery is based on a cluster of relational database management servers and uses a modified SQL as its programming language. To access scientific data, you have to implement data filtering tasks and cross-match problems as SQL queries. To formulate SQL queries, you need to be aware of the structure of the data stored in the database. Relational databases consist of tables with a predefined set of columns (fields) and an arbitrary number of rows containing data. Each database table has a name unique to the database and each column has a name unique to the table. Columns also have a pre-defined data type, such as integer number, floating-point number, string, etc. In typical scenarios, a database contains data from a single sky survey or mission archive. While most archives consist of a single table storing information on all detected objects, many newer catalogs contain multiple tables storing important meta-data about the observations.
        The schema is the ensamble of database objects that include, among others, tables for storing the source catalogs and functions that provide scientific processing capabilities.</p>
    <p>Use the &#39;schema&#39; menu to get information about the available catalogs. First select an astronomical catalog from the first dropdown on the top of the page, then select the type of objects you want to display. Select &#39;Tables&#39; to the list of available data tables. Select the table or a view, etc. to display its columns, indices, parameters, etc.</p>
    <p>To access the schema from a script, use the schema web service at this link.</p>
    <h2>Datasets</h2>
    <p>At the top level of the schema hierarchy of SkyQuery are datasets. A dataset usually corresponds to an astronomical catalog which may consist of multiple data tables but the user-writable MyDB and MyScratch are also considered datasets. Since SkyQuery runs on a cluster of database servers, datasets are treated differently as Microsoft SQL Server&#39;s and Postgres&#39; databases or MySQL&#39;s schemas. SkyQuery uses three special dataset names: MYDB, MYSCRATCH and CODE which refer to the permantent and temporary user data spaces and a special dataset containing user-defined types, as well as scalar and table-valued functions.</p>
    <p>In queries, when specifying table names, the dataset name must also be specified, separated by a colon as in the following example:</p>
    <pre>SELECT objID, ra, dec FROM <strong>SDSSDR12:</strong>SpecObj WHERE z BETWEEN 0.01 AND 0.02</pre>
    <p>
        If the dataset name is not specified, MYDB is assumed by default for tables and views and CODE is for user-defined types and functions. The default database cannot be changed, datasets other than the default must be explicitly specified in queries.</p>
    <h3>MyDB and MyScratch datasets</h3>
    <p>
        <strong>MyDB</strong> are <strong>MyScratch</strong> are only visible after signing in. These are the catalogs containing your own tables, either created from uploaded files or generated as query outputs. Go to the &#39;my data&#39; menu to see more detailed information on your own tables and to import and export data.
    </p>
    <h3>CODE dataset</h3>
    <p>
        The <strong>CODE</strong> dataset does not contain any data but provides access to spatial and scientific scalar and table-values functions.
    </p>
    <h2>Tables</h2>
    <p>All data in databases are stored in the form of tables. The definition of a table consists of a predefined set of columns with a name and data type. Tables can contain an arbitrary number of rows.</p>
    <p>Source catalog tables are read only while MyDB and MyScratch tables are read-write. Currently, it is not possible to create tables explicitly by using the CREATE TABLE or similar SQL statements. Tables are automatically created in MyDB when SELECT statements are executed. The SELECT ... INTO syntax can be used to specify the target table name, for example:</p>
    <pre>SELECT objID, ra, dec INTO <strong>MYDB:mydata</strong> FROM SDSSDR12:SpecObj</pre>
    <p>
        Inserting into existing tables is currently not possible, so repeated query executions must write results into new tables. This is also true for file imports. Tables can be renamed or deleted from the &#39;my data&#39; menu.</p>
    <h2>Columns</h2>
    <p>Table columns have names and data types. Data types are usually restricted to those that are standard to Microsoft SQL Server. Future releases of SkyQuery will add support to more sophisticated data types such as complex numbers, arrays and spherical shapes. SkyQuery also adds detailed meta-data to columns, such as textual description of the contents, physical quantities and units. Meta-data are both for human reading and for software .Meta-data is not currently available from SQL queries but will be used in the future to automate query writing.</p>
    <p>Because tables in MyDB are generated automatically during execution of SELECT and SELECT ... INTO queries, target column names should be specified in the queries using the <strong>column AS name</strong> syntax, for example:</p>
    <pre>SELECT objID, ra AS ra_j2000, dec AS dec_j2000 FROM SDSSDR12:SpecObj</pre>
    <h2>&nbsp;Views</h2>
    <p>Certain source catalogs define views over data tables. In most cases a view is a subset of the data of the original table. For example, SDSS stores all photometric detections in the PhotoObjAll table and uses constraints on fields to filter the original table down to primary objects view PhotoObj or to the views Galaxy and Star, etc.</p>
    <p>It is currently not possible to create user-defined views.</p>
    <h2>Indexes and primary keys</h2>
    <p>In database systems, indexes are used to speed up query execution. In most cases, SkyQuery will automaticall find the right index on source catalog tables and use them. Special indexes for spatil filtering (i.e. spherical region constraints) and cross-matching. For every catalog, a Hierarchical Triangular Mesh index and a Zone index is defined over the coordinate columns.</p>
    <p>To uniquely identify objects in source catalogs, an ID column is used. The primary key (often also called as clustered index) is built on this ID column. On the user interface, the primary key column is marked with a small key icon. To run a cross-match job, all tables that are part of the matching must have a primary key defined. This is already done for the source catalogs but a primary key is not always automatically created for user tables. SkyQuery&#39;s table import tool, available under the &#39;my data&#39; menu, has an option to automatically generate a primary key during file upload. Already existing tables, such as tables generated from query results may not have primary keys at all, or may have columns which are unique and fulfil the requirements of primary keys but the index itself is not created. To create a primary key on an existing table, under the &#39;my data&#39; menu, go to the &#39;tables&#39; tab, open the details view by clicking on the downward arrow next to the table name and click on 
        the &#39;primary key&#39; button.</p>
</asp:Content>
