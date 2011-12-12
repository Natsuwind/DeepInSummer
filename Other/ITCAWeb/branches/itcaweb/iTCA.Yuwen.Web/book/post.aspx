<%@ Page Language="C#" MasterPageFile="~/include/itca.Master" AutoEventWireup="true"
    Inherits="post, App_Web_s0wfswid" Title="发布留言 - 留言板 - iTCA 重庆工学院计算机协会" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="right" style="float: right; padding-right: 0">
        <asp:HyperLink ID="hl_logined" Text="留言请登陆" NavigateUrl="login.aspx" runat="server"></asp:HyperLink>&nbsp;
        <asp:HyperLink ID="hl_uCenter" Text="用户中心" NavigateUrl="uCenter.aspx" runat="server"
            Visible="False"></asp:HyperLink>&nbsp;
        <asp:HyperLink ID="hl_post" Text="我要留言" NavigateUrl="post.aspx" runat="server" Visible="False"></asp:HyperLink></div>
    <br /><br />
    <center>
        <table style="width: 582px">
            <tr>
                <td style="width: 176px">
                    标题</td>
                <td style="width: 582px">
                    <asp:TextBox ID="tb_title" runat="server" Width="522px" MaxLength="20"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="vertical-align: text-top; width: 176px; text-align: center">
                    内容</td>
                <td style="width: 582px">
                    <asp:TextBox ID="tb_content" runat="server" Height="236px" TextMode="MultiLine" Width="522px"
                        MaxLength="200"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    验证码:<img alt="区分大小写" src="../code.aspx" />
                    <asp:TextBox ID="tb_code" runat="server" ToolTip="区分大小写" Width="70px" MaxLength="5"></asp:TextBox><asp:Button
                        ID="Button1" runat="server" Text="发表" OnClick="Button1_Click" Width="110px" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tb_title"
                        Display="Dynamic" ErrorMessage="请输入标题" SetFocusOnError="True"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tb_content"
                        Display="Dynamic" ErrorMessage="请输入内容" SetFocusOnError="True"></asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
    </center>
</asp:Content>
