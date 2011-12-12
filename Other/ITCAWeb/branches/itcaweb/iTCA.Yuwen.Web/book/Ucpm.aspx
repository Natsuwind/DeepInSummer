<%@ Page Language="C#" MasterPageFile="~/include/itca.Master" AutoEventWireup="true"
    Inherits="ziliao, App_Web_s0wfswid" Title="修改资料 - 留言板 - iTCA 重庆工学院计算机协会" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- #include file="left.html" -->
    <div style="width: 658px; height: 540px; float: right">
        <br />
        <center>
            <table style="vertical-align: middle; width: 497px; text-align: left">
                <tr>
                    <td style="width: 105px; height: 21px">
                        用户名</td>
                    <td style="width: 400px; height: 21px">
                        <asp:Label ID="lb_name" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 105px; height: 21px">
                        E_mail</td>
                    <td style="width: 400px; height: 21px">
                        <asp:TextBox ID="tb_email" runat="server" Width="229px"></asp:TextBox><asp:RegularExpressionValidator
                            ID="REV_email" runat="server" ControlToValidate="tb_email" Display="Dynamic"
                            ErrorMessage="格式错误" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator><asp:RequiredFieldValidator
                                ID="RFV4" runat="server" ControlToValidate="tb_email" ErrorMessage="E_mail必填"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td style="width: 105px">
                        密码提示问题</td>
                    <td style="width: 400px">
                        <asp:Label ID="lb_ask" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 105px; height: 26px">
                        QQ</td>
                    <td style="width: 400px; height: 26px">
                        <asp:TextBox ID="tb_qq" runat="server" Width="115px"></asp:TextBox><asp:RegularExpressionValidator
                            ID="RegularExpressionValidator4" runat="server" ControlToValidate="tb_qq" Display="Dynamic"
                            ErrorMessage="请输入正确的QQ号码" ValidationExpression="^\d{5,11}"></asp:RegularExpressionValidator></td>
                </tr>
                <tr>
                    <td style="width: 105px; height: 26px;">
                        MSN</td>
                    <td style="width: 400px; height: 26px;">
                        <asp:TextBox ID="tb_msn" runat="server" Width="115px"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tb_msn"
                            Display="Dynamic" ErrorMessage="格式错误" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator></td>
                </tr>
                <tr>
                    <td style="width: 105px; vertical-align: text-top; text-align: left;">
                        个人介绍</td>
                    <td style="width: 400px">
                        <asp:TextBox ID="tb_message" runat="server" TextMode="MultiLine" Height="62px" Width="250px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 105px">
                    </td>
                    <td style="width: 400px">
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="bt_sub" runat="server" OnClick="bt_ziliao_Click" Text="提交" Width="86px" />
                        &nbsp; &nbsp; &nbsp;
                        <asp:Button ID="bt_cancel" runat="server" Text="取消" Width="86px" /></td>
                </tr>
            </table>
            <asp:Label ID="lb1" runat="server"></asp:Label>
        </center>
        <center>
            <asp:Label ID="lb2" runat="server"></asp:Label>
        </center>
    </div>
</asp:Content>
