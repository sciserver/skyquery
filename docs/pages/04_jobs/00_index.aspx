<%@ Page Language="C#" AutoEventWireup="true" Title="Jobs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Text" runat="server">
    <h3>
        Jobs</h3>
    <p>
        Submitted jobs can be viewed by clicking on the 'jobs' menu item. 
        Jobs can be either queries or export jobs. The status column indicates the 
        current state of execution. If a job is &#39;scheduled&#39; it means it is in the queue 
        but the scheduled has not picked it up yet. &#39;Executing&#39; means the job is under 
        processing.</p>
    <h4>
        Query jobs</h4>
    <p>
        Every single query is compiled into a job regardless of its complexity. Because 
        of this, even quick jobs can take a couple of seconds to complete because the 
        scheduler polls job queues one in a second for newly created jobs. Old query 
        jobs retain an entire history of earlier queries.</p>
    <h4>
        Export jobs</h4>
    <p>
        Export jobs convert data tables into downloadable files. Export of large 
        datasets can take a longer time. Exported files are kept on the web server for a 
        week and get deleted afterwards.</p>
    <h4>
        Failed jobs</h4>
    <p>
        Jobs can either complete sucessfully or fail. Error information about failed 
        jobs can be viewed by selecting the job and clicking on the &#39;Details&#39; button. 
        Certain error arise from wrong queries and correcting them is the responsibility 
        of the user. Other errors can be infrastructural. All errors are logged and 
        subseqently analyzed to perfect SkyQuery. If you think an error is 
        infrastructural, you can send a direct email to the site operators and describe 
        the circumstances.</p>
</asp:Content>
