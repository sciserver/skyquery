<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Basic/UI.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Jhu.SkyQuery.Web.UI.Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="middle" runat="server">
    <jgwuc:Form runat="server" ID="WelcomeForm" ImageUrl="~/Images/skyquery.png">
        <FormTemplate>
            <p>
                SkyQuery is a scalable database system for cross-matching astronomical source 
        catalogs. With SkyQuery you can
            </p>
            <ul>
                <li>Access a growing list of astronomical catalogs</li>
                <li>Access your own data in SkyServer MyDB</li>
                <li>Write SQL to filter and process data</li>
                <li>Do efficient spatial filtering</li>
                <li>Run full-catalog cross-matches</li>
                <li>Cross-match multiple catalogs using an algorithm based on parallel data processing and Bayesian statistics</li>
                <li>Upload your data and download cross-match results in various standard formats</li>
            </ul>
            <p>
                Since SkyQuery provides high performance data processing with storage
                allocated for each user, you have to sign in with you existing SciServer (former SkyServer)
                account. Click on the sign-in link in the top right corner to start.
            </p>
        </FormTemplate>
        <ButtonsTemplate>
        </ButtonsTemplate>
    </jgwuc:Form>
</asp:Content>
