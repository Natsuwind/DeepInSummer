<%@ Page Language="C#" MasterPageFile="~/include/itca.Master" AutoEventWireup="true"
    Inherits="book_chgpwd, App_Web_s0wfswid" Title="修改资料 - 留言板 - iTCA 重庆工学院计算机协会" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- #include file="left.html" -->
    <div style="width: 658px; height: 540px; float: right">
        <br />
        <table style="vertical-align: middle; width: 497px; text-align: left">
            <tr>
                <td style="width: 105px; height: 21px">
                </td>
                <td style="width: 400px; height: 21px">
                    更改密码</td>
            </tr>
            <tr>
                <td style="width: 105px; height: 21px">
                    用户名</td>
                <td style="width: 400px; height: 21px">
                    &nbsp;<asp:Label ID="lb_name" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 105px; height: 28px">
                    原始密码</td>
                <td style="width: 400px; height: 28px">
                    <asp:TextBox ID="tb_oldpwd" runat="server" TextMode="Password" Width="115px"></asp:TextBox></td>
            </tr>
            <tr style="color: #000000">
                <td style="width: 105px; height: 28px">
                    密 码</td>
                <td style="width: 400px; height: 28px">
                    <asp:TextBox ID="tb_pwd" runat="server" TextMode="Password" Width="115px"></asp:TextBox>
                    *<asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                        ControlToValidate="tb_pwd" Display="Dynamic" ErrorMessage="密码长度6-18字符" ValidationExpression="^\w{6,18}"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tb_pwd"
                        ErrorMessage="必填选项"></asp:RequiredFieldValidator></td>
            </tr>
            <tr style="color: #000000">
                <td style="width: 105px; height: 28px">
                    重复密码</td>
                <td style="width: 400px; height: 28px">
                    <asp:TextBox ID="tb_pwd2" runat="server" TextMode="Password" Width="115px"></asp:TextBox>
                    *<asp:CompareValidator ID="CV_pwd" runat="server" ControlToCompare="tb_pwd" ControlToValidate="tb_pwd2"
                        Display="Dynamic" ErrorMessage="2次输入不匹配"></asp:CompareValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tb_pwd2"
                        ErrorMessage="必填选项"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="bt_reg" runat="server" OnClick="bt_reg_Click" Text="提交" Width="86px" />
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:Label ID="lb_message"
                        runat="server" ForeColor="Red"></asp:Label>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
