<%@ Page Language="C#" AutoEventWireup="true" Title="Introduction" %>

<asp:Content ContentPlaceHolderID="Text" runat="server">
    <h1>
        Introduction</h1>
    <p>
        Welcome to SkyQuery!</p>
    <p>
        SkyQuery is a free service for cross-matching astronomical source catalogs.</p>
    <ul>
        <li>It&#39;s scalable - SkyQuery runs in parallel on a cluster of servers with high-end IO subsystems to provide excellent throughput. This allows for performing full-catalog cross-matching even between catalogs with billions of objects.</li>
        <li>It&#39;s comprehensive - SkyQuery already contains 50+ important catalogs and the number is growing.</li>
        <li>It&#39;s for your data too - SkyQuery allows uploading you own catalogs in various data formats for cross-matching.</li>
        <li>It&#39;s sophisticated - SkyQuery&#39;s matching algorithm is based on Bayesian statistics and allows for matching multiple catalogs with astrometric error varying from object to object.</li>
        <li>It&#39;s a database system - SkyQuery - similarly to other astronomical databases such as SDSS SkyServer - is based on SQL database technology. As a result, cross-match problems can be formulated in an extended version of the SQL language. The SQL language, extended with a very efficient spherical search library, offers excellent filtering capabilities and fast execution.</li>
        <li>It&#39;s integrated - SkyQuery is integrated into the SciServer and SkyServer infrastructure. This means users can sign in with their existing account and have access to local databases, storage and compute facilities such as MyDB, MyScratch and SciServer Compute.</li>
        <li>It&#39;s a web service - 100% of SkyQuery functionality is accessible via REST web services which means cross-match functionality can easily be incorporated into scripts and data processing pipelines.</li>
    </ul>
    <h2>Quick Start Guide</h2>
    <p>
        For a quick start to SkyQuery, follow this link.</p>
    <h2>Main concepts</h2>
    <h3>Catalogs as datasets and tables</h3>
    <p>
        SkyQuery is based on database technology which means two things: It stores astronomical catalogs in the form of datasets and tables and it uses an extended version of the SQL language to manipulate data. Certain catalogs may consist of multiple tables (such as photometric and spectroscopic data, etc.). In SkyQuery, tables are identified by a compound name in the form of DATASET:TableName. For example, SDSS DR12 spectroscopy is available in the SDSSDR12:SpecObj table. Tables consist of columns which usually contain a single scalar parameter of the astronomical object, such as the JD of observation or right ascension. Since databases usually prefer scalar variables over compond variables, multiple columns are used to store coordinates (most often named RA and Dec). Certain catalogs also have views, which are simply filtered sets of tables. For example, SDSS DR12 has views called star and galaxy over the photoObjAll table which are just subsets of all photomerically observed objects.</p>
    <h3>Database schema</h3>
    <p>
        The ensemble of all data sets, tables and columns is called the schema. Use the schema browser to explore the catalogs available in SkyQuery.</p>
    <h3>Primary keys</h3>
    <p>
        In every catalog table, an integer column, called the primary key, is necessary to uniquely identify objects in the catalog. This column is most often named objID and it is strictly unique within a data table. This column is used in cross-match operations so user-uploaded tables also need to have primary keys.</p>
    <h3>Coordinates</h3>
    <p>
        Coordinates are always equatorial J2000. Coordinate epochs of galactic stars with significant proper motion might vary from catalog to catalog. The pair of cordinates are usually stored in the RA and Dec columns. To allow efficient spherical search and spatial filtering, catalog tables are augmented with additional columns derived from the coordinates. Cartesian unit vector components (usually the columns cx, cy and cz) are just another representation of the spherical coordinates, whereas the index columns htmid and zoneid are hash values computed from the spherical coordinates and are used to speed up spherical search and cross-match.</p>
    <h3>User databases</h3>
    <p>
        SkyQuery uses only databases to store data which includes the source catalogs, user data and cross-match results. Registered users get instant access to their own data space called MyDB and much larger shared data space called MyScratch. Data in both MyDB and MyScratch are private to the user but MyScratch is temporary storage only and tables stored there expire and might get deleted without prior notification. Users can upload data to MyDB and MyScratch or download tables in a variety of astronomical data formats including FITS, VOTable and CVS. Uploaded files are automatically converted into database tables that appear under the MyDB or MyScratch dataset. Tables of the user databases can be managed from the my data page. This page also offers creating primary keys on user tables which is necessary for cross-matching.</p>
    <p>
        The output from cross-matching and regular SQL queries are saved into automatically created new tables in either MyDB or MyScratch. Both of these databases are available from SkyServer&#39;s CasJobs interface, hence user tables are accessible from both systems.
    </p>
    <h3>XMatch form</h3>
    <p>
        The XMatch user interface is the simplest way to start using the system. Just add the catalogs to match, specify the columns to return (including best match coordinates, magnitudes, identifiers, etc.) and execute the job. Cross-match tasks can also be converted into SQL queries for further tweaking and scripting.</p>
    <h3>Queries</h3>
    <p>
        Writing queries is the primary way of interacting with databases. SkyQuery uses an extended version of the SQL language which feature syntax constructs to describe complex cross-match problems and spatial filtering criteria but it also supports standard SQL, including table joins, for simple filtering and processing of the data. Follow to this page for a comprehensive documentation on the query syntax. While SkyQuery offers cross-matching capabilities, its SQL capabilies are limited at the moment to single select statements.</p>
    <h3>Jobs and queues.</h3>
    <p>
        Since cross-match jobs, and even simpler queries running on large databases, can take a significant time to complete, SkyQuery uses a job queue to schedule and execute user tasks. There&#39;s a quick queue for fast-running jobs that are supposed to complete within a minute and a long queue with a timeout of several hours. The quick queue is intended for query testing and debugging and has a time out of one minute.</p>
    <p>
        Most SkyQuery operations (including queries, data export and import, etc.) are performed asynchronously by the job system and the job progess can be followed on this page.</p>
    <h3>REST API</h3>
    <p>
        SkyQuery provides a set of RESTful web services to interact with the system. The services provide funtionality to view the database schema, access user tables (import and export), submit new jobs (queries, import, export, etc.), track job execution and cancel running jobs. A python wrapper for the web services is provided as part of the SciScript package which is also available from SciServer Compute.</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
</asp:Content>
