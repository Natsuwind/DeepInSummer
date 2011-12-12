<%@ Page Language="C#" MasterPageFile="~/include/itca.Master" AutoEventWireup="true" Inherits="yuwenLogin, App_Web_s0wfswid" Title="会员登录 - 留言板 - iTCA 重庆工学院计算机协会" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="background-image: url(../images/book/login.jpg)">
        <br />
        <br />
        <br />
        <br />
        <center>
                <table border="1" cellpadding="4" cellspacing="0" style="border-collapse: collapse">
                    <tr>
                        <td style="width: 240px">
                            <table border="0" cellpadding="0">
                                <tr>
                                    <td align="center" colspan="2" style="font-weight: bold; font-size: 0.9em; color: white;
                                        background-color: #507cd1; height: 16px;">
                                        登录</td>
                                </tr>
                                <tr>
                                    <td align="left" style="height: 21px; width: 55px;">
                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">账号:</asp:Label></td>
                                    <td style="height: 21px">
                                        <asp:TextBox ID="UserName" runat="server" Font-Size="14px" Width="151px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                            ErrorMessage="必须填写“用户名”。" ToolTip="必须填写“用户名”。" ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 55px">
                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">密码:</asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="Password" runat="server" Font-Size="14px" TextMode="Password" Width="151px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                            ErrorMessage="必须填写“密码”。" ToolTip="必须填写“密码”。" ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2" style="height: 35px">
                                        验证码: &nbsp; &nbsp;<img src="../code.aspx" alt="区分大小写" height="17" width="55" />
                                        &nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;<asp:TextBox ID="tb_code" runat="server" Width="51px"
                                            ToolTip="区分大小写"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2" style="height: 14px">
                                        有效时间:<asp:DropDownList ID="DDL_cookies" Width="64px" runat="server">
                                            <asp:ListItem Value="0">即时</asp:ListItem>
                                            <asp:ListItem Value="1d">一天</asp:ListItem>
                                            <asp:ListItem Value="30d" Selected="True">一月</asp:ListItem>
                                            <asp:ListItem Value="365d">一年</asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp; &nbsp;&nbsp;&nbsp; &nbsp;
                                        <asp:Button ID="LoginButton" runat="server" BackColor="White" BorderColor="#507CD1"
                                            BorderStyle="Solid" BorderWidth="1px" CommandName="Login" Font-Names="Verdana"
                                            Font-Size="Larger" ForeColor="#284E98" Text="登录" ValidationGroup="Login1" OnClick="LoginButton_Click1"
                                            Height="22px" Width="56px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 55px">
                                        <a href="find.aspx">忘记密码?</a></td>
                                    <td align="right">
                                        <a href="reg.aspx">我要注册!</a>&nbsp; &nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
        </center>
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
    </div>
</asp:Content>
