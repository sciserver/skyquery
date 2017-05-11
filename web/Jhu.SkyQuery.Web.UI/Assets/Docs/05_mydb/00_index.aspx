<%@ Page Language="C#" AutoEventWireup="true" Title="My data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Text" runat="server">
    <h3>MyDB</h3>
    <p>
        Registered users of SciServer (formerly: SkyServer) automatically get a limited-size
        private database called MyDB. MyDB is used as a sandbox that stores query results and 
        uploaded data in relational data tables. Data tables in MyDB are only visible to the user.
        MyDB is directly accessible from the SciServer CasJobs, as well as from SkyQuery.
        Data tables created with CasJobs can be read by SkyQuery and vice-versa. MyDB is a
        persistent storage are for database tables that is regularily backed-up.
    </p>
    <p>
        <asp:HyperLink runat="server" NavigateUrl="~/Apps/MyDb/Tables.aspx?dataset=MYDB">View tables in MyDB</asp:HyperLink></p>
    <h3>MyScratch</h3>
    <p>
        Since MyDB is limited in size, to allow processing larger data sets, SkyQuery
        provides access to MyScratch, a multi-terabyte database for staging data. 
        MyScratch tables are only visible to the user but storage space is
        shared among users, hence tables stored here are not guaranteed to be backed up
        and are subject to deletion after a short period of time. Users are encouraged
        to use MyScratch for intermediate query results but store final data in MyDB or
        download as data files.
    </p>
    <p>
        <asp:HyperLink runat="server" NavigateUrl="~/Apps/MyDb/Tables.aspx?dataset=MYScratch">View tables in MyScratch</asp:HyperLink></p>
    <h3>Copying tables</h3>
    SkyQuery provides a fast table copy between MyDB and MyScratch.
    <p>
        <asp:HyperLink runat="server" NavigateUrl="~/Apps/MyDb/Copy.aspx">Go to table copy form</asp:HyperLink></p>
    <h3>Importing data</h3>
    <p>
        Users can upload their own data into MyDB or MyScratch. Uploaded data files are
        automatically converted into database tables. SkyQuery support importing the
        following data formats:
        <ul>
            <li>Comma separated values (CSV)</li>
            <li>Tab separated text file (TXT)</li>
            <li>Flexible Image Transport Format (FITS) - binary tables only</li>
            <li>Virtual Observatory Data Table (VOTable)</li>
            <li>Microsoft SQL Server native table dump - bcp utility</li>
        </ul>
        During upload, column names, data types etc. are detected automatically. In addition
        to uploading files directly via the browser, large files can be imported from
        HTTP, FTP and SciDrive. Data import supports tar, gzip, bzip2 and zip compressions.
    </p>
    <p>
        <asp:HyperLink runat="server" NavigateUrl="~/Apps/MyDb/Import.aspx">Go to data import form</asp:HyperLink></p>
    <h3>Exporting data</h3>
    <p>
        Data tables stored in MyDB and MyScratch can be exported and download in a variety
        of data formats:
        <ul>
            <li>Comma separated values (CSV)</li>
            <li>Tab separated text file (TXT)</li>
            <li>XHTML file</li>
            <li>Flexible Image Transport Format (FITS) - binary tables only</li>
            <li>Virtual Observatory Data Table (VOTable)</li>
            <li>Microsoft SQL Server native table dump - bcp utility</li>
        </ul>
        Tables in MyDB can be downloaded via HTTP or exported to an FTP server or SciDrive.
        Data export supports tar, gzip, bzip2 and zip compression.
    </p>
    <p>
        <asp:HyperLink runat="server" NavigateUrl="~/Apps/MyDb/Export.aspx">Go to data export form</asp:HyperLink></p>
</asp:Content>
