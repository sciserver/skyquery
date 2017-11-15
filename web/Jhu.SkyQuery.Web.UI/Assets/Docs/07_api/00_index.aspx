<%@ Page Language="C#" AutoEventWireup="true" Title="Programming API" %>

<asp:Content ContentPlaceHolderID="Text" runat="server">
    <h1>Programming API</h1>
    <p>
        SkyQuery provides programmatic access to its features via a set of REST web 
        services. Functionality is organized into three services
    </p>
    <ul>
        <li>The Schema Service allows querying the database schema and meta data of the 
            astronomical catalogs</li>
        <li>The Jobs Service allows submitting new jobs (standard SQL queries, XMatch jobs,
            table import and export jobs, etc) and checking job status</li>
        <li>The Data Service provides direct access to user tables in MyDB and MyScratch. 
            Tables can be downloaded or uploaded in many formats.</li>
    </ul>
    <p>
        For the description of the service URLs and request/response format examples, go
        to the <a href="<%= ResolveUrl(Jhu.Graywulf.Web.UI.Apps.Api.Default.GetUrl()) %>">api
            page</a>
        and follow the links to the services. Services understand XML and JSON requests
        and can also return the response in either format. The format of the request 
        must be defined in the
        <id>Content-Type</id>
        HTTP header, whereas the response format is
        determined by the
        <id>Accept</id>
        header. The mime types are
        <id>application/xml</id>
        for XML and 
        <id>application/json</id>
        for JSON.
    </p>
    <h1>Data Service file formats</h1>
    <p>
        For a description and the layout of the various file formats, follow
        <a href="http://">this link</a>. The Data Service currently accepts the following
        file 
        formats.
    </p>
    <h2>Upload
        file formats</h2>
    <p>
        Uploaded data format is determined from the Content-Type HTTP header.
    </p>
    <ul>
        <li>Tab-separated text file; default extension: txt; mime-type: text/plain</li>
        <li>Comma-separated values; default extension: csv; mime-type: text/csv</li>
        <li>VOTable; default extension: votable; mime-type: application/x-votable+xml</li>
        <li>FITS; default extension: fits; mime-type: image/fits</li>
    </ul>
    <h2>Download file formats</h2>
    <p>
        Download data format is determined from the Accept HTTP header. The format list
        is same as above but in addition the following download formats are currently 
        supported.
    </p>
    <ul>
        <li>SQL Server native binary data file; default extension: bcp; mime-type: 
            application/sqlserver</li>
    </ul>
    <h1>Client libraries</h1>
    <p>
        The <a href="https://github.com/sciserver/SciScript-Python">SciScript python 
        client library</a> is a wrapper around <a href="http://www.sciserver.org/">SciServer</a>
        functionality including SkyQuery. SciScript is readily available 
        from <a href="http://www.sciserver.org/tools/compute/">SciServer Compute</a>.
    </p>
    <h1>Examples</h1>
    <h2>Python using the SciScript library</h2>
    <jgwuc:SyntaxHighlight runat="server" Brush="python">
from SciServer import Authentication, LoginPortal, Config, CasJobs, SkyServer, SkyQuery, SciDrive

# Define login Name and password before running the tests:
loginName = '***';
loginPassword = '***'

token = Authentication.login(loginName, loginPassword);

jobId = SkyQuery.submitJob(query="SELECT TOP 10 Ra, Dec INTO testtable FROM SDSSDR12:SpecObj", queue="quick")
jobDescription = SkyQuery.waitForJob(jobId=jobId, verbose=True)

table = SkyQuery.getTable(tableName=testtable, datasetName="MyDB", top=10)
    </jgwuc:SyntaxHighlight>
    <h2>Curl</h2>
    <p>The first call to the login-portal get an authentication token that you have to 
        resubmit to the SkyQuery services in the X-Auth-Token HTTP header.</p>
    <jgwuc:SyntaxHighlight runat="server" Brush="bash">
ApplicationUrl="[$ApplicationUrl]"

curl -is -H "Content-Type: application/json" \
     https://portal.sciserver.org/login-portal/keystone/v3/tokens \
     -d '{"auth":{"identity":{"password":{"user":{"name":"***","password":"***"}}}}}' \
     | grep -Eo 'X-Subject-Token: [0-9a-f]+'

curl -X POST -H "X-Auth-Token: ****" \
     -H "Content-Type: application/json" -H "Accept: application/json" \
     http://$ApplicationUrl/Api/V1/Jobs.svc/queues/quick/jobs \
     -d '{"queryJob":{"query":"SELECT TOP 10 RA, Dec FROM SDSSDR12:SpecObj"}}'

curl -H "X-Auth-Token: ****" \
     -H "Content-Type: application/json" -H "Accept: application/json" \
     http://$ApplicationUrl/Api/V1/Jobs.svc/jobs/***JobID***

curl -H "X-Auth-Token: ****" \
     -H "Content-Type: application/json" -H "Accept: text/plain" \
     http://$ApplicationUrl/Api/V1/Data.svc/MyDB/quickresults
    </jgwuc:SyntaxHighlight>
    <h2>JavaScript</h2>
    <p>Comming soon...</p>
</asp:Content>
