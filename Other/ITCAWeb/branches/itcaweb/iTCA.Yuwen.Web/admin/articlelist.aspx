<%@ Page Language="C#" AutoEventWireup="true" Codebehind="articlelist.aspx.cs" Inherits="iTCA.Yuwen.Web.Admin.articlelist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Main.css" rel="stylesheet" type="text/css" />
    <title>管理文章</title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="CMSColumn" style="float: right">
            选择管理的栏目：<asp:DropDownList ID="ddlColumns" runat="server" AutoPostBack="True"
                OnSelectedIndexChanged="ddlColumns_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <div id="pager" style="float: right; clear: both">分页:<%=pagecounthtml%></div>
        <div style="clear:both"></div>
        <asp:Repeater ID="rptArticleList" runat="server">
            <HeaderTemplate>
                <table style="width: 100%">
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td style="width: 35px">
                        <a href="deleteadmin.aspx?articleid=<%# DataBinder.Eval(Container.DataItem, "articleid")%>">[删除]</a>
                    </td>
                    <td style="width: 10px">
                        <asp:CheckBox ID="ckbox_single" runat="server" /><input type="hidden" id="SelectedID"
                            runat="server" value='<%# DataBinder.Eval(Container.DataItem, "articleid")%>' />
                    </td>
                    <td>
                        [<%# DataBinder.Eval(Container.DataItem, "columnname")%>]<a href="postarticle.aspx?id=<%# DataBinder.Eval(Container.DataItem,"articleid") %>&action=edit"><%# DataBinder.Eval(Container.DataItem,"title") %></a>
                    </td>
                    <td style="width: 80px">
                        <a href="#" target="_blank">
                            <%# DataBinder.Eval(Container.DataItem,"username") %>
                        </a>
                    </td>
                    <td style="width: 80px">
                        <%# DataBinder.Eval(Container.DataItem, "postdate", "{0:yyyy-MM-dd}")%>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table></FooterTemplate>
        </asp:Repeater>
    </form>
</body>
</html>
