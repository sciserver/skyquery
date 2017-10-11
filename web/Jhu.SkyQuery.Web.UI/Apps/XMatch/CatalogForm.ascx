<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CatalogForm.ascx.cs"
    Inherits="Jhu.SkyQuery.Web.UI.Apps.XMatch.CatalogForm" %>

<div class="gw-box-frame-top">
    <div class="gw-box-header">
        <div class="gw-box-row">
            <asp:Label ID="tableLabel" runat="server" Text="Catalog table:" Width="180px" />
            <asp:Label ID="table" runat="server" Text="Label" CssClass="gw-box-span" />
            <asp:LinkButton ID="remove" CommandName="remove" runat="server" Text="remove" Width="64px" />
        </div>
    </div>
</div>
<div class="gw-box-frame">
    <div class="gw-box-item">
        <div class="gw-box-row">
            <table style="width: 100%">
                <tr>
                    <td style="vertical-align: top; width: 420px">
                        <table class="FormTable">
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label runat="server" ID="aliasLabel" Text="Alias:" />
                                </td>
                                <td class="FormField">
                                    <asp:TextBox runat="server" ID="alias" CssClass="FormField" />
                                    <asp:RequiredFieldValidator runat="server" ID="aliasRequiredValidator"
                                        ControlToValidate="alias" Display="Dynamic"
                                        ErrorMessage="&lt;br /&gt;Required field" />
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="coordinateModeLabel" runat="server" Text="Coordinate columns:"></asp:Label>
                                </td>
                                <td class="FormField">
                                    <asp:DropDownList runat="server" ID="coordinateMode" CssClass="FormField"
                                        AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="CoordinateMode_SelectedIndexChanged">
                                        <asp:ListItem Text="Select automatically" Value="Automatic" Selected="True" />
                                        <asp:ListItem Text="Select manually (slow!)" Value="Manual" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr runat="server" id="coordinatesRow" visible="false">
                                <td class="FormLabel">
                                    <asp:Label ID="coordinatesLabel" runat="server" Text="RA, Dec:" />
                                </td>
                                <td class="FormField">
                                    <asp:DropDownList ID="ra" runat="server" CssClass="FormField"
                                        Width="98px">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="dec" runat="server" CssClass="FormField"
                                        Width="98px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="errorModeLabel" runat="server" Text="Astrometric error mode:"></asp:Label>
                                </td>
                                <td class="FormField">
                                    <asp:DropDownList runat="server" ID="errorMode" CssClass="FormField"
                                        CausesValidation="false" AutoPostBack="true" OnSelectedIndexChanged="ErrorMode_SelectedIndexChanged">
                                        <asp:ListItem Text="Constant" Value="Constant" Selected="True" />
                                        <asp:ListItem Text="Column" Value="Column" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr runat="server" id="errorRow" visible="true">
                                <td class="FormLabel">
                                    <asp:Label ID="errorValueLabel" runat="server" Text="Error in arc sec:"></asp:Label>
                                </td>
                                <td class="FormField">
                                    <asp:TextBox ID="errorValue" runat="server" CssClass="FormField"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="errorValueRequiredValidator"
                                        ControlToValidate="errorValue" Display="Dynamic"
                                        ErrorMessage="&lt;br /&gt;Required field" />
                                    <asp:RegularExpressionValidator ID="errorValueFormatValidator" runat="server"
                                        ControlToValidate="errorValue" Display="Dynamic" ErrorMessage="&lt;br /&gt;Invalid format"
                                        ValidationExpression="[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?" />
                                </td>
                            </tr>
                            <tr runat="server" id="errorColumnRow" visible="false">
                                <td class="FormLabel">
                                    <asp:Label ID="errorColumnLabel" runat="server" Text="Error column:"></asp:Label>
                                </td>
                                <td class="FormField">
                                    <asp:DropDownList ID="errorColumn" runat="server" CssClass="FormField">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr runat="server" id="errorLimitsRow" visible="false">
                                <td class="FormLabel">
                                    <asp:Label ID="errorLimitsLabel" runat="server" Text="Min/max. error in arc sec:"></asp:Label>
                                </td>
                                <td class="FormField">
                                    <asp:TextBox ID="errorMin" runat="server" CssClass="FormField" Width="98px"></asp:TextBox>
                                    <asp:TextBox ID="errorMax" runat="server" CssClass="FormField" Width="98px"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="errorMinRequiredValidator"
                                        ControlToValidate="errorMin" Display="Dynamic"
                                        ErrorMessage="&lt;br /&gt;Required field" />
                                    <asp:RegularExpressionValidator ID="errorMinFormatValidator" runat="server"
                                        ControlToValidate="errorMin" Display="Dynamic" ErrorMessage="&lt;br /&gt;Invalid format"
                                        ValidationExpression="[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?" />
                                    <asp:RequiredFieldValidator runat="server" ID="errorMaxRequiredValidator"
                                        ControlToValidate="errorMax" Display="Dynamic"
                                        ErrorMessage="&lt;br /&gt;Required field" />
                                    <asp:RegularExpressionValidator ID="errorMaxFormatValidator" runat="server"
                                        ControlToValidate="errorMax" Display="Dynamic" ErrorMessage="&lt;br /&gt;Invalid format"
                                        ValidationExpression="[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?" />
                                </td>
                            </tr>
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="whereLabel" runat="server" Text="Additional filters:"
                                        ToolTip="Specify any SQL 'WHERE' criteria here without the 'WHERE' keyword. Combine criteria with 'AND' and 'OR'."></asp:Label>
                                </td>
                                <td class="FormField">
                                    <asp:TextBox ID="where" runat="server" CssClass="FormField"
                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 4px; vertical-align: top"></td>
                    <td style="width: auto; vertical-align: top">
                        <table class="FormTable">
                            <tr>
                                <td class="FormLabel">
                                    <asp:Label ID="columnListLabel" runat="server" Text="Columns to return:" />
                                </td>
                            </tr>
                            <tr>
                                <td class="FormField" style="width: 100%;">
                                    <div class="FormField" style="width: 100%; height: 155px">
                                        <asp:CheckBoxList ID="columnList" runat="server"
                                            RepeatLayout="UnorderedList"
                                            RepeatDirection="Vertical" CssClass="FormField"
                                            Height="153px" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
