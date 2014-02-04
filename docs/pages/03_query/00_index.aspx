<%@ Page Language="C#" AutoEventWireup="true" Title="Queries" MasterPageFile="~/Docs/Docs.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Text" runat="server">
    <h3>
        Queries</h3>
    <p>
        SkyQuery uses a modified flavour of the SQL language based on Microsoft SQL Server's
        syntax, just like <a href="http://skyserver.sdss.org">SkyServer</a>. The current
        version supports single SELECT statements only.</p>
    <p>
        The following simple query can be used to test basic functionality. It should return
        a few objects from the SDSS DR7 photometric catalog.</p>
    <jgwuc:Query runat="server">
SELECT TOP 100 objid, ra, dec, u, g, r, i, z
FROM SDSSDR7:PhotoObjAll
    </jgwuc:Query>
    <p>
        Queries are written using the query editor. Use the &#39;syntax check&#39; button
        to verify typos before submitting a query. Queries are always queued for execution
        to avoid the flooding of the system by too many users. There are two queues: a quick
        queue for almost instantanious execution of short queries and a long queue for long-running
        complex queries. To send a query into the quick queue, simply click on the &#39;quick&#39;
        button. Result will appear on the bottom of the screen but also get stored in the
        table MyDB:quickresults. Because quick queries are also subject to queueing, a couple
        of seconds might elapse before execution begins. Long queries can be executed by
        clicking the &#39;execute&#39; button. Results from long queries are always stored
        in MyDB. To explicitely direct query results to a given table, enter the name of
        the target table in the form, or use the INTO synstax as follows.</p>
    <jgwuc:Query ID="Query1" runat="server">
SELECT TOP 100 objid, ra, dec, u, g, r, i, z
INTO MyNewTable
FROM SDSSDR7:PhotoObjAll
    </jgwuc:Query>
    <p>
        To filter output, use the WHERE clause.</p>
    <jgwuc:Query ID="Query2" runat="server">SELECT objid, ra, dec, u, g, r, i, z
INTO MyNewTable
FROM SDSSDR7:PhotoObjAll
WHERE ra BETWEEN 0 AND 0.5 AND dec BETWEEN 0 AND 0.5</jgwuc:Query>
    <p>
        Cross-match queries use extensions to the SQL language to formulate the cross-match
        problem. Tables have to be listed after the FROM keyword and combined using the
        CROSS JOIN operator. An XMATCH clause at the end of the query is then used to set
        the cross-matching criteria. The following query cross-matches the SDSS DR7 catalog
        with Galex in a small area of the sky. Multi-catalog and full-catalog cross-matches
        are also possible. Most catalogs denote invalid values by -9999, so filtering out
        wrong coordinates is always necessary. Also, restricting all catalogs to a small
        area results in significantly faster query execution.</p>
    <jgwuc:Query runat="server">SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, x.ra, x.dec
INTO twowayxmatch
FROM SDSSDR7:PhotoObjAll AS s WITH(POINT(s.ra, s.dec), ERROR(0.1, 0.1, 0.1))
CROSS JOIN GALEX:PhotoObjAll AS g WITH(POINT(g.ra, g.dec), ERROR(0.2, 0.2, 0.2))
XMATCH BAYESFACTOR x
MUST EXIST s
MUST EXIST g
HAVING LIMIT 1e3
WHERE s.ra BETWEEN 0 AND 5 AND s.dec BETWEEN 0 AND 5
	AND g.ra BETWEEN 0 AND 5 AND g.dec BETWEEN 0 AND 5</jgwuc:Query>
    <p>
        This query cross-matches objects from the SDSS DR7 and the Galex catalog based on
        the equatorial J2000 coordinates ra, dec, assuming an astrometric error of 0.1 and
        0.2 for SDSS and Galex, respectively. Cross-matching is limited to the 0 &lt; ra
        &lt; 0.5 degree interval. Conditions set in the form of coord &gt; -9999 are to
        filter out wrong measurements.</p>
    <p>
        Cross-matching uses a bayesian algorithm to calculate possible matches. Because
        priors may vary from problem to problem, cuts are made on the Bayes factor intead
        of the posterior probability. The limit on the Bayes factor is defined in the HAVING
        LIMIT clause. A value of 10<sup>3</sup> or higher usually means a good match.</p>
    <p>
        In the latter query, because multiple catalog tables are referenced and each table
        has columns with the same name (ra, dec, etc.), aliases for the tables are defined
        by appending &#39;AS alias&#39; after the table name when listed in the FROM clause.
        Average coordinates are also calculated and can be accessed as a computed table.
        The &#39;x&#39; after XMATCH BAYESFACTOR defines the alias for this computed table.
        The computed table has the following columns: ra, dec, cx, cy, cz, logBF, which
        are right ascension, declination, cartesian unit vector coordinates, and logarithm
        of the Bayes factor, respectively.</p>
</asp:Content>
