<%@ Page Language="C#" MasterPageFile="~/include/itca.Master" AutoEventWireup="true" Inherits="book_find, App_Web_s0wfswid" Title="找回密码 - 留言板 - iTCA 重庆工学院计算机协会" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div  style="background-image:url(../images/book/login.jpg)">
          <p>&nbsp;</p>
          <p>&nbsp;</p>
          <p>&nbsp;</p>
          <p><br />
            <br />
            <br />
            </p>
          <p>&nbsp;</p>
          <table align="center" style="width: 500px; text-align:left">
            <tr>
                <td style="width: 103px">
                    <asp:Label ID="lb_strName" runat="server" Visible="False">用户名</asp:Label></td>
                <td style="width: 269px">
                    <asp:Label ID="lb_name" runat="server" Visible="False"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 103px">
                    <asp:Label ID="lb_strAsk" runat="server" Text="提示问题" Visible="False"></asp:Label></td>
                <td style="width: 269px">
                    <asp:Label ID="lb_ask" runat="server" Visible="False"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 103px; height: 45px">
                    <asp:Label ID="lb_tb1" runat="server" Text="请输入用户名"></asp:Label></td>
                <td style="width: 269px; height: 45px">
                    <asp:TextBox ID="tb1" runat="server"></asp:TextBox><asp:RegularExpressionValidator
                        ID="REV1" runat="server" ControlToValidate="tb1" Display="Dynamic" ErrorMessage="密码长度6-12字符"
                        ValidationExpression="^\w{6,12}" Visible="False"></asp:RegularExpressionValidator><asp:RequiredFieldValidator
                            ID="RFV1" runat="server" ControlToValidate="tb1" Display="Dynamic" ErrorMessage="必填选项"
                            Visible="False"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="width: 103px">
                    <asp:Label ID="lb_tb2" runat="server" Text="验证码" Visible="False"></asp:Label></td>
                <td style="width: 269px">
                    <asp:TextBox ID="tb2" runat="server" Visible="False"></asp:TextBox>
                    <asp:CompareValidator ID="REV2" runat="server" ControlToCompare="tb1" ControlToValidate="tb2"
                        Display="Dynamic" ErrorMessage="2次输入不匹配" Visible="False"></asp:CompareValidator><asp:RequiredFieldValidator
                            ID="RFV2" runat="server" ControlToValidate="tb2" Display="Dynamic" ErrorMessage="必填选项"
                            Visible="False"></asp:RequiredFieldValidator><asp:Image ID="img_validator" runat="server"
                                EnableTheming="False" EnableViewState="False" ToolTip="区分大小写" Visible="False" /></td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="bt_sub" runat="server" OnClick="bt_sub_Click" Text="下一步" /></td>
            </tr>
        </table>
	      <p>&nbsp;</p>
	      <p>&nbsp;</p>
	      <p>&nbsp;</p>
	      <p>&nbsp;</p>
	      <p>&nbsp;</p>
	  </div>
</asp:Content>

