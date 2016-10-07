<%@ Page Title="" Language="C#" MasterPageFile="~/App_Masters/Basic/UI.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Jhu.SkyQuery.Web.UI.Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="middle" runat="server">
    <jgwuc:Form runat="server" ID="WelcomeForm" ImageUrl="~/Images/skyquery.png">
        <FormTemplate>
            <p>
                You need to register and sign in before using this web site.
                You will be redirected to the sign in page by clicking any
                of the menu buttons above.
            </p>
        </FormTemplate>
        <ButtonsTemplate>
        </ButtonsTemplate>
    </jgwuc:Form>
</asp:Content>
