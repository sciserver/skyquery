<%@ Page Language="C#" AutoEventWireup="true" Title="Quick Start Guide" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Text" runat="server">
    <h1>Quick Start Guide</h1>
    <h2>Catalogs as datasets and tables</h2>
    <p>
        SkyQuery is based on database technology which means two things: It stores astronomical
        catalogs in the form of <i>data tables</i> organized into <i>datasets</i> and, it
        uses an extended version of the SQL language to manipulate data. Certain catalogs
        may consist of multiple tables
        (such as photometric and spectroscopic data, etc.). In SkyQuery, tables are identified
        by a compound name in the form of <id>DATASET:TableName</id>.
        For example, SDSS DR12 spectroscopy
        is available in the <id>SDSSDR12:SpecObjAll</id> table. Tables
        consist of columns which usually
        contain a single scalar parameter of the astronomical object, such as the JD of
        observation or right ascension. Since databases usually prefer scalar values
        to compond variables (structs), multiple columns are used to store coordinates (most
        often
        named <id>RA</id> and <id>Dec</id>).
        Certain catalogs also have <i>views</i>, which are simply filtered sets
        of tables. For example, SDSS DR12 has views called star and galaxy over the 
        <id>photoObjAll</id>
        table which are just subsets of all photomerically observed objects.
    </p>
    <h3>Database schema</h3>
    <p>
        The ensemble of all data sets, tables and columns is called the <i>schema</i>. Use
        the
        schema browser to explore the catalogs available in SkyQuery.
    </p>
    <h3>Primary keys</h3>
    <p>
        In every catalog table, an integer column, called the <i>primary key</i>, is necessary
        to uniquely identify objects in the catalog. This column is most often named <span
            class="gw-docs-id">objID</id>
        and it is strictly unique within a data table. This column is used in cross-match
        operations so user-uploaded tables also need to have primary keys.
    </p>
    <h3>Coordinates</h3>
    <p>
        Coordinates of extragalactic objects are always equatorial J2000. Coordinate epochs
        of galactic stars with significant proper motion might vary from catalog
        to catalog. The pair of cordinates are usually stored in the <id>
            RA</id> and <id>Dec</id> columns.
        To allow efficient spherical search
        and spatial filtering, catalog tables are augmented with additional columns derived
        from the coordinates. Cartesian unit vector components (usually the columns <span
            class="gw-docs-id">cx</id>,
        <id>cy</id> and <id>cz</id>) are just
        another representation of the spherical coordinates, whereas
        the index columns <id>htmid</id> and <id>
            zoneid</id> are hash values computed from the spherical coordinates
        and are used to speed up spherical search and cross-match.
    </p>
    <h2>User databases</h2>
    <p>
        SkyQuery stores all data in databases which include the source catalogs, user
        data and cross-match results. Registered users get instant access to their own data
        space called <id>MyDB</id> and much larger shared data space
        called 
        <id>MyScratch</id>. Data in both
        <id>MyDB</id> and <id>MyScratch</id>
        are private to the user but <id>MyScratch</id> is temporary
        storage only
        and tables stored there expire and might get deleted without prior notification.
        Users can upload data to <id>MyDB</id> and <id>
            MyScratch</id> or download tables in a variety of astronomical
        data formats including FITS, VOTable and CVS. Uploaded files are automatically converted
        into database tables that appear under the <id>MyDB</id>
        or <id>MyScratch</id> dataset. Tables of
        the user databases can be managed from the
        <a href="<%= ResolveUrl(Jhu.Graywulf.Web.UI.Apps.MyDB.Default.GetUrl()) %>">my data</a>
        page. This page also offers creating
        primary keys on user tables which is necessary for cross-matching.
    </p>
    <p>
        The output from cross-matching and regular SQL queries are saved into automatically
        created new tables in either <id>MyDB</id> or <id>
            MyScratch</id>. Both of these databases are available
        from <a href="http://www.sciserver.org/tools/casjobs/">SkyServer&#39;s CasJobs</a>
        interface, hence user tables are accessible from both
        systems.
    </p>
    <h2>XMatch form</h2>
    <p>
        The <a href="<%= ResolveUrl(Jhu.SkyQuery.Web.UI.Apps.XMatch.Default.GetUrl()) %>">XMatch</a>
        user interface is the simplest way to start using the system. Just add
        the catalogs to match, specify the columns to return (including best match coordinates,
        magnitudes, identifiers, etc.) and execute the job. Cross-match tasks can also be
        converted into SQL queries for further tweaking and scripting.
    </p>
    <h2>Queries</h2>
    <p>
        Writing queries is the primary way of interacting with databases. SkyQuery uses
        an extended version of the SQL language which feature syntax constructs to describe
        complex cross-match problems and spatial filtering criteria but it also supports
        standard SQL, including table joins, for simple filtering and processing of the
        data. A typical cross-match query looks like the following.
    </p>
    <jgwuc:Query ID="Query1" runat="server">SELECT x.matchID, x.ra, x.dec, s.objid, s.ra, s.dec, g.objid, g.ra, g.dec
INTO twowayxmatch
FROM XMATCH
     (MUST EXIST IN SDSSDR13:PhotoObjAll AS s,
      MUST EXIST IN GALEXGR6:PhotoObjAll AS g,
      LIMIT BAYESFACTOR TO 1e3) AS x
REGION 'CIRCLE J2000 0.0 0.0 30.0' </jgwuc:Query>
    <p>
        While SkyQuery offers cross-matching capabilities, its SQL capabilies are limited
        at the moment to single select statements.
    </p>
    <div class="gw-docs-callout">
        Follow to <a href="../04_query/00_index.aspx">this page</a> for a comprehensive
        documentation on the query syntax.
    </div>
    <h2>Jobs and queues</h2>
    <p>
        Since cross-match jobs, and even simpler queries running on large databases, can
        take a significant time to complete, SkyQuery uses a job queue to schedule and execute
        user tasks. There&#39;s a <i>quick queue</i> for fast-running jobs that are supposed
        to
        complete within a minute and a <i>long queue</i> with a timeout of several hours.
        The quick
        queue is intended for query testing and debugging and has a time out of one minute.
    </p>
    <p>
        Most SkyQuery operations (including queries, data export and import, etc.) are performed
        asynchronously by the job system and the job progess can be followed on the 
        <a href="<%= ResolveUrl(Jhu.Graywulf.Web.UI.Apps.Jobs.Default.GetUrl()) %>">jobs page</a>.
    </p>
    <h2>REST API</h2>
    <p>
        SkyQuery provides a set of <a href="https://en.wikipedia.org/wiki/Representational_state_transfer">
            RESTful web services</a> to interact with the system. The
        services provide an <a href="<%= ResolveUrl(Jhu.Graywulf.Web.UI.Apps.Api.Default.GetUrl()) %>">
            API</a> to view the database schema, access user tables (import
        and export), submit new jobs (queries, import, export, etc.), track job execution
        and cancel running jobs. A python wrapper for the web services is provided as part
        of the <a href="https://github.com/sciserver/SciScript-Python">SciScript package</a>
        which is also available from <a href="http://www.sciserver.org/tools/compute/">SciServer
            Compute</a>.
    </p>
</asp:Content>
