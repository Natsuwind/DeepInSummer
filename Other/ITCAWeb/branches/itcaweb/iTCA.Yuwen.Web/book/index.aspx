<%@ Page Language="C#" MasterPageFile="~/include/itca.Master" AutoEventWireup="true"
    Inherits="_Default, App_Web_s0wfswid" Title="首页 - 留言板 - iTCA 重庆工学院计算机协会" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="clear: both; float: right; padding-right: 0;">
        <asp:HyperLink ID="hl_logined" Text="留言请登陆" NavigateUrl="login.aspx" runat="server"></asp:HyperLink>&nbsp;
        <asp:HyperLink ID="hl_uCenter" Text="注册会员" NavigateUrl="reg.aspx" runat="server"></asp:HyperLink>&nbsp;
        <asp:HyperLink ID="hl_post" Text="我要留言" NavigateUrl="post.aspx" runat="server" Visible="False"></asp:HyperLink></div>
    <br />
    <asp:Repeater ID="rptList" runat="server" OnItemCommand="rptList_ItemCommand">
        <ItemTemplate>
            <!--top table Start-->
            <table width="100%" cellpadding="0" cellspacing="0" bgcolor="#888888">
                <tr>
                    <td>
                        <!--level 1 table Start-->
                        <table width="100%" cellpadding="0" cellspacing="1">
                            <tr bgcolor="#ffffff">
                                <td align="left" style="width: 20%; vertical-align: top" rowspan="3">
                                    <div style="text-align: center">
                                        <%# "&nbsp;<br><img src=../images/face/"+DataBinder.Eval(Container.DataItem,"g_Face")+".gif border=0>" %>
                                    </div>
                                    <br />
                                    &nbsp;&nbsp;留言人:<%# DataBinder.Eval(Container.DataItem,"g_User") %>
                                    <br>
                                </td>
                                <td style="width: 80%">
                                    <!--level 2 table1 Start-->
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="height: 28px" align="left">
                                                <strong>标题：<%# DataBinder.Eval(Container.DataItem, "g_title")%></strong></td>
                                            <td align="right">
                                                <asp:Panel ID="pan_admin1" Enabled="false" Visible="false" runat="server">
                                                    <%# "<a href=Del.aspx?id="+ DataBinder.Eval(Container.DataItem,"g_ID") +" onclick="+(char)34+"return confirm('确定删除吗？');"+(char)34+">删除</a>" %>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <!--level 2 table1 End-->
                                </td>
                            </tr>
                            <tr bgcolor="#FFFFFF">
                                <td align="left" valign="top" style="height: 140px">
                                    <!--level 2 table2 Start-->
                                    <table width="100%" cellpadding="5" cellspacing="0">
                                        <tr>
                                            <td valign="top" align="left">
                                                <br />
                                                <%# DataBinder.Eval(Container.DataItem, "g_content")%>
                                            </td>
                                        </tr>
                                    </table>
                                    <!--level 2 table2 End-->
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor="#ffffff" style="height: 28px" align="left">
                                    留言于<%# DataBinder.Eval(Container.DataItem, "g_date", "{0:yyyy-MM-dd hh:mm}")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;来源：<%# DataBinder.Eval(Container.DataItem,"g_Ip") %>
                                </td>
                            </tr>
                        </table>
                        <!--level 1 table1 End-->
                    </td>
                </tr>
                <tr>
                    <td>
                        <!--level 1 table2 Start-->
                        <table width="800px" cellpadding="0" cellspacing="1">
                            <tr bgcolor="#d5d5d5">
                                <td align="left" style="width: 20%; vertical-align: top" rowspan="3">
                                    <div align="center">
                                        <%# "&nbsp;<br><img src=../images/face/" + DataBinder.Eval(Container.DataItem, "g_ReplyFace") + ".gif border=0>"%>
                                    </div>
                                    <br />
                                    &nbsp;&nbsp;回复人:<%# DataBinder.Eval(Container.DataItem, "g_ReplyUser")%>
                                    <br />
                                    <br />
                                </td>
                                <td style="width: 80%">
                                    <!--level 2 table3 Start-->
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td height="28" align="left">
                                                回复于<%# DataBinder.Eval(Container.DataItem, "g_Replydate", "{0:yyyy-MM-dd hh:mm}")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;来源：<%# DataBinder.Eval(Container.DataItem,"g_replyip") %></td>
                                            <td align="right">
                                                <asp:Panel ID="pan_admin2" Enabled="false" Visible="false" runat="server">
                                                    <%# "<a href=ashow.aspx?id="+ DataBinder.Eval(Container.DataItem,"g_id")+">编辑</a>" %>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <!--level 2 table3 End-->
                                </td>
                            </tr>
                            <tr bgcolor="#d5d5d5">
                                <td align="left" valign="top" height="140">
                                    <!--level 2 table4 Start-->
                                    <table width="100%" cellpadding="5" cellspacing="0">
                                        <tr>
                                            <td valign="top" align="left">
                                                <%#  DataBinder.Eval(Container.DataItem, "g_ReplyContent")%>
                                            </td>
                                        </tr>
                                    </table>
                                    <!--level 2 table4 End-->
                                </td>
                            </tr>
                        </table>
                        <!--level 1 table2 End-->
                    </td>
                </tr>
            </table>
            <!--top table End-->
            <br>
        </ItemTemplate>
    </asp:Repeater>
    共<asp:Label ID="lbTotalPage" runat="server"></asp:Label>页|<asp:HyperLink ID="hlkFirstPage"
        runat="server">首页</asp:HyperLink>|<asp:HyperLink ID="hlkPrevPage" runat="server">上一页</asp:HyperLink>|<asp:HyperLink
            ID="hlkNextPage" runat="server">下一页</asp:HyperLink>|<asp:HyperLink ID="hlkLastPage"
                runat="server">末页</asp:HyperLink>|第<asp:Label ID="lbCurrentPage" runat="server"></asp:Label>页
</asp:Content>
