<%@ Page Language="C#" AutoEventWireup="true" Title="XMatch" %>

<asp:Content ContentPlaceHolderID="Text" runat="server">
    <h1>XMatch</h1>
    <p>Cross-match jobs can be defined using the xmatch form tool user interface or by writing SQL queries. The xmatch tool is easy to use but offers less parameterization options compared to queries. Behind the scneces, the xmatch form works by generating SQL queries automatically so it can be used as a starting point to writing custom SQL queries.</p>
    <h2>Cross-match algorithm</h2>
    <p>The cross-match algorithm is based on Bayesian statistics and sophisticated query techniques that allow executing full-catalog cross identification jobs in a reasonable time. Instead of calculating the posterior probabilty of the hypothesis $H$ that all detections in a match originate from very same celestial object it calculates the Bayes factor $$B(H,K|D) = \frac{P(H|D)}{P(H)} \Big/ \frac{P(K|D)}{P(K)} = \frac{P(D|H)}{P(D|K)},$$ where $D$ denotes the data and $K$ is the complement hypothesis of $H$. The Bayes factor can be seen as a number that measures the weight of the evidence in favor of the hypothesis $H$ vs. the evidence against it. A value of $B$ above $B &gt; 10^3$ is a pretty strong evidence in favor of $H$, whereas $B &lt; 10^{-3}$ strongly favors the complement hypothesis instead. The value of $B$ around unity means that the evidence is not conclusive.Instead of working with posterior probabilities, SkyQuery uses the Bayes factor to set the lower limit on the goodness of a match. For details on how to calculate the Bayes factor for a match, we refer to 
        Budavári &amp; Szalay (2008).</p>
    <p>SkyQuery can cross-match multiple catalogs but the matching is done by two catalogs at a time. It can be shown that an n-way matching can be done as a chain of matchings between the next catalog and the output coordinates from the previous pairwise match. Moreover, the order of pair-wise matching doesn&#39;t matter and starting with the smallest catalogs is usually the most beneficial. By knowing the astrometric error limits for each catalog, false match candidates can be eliminated early on during the matching chain, hence reducing computational time.</p>
    <h2>XMatch form</h2>
    <p>The xmatch tool is an easy entry point to using SkyQuery. </p>
    <p>In the &#39;Global parameters&#39; select the target database. Here you can choose MyDB as the target for small jobs or MyScratch if larger output is expected. Note, that MyScratch is intended for data staging purposes only and although tables are private to the user, they might be deleted without prior notice. Also give the output table a meaningful name and select the columns you need in the resultset. The available output columns are the following:</p>
    <ul>
        <li>MatchID - ID of the match, unique to the output table and can be used as primary key for further cross-matching</li>
        <li>RA - right ascension of best fit position</li>
        <li>Dec - declination of best fit position</li>
        <li>Cx - Castesian X coordinate of the unit vector pointing to the best fit position</li>
        <li>Cy - Castesian Y coordinate of the unit vector pointing to the best fit position</li>
        <li>Cz - Castesian Z coordinate of the unit vector pointing to the best fit position</li>
        <li>N - reserved for internal use</li>
        <li>A - reserved for internal use</li>
        <li>L - reserved for internal use</li>
        <li>Q - reserved for internal use</li>
        <li>LogBF - logarithm of the Bayes factor</li>
    </ul>
    <p>
        Change the default value of the Bayes factor cut. Increase or decrease it by a magnitude if you are not satisfied with the results.</p>
    <p>
        Specify a spherical region constraint to restrict the matching to a certain area of the sky. Clear the field to run an all-sky matching. Circular regions are in the form of CIRCLE J2000 RA DEC RADIUS where the coordinates are in decimal degrees and the radius is in arc minutes. For more details on the region syntax, follow this link. Do not specify a region constraint when any of the catalogs you&#39;re matching are small (under 1M objects), as it may slow down query processing.</p>
    <p>
        To add a catalog to the cross-match job, simply select a dataset and a table from the list on the top of the page and click &#39;add catalog&#39;. A new box will appear below in which you can specify the job parameters specific to the catalog. The alias field can be left as it is, it&#39;s only relevant when generating cross-match queries. The coordinate columns can be selected automatically or specified manually. Automatic selection should work for the large catalogs that are already part of skyquery. Overriding the coordinate columns manually for large datasets results in slow query execution. In case of user tables in MyDB or MyScratch, however, always make sure you select the columns manually. The astrometric error can either be constant for all objects in the tables; in this case specify a decimal number in arc seconds. The error can also be taken from a table column, in this case it has to be also in arc seconds. Additional filters on the columns of the table can be specified by 
        following the usual syntax of the WHERE clause of SQL. For example, a simple filter of mag_g &lt; 17 will restrict the search to bright objects, given the column mag_g exists in the table. Multiple logical expression can be combined using the OR and AND keywords and round brackets. Finally, select the columns you want to retrieve as part of the resultset. It is often desirable to retrieve the primary key columns and the original coordinates, though it is not a requirement.</p>
    <p>
        Please note, that all tables that are part of the cross-matching must have a primary key column defined. To define a column containing unique numeric values, or generate a primary key column automatically, go to the &#39;my data&#39; menu, &#39;tables&#39; tab, click on the details arrow next to the table&#39;s name and click the &#39;primary key&#39; button.</p>
    <p>
        To submit the cross-match job for execution immadietely, click on &#39;submit as job&#39;. If you want to see the automatically generated xmatch query instead, click on &#39;generate query&#39;.</p>
    <h2>XMatch queries</h2>
    <p>The following query is a good starting point to explain the cross-match SQL syntax.</p>
    <pre>SELECT x.MatchID, x.RA, x.Dec, 
       m.ID, m.ra, m.dec,
       s.objID, s.ra, s.dec, s.u, s.g, s.r, s.i, s.z,
       w.cntr, w.ra, w.dec, w.w1mag, w.w2mag, w.w3mag, w.w4mag,
       s.r - w.w1mag AS r_w1
