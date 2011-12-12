<%@ Page Language="C#" MasterPageFile="~/include/itca.Master" AutoEventWireup="true"
    Inherits="UTitle, App_Web_s0wfswid" Title="我的留言 - 留言板 - iTCA 重庆工学院计算机协会" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- #include file="left.html" -->
    <div style="width: 658px; height: 540px; float: right; text-align:left">
        <br />
        <a href="?type=1">显示已回复</a> | <a href="?type=2">显示未回复</a>
        <br /><br />
        <div id="mypostslist">
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <table style="width:80%">
                        <tr>
                            <td id="title">
                                标题</a>
                            </td>
                            <td class="time">
                                留言日期
                            </td>
                            <td class="time">
                                回复日期
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td id="title" align="left">
                            <a href="read.aspx?id=<%# DataBinder.Eval(Container.DataItem,"g_ID") %>" target="_blank">
                                <%# DataBinder.Eval(Container.DataItem,"g_title") %>
                            </a>
                        </td>
                        <td class="time">
                            <%# DataBinder.Eval(Container.DataItem, "g_date", "{0:yyyy-MM-dd hh:mm}")%>
                        </td>
                        <td class="time">
                            <%# DataBinder.Eval(Container.DataItem, "g_replydate", "{0:yyyy-MM-dd}")%>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <br />
        <div class="pager">
            <center>
            <table>
                <tr>
                    <td style="height: 17px">
                        共<asp:Label ID="lbTotalPage" runat="server"></asp:Label>页|<asp:HyperLink ID="hlkFirstPage"
                            runat="server">首页</asp:HyperLink>|<asp:HyperLink ID="hlkPrevPage" runat="server">上一页</asp:HyperLink>|<asp:HyperLink
                                ID="hlkNextPage" runat="server">下一页</asp:HyperLink>|<asp:HyperLink ID="hlkLastPage"
                                    runat="server">末页</asp:HyperLink>|第<asp:Label ID="lbCurrentPage" runat="server"></asp:Label>页
                    </td>
                </tr>
            </table>
            </center>
        </div>
    </div>
</asp:Content>
