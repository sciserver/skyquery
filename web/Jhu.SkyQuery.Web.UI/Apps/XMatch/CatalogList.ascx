<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CatalogList.ascx.cs" Inherits="Jhu.SkyQuery.Web.UI.Apps.XMatch.CatalogList" %>

<%@ Register Src="CatalogForm.ascx" TagPrefix="uc1" TagName="Catalog" %>

<asp:ListView runat="server" ID="catalogList" OnItemDataBound="CatalogList_ItemDataBound"
    OnItemCommand="CatalogList_ItemCommand">
    <LayoutTemplate>
        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
    </LayoutTemplate>
    <ItemTemplate>
        <uc1:Catalog runat="server" ID="catalog" />
    </ItemTemplate>
    <EmptyDataTemplate>
        No catalogs have been selected yet. Please select a catalog and table and click on the 'add catalog' button above.
    </EmptyDataTemplate>
</asp:ListView>
