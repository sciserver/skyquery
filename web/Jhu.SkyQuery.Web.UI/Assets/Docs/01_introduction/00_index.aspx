<%@ Page Language="C#" AutoEventWireup="true" Title="Introduction" %>

<asp:Content ContentPlaceHolderID="Text" runat="server">
    <h1>Introduction</h1>
    <p>
        Welcome to SkyQuery!
    </p>
    <p>
        SkyQuery is a free service for cross-matching astronomical source catalogs.
    </p>
    <ul>
        <li>It&#39;s scalable - SkyQuery runs in parallel on a cluster of servers with high-end
            IO subsystems to provide excellent throughput. This allows for performing full-catalog
            cross-matching even between catalogs with billions of objects.</li>
        <li>It&#39;s comprehensive - SkyQuery already contains 50+ important catalogs and the
            number is growing.</li>
        <li>It&#39;s for your data too - SkyQuery allows uploading your own catalogs in various
            data formats for cross-matching.</li>
        <li>It&#39;s sophisticated - SkyQuery&#39;s matching algorithm is based on Bayesian
            statistics and allows for matching multiple catalogs with astrometric error varying
            from object to object.</li>
        <li>It&#39;s a database system - SkyQuery - similarly to other astronomical databases
            such as SDSS SkyServer - is based on SQL database technology. As a result, cross-match
            problems can be formulated in an extended version of the SQL language. The SQL language,
            extended with a very efficient spherical search library, offers excellent filtering
            capabilities and fast execution.</li>
        <li>It&#39;s integrated - SkyQuery is integrated into the SciServer and SkyServer infrastructure.
            This means 
            you can sign in with your existing account and have access to local databases, storage
            and compute facilities such as MyDB, MyScratch and SciServer Compute.</li>
        <li>It&#39;s a web service - 100% of SkyQuery functionality is accessible via REST web
            services which means cross-match functionality can easily be incorporated into scripts
            and data processing pipelines.</li>
    </ul>
    <div class="gw-docs-callout">
        <p>
            For a quick start to SkyQuery,
        <asp:HyperLink runat="server" NavigateUrl="01_quickstart.aspx">follow this link</asp:HyperLink>.
        </p>
    </div>
</asp:Content>