INTO MYDB:xmatchtable
FROM XMATCH ( 
       MUST EXIST IN MYDB:mytable AS m WITH(POINT(ra, dec), ERROR(0.2)),
       MUST EXIST IN SDSSDR12:Galaxy AS s WITH(ERROR(RaErr, 0.1, 0.3)),
       MUST EXIST IN WISE:PhotoObj AS w WITH(ERROR(0.1)),
       LIMIT BAYESFACTOR TO 1000) AS x
     INNER JOIN SDSSDR12:SpecObj sp ON sp.bestObjID = s.objID
WHERE s.r < 17 AND s.g - s.r BETWEEN 0 AND 2
REGION &#39;CIRCLE J2000 0.0 0.0 10.0&#39;</pre>

    <p>The column list following the SELECT keyword is standard SQL and simply lists the source table and xmatch output columns that will be part of the resultset. The very last column demonstrates how to use expressions and specify a column name with the AS keyword. In constract to standard SQL, if no column names are specified explicitly, SkyQuery generates names automatically.</p>
    <p>The INTO part is also very similar to SQL except that the output dataset is also specified with the dataset:table syntax. The output can only be MYDB or MYSCRACH because these are the writable databases. The default is always MYDB and if no INTO clause is specified a new output table name is generated automatically.</p>
    <p>The XMATCH part is an extension to SQL and it is used to describe the matching job. Note, that the <strong>XMATCH( ) AS x</strong> construct basically acts like a function that returns multiple tables. The <strong>AS x</strong> part specifies that the best position columns will be referenced with the x alias, c.f. the select list. Inside the brackets a list of tables follows which will be matched against each other. Currently only the <strong>MUST EXISTS</strong> specifier is supported which means in order to succesfully match, a detection must exists in each of the tables. </p>
    <p>In the example above the first table is a user table and the position columns are explicitly specified because no meta-data describing the columns is usually available for user tables. The <strong>ERROR(d)</strong> part sets the constant astrometric error in arc seconds. In case of the SDSS data, specifying the position is not necessary because it is figured out automatically from the meta-data. The astrometric error is taken from the column raErr (we assume symmetric position errors here), and the two constants following the column name are the minimum and maximum possible astrometric error. These values are required for the early mismatch foltering and specifying them explicitly can significantly speed up xmatch query execution.</p>
    <p>The last parameter in the XMATCH list specifies the matching algorithm (Bayesian) and the minimum value of the Bayes factor to accept the match. Do not specify the constraint on Bayes factor in the form of WHERE x.LogBF &gt; 3, it will not be recognized by the query optimizer. Also note, that the XMATCH syntax is indeed an extension to the SQL language and standard JOIN constructs are also allowed, such as in the case above, when we join on the table SpecObj to only match those object that have spectroscopic counterparts in SDSS.</p>
    <p>Cross-match queries also support the standard WHERE clause of SQL. The query optimizer analyzes the logical expression of the WHERE clause and if it is possible applies the constaints <em>before</em> performing the cross match.</p>
    <h2>Spatial constraints</h2>
    <p>The cross-match query above contains a REGION clause which is also an extension to the standard SQL syntax. The REGION clause restrict the match within a certain part of the sky. The filter is applied <em>before</em> performing the cross-match operation, so in very extreme cases it might be possible that final best match positions are actually outside the region. </p>
    <p>The REGION clause can also be used with tables not part of the XMATCH operation. In this case the coordinate columns must explicitly be specified using the WITH(POINT(ra, dec) syntax, for example:</p>
    <pre>SELECT specObjID, ra, dec
FROM SDSSDR12:SpecObj WITH(POINT(ra, dec))
REGION 'CIRCLE J2000 0.0 0.0 10.0'</pre>
    <p>The spherical shapes can be fairly complex and the detailed syntax of region strings can be found here. Here are a few simple examples to define circles, rectangles, polygons and convex hulls, respectively:</p>
    <ul>
        <li>CIRCLE J2000 ra dec radius</li>
        <li>RECT J2000 ra1 dec1 rad2 dec2</li>
        <li>POLY J2000 ra1 dec1 ra2 dec2 ra3 dec3 ...</li>
        <li>CHULL J2000 ra1 dec1 ra2 dec2 ra3 dec3 ...</li>
    </ul>
    <p>&nbsp;</p>
    
</asp:Content>
