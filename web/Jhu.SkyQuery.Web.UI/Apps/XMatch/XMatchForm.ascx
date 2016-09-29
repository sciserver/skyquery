<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="XMatchForm.ascx.cs" Inherits="Jhu.SkyQuery.Web.UI.Apps.XMatch.XMatchForm" %>

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
                                <asp:Label ID="targetTableLabel" runat="server" Text="Target table:" /></td>
                            <td class="FormField">
                                <asp:TextBox ID="targetTable" runat="server" Text="xmatchtable" CssClass="FormField" />
                                <asp:RequiredFieldValidator runat="server" ID="targetTableRequiredValidator"
                                    ControlToValidate="targetTable" Display="Dynamic" ErrorMessage="&lt;br /&gt;Required field" />
                                <asp:RegularExpressionValidator ID="targetTableFormatValidator" runat="server"
                                    ControlToValidate="targetTable" Display="Dynamic" ErrorMessage="&lt;br /&gt;Invalid format"
                                    ValidationExpression="[a-zA-Z_]+[a-zA-Z_0-9]*" />
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="bayesFactorLabel" runat="server" Text="Bayes factor cut:" /></td>
                            <td class="FormField">
                                <asp:TextBox ID="bayesFactor" runat="server" Text="1000" CssClass="FormField" />
                                <asp:RequiredFieldValidator runat="server"
                                    ID="bayesFactorRequiredValidator" ErrorMessage="&lt;br /&gt;Required field" Display="Dynamic"
                                    ControlToValidate="bayesFactor" />
                                <asp:RegularExpressionValidator ID="bayesFactorFormatValidator" runat="server"
                                    ControlToValidate="bayesFactor" Display="Dynamic" ErrorMessage="&lt;br /&gt;Invalid format"
                                    ValidationExpression="[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?" />
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="regionLabel" runat="server" Text="Region constraint:" /></td>
                            <td class="FormField">
                                <asp:TextBox ID="region" runat="server" CssClass="FormField"
                                    TextMode="MultiLine" Rows="3"></asp:TextBox>
                                <asp:CustomValidator runat="server" ID="regionValidator"
                                    ControlToValidate="region" Display="Dynamic" OnServerValidate="RegionValidator_ServerValidate" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 4px; vertical-align: top"></td>
                <td style="width: auto; vertical-align: top">
                    <table class="FormTable" style="width: 100%">
                        <tr>
                            <td class="FormLabel">
                                <asp:Label ID="xmatchColumnListLabel" runat="server" Text="XMatch columns to return:" /></td>
                        </tr>
                        <tr>
                            <td class="FormField">
                                <div class="FormField" style="width: 100%">
                                    <asp:CheckBoxList ID="xmatchColumnList" runat="server"
                                        RepeatLayout="UnorderedList"
                                        RepeatDirection="Vertical" CssClass="FormField" Height="85px" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</div>
