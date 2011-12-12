<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="template.aspx.cs" Inherits="LiteCMS.Web.Admin.template" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>模板生成</title>
    <link href="Main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Panel ID="plFileList" runat="server" Visible="false">
            <asp:CheckBoxList ID="cbxlTemplateFileList" runat="server"></asp:CheckBoxList>
            <asp:Button ID="btnCreateAll" runat="server" OnClick="btnCreateAll_Click" Text="全部生成" />
        </asp:Panel>
        
        <asp:Panel ID="plFolderList" runat="server">
            <asp:Repeater ID="rptFolderList" runat="server">
            <HeaderTemplate>
                <table style="width: 100%">
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td style="width: 35px">
                        <a href="#">[删除]</a>
                    </td>
                    <td style="width: 10px">
                        <asp:CheckBox ID="ckbox_single" runat="server" /><input type="hidden" id="SelectedID"
                            runat="server" value='0' />
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem,"folder") %>
                    </td>
                    <td style="width: 80px">
                        <a href="frame.aspx?action=createtemplate&name=<%# DataBinder.Eval(Container.DataItem,"folder") %>">使用此模板</a>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table></FooterTemplate>
        </asp:Repeater>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
