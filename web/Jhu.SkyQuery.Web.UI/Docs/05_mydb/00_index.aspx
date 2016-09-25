<%@ Page Language="C#" AutoEventWireup="true" Title="MyDB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Text" runat="server">
    <h3>
        MyDB</h3>
    <p>
        At registration, every user gets a scratch database, the so called MyDB. MyDB is 
        used as a sandbox that stores query results and uploaded data in relational data 
        tables. Data tables in MyDB are only visible to the user. MyDB&#39;s size is limited 
        to 128Mb by default, but can be extended on request.</p>
    <h3>
        Importing data</h3>
    <p>
        Users can upload their own data into the system. Uploaded data will be stored as
        a table in MyDB. The only supported file format currently is CSV (comma-separated
        values). The system figures out column names, data types etc. automatically from 
        the uploaded file, creates a new table for the import in MyDB. In advanced mode 
        the name of the target table can be specified. Optionally, a primary key on the 
        new table can be created.</p>
<h3>
        Exporting data</h3>
<p>
        Tables in MyDB can be downloaded via HTTP. First tables have to be exported in 
        any of the supported file formats (currently only CSV) and once the export 
        operation is completed, can be downloaded from the web server. As exports can 
        take a significant time, they are done asynchronously by scheduling a job for 
        every export.</p>
</asp:Content>
