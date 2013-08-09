<%@ Page Language="C#" AutoEventWireup="true" Title="Glossary" MasterPageFile="~/Docs/Docs.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Text" runat="server">
    <h3>
        Concepts</h3>
    <h4>
        Schema</h4>
    <p>
        Database schema is a collection of data structures (tables) and functions 
        (stored procedures, scalar functions, table-valued functions, etc.). The schema 
        of data sets can be viewed by clicking the &#39;schema&#39; button in the menu.</p>
    <h4>
        Data set</h4>
    <p>
        A data set is a collection of data tables containing logicatlly related data. 
        Every dataset has a unique name that must be used in queries to reference them. 
        Data sets are equivalents of databases in traditional database systems.</p>
    <h4>
        Data table</h4>
    <p>
        Data sets consist of data tables. A table is the basic entity contaning data 
        organized into columns and rows. While table columns are defined at the creation 
        of table and are fixed, the number of rows may vary.</p>
    <h4>
        Table column</h4>
    <p>
        Tables consist of columns and rows. When a table is created, columns have to be 
        defined. Every column is identified by a name unique to the table and has a 
        specific data type (number, text, etc).</p>
    <h4>
        Table row</h4>
    <p>
        A table consist of any number of rows but the number and data type of columns 
        are always fixed.</p>
    <h4>
        Primary key</h4>
    <p>
        Table rows are usually identified by a value called primary key. Primary key 
        values are unique to the table.</p>
    <h4>
        Foreign key</h4>
    <p>
        &nbsp;</p>
    <h4>
        Query</h4>
    <p>
        Data transformations and calculations must be formulated as SQL queries. Queries 
        take one or more data tables and tranform them into another by combining data 
        rows from the two tables</p>
    <h4>
        MyDB</h4>
    <p>
        At registration, every user gets a scratch database, the so called MyDB. MyDB is 
        used as a sandbox that stores query results and uploaded data in relational data 
        tables. Data tables in MyDB are only visible to the user.</p>
</asp:Content>
