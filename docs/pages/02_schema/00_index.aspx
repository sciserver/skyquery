<%@ Page Language="C#" AutoEventWireup="true" Title="Schema" %>

<asp:Content ContentPlaceHolderID="Text" runat="server">
    <h3>
        Schema</h3>
    <p>
        Use the &#39;schema&#39; menu to get information about the available catalogs. First 
        select an astronomical catalog from the first dropdown on the top of the page. 
        MyDB is the catalog containing your own tables, either updated or generated as 
        an output of a query. Then select the type of objects you want to display. 
        Tables and Views can be queried in the current version. Stored procedures and 
        functions are not supported yet. Select the object from the last list to display 
        its columns, indices, parameters, etc. You can use the &#39;Peek&#39; button to get a 
        sample a data tables and the &#39;Export&#39; button to download data. Export works only 
        on MyDB tables because entire catalogs are too large to be downloaded directly.</p>
</asp:Content>
