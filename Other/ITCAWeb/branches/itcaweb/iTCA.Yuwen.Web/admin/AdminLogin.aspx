<%@ page language="C#" autoeventwireup="true" inherits="yuwenLogin, App_Web_ewwuxgiv" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>登陆入口</title>
</head>
<body>
<br />
<br />
<br />
<br />
    <center>
    <div>
        <form id="Login1" runat="server"  >            
                <table border="1" cellpadding="4" cellspacing="0" style="border-collapse: collapse" >
                    <tr>
                        <td style="width: 219px">
                            <table border="0" cellpadding="0">
                                <tr>
                                    <td align="center" colspan="2" style="font-weight: bold; font-size: 0.9em; color: white;
                                        background-color: #507cd1; height: 16px;">
                                        登录</td>
                                </tr>
                                <tr>
                                    <td align="right" style="height: 21px">
                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">账号:</asp:Label></td>
                                    <td style="height: 21px">
                                        <asp:TextBox ID="UserName" runat="server" Font-Size="0.8em" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                            ErrorMessage="必须填写“用户名”。" ToolTip="必须填写“用户名”。" ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">密码:</asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="Password" runat="server" Font-Size="0.8em" TextMode="Password" Width="148px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                            ErrorMessage="必须填写“密码”。" ToolTip="必须填写“密码”。" ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td align="left" colspan="2" style="color: red; height: 19px;">
                                        验证码:<img src="../code.aspx" alt="区分大小写" />
                                        <asp:TextBox ID="tb_code" runat="server"
                                            Width="70px" ToolTip="区分大小写"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2" style="height: 22px">
                                        <asp:Button ID="LoginButton" runat="server" BackColor="White" BorderColor="#507CD1"
                                            BorderStyle="Solid" BorderWidth="1px" CommandName="Login" Font-Names="Verdana"
                                            Font-Size="0.8em" ForeColor="#284E98" Text="登录" ValidationGroup="Login1" OnClick="LoginButton_Click1" />
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>            
        </form>
        &nbsp;</div>
    </center>

</body>
</html>
