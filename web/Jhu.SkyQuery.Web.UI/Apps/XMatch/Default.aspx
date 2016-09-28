<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Jhu.SkyQuery.Web.UI.Apps.XMatch.Default" MasterPageFile="~/App_Masters/Basic/UI.Master" %>

<%@ Register Src="CatalogList.ascx" TagPrefix="uc1" TagName="CatalogList" %>



<asp:Content runat="server" ContentPlaceHolderID="toolbar">
    <script>
        function ShowRefreshing() {
            var datasetList = document.getElementById("<%= datasetList.ClientID %>");
            var tableList = document.getElementById("<%= tableList.ClientID %>");

            datasetList.disabled = true;
            tableList.disabled = true;

            tableList.options.length = 0;
            var opt = new Option("Refreshing...", "");
            tableList.options.add(opt);
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(ShowRefreshing);
    </script>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <jgwc:Toolbar runat="server">
                <jgwc:ToolbarElement runat="server" Style="width: 140px">
                    <asp:Label ID="datasetListLabel" runat="server" Text="Catalog:" /><br />
                    <asp:DropDownList ID="datasetList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DatasetList_SelectedIndexChanged"
                        CssClass="ToolbarControl" Width="140px">
                    </asp:DropDownList>
                </jgwc:ToolbarElement>
                <jgwc:ToolbarElement runat="server">
                    <asp:Label ID="tableListLabel" runat="server" Text="Table:" /><br />
                    <asp:DropDownList ID="tableList" runat="server"
                        CssClass="ToolbarControl" Style="width: 100%; box-sizing: border-box;">
                    </asp:DropDownList>
                </jgwc:ToolbarElement>
                <jgwc:ToolbarButton runat="server" ID="addCatalog" Text="add catalog" OnClick="AddCatalog_Click" />
                <jgwc:ToolbarButton runat="server" ID="generateQuery" Text="generate query" />
                <jgwc:ToolbarButton runat="server" ID="submitJob" Text="submit as job" />
            </jgwc:Toolbar>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="middle">
    <asp:UpdatePanel runat="server" class="dock-fill dock-container">
        <ContentTemplate>
            <jgwuc:Form runat="server" ID="introForm" SkinID="Schema" Text="Getting started with the xmatch form">
                <FormTemplate>
                    <ul>
                        <li>Select and catalog and a table from the lists above.</li>
                        <li>MyDB referers to your own user data space.</li>
                        <li>Specify target table.</li>
                        <li>Click on 'add catalog' to select and add table to xmatch.</li>
                        <li>Click on 'generate query' to see xmatch SQL.</li>
                        <li>Click on 'submit as job' to run xmatch job immediately</li>
                    </ul>
                </FormTemplate>
                <ButtonsTemplate>
                </ButtonsTemplate>
            </jgwuc:Form>
            <asp:Panel runat="server" ID="catalogListPanel" Visible="false" CssClass="dock-fill dock-scroll">
                <div class="Frame">
                    <div class="FrameHeader">
                        <asp:Label runat="server">Global parameters</asp:Label>
                    </div>
                    <div class="FrameBody">
                        <table style="width: 100%">
                            <tr>
                                <td style="vertical-align: top; width: 420px">
                                    <table class="FormTable">
                                        <tr>
                                            <td class="FormLabel">
                                                <asp:Label ID="intoLabel" runat="server" Text="Target table:" /></td>
                                            <td class="FormField">
                                                <asp:TextBox ID="into" runat="server" Text="xmatchtable" /></td>
                                        </tr>
                                        <tr>
                                            <td class="FormLabel">
                                                <asp:Label ID="bayesFactorLabel" runat="server" Text="Bayes factor cut:" /></td>
                                            <td class="FormField">
                                                <asp:TextBox ID="bayesFactor" runat="server" Text="1000" /></td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: auto; vertical-align: top">
                                    <table class="FormTable" style="width: 100%">
                                        <tr>
                                            <td class="FormLabel">
                                                <asp:Label ID="regionLabel" runat="server" Text="Region constraint:" /></td>
                                            <td class="FormField">
                                                <asp:TextBox ID="region" runat="server"
                                                    TextMode="MultiLine" Rows="3" Width="100%"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <uc1:CatalogList runat="server" ID="catalogList" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
