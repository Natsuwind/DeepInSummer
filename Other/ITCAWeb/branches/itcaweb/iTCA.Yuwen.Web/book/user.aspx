<%@ Page Language="C#" MasterPageFile="~/include/itca.Master" AutoEventWireup="true"
    Inherits="book_user, App_Web_s0wfswid" Title="查看用户资料 - 留言板 - iTCA 重庆工学院计算机协会" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <center>
        <table style="vertical-align: middle; width: 497px; height: 480px; text-align: left">
            <tr>
                <td style="width: 105px; height: 21px">
                    用户名</td>
                <td style="width: 400px; height: 21px">
                    <asp:Label ID="lb_name" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 105px; height: 21px">
                    姓名</td>
                <td style="width: 400px; height: 21px">
                    <asp:Label ID="lb_truename" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 105px; height: 21px">
                    院系</td>
                <td style="width: 400px; height: 21px">
                    <asp:Label ID="lb_pro" runat="server">
                            
                    </asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 105px; height: 21px">
                    学号</td>
                <td style="width: 400px; height: 21px">
                    <asp:Label ID="lb_no" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 105px; height: 21px">
                    E_mail</td>
                <td style="width: 400px; height: 21px">
                    <asp:Label ID="lb_email" runat="server" Width="250px"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 105px; height: 26px">
                    QQ</td>
                <td style="width: 400px; height: 26px">
                    <asp:Label ID="lb_qq" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 105px">
                    MSN</td>
                <td style="width: 400px">
                    <asp:Label ID="lb_msn" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 105px">
                    个人介绍</td>
                <td style="width: 400px">
                    <asp:Label ID="lb_message" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 105px">
                    注册IP</td>
                <td style="width: 400px">
                    <asp:Label ID="lb_regip" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 105px">
                    登陆IP</td>
                <td style="width: 400px">
                    <asp:Label ID="lb_lastip" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 105px">
                    注册日期</td>
                <td style="width: 400px">
                    <asp:Label ID="lb_regdate" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 105px">
                    最后活动日期</td>
                <td style="width: 400px">
                    <asp:Label ID="lb_lastdate" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                </td>
            </tr>
        </table>
    </center>
</asp:Content>
